using System.Diagnostics;
using System.Diagnostics.Metrics;
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
    private readonly StateGroup _validInitialStates = problem.InitialStates;

    public bool Evaluate(Query query)
    {
        if (_validInitialStates.SpecifiedFluentGroups.Count == 0) {  return false; }
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
        var histories = _validInitialStates.EnumerateStates(problem.FluentUniverse)
                                           .Select(start => _history.ComputeHistories(start, query.Program.Actions.ToList()));

        return histories.All(history => query.Type switch
        {
            QueryType.Possibly => history.Any(predicate),
            QueryType.Necessarily => history.All(predicate),
            _ => throw new UnreachableException($"Query type not implemented: {query.Type}")
        });
    }

    private bool EvaluateExecutable(ExecutableQuery query)
    {
        return CheckTrajectories(query, trajectory => trajectory.Count == query.Program.Actions.Count + 1);
    }

    private bool EvaluateAccessible(AccessibleQuery query)
    {
        return CheckTrajectories(query, trajectory =>
            trajectory.Count == query.Program.Actions.Count + 1
            && query.States.Contains(trajectory[^1]));
    }



    private bool AffordablePredicate(uint costLimit, ActionProgram actions, IReadOnlyList<State> trajectory)
    {
        int cost = 0;
        for(int i=0; i<actions.Actions.Count; i++)
        {
            var action = actions.Actions[i];

            foreach (var cause in action.Effects)
            {
                if (!cause.Condition.IsSatisfiedBy(trajectory[i])) { continue; }

                if (!cause.Effect.IsSatisfiedBy(trajectory[i]) && cause.Effect.IsSatisfiedBy(trajectory[i+1])) {
                    cost += cause.CostIfChanged;
                }
            }

            foreach (var release in action.Releases)
            {
                if (!release.Condition.IsSatisfiedBy(trajectory[i])) { continue; }

                Formula fluentState = new FluentIsSet(release.ReleasedFluent);

                if (fluentState.IsSatisfiedBy(trajectory[i]) != fluentState.IsSatisfiedBy(trajectory[i + 1]))
                {
                    cost += release.CostIfChanged;
                }
            }
        }
        return cost <= costLimit;
    }

    private bool EvaluateAffordable(AffordableQuery query)
    {
        return CheckTrajectories(query, trajectory =>
            trajectory.Count == query.Program.Actions.Count + 1
            && AffordablePredicate(query.CostLimit, query.Program, trajectory));
    }
}
