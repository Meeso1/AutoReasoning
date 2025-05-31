using System.Diagnostics;

namespace Logic.States.Models;

using ReadOnlyFluentDict = IReadOnlyDictionary<Fluent, bool>;

/// <summary>
/// 	Single state specifying all fluent values
/// </summary>
/// <param name="FluentValues">Dictionary containing all fluent values</param>
public sealed record State(ReadOnlyFluentDict FluentValues) : IEquatable<State>
{
    public bool Equals(State? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        if (FluentValues.Count != other.FluentValues.Count) return false;

        return FluentValues.All(kvp =>
            other.FluentValues.TryGetValue(kvp.Key, out var value) &&
            kvp.Value == value);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var kvp in FluentValues.OrderBy(x => x.Key.Name))
        {
            hash.Add(kvp.Key);
            hash.Add(kvp.Value);
        }
        return hash.ToHashCode();
    }
}

/// <summary>
/// 	Group of states, specified by a list of possible parial fluent value specifications
/// </summary>
/// <param name="SpecifiedFluentGroups">List of dictionaries, each specifying some of the fluent values</param>
/// <remarks>
/// 	The state is contained in the group, if there exists a dictionary in the list, 
/// 	for which all fluent values that it specifies match fluent values in the state.
/// 	This implicitly represents OR between all the conditions (dictionaries).
/// 	That means that a single empty dictionary represents True, and a empty list represents False.
/// </remarks>
public sealed record StateGroup(IReadOnlyList<ReadOnlyFluentDict> SpecifiedFluentGroups)
{
    public static StateGroup Empty => new([]);

    public static StateGroup All => new([new Dictionary<Fluent, bool>()]);

    public bool Contains(State state)
    {
        return SpecifiedFluentGroups.Any(group => IsSubsetOf(group, state.FluentValues));
    }

    private static bool IsSubsetOf(
        ReadOnlyFluentDict specifiedFluents,
        ReadOnlyFluentDict state)
    {
        foreach (var key in specifiedFluents.Keys)
        {
            if (!state.ContainsKey(key))
            {
                throw new UnreachableException("State does not specify one of the fluents");
            }

            if (specifiedFluents[key] != state[key])
            {
                return false;
            }
        }

        return true;
    }

    public static StateGroup And(StateGroup group1, StateGroup group2)
    {
        return FormulaReducer.PermutationMergeWithStrategy(group1, group2, AndMergeStrategy.Merge);
    }

    public IEnumerable<State> EnumerateStates(IReadOnlyList<Fluent> fluentUniverse)
    {
        var alreadyReturned = new HashSet<State>();
        var allUnknownFluents = SpecifiedFluentGroups.SelectMany(constraintDict => constraintDict.Keys.Except(fluentUniverse))
                                                     .Distinct()
                                                     .ToList();

        if (allUnknownFluents.Count != 0)
        {
            throw new ArgumentException($"Constraints contain fluents not in universe: {allUnknownFluents.Select(f => f.Name)}");
        }

        foreach (var constraintDict in SpecifiedFluentGroups)
        {
            // Get unspecified fluents for this constraint
            var unspecifiedFluents = fluentUniverse.Except(constraintDict.Keys).ToList();

            // Generate all combinations for unspecified fluents
            int totalPermutations = 1 << unspecifiedFluents.Count;

            for (int i = 0; i < totalPermutations; i++)
            {
                // Start with a copy of the constraint dictionary
                var fluentValues = new Dictionary<Fluent, bool>(constraintDict);

                // Add permutations of unconstrained values
                for (int j = 0; j < unspecifiedFluents.Count; j++)
                {
                    bool value = (i & (1 << j)) != 0;
                    fluentValues[unspecifiedFluents[j]] = value;
                }

                var newState = new State(fluentValues);
                if (alreadyReturned.Add(newState))
                {
                    yield return newState;
                }
            }

            // Early termination if we've generated all possible states
            if (alreadyReturned.Count == 1 << fluentUniverse.Count)
            {
                yield break;
            }
        }
    }
}
