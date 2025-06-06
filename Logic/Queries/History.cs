using System.Diagnostics;
using Logic.Problem.Models;
using Logic.States;
using Logic.States.Models;
using Action = Logic.Problem.Models.Action;

namespace Logic.Queries;

public sealed class History(ProblemDefinition problem, FormulaReducer formulaReducer)
{
    private readonly Dictionary<Formula, StateGroup> _cachedReducedStates = [];
    private readonly Dictionary<IReadOnlyList<Action>, IReadOnlyList<AfterStatement>> afterDict = problem.SatisfiabilityStatements
        .OfType<AfterStatement>()
        .GroupBy(s => s.ActionChain.Actions)
        .ToDictionary(g => g.Key, g => (IReadOnlyList<AfterStatement>)g.ToList().AsReadOnly());
    private readonly Dictionary<IReadOnlyList<Action>, IReadOnlyList<ObservableStatement>> observableDict = problem.SatisfiabilityStatements
        .OfType<ObservableStatement>()
        .GroupBy(s => s.ActionChain.Actions)
        .ToDictionary(g => g.Key, g => (IReadOnlyList<ObservableStatement>)g.ToList().AsReadOnly());

    public IEnumerable<IReadOnlyList<State>> ComputeHistories(State initialState, List<Action> actions, IReadOnlyList<Action> pastActions)
    {
        IReadOnlyList<AfterStatement>? matchingAfterStatements;
        bool modelsAfter = true;
        if (afterDict.TryGetValue(pastActions, out matchingAfterStatements))
        {
            modelsAfter = matchingAfterStatements.All(s => s.Effect.IsSatisfiedBy(initialState));
        }

        // If modelsAfter is false we need to remove this whole trajectory tree

        IReadOnlyList<ObservableStatement>? matchingObservableStatements;
        HashSet<ObservableStatement> failedObservaleStatements = [];
        if (observableDict.TryGetValue(pastActions, out matchingObservableStatements))
        {
            failedObservaleStatements.UnionWith(matchingObservableStatements.Where(s => !s.Effect.IsSatisfiedBy(initialState)).ToList());
        }

        // We need union this with the intersection of all children matchingObservableStatements sets. At root if the intersection is non empty that means an observable statement failed in every branch so remove the whole trajectory tree

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

         List<Action> executedActions = [.. pastActions, firstAction];

        foreach (var endState in endStates)
        {
            foreach (var history in ComputeHistories(endState, actions[1..], executedActions))
            {
                yield return history.Prepend(initialState).ToList();
            }
        }
    }

    private StateGroup ExecuteAction(State state, Action action)
    {
        // If condition is satisfied then the action is impossible
        if (action.Conditions.Any(c => c.Condition.IsSatisfiedBy(state)))
        {
            // Action cannot be executed in this state
            return StateGroup.Empty;
        }

        var possibleEndStates = ResZero(state, action);
        var notCountedFluents = GetNotCountedForMinimalization(state, action);
        var releasedFluents = GetReleasedFluents(state, action);
        var minChangeStates = GetMinimalChangeStates(state, possibleEndStates, notCountedFluents, releasedFluents);
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

        return result;
    }

    private HashSet<Fluent> GetReleasedFluents(State state, Action action)
    {
        var result = new HashSet<Fluent>();
        var relevantReleaseStatements = action.Releases.Where(e => e.Condition.IsSatisfiedBy(state));
        result.UnionWith(relevantReleaseStatements.Select(e => e.ReleasedFluent));

        return result;
    }

    private static HashSet<Fluent> GetChangedFluents(State start, State end, HashSet<Fluent> notCountedFluents, HashSet<Fluent> releasedFluents)
    {
        var changedFluents = new HashSet<Fluent>();
        changedFluents.UnionWith(releasedFluents);
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
                changedFluents.Add(fluent.Key);
            }
        }

        return changedFluents;
    }

    private StateGroup GetMinimalChangeStates(State start, StateGroup possibleEnds, HashSet<Fluent> notCountedFluents, HashSet<Fluent> releasedFluents)
    {
        var statesWithChanges = new List<(State state, HashSet<Fluent> changedFluents)>();
        
        // First, collect all states with their corresponding changed fluent sets
        foreach (var endState in possibleEnds.EnumerateStates(problem.FluentUniverse))
        {
            var changedFluents = GetChangedFluents(start, endState, notCountedFluents, releasedFluents);
            statesWithChanges.Add((endState, changedFluents));
        }

        // Find minimal states based on set inclusion
        var minimalStates = new List<State>();

        foreach (var (candidateState, candidateChanges) in statesWithChanges)
        {
            bool isMinimal = true;

            // Check if there exists another state with a subset of changes
            foreach (var (otherState, otherChanges) in statesWithChanges)
            {
                if (candidateState.Equals(otherState))
                    continue;

                // If otherChanges is a proper subset of candidateChanges, then candidate is not minimal
                if (otherChanges.IsProperSubsetOf(candidateChanges))
                {
                    isMinimal = false;
                    break;
                }
            }

            if (isMinimal)
            {
                minimalStates.Add(candidateState);
            }
        }

        return new StateGroup(minimalStates.Select(s => s.FluentValues).ToList());
    }
}