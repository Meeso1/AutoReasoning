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
        
        var stateGroup = Reduce(not);

        // empty StateGroup represents False
        if (stateGroup.SpecifiedFluentGroups.Count == 0)
        {
            return Reduce(new True());
        }

        // negation of a statement consisting only of expressions and ors consists of negating each expression and converting every OR to AND
        // if we use CompressMergeWithStrategy with AndMergeStrategy this will deal with simplifying all the ANDs
        throw new NotImplementedException();
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

    public interface IFluentDictionaryMergeStrategy
    {
        /// <summary>
        /// Merges two fluent dictionaries according to a specific strategy
        /// </summary>
        /// <param name="left">The first dictionary to merge</param>
        /// <param name="right">The second dictionary to merge</param>
        /// <returns>A list of resulting merged dictionaries, length should be 0, 1 or 2</returns>
        public static abstract List<FluentDict> Merge(ReadOnlyFluentDict lessSpecific, ReadOnlyFluentDict moreSpecific);

        public delegate List<FluentDict> MergeDelegate(ReadOnlyFluentDict lessSpecific, ReadOnlyFluentDict moreSpecific);
    }

    public class AndMergeStrategy : IFluentDictionaryMergeStrategy
    {
        public static List<FluentDict> Merge(ReadOnlyFluentDict lessSpecific, ReadOnlyFluentDict moreSpecific)
        {
            // Case 1: Different number of keys - check if moreSpecific represents a state subset (has additional constraints) of lessSpecific
            // If moreSpecific is a subset of lessSpecific, moreSpecific already covers all states that will fullfill the logic expression
            // Case 2: Same number of keys - check if they have the same keys
            // Different keys but same count, return both
            // Case 3: Same keys - count differing values
            // Case 3a: No differences, return only one copy
            // Case 3b: Exactly one differing fluent - return empty list
            // Case 3c: Multiple differences - return both

            throw new NotImplementedException();
        }
    }

    public class OrMergeStrategy : IFluentDictionaryMergeStrategy
    {
        public static List<FluentDict> Merge(ReadOnlyFluentDict lessSpecific, ReadOnlyFluentDict moreSpecific)
        {
            // Case 1: Different number of keys - check if moreSpecific represents a state subset (has additional constraints) of lessSpecific
            if (lessSpecific.Keys.Count() != moreSpecific.Keys.Count())
            {
                bool isSubset = true;
                foreach (var kvp in lessSpecific)
                {
                    if (!moreSpecific.TryGetValue(kvp.Key, out bool value) || value != kvp.Value)
                    {
                        isSubset = false;
                        break;
                    }
                }

                if (isSubset)
                {
                    // If moreSpecific is a subset of lessSpecific, lessSpecific already covers all states that will fullfill the logic expression
                    return new List<FluentDict> { new FluentDict(lessSpecific) };
                }

                // Not a subset, return both
                return new List<FluentDict>
                {
                    new FluentDict(lessSpecific),
                    new FluentDict(moreSpecific)
                };
            }

            // Case 2: Same number of keys - check if they have the same keys
            bool sameKeys = lessSpecific.Keys.All(k => moreSpecific.ContainsKey(k));
            if (!sameKeys)
            {
                // Different keys but same count, return both
                return new List<FluentDict>
                {
                    new FluentDict(lessSpecific),
                    new FluentDict(moreSpecific)
                };
            }

            // Case 3: Same keys - count differing values
            int differentValues = 0;
            Fluent? differentFluent = null;

            foreach (var key in lessSpecific.Keys)
            {
                if (lessSpecific[key] != moreSpecific[key])
                {
                    differentValues++;
                    differentFluent = key;

                    // Early exit if we find more than one difference
                    if (differentValues > 1)
                    {
                        break;
                    }
                }
            }


            // Case 3a: No differences, return only one copy
            if (differentValues == 0) 
            {
                return new List<FluentDict>
                {
                    new FluentDict(lessSpecific)
                };
            }

            // Case 3b: Exactly one differing fluent - create intersection
            if (differentValues == 1)
            {
                var intersection = new FluentDict();
                foreach (var kvp in lessSpecific)
                {
                    if (!kvp.Key.Equals(differentFluent))
                    {
                        intersection[kvp.Key] = kvp.Value;
                    }
                }

                return new List<FluentDict> { intersection };
            }

            // Case 3c: Multiple differences - return both
            return new List<FluentDict>
            {
                new FluentDict(lessSpecific),
                new FluentDict(moreSpecific)
            };
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
    private static StateGroup PermutiationMergeWithStrategy(StateGroup leftGroup, StateGroup rightGroup, IFluentDictionaryMergeStrategy.MergeDelegate strategy)
    {
        var result = new List<ReadOnlyFluentDict>();

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
        throw new NotImplementedException();
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
