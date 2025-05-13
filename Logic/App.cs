using Logic.Problem;
using Logic.Problem.Models;
using Logic.Queries;
using Logic.States;

namespace Logic;

/// <summary>
///     App entry point for UI and stuff
/// </summary>
public sealed class App
{
    private readonly ProblemDefinitionParser _problemParser = new();
    private ProblemSpecificStuff? _problemDependent;

    public SetModelResult SetModel(string definition)
    {
        if (!_problemParser.TryParse(definition, out var problem, out var errors))
        {
            return new SetModelResult(false, errors);
        }

        _problemDependent = new(
            problem,
            new QueryParser(
                problem,
                new FormulaReducer(),
                new FormulaParser(new FormulaTokenizer())),
            new QueryEvaluator(problem));
        return new SetModelResult(true, []);
    }

    public EvaluateQueryResult EvaluateQuery(string queryString)
    {
        if (_problemDependent is null)
        {
            return new(null, false,
                ["Model needs to be set before queries can be evaluated"]);
        }

        if (!_problemDependent.QueryParser.TryParse(queryString, out var query, out var errors))
        {
            return new(null, false, errors);
        }

        var result = _problemDependent.Evaluator.Evaluate(query);
        return new(result, true, []);
    }

    private sealed record ProblemSpecificStuff(
        ProblemDefinition Problem,
        QueryParser QueryParser,
        QueryEvaluator Evaluator);
}

public sealed record SetModelResult(bool IsValid, IReadOnlyList<string> Errors);

public sealed record EvaluateQueryResult(bool? Success, bool IsValid, IReadOnlyList<string> Errors);
