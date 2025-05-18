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

        // negation of a statement consisting only of expressions and ORs consists of negating each expression and converting every OR to AND
        // if we use CompressMergeWithStrategy with AndMergeStrategy this will deal with simplifying all the ANDs

        // Create a list to hold all the negated fluent dictionaries
        List<ReadOnlyFluentDict> negatedGroups = [];

        foreach (var fluentDict in stateGroup.SpecifiedFluentGroups)
        {
            // Create a negated version of the current fluent dictionary
            FluentDict negatedDict = [];

            foreach (var kvp in fluentDict)
            {
                // Negate each value in the dictionary
                negatedDict[kvp.Key] = !kvp.Value;
            }

            negatedGroups.Add(negatedDict);
        }

        // If there's only one negated group, return it directly
        if (negatedGroups.Count == 1)
        {
            return new StateGroup(negatedGroups);
        }

        // Create two StateGroups to merge with CompressMergeWithStrategy
        StateGroup leftGroup = new(new List<ReadOnlyFluentDict> { negatedGroups[0] });
        StateGroup rightGroup = new(negatedGroups.Skip(1).ToList());

        // Use CompressMergeWithStrategy with AndMergeStrategy to merge all negated dictionaries
        return CompressMergeWithStrategy(leftGroup, rightGroup, AndMergeStrategy.Merge);
    }

    private StateGroup ReduceAnd(And and)
    {
        var leftGroup = Reduce(and.First);
        var rightGroup = Reduce(and.Second);

        return PermutiationMergeWithStrategy(leftGroup, rightGroup, AndMergeStrategy.Merge);
    }

    private StateGroup ReduceOr(Or or)
    {
        var leftGroup = Reduce(or.First);
        var rightGroup = Reduce(or.Second);

        return CompressMergeWithStrategy(leftGroup, rightGroup, OrMergeStrategy.Merge);
        
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
    private static StateGroup PermutiationMergeWithStrategy(StateGroup leftGroup, StateGroup rightGroup, IFluentDictionaryMergeStrategy.MergeDelegate strategy)
    {
        List<ReadOnlyFluentDict> result = [];

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

                result.AddRange(resolved);
            }
        }
        return new StateGroup(result);
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
    private static StateGroup CompressMergeWithStrategy(StateGroup leftGroup, StateGroup rightGroup, IFluentDictionaryMergeStrategy.MergeDelegate strategy)
    {
        // Combine all fluent dictionaries from both groups into a single working set
        List<ReadOnlyFluentDict> workingSet = [];
        workingSet.AddRange(leftGroup.SpecifiedFluentGroups);
        workingSet.AddRange(rightGroup.SpecifiedFluentGroups);

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
                        workingSet[j] = null;
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
            workingSet = mergeResults.Where(dict => dict != null).ToList();

        } while (changesMade);

        return new StateGroup(workingSet);
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
