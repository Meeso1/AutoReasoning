using System.Collections.Immutable;
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
    private readonly Dictionary<ImmutableList<Action>, IReadOnlyList<AfterStatement>> afterDict = problem.ValueStatements
    .OfType<AfterStatement>()
    .GroupBy(s => s.ActionChain.Actions.ToImmutableList())
    .ToDictionary(g => g.Key, g => (IReadOnlyList<AfterStatement>)g.ToList().AsReadOnly());
    private readonly Dictionary<ImmutableList<Action>, IReadOnlyList<ObservableStatement>> observableDict = problem.ValueStatements
        .OfType<ObservableStatement>()
        .GroupBy(s => s.ActionChain.Actions.ToImmutableList())
        .ToDictionary(g => g.Key, g => (IReadOnlyList<ObservableStatement>)g.ToList().AsReadOnly());

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

    private bool HistoryIsModel(List<Action> sequence, IEnumerable<IReadOnlyList<State>> trajectories)
    {
        int actionCount = sequence.Count;

        // Get states at position actionCount from all trajectories
        var statesAtPosition = trajectories
            .Where(trajectory => trajectory.Count > actionCount)
            .Select(trajectory => trajectory[actionCount])
            .ToList();

        if (!statesAtPosition.Any())
            return true; // No valid states to check against

        ImmutableList<Action> sequenceKey = sequence.ToImmutableList();

        // Check "after" statements - must be true in EVERY state at the position
        if (afterDict.TryGetValue(sequenceKey, out var matchingAfterStatements))
        {
            bool allAfterSatisfied = matchingAfterStatements.All(afterStmt =>
                statesAtPosition.All(state => afterStmt.Effect.IsSatisfiedBy(state)));

            if (!allAfterSatisfied)
                return false;
        }

        // Check "observable" statements - must be true in AT LEAST ONE state at the position
        if (observableDict.TryGetValue(sequenceKey, out var matchingObservableStatements))
        {
            bool anyObservableSatisfied = matchingObservableStatements.All(obsStmt =>
                statesAtPosition.Any(state => obsStmt.Effect.IsSatisfiedBy(state)));

            if (!anyObservableSatisfied)
                return false;
        }

        return true;
    }

    private IEnumerable<IEnumerable<IReadOnlyList<State>>> SelectModels(ActionProgram fullProgram, IEnumerable<IEnumerable<IReadOnlyList<State>>> possibleModels)
    {
        List<Action> sequence = [];
        var candidates = possibleModels.ToList();

        for(int actionInd=-1; actionInd < fullProgram.Actions.Count; actionInd++)
        {
            if (actionInd != -1)
            {
                sequence.Add(fullProgram.Actions[actionInd]);
            }

            int removedCount = 0;

            for (int i=0; i<candidates.Count; i++) //
            {
                if (!HistoryIsModel(sequence, candidates[i]))
                {
                    candidates.RemoveAt(i);
                    removedCount++;
                    i--;
                }
            }
        }
        return candidates;
    }

    private bool CheckTrajectories(Query query, Func<IReadOnlyList<State>, bool> predicate)
    {
        var potentialHistories = problem.ValidStates.EnumerateStates(problem.FluentUniverse)
                                           .Select(start => _history.ComputeHistories(start, query.Program.Actions.ToList()));

        var histories = SelectModels(query.Program ,potentialHistories);

        if (histories.Count() == 0) { return false; } //TODO: This should be an enum and we should say the model is unconclusive

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
