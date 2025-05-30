using System.Diagnostics;
using System.Linq;
using Logic.Problem.Models;
using Logic.Queries.Models;
using Logic.States;
using Logic.States.Models;
using Action = Logic.Problem.Models.Action;

namespace Logic.Queries;

/// <summary>
/// 	Class that evaluates queries for one given problem
/// </summary>
/// <remarks>
/// 	Can cache problem-specific data - if a new problem is specified, a new instance of the evaluator will be created
/// </remarks>
/// <param name="problem">
/// 	Problem definition
/// </param>
public sealed class QueryEvaluator(ProblemDefinition problem, FormulaReducer formulaReducer)
{
    private readonly History _history = new(problem, formulaReducer);

    public bool Evaluate(Query query)
    {
        return query switch
        {
            ExecutableQuery q => EvaluateExecutable(q),
            AccessibleQuery q => EvaluateAccessible(q),
            AffordableQuery q => EvaluateAffordable(q),
            _ => throw new UnreachableException($"Query type not implemented: {query.GetType()}")
        };
    }

    private bool CheckTrajectories(Query query, Func<IReadOnlyList<State>, bool> predicate)
    {
        var histories = problem.InitialStates
                               .EnumerateStates(problem.FluentUniverse)
                               .Select(start => _history.ComputeHistories(start, query.Program.Actions.ToList()));

        return histories.All(history => query.Type switch
        {
            QueryType.Possibly => history.All(predicate),
            QueryType.Necessarily => history.Any(predicate),
            _ => throw new UnreachableException($"Query type not implemented: {query.Type}")
        });
    }

    private bool EvaluateExecutable(ExecutableQuery query)
    {
        return CheckTrajectories(query, trajectory => trajectory.Count == query.Program.Actions.Count+1);
    }

    private bool EvaluateAccessible(AccessibleQuery query)
    {
        return CheckTrajectories(query, trajectory => 
            trajectory.Count == query.Program.Actions.Count+1 
            && query.States.Contains(trajectory[^1]));
    }

    private bool EvaluateAffordable(AffordableQuery query)
    {
        throw new NotImplementedException();
    }
}
