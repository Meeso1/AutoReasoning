using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Logic.States;

public static partial class FormulaTokenizer
{
    public static IReadOnlyList<string> ReservedIdentifiers { get; } = new List<string>
    {
        "with", "budget", "necessarily", "possibly", "executable", "affordable", "accessible"
    };

    public static bool TryTokenize(
        string input,
        out List<Token> tokens,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        tokens = [];
        var errorList = new List<string>();
        errors = null;

        int currentPos = 0;
        foreach (var match in (ValidTokensRegex().Matches(input) as IReadOnlyList<Match>)!)
        {
            // Check if new match starts right after the previous match
            if (currentPos < match.Index)
            {
                errorList.Add($"Unexpected character at position {currentPos}: '{input[currentPos..Math.Min(currentPos + 5, input.Length)]}'");
                errors = errorList;
                return false;
            }

            // Skip whitespace since regex also matches any whitespace sequences
            if (match.Value.Trim().Length == 0)
            {
                currentPos += match.Length;
                continue;
            }

            var type = GetTokenType(match);

            // Check if identifier is in the reserved list
            if (type == TokenType.Identifier && ReservedIdentifiers.Contains(match.Value))
            {
                errorList.Add($"'{match.Value}' at position {currentPos} cannot be used as an identifier - it is a reserved keyword");
                errors = errorList;
                return false;
            }

            tokens.Add(new Token(type, match.Value, currentPos));
            currentPos += match.Length;
        }

        // Check if we've consumed the entire input
        if (currentPos < input.Length)
        {
            var invalidToken = input.Substring(currentPos, Math.Min(10, input.Length - currentPos));
            errorList.Add($"Invalid token at position {currentPos}: '{invalidToken}'");
            errors = errorList;
            return false;
        }

        if (errorList.Count > 0)
        {
            errors = errorList;
            return false;
        }

        return true;
    }

    private static TokenType GetTokenType(Match match)
    {
        if (match.Groups[1].Success) return TokenType.OpenParen;
        if (match.Groups[2].Success) return TokenType.CloseParen;
        if (match.Groups[3].Success) return TokenType.True;
        if (match.Groups[4].Success) return TokenType.False;
        if (match.Groups[5].Success) return TokenType.Not;
        if (match.Groups[6].Success) return TokenType.And;
        if (match.Groups[7].Success) return TokenType.Or;
        if (match.Groups[8].Success) return TokenType.Implies;
        if (match.Groups[9].Success) return TokenType.Equivalent;
        return TokenType.Identifier;
    }

    [GeneratedRegex(@"\s+|(\()|(\))|(\btrue\b)|(\bfalse\b)|(\bnot\b)|(\band\b)|(\bor\b)|(\bimplies\b)|(\bequivalent\b)|([a-zA-Z_][a-zA-Z0-9_]*)")]
    private static partial Regex ValidTokensRegex();
}

public enum TokenType
{
    OpenParen,
    CloseParen,
    True,
    False,
    Not,
    And,
    Or,
    Implies,
    Equivalent,
    Identifier
}

public sealed record Token(TokenType Type, string Value, int Position);
