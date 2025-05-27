using Logic.Problem;
using Logic.Problem.Models;
using Logic.Queries;
using Logic.States;
using Logic.States.Models;

namespace Logic;

/// <summary>
///     App entry point for UI and stuff
/// </summary>
public sealed class App
{
    public FormulaParser FormulaParser { get; } = new FormulaParser(new FormulaTokenizer());
    public FormulaReducer FormulaReducer { get; } = new FormulaReducer();
    public ProblemDefinitionParser ProblemParser { get; } = new();
    public QueryEvaluator? QueryEvaluator { get; private set; }
    public ProblemSpecificStuff? ProblemDependent { get; private set; }

    public SetModelResult SetModel(IReadOnlyDictionary<string, Fluent> fluents,
        IReadOnlyList<ActionStatement> actionStatements,
        IReadOnlyDictionary<Fluent, bool> initials,
        IReadOnlyList<Formula> always)
    {
        ProblemDefinition problem = ProblemParser.CreateProblemDefinition(fluents, actionStatements, initials, always);
        QueryEvaluator = new QueryEvaluator(problem);

        ProblemDependent = new(
            problem,
            new QueryParser(
                problem,
                FormulaReducer,
                FormulaParser),
                QueryEvaluator);
        return new SetModelResult(true, []);
    }

    public EvaluateQueryResult EvaluateQuery(string queryString)
    {
        if (ProblemDependent is null)
        {
            return new(null, false,
                ["Model needs to be set before queries can be evaluated"]);
        }

        if (!ProblemDependent.QueryParser.TryParse(queryString, out var query, out var errors))
        {
            return new(null, false, errors);
        }

        var result = ProblemDependent.Evaluator.Evaluate(query);
        return new(result, true, []);
    }

    public sealed record ProblemSpecificStuff(
        ProblemDefinition Problem,
        QueryParser QueryParser,
        QueryEvaluator Evaluator);
}

public sealed record SetModelResult(bool IsValid, IReadOnlyList<string> Errors);

public sealed record EvaluateQueryResult(bool? Success, bool IsValid, IReadOnlyList<string> Errors);
