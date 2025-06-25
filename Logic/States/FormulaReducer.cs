using System.Collections.Generic;
using Logic.States.Models;

namespace Logic.States;

using ReadOnlyFluentDict = IReadOnlyDictionary<Fluent, bool>;
using FluentDict = Dictionary<Fluent, bool>;
/// <summary>
/// 	Class that reduces logic formulas that specify a group of states 
/// 	into a <see cref="StateGroup"/> that satisfies it
/// </summary>
public sealed class FormulaReducer
{
    /// <summary>
    /// 	Reduces a formula to its simplest form
    /// </summary>
    /// <param name="validStates">
    ///     A StateGroup of all valid states in the whole problem
    /// </param>
    /// <param name="formula">
    /// 	Formula to reduce
    /// </param>
    /// <returns>
    /// 	<see cref="StateGroup"/> that represents all states that satisfy the formula
    /// </returns>
    public StateGroup Reduce(Formula formula)
    {
        return formula switch
        {
            True => new StateGroup(new List<ReadOnlyFluentDict>()
            {
                new Dictionary<Fluent, bool>()
            }),

            False => new StateGroup(new List<ReadOnlyFluentDict>()),

            FluentIsSet f => new StateGroup(new List<ReadOnlyFluentDict>
            {
                new Dictionary<Fluent, bool> { [f.Fluent] = true }
            }),

            Not n => ReduceNot(n),

            And a => ReduceAnd(a),

            Or o => ReduceOr(o),

            Implies i => ReduceImplies(i),

            Equivalent e => ReduceEquivalent(e),

            _ => throw new ArgumentException($"Unknown formula type: {formula.GetType().Name}")
        };
    }

    private StateGroup ReduceNot(Not not)
    {
        var stateGroup = Reduce(not.Formula);

        // empty StateGroup represents False
        if (stateGroup.SpecifiedFluentGroups.Count == 0)
        {
            return Reduce(new True());
        }
        // StateGroup with single empty dict represents True
        if (stateGroup.SpecifiedFluentGroups.Count == 1 && stateGroup.SpecifiedFluentGroups[0].Count == 0)
        {
            return Reduce(new False());
        }


        // Apply De Morgan's law: not(A or B or C) = not A and not B and not C
        // Each fluent dictionary in the original StateGroup represents a disjunct
        // We need to negate each disjunct and then AND them all together

        List<StateGroup> negatedGroups = [];

        foreach (var fluentDict in stateGroup.SpecifiedFluentGroups)
        {
            // Negate this disjunct by creating an OR of all negated fluents
            List<ReadOnlyFluentDict> negatedFluents = [];

            foreach (var kvp in fluentDict)
            {
                // Create a state where only this fluent is negated
                negatedFluents.Add(new FluentDict { [kvp.Key] = !kvp.Value });
            }

            // If the original disjunct was empty (representing True), its negation is False
            if (fluentDict.Count == 0)
            {
                negatedGroups.Add(Reduce(new False()));
            }
            else
            {
                negatedGroups.Add(new StateGroup(negatedFluents));
            }
        }

        // AND all the negated disjuncts together
        if (negatedGroups.Count == 0)
        {
            return Reduce(new False());
        }

        StateGroup result = negatedGroups[0];
        for (int i = 1; i < negatedGroups.Count; i++)
        {
            result = PermutationMergeWithStrategy(result, negatedGroups[i], AndMergeStrategy.Merge);
        }
        return result;
    }

    private StateGroup ReduceAnd(And and)
    {
        var leftGroup = Reduce(and.First);
        var rightGroup = Reduce(and.Second);

        return PermutationMergeWithStrategy(leftGroup, rightGroup, AndMergeStrategy.Merge);
    }

    private StateGroup ReduceOr(Or or)
    {
        var leftGroup = Reduce(or.First);
        var rightGroup = Reduce(or.Second);

        return CompressMergeWithStrategy(leftGroup, rightGroup, OrMergeStrategy.Merge);
    }

    public StateGroup CompressStateGroup(StateGroup stateGroup)
    {
        return CompressMergeWithStrategy(stateGroup, stateGroup, OrMergeStrategy.Merge);
    }

    public class FluentDictComparer : IEqualityComparer<ReadOnlyFluentDict>
    {
        public bool Equals(ReadOnlyFluentDict? x, ReadOnlyFluentDict? y)
        {
            if (x == null || y == null) return false;
            if (x == y) return true;
            if (x.Count != y.Count) return false;

            foreach (var kvp in x)
            {
                if (!y.TryGetValue(kvp.Key, out var value) || value != kvp.Value)
                    return false;
            }
            return true;
        }

        public int GetHashCode(ReadOnlyFluentDict obj)
        {
            if (obj == null) return 0;

            int hash = 0;
            foreach (var kvp in obj.OrderBy(x => x.Key.Name))
            {
                hash ^= kvp.Key.GetHashCode() ^ kvp.Value.GetHashCode();
            }
            return hash;
        }
    }

