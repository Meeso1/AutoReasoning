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
    private StateGroup? _startingStates = null;

    public QueryResult Evaluate(Query query)
    {
        if (GetStartingStates().SpecifiedFluentGroups.Count == 0)
        {
            return QueryResult.Inconsistent;
        }

        return query switch
        {
            ExecutableQuery q => EvaluateExecutable(q),
            AccessibleQuery q => EvaluateAccessible(q),
            AffordableQuery q => EvaluateAffordable(q),
            _ => throw new UnreachableException($"Query type not implemented: {query.GetType()}")
        } ? QueryResult.Consequence : QueryResult.NotConsequence;
    }

    private bool CheckTrajectories(Query query, Func<IReadOnlyList<State>, bool> predicate)
    {
        var histories = GetStartingStates().EnumerateStates(problem.FluentUniverse)
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

    private static bool AffordablePredicate(uint costLimit, ActionProgram actions, IReadOnlyList<State> trajectory)
    {
        var cost = 0;
        foreach (var (action, i) in actions.Actions.Select((action, i) => (action, i)))
        {
            foreach (var cause in action.Effects)
            {
                if (!cause.Condition.IsSatisfiedBy(trajectory[i]))
                {
                    continue;
                }

                if (!cause.Effect.IsSatisfiedBy(trajectory[i]) && cause.Effect.IsSatisfiedBy(trajectory[i + 1]))
                {
                    cost += cause.CostIfChanged;
                }
            }

            foreach (var release in action.Releases)
            {
                if (!release.Condition.IsSatisfiedBy(trajectory[i]))
                {
                    continue;
                }

                var fluentState = new FluentIsSet(release.ReleasedFluent);
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

    private StateGroup GetStartingStates()
    {
        if (_startingStates is not null)
        {
            return _startingStates;
        }

        var initialStates = problem.ValidStates;

        var initialStatements = problem.ValueStatements.Where(statement => statement.ActionChain.Actions.Count == 0);
        foreach (var statement in initialStatements)
        {
            var matchingStates = formulaReducer.Reduce(statement.Effect);
            initialStates = StateGroup.And(initialStates, matchingStates);
        }

        var otherStatements = problem.ValueStatements.Where(statement => statement.ActionChain.Actions.Count > 0);
        foreach (var statement in otherStatements)
        {
            var matchingStates = new List<State>();
            foreach (var initialState in initialStates.EnumerateStates(problem.FluentUniverse))
            {
                var trajectories = _history.ComputeHistories(initialState, statement.ActionChain.Actions.ToList())
                                           .Where(trajectory => trajectory.Count == statement.ActionChain.Actions.Count + 1);

                var isSatisfied = statement switch
                {
                    AfterStatement s => trajectories.All(trajectory => s.Effect.IsSatisfiedBy(trajectory[^1])),
                    ObservableStatement s => trajectories.Any(trajectory => s.Effect.IsSatisfiedBy(trajectory[^1])),
                    _ => throw new UnreachableException($"Value statement type not implemented: {statement.GetType()}")
                };

                if (isSatisfied)
                {
                    matchingStates.Add(initialState);
                }
            }

            initialStates = formulaReducer.CompressStateGroup(new StateGroup(matchingStates.Select(state => state.FluentValues).ToList()));
        }

        _startingStates = initialStates;
        return initialStates;
    }
}
