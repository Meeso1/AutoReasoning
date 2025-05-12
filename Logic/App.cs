using Logic.Model;
using Logic.Model.Models;
using Logic.Queries;

namespace Logic;

/// <summary>
///     App entry point for UI and stuff
/// </summary>
public sealed class App
{
    private readonly ModelDefinitionParser _modelParser = new();
    private ModelSpecificStuff? _modelDependent;

    public SetModelResult SetModel(string definition)
    {
        if (!_modelParser.TryParse(definition, out var model, out var errors))
        {
            return new SetModelResult(false, errors);
        }

        _modelDependent = new(model, new QueryParser(model), new QueryEvaluator(model));
        return new SetModelResult(true, []);
    }

    public EvaluateQueryResult EvaluateQuery(string queryString)
    {
        if (_modelDependent is null)
        {
            return new(null, false,
                ["Model needs to be set before queries can be evaluated"]);
        }
        
        if (!_modelDependent.QueryParser.TryParse(queryString, out var query, out var errors))
        {
            return new(null, false, errors);
        }

        var result = _modelDependent.Evaluator.Evaluate(query);
        return new(result, true, []);
    }

    private sealed record ModelSpecificStuff(
        ModelDefinition Model, 
        QueryParser QueryParser, 
        QueryEvaluator Evaluator);
}

public sealed record SetModelResult(bool IsValid, IReadOnlyList<string> Errors);

public sealed record EvaluateQueryResult(bool? Success, bool IsValid, IReadOnlyList<string> Errors);
