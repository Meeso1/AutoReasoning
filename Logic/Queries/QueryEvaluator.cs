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
    private readonly Dictionary<Formula, StateGroup> _cachedReducedStates = new();
    
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
                               .Select(start => ComputeHistories(start, query.Program.Actions.ToList()));

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

    private IEnumerable<IReadOnlyList<State>> ComputeHistories(State initialState, List<Action> actions)
    {
        if (actions.Count == 0)
        {
            yield return [initialState];
        }
        
        var firstAction = actions[0];
        foreach (var endState in ExecuteAction(initialState, firstAction).EnumerateStates(problem.FluentUniverse))
        {
            foreach (var history in ComputeHistories(endState, actions[1..]))
            {
                yield return history.Prepend(initialState).ToList();
            }
        }
    }

    private StateGroup ExecuteAction(State state, Action action)
    {
        if (!action.Conditions.All(c => c.Condition.IsSatisfiedBy(state)))
        {
            // Action cannot be executed in this state
            return new StateGroup([]);
        }

        var possibleEndStates = ResZero(state, action);
        var notCountedFluents = GetNotCountedForMinimalization(state, action);

        var lowestCost = int.MaxValue;
        var minCostStates = new List<State>();
        foreach (var endState in possibleEndStates.EnumerateStates(problem.FluentUniverse))
        {
            var cost = Changed(state, endState, notCountedFluents);
            if (cost < lowestCost)
            {
                lowestCost = cost;
                minCostStates.Clear();
                minCostStates.Add(endState);
            }
            else if (cost == lowestCost)
            {
                minCostStates.Add(endState);
            }
        }

        return formulaReducer.CompressStateGroup(new StateGroup(minCostStates.Select(s => s.FluentValues).ToList()));
    }

    private StateGroup ResZero(State state, Action action)
    {
        var applicableEffects = action.Effects.Where(e => e.Condition.IsSatisfiedBy(state));

        var result = problem.ValidStates;
        foreach (var effect in applicableEffects)
        {
            if (!_cachedReducedStates.TryGetValue(effect.Effect, out var reducedEffect))
            {
                reducedEffect = formulaReducer.Reduce(effect.Effect);
                _cachedReducedStates[effect.Effect] = reducedEffect;
            }

            result = StateGroup.And(result, reducedEffect);
        }

        return result;
    }

    private IReadOnlyList<Fluent> GetNotCountedForMinimalization(State state, Action action)
    {
        var result = new HashSet<Fluent>();
        result.UnionWith(problem.FluentUniverse.Where(f => !f.IsInertial));

        var relevantReleaseStatements = action.Releases.Where(e => e.Condition.IsSatisfiedBy(state));
        result.UnionWith(relevantReleaseStatements.Select(e => e.ReleasedFluent));

        return result.ToList();
    }

    private static int Changed(State start, State end, IReadOnlyList<Fluent> notCountedFluents)
    {
        var result = 0;
        foreach (var fluent in start.FluentValues)
        {
            if (notCountedFluents.Contains(fluent.Key))
            {
                throw new UnreachableException($"End state does not specify all fluents: {fluent.Key} is present in start state, but not in end state");
            }

            if (start.FluentValues[fluent.Key] != end.FluentValues[fluent.Key])
            {
                result += 1;
            }
        }

        return result;
    }
}
