using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Logic.Problem.Models;
using Logic.Queries.Models;
using Logic.States;
using Action = Logic.Problem.Models.Action;

namespace Logic.Queries;

/// <summary>
/// 	Class that parses and validates queries for a given problem
/// </summary>
/// <remarks>
///     Can cache problem-specific data - if a new problem is specified, a new instance of the parser will be created
/// </remarks>
/// <param name="problem">
/// 	Problem definition
/// </param>
public sealed class QueryParser(
    ProblemDefinition problem,
    FormulaReducer reducer,
    FormulaParser parser
)
{


    /// <summary>
    /// 	Parses and validates a query
    /// </summary>
    /// <param name="input">
    /// 	Query to parse
    /// </param>
    /// <param name="query">
    /// 	Parsed query if parsing was successful, null otherwise
    /// </param>
    /// <param name="errors">
    /// 	Errors that occurred during parsing, or null if parsing was successful
    /// </param>
    /// <returns>
    /// 	True if parsing was successful, false otherwise
    /// </returns>
    public bool TryParse(
        string input,
        [NotNullWhen(true)] out Query? query,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        query = null;
        var lines = input.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
        {
            errors = ["Query is empty"];
            return false;
        }
        else if (lines.Length > 1)
        {
            errors = ["Query must contain only one line"];
            return false;
        }

        var (queryTypeString, remaining) = PopWord(lines[0].Trim());
        if (!TryParseQueryType(queryTypeString, out var queryType, out errors))
        {
            return false;
        }

        (var queryKindString, remaining) = PopWord(remaining);
        if (!TryParseQueryKind(queryKindString, out var queryKind, out errors))
        {
            return false;
        }

        switch (queryKind)
        {
            case QueryKind.Executable:
                if (!TryParseExecutableQuery(remaining, queryType.Value, out var executableQuery, out errors))
                {
                    return false;
                }
                query = executableQuery;
                return true;
            case QueryKind.Accessible:
                if (!TryParseAccessibleQuery(remaining, queryType.Value, out var accessibleQuery, out errors))
                {
                    return false;
                }
                query = accessibleQuery;
                return true;
            case QueryKind.Affordable:
                if (!TryParseAffordableQuery(remaining, queryType.Value, out var affordableQuery, out errors))
                {
                    return false;
                }
                query = affordableQuery;
                return true;
            default:
                throw new UnreachableException($"Unhandled query kind: {queryKind}");
        }
    }

    private static PopWordResult PopWord(string input)
    {
        var split = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return new PopWordResult(split.FirstOrDefault(), split.Length > 1 ? split[1] : string.Empty);
    }

    private static bool TryParseQueryType(
        string? input,
        [NotNullWhen(true)] out QueryType? queryType,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        errors = null;
        queryType = null;

        if (input is null)
        {
            errors = ["Query is empty"];
            return false;
        }

        var lower = input.ToLowerInvariant();
        queryType = lower switch
        {
            "necessarily" => QueryType.Necessarily,
            "possibly" => QueryType.Possibly,
            _ => null
        };

        if (queryType is null)
        {
            errors = ["Invalid query type. Valid types are: necessarily, possibly"];
        }

        return queryType != null;
    }

    private static bool TryParseQueryKind(
        string? input,
        [NotNullWhen(true)] out QueryKind? queryKind,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        errors = null;
        queryKind = null;

        if (input is null)
        {
            errors = ["Query kind not specified. Query type (necessarily/possibly) must be followed by a query kind (executable/accessible/affordable)"];
            return false;
        }

        var lower = input.ToLowerInvariant();
        queryKind = lower switch
        {
            "executable" => QueryKind.Executable,
            "accessible" => QueryKind.Accessible,
            "affordable" => QueryKind.Affordable,
            _ => null
        };

        if (queryKind is null)
        {
            errors = ["Invalid query kind. Valid kinds are: executable, accessible, affordable"];
        }

        return queryKind != null;
    }

    private bool TryParseExecutableQuery(
        string remainingInput,
        QueryType queryType,
        [NotNullWhen(true)] out ExecutableQuery? query,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        query = null;
        errors = null;

        // Executable queries are always of the form "<type> executable <program>", so we parse all remaining input as a program
        if (!TryParseActionProgram(remainingInput, out var program, out var programErrors))
        {
            errors = programErrors;
            return false;
        }

        query = new ExecutableQuery(queryType, program);
        return true;
    }

    private bool TryParseAccessibleQuery(
        string remainingInput,
        QueryType queryType,
        [NotNullWhen(true)] out AccessibleQuery? query,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        query = null;

        var split = remainingInput.Split(" with ", 2, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2)
        {
            errors = ["Accessible query must be of form: <type> accessible <condition> with <program>"];
            return false;
        }

        if (!parser.TryParse(split[0], problem.Fluents, out var formula, out errors))
        {
            return false;
        }

        if (!TryParseActionProgram(split[1], out var program, out errors))
        {
            return false;
        }

        var stateGroup = reducer.Reduce(formula);
        query = new AccessibleQuery(queryType, program, stateGroup);
        return true;
    }

    private bool TryParseAffordableQuery(
        string remainingInput,
        QueryType queryType,
        [NotNullWhen(true)] out AffordableQuery? query,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        query = null;

        var split = remainingInput.Split(" with budget ", 2, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2)
        {
            errors = ["Affordable query must contain a budget specification, as \"with budget <budget>\" (where <budget> is a non-negative integer)"];
            return false;
        }

        if (!TryParseActionProgram(split[0], out var program, out errors))
        {
            return false;
        }

        if (!uint.TryParse(split[1], out var budget))
        {
            errors = [$"Budget specification is not an unsigned integer: \"{split[1]}\""];
            return false;
        }

        if (budget < 0)
        {
            errors = [$"Budget must be non-negative: \"{budget}\""];
            return false;
        }

        query = new AffordableQuery(queryType, program, budget);
        return true;
    }

    private bool TryParseActionProgram(
        string input,
        [NotNullWhen(true)] out ActionProgram? program,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        program = null;
        errors = null;

        var sequence = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToList();

        if (sequence.Count == 0)
        {
            errors = ["Action program must contain at least one action"];
            return false;
        }

        var actions = new List<Action>();
        var invalidActionNames = new List<string>();
        foreach (var actionName in sequence)
        {
            if (!problem.Actions.TryGetValue(actionName, out var action))
            {
                invalidActionNames.Add(actionName);
                continue;
            }

            actions.Add(action);
        }

        if (invalidActionNames.Count > 0)
        {
            errors = ["Invalid action names: " + string.Join(", ", invalidActionNames.Select(name => $"\"{name}\""))];
            return false;
        }

        program = new ActionProgram(actions);
        return true;
    }

    private sealed record PopWordResult(string? Word, string Remaining);

    private enum QueryKind
    {
        Executable,
        Accessible,
        Affordable
    }
}
