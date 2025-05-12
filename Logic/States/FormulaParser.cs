using System.Diagnostics.CodeAnalysis;
using Logic.States.Models;

namespace Logic.States;

/// <summary>
/// 	Class that parses and validates formulas
/// </summary>
public sealed class FormulaParser
{
    /// <summary>
    /// 	Parse and validate a formula
    /// </summary>
    /// <param name="input">
    /// 	Formula to parse (from any condition specification)
    /// </param>
    /// <param name="fluentNames">
    /// 	Names of all valid fluents in the problem
    /// </param>
    /// <param name="formula">
    /// 	Parsed formula if parsing was successful, null otherwise
    /// </param>
    /// <param name="errors">
    /// 	Errors that occurred during parsing, or null if parsing was successful
    /// </param>
    /// <returns>
    /// 	True if parsing was successful, false otherwise
    /// </returns>
    public bool TryParse(
        string input,
        IReadOnlyDictionary<string, Fluent> fluents,
        [NotNullWhen(true)] out Formula? formula,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        formula = null;

        if (!FormulaTokenizer.TryTokenize(input, out var tokens, out errors))
        {
            return false;
        }

        int position = 0;
        var parseResult = ParseFormula(tokens, fluents, ref position, 0);
        if (!parseResult.IsSuccess)
        {
            errors = parseResult.Errors;
            return false;
        }

        if (position < tokens.Count)
        {
            errors = [$"Unexpected tokens at the end of input starting at position {tokens[position].Position}"];
            return false;
        }

        formula = parseResult.Formula;
        return true;
    }

    /// <summary>
    /// 	Parse any formula, handling operator precedence
    /// </summary>
    private ParseResult ParseFormula(List<Token> tokens, IReadOnlyDictionary<string, Fluent> fluents, ref int position, int precedence)
    {
        // Parse the left-hand side
        var leftResult = ParsePrimary(tokens, fluents, ref position);
        if (!leftResult.IsSuccess)
        {
            return leftResult;
        }

        var left = leftResult.Formula;

        // Parse operators with higher precedence than the current one
        while (position < tokens.Count)
        {
            var currentToken = tokens[position];
            int currentPrecedence = GetPrecedence(currentToken.Type);

            // If current operator has lower precedence than allowed, stop parsing
            if (currentPrecedence < precedence)
            {
                break;
            }

            // Handle binary operators
            if (currentToken.Type == TokenType.And ||
                currentToken.Type == TokenType.Or ||
                currentToken.Type == TokenType.Implies ||
                currentToken.Type == TokenType.Equivalent)
            {
                position++; // Consume the operator

                // Parse the right-hand side with a higher precedence
                var rightResult = ParseFormula(tokens, fluents, ref position, currentPrecedence + 1);
                if (!rightResult.IsSuccess)
                {
                    return rightResult;
                }

                var right = rightResult.Formula;

                // Create the binary operation formula
                switch (currentToken.Type)
                {
                    case TokenType.And:
                        left = new And(left, right);
                        break;
                    case TokenType.Or:
                        left = new Or(left, right);
                        break;
                    case TokenType.Implies:
                        left = new Implies(left, right);
                        break;
                    case TokenType.Equivalent:
                        left = new Equivalent(left, right);
                        break;
                }
            }
            else
            {
                break;
            }
        }

        return new ParseResult(left, []);
    }

    /// <summary>
    /// 	Parse a primary expression (everything except binary operators)
    /// </summary>
    private ParseResult ParsePrimary(List<Token> tokens, IReadOnlyDictionary<string, Fluent> fluents, ref int position)
    {
        if (position >= tokens.Count)
        {
            return new ParseResult(null, ["Unexpected end of input"]);
        }

        var token = tokens[position++];
        switch (token.Type)
        {
            case TokenType.True:
                return new ParseResult(new True(), []);

            case TokenType.False:
                return new ParseResult(new False(), []);

            case TokenType.Not:
                var notResult = ParsePrimary(tokens, fluents, ref position);
                if (!notResult.IsSuccess)
                {
                    return notResult;
                }
                return new ParseResult(new Not(notResult.Formula), []);

            case TokenType.OpenParen:
                var parenResult = ParseFormula(tokens, fluents, ref position, 0);
                if (!parenResult.IsSuccess)
                {
                    return parenResult;
                }

                if (position >= tokens.Count || tokens[position].Type != TokenType.CloseParen)
                {
                    return new ParseResult(null, [$"Expected closing parenthesis at position {token.Position}"]);
                }

                position++; // Consume the closing parenthesis
                return parenResult;

            case TokenType.Identifier:
                if (!fluents.TryGetValue(token.Value, out var fluent))
                {
                    return new ParseResult(null, [$"Unknown fluent '{token.Value}' at position {token.Position}"]);
                }

                return new ParseResult(new FluentIsSet(fluent), []);

            default:
                return new ParseResult(null, [$"Unexpected token '{token.Value}' at position {token.Position}"]);
        }
    }

    private static int GetPrecedence(TokenType type)
    {
        return type switch
        {
            TokenType.Equivalent => 1,
            TokenType.Implies => 2,
            TokenType.Or => 3,
            TokenType.And => 4,
            TokenType.Not => 5,
            _ => 0
        };
    }

    private sealed record ParseResult(Formula? Formula, IReadOnlyList<string> Errors)
    {
        [MemberNotNullWhen(true, nameof(Formula))]
        public bool IsSuccess => Errors.Count == 0;
    }
}
