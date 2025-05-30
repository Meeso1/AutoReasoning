using System.Diagnostics;
using Logic.Problem.Models;
using Logic.States;
using Logic.States.Models;
using Action = Logic.Problem.Models.Action;

namespace Logic.Queries;

public sealed class History(ProblemDefinition problem, FormulaReducer formulaReducer)
{
    private readonly Dictionary<Formula, StateGroup> _cachedReducedStates = [];

    public IEnumerable<IReadOnlyList<State>> ComputeHistories(State initialState, List<Action> actions)
    {
        if (actions.Count == 0)
        {
            yield return [initialState];
            yield break;
        }
        
        var firstAction = actions[0];
        var endStates = ExecuteAction(initialState, firstAction).EnumerateStates(problem.FluentUniverse);

        // Action is impossible - truncate trajectory
        if (!endStates.Any())
        {
            yield return [initialState];
            yield break;
        }

        foreach (var endState in endStates)
        {
            foreach (var history in ComputeHistories(endState, actions[1..]))
            {
                yield return history.Prepend(initialState).ToList();
            }
        }
    }

    private StateGroup ExecuteAction(State state, Action action)
    {
        if (action.Conditions.Any(c => !c.Condition.IsSatisfiedBy(state)))
        {
            // Action cannot be executed in this state
            return StateGroup.Empty;
        }

        var possibleEndStates = ResZero(state, action);
        var notCountedFluents = GetNotCountedForMinimalization(state, action);
        var minChangeStates = GetMinimalChangeStates(state, possibleEndStates, notCountedFluents);
        return formulaReducer.CompressStateGroup(minChangeStates);
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

    private HashSet<Fluent> GetNotCountedForMinimalization(State state, Action action)
    {
        var result = new HashSet<Fluent>();
        result.UnionWith(problem.FluentUniverse.Where(f => !f.IsInertial));

        var relevantReleaseStatements = action.Releases.Where(e => e.Condition.IsSatisfiedBy(state));
        result.UnionWith(relevantReleaseStatements.Select(e => e.ReleasedFluent));

        return result;
    }

    private static int Changed(State start, State end, HashSet<Fluent> notCountedFluents)
    {
        var result = 0;
        foreach (var fluent in start.FluentValues)
        {
            if (!end.FluentValues.ContainsKey(fluent.Key))
            {
                throw new UnreachableException($"End state does not specify all fluents: {fluent.Key} is present in start state, but not in end state");
            }

            if (notCountedFluents.Contains(fluent.Key))
            {
                continue;
            }

            if (start.FluentValues[fluent.Key] != end.FluentValues[fluent.Key])
            {
                result += 1;
            }
        }

        return result;
    }

    private StateGroup GetMinimalChangeStates(State start, StateGroup possibleEnds, HashSet<Fluent> notCountedFluents)
    {
        var smallestChange = int.MaxValue;
        var minChangeStates = new List<State>();
        foreach (var endState in possibleEnds.EnumerateStates(problem.FluentUniverse))
        {
            var change = Changed(start, endState, notCountedFluents);
            if (change < smallestChange)
            {
                smallestChange = change;
                minChangeStates.Clear();
                minChangeStates.Add(endState);
            }
            else if (change == smallestChange)
            {
                minChangeStates.Add(endState);
            }
        }

        return new StateGroup(minChangeStates.Select(s => s.FluentValues).ToList());
    }
}