    /// <summary>
    /// Merges two <see cref="StateGroup"/> by merging each FluentDict on the left with each FluentDict on the right. 
    /// Important for when we have (... OR ... ) AND ( ... OR ... )
    /// </summary>
    /// <param name="leftGroup">StateGroup representing left side of an operator</param>
    /// <param name="rightGroup">StateGroup representing left side of an operator</param>
    /// <param name="strategy">A function governing how two FluentDicts should be merged</param>
    /// <returns>
    /// A smallest possible StateGroup representing the merge of the input StateGroups
    /// </returns>
    public static StateGroup PermutationMergeWithStrategy(StateGroup leftGroup, StateGroup rightGroup, IFluentDictionaryMergeStrategy.MergeDelegate strategy)
    {
        HashSet<ReadOnlyFluentDict> result = new(new FluentDictComparer());

        for (int leftIdx = 0; leftIdx < leftGroup.SpecifiedFluentGroups.Count; leftIdx++)
        {
            var leftFluentGroup = leftGroup.SpecifiedFluentGroups[leftIdx];
            for (int rightIdx = 0; rightIdx < rightGroup.SpecifiedFluentGroups.Count; rightIdx++)
            {
                var rightFluentGroup = rightGroup.SpecifiedFluentGroups[rightIdx];

                ReadOnlyFluentDict lessSpecificGroup;
                ReadOnlyFluentDict moreSpecificGroup;
                if (leftFluentGroup.Keys.Count() <= rightFluentGroup.Keys.Count())
                {
                    lessSpecificGroup = leftFluentGroup;
                    moreSpecificGroup = rightFluentGroup;
                }
                else
                {
                    lessSpecificGroup = rightFluentGroup;
                    moreSpecificGroup = leftFluentGroup;
                }

                var resolved = strategy(lessSpecificGroup, moreSpecificGroup);

                result.UnionWith(resolved);
            }
        }
        return new StateGroup(result.ToList());
    }

    /// <summary>
    /// Merges two <see cref="StateGroup"/> by merging all fluent dicts repeatedly until no new changes occur
    /// </summary>
    /// <param name="leftGroup">StateGroup representing left side of an operator</param>
    /// <param name="rightGroup">StateGroup representing left side of an operator</param>
    /// <param name="strategy">A function governing how two FluentDicts should be merged</param>
    /// <returns>
    /// A smallest possible StateGroup representing the merge of the input StateGroups
    /// </returns>
    public static StateGroup CompressMergeWithStrategy(StateGroup leftGroup, StateGroup rightGroup, IFluentDictionaryMergeStrategy.MergeDelegate strategy)
    {
        // Combine all fluent dictionaries from both groups into a single working set
        var comparer = new FluentDictComparer();
        List<ReadOnlyFluentDict> workingSet = [];

        // Add items while avoiding duplicates
        foreach (var dict in leftGroup.SpecifiedFluentGroups.Concat(rightGroup.SpecifiedFluentGroups))
        {
            if (!workingSet.Any(existing => comparer.Equals(existing, dict)))
            {
                workingSet.Add(dict);
            }
        }

        // Continue merging until no new changes occur
        bool changesMade;
        do
        {
            changesMade = false;
            List<ReadOnlyFluentDict> mergeResults = [];

            // Process each dictionary in the working set
            for (int i = 0; i < workingSet.Count; i++)
            {
                bool merged = false;

                // Try to merge current dictionary with each subsequent dictionary
                for (int j = i + 1; j < workingSet.Count; j++)
                {
                    ReadOnlyFluentDict lessSpecificGroup;
                    ReadOnlyFluentDict moreSpecificGroup;

                    // Determine which dictionary is less specific
                    if (workingSet[i].Keys.Count() <= workingSet[j].Keys.Count())
                    {
                        lessSpecificGroup = workingSet[i];
                        moreSpecificGroup = workingSet[j];
                    }
                    else
                    {
                        lessSpecificGroup = workingSet[j];
                        moreSpecificGroup = workingSet[i];
                    }

                    // Apply the merge strategy
                    var resolved = strategy(lessSpecificGroup, moreSpecificGroup);

                    // If the merge produced a result different from the inputs, we've made progress
                    if (resolved.Count != 2 ||
                        (resolved.Count == 2 &&
                         (!DictionariesEqual(resolved[0], lessSpecificGroup) ||
                          !DictionariesEqual(resolved[1], moreSpecificGroup))))
                    {
                        // Mark dictionaries as merged
                        merged = true;
                        changesMade = true;

                        // Add merge results to our results list
                        mergeResults.AddRange(resolved);

                        // Mark the second dictionary as merged by removing it from consideration
                        workingSet.RemoveAt(j);
                        j--;
                        break;
                    }
                }

                // If this dictionary wasn't merged with anything, keep it as is
                if (!merged)
                {
                    mergeResults.Add(workingSet[i]);
                }
            }

            // Update working set for next iteration, removing any null entries
            workingSet = mergeResults;

        } while (changesMade);

        HashSet<ReadOnlyFluentDict> result = new(new FluentDictComparer());
        result.UnionWith(workingSet);

        return new StateGroup(result.ToList());
    }

    /// <summary>
    /// Helper method to check if two ReadOnlyFluentDict instances are equal
    /// </summary>
    private static bool DictionariesEqual(ReadOnlyFluentDict dict1, ReadOnlyFluentDict dict2)
    {
        if (dict1 == dict2) return true;
        if (dict1 == null || dict2 == null) return false;
        if (dict1.Keys.Count() != dict2.Keys.Count()) return false;

        foreach (var key in dict1.Keys)
        {
            if (!dict2.ContainsKey(key) || dict1[key] != dict2[key])
                return false;
        }

        return true;
    }

    private StateGroup ReduceImplies(Implies implies)
    {
        return Reduce(new Or(
            new Not(implies.Prior),
            implies.Posterior
            ));
    }

    private StateGroup ReduceEquivalent(Equivalent equivalent)
    {
        return Reduce(new And(
            new Implies(equivalent.First, equivalent.Second),
            new Implies(equivalent.Second, equivalent.First)
            ));
    }
}
