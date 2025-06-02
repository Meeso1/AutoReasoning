using Logic.Problem;
using Logic.Problem.Models;
using Logic.Queries;
using Logic.States;
using Logic.States.Models;
using Logic.Queries.Models;

namespace Logic;

/// <summary>
///     App entry point for UI and stuff
/// </summary>
public sealed class App
{
    public FormulaParser FormulaParser { get; } = new FormulaParser(new FormulaTokenizer());
    public FormulaReducer FormulaReducer { get; } = new FormulaReducer();
    public QueryEvaluator? QueryEvaluator { get; private set; }
    public ProblemSpecificStuff? ProblemDependent { get; private set; }

    public SetModelResult SetModel(IReadOnlyDictionary<string, Fluent> fluents,
        IReadOnlyList<ActionStatement> actionStatements,
        IReadOnlyList<Formula> initials,
        IReadOnlyList<Formula> always)
    {
        ProblemDefinition problem = ProblemDefinitionParser.CreateProblemDefinition(fluents, actionStatements, initials, always);
        QueryEvaluator = new QueryEvaluator(problem, FormulaReducer);

        ProblemDependent = new(problem, QueryEvaluator);
        return new SetModelResult(true, []);
    }

    public EvaluateQueryResult EvaluateQuery(Query query)
    {
        if (ProblemDependent is null)
        {
            return new(null, false,
                ["Model needs to be set before queries can be evaluated"]);
        }

        var result = ProblemDependent.Evaluator.Evaluate(query);
        return new(result, true, []);
    }

    public sealed record ProblemSpecificStuff(
        ProblemDefinition Problem,
        QueryEvaluator Evaluator);
}

public sealed record SetModelResult(bool IsValid, IReadOnlyList<string> Errors);

public sealed record EvaluateQueryResult(bool? Success, bool IsValid, IReadOnlyList<string> Errors);
