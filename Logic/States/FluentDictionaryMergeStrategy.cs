using Logic.States.Models;

namespace Logic.States;

using ReadOnlyFluentDict = IReadOnlyDictionary<Fluent, bool>;
using FluentDict = Dictionary<Fluent, bool>;

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
        if (lessSpecific.Count > moreSpecific.Count)
        {
            throw new ArgumentException("lessSpecific has a bigger Count than moreSpecific");
        }
        FluentDict mergedDict = new FluentDict();
        bool conditionsAreConsistent = true;


        // Check that all values found in lessSpecific have the same value in moreSpecific if they exist
        foreach (var kvp in lessSpecific)
        {
            bool keyExists = moreSpecific.TryGetValue(kvp.Key, out bool value);
            if (keyExists && value != kvp.Value)
            {
                conditionsAreConsistent = false;
                break;
            }
            mergedDict[kvp.Key] = kvp.Value;
        }

        if (!conditionsAreConsistent) { return new List<FluentDict>(); }


        // Since the two dicts are consistent we can move all of the values from the moreSpecific without worry
        foreach (var kvp in moreSpecific)
        {
            mergedDict[kvp.Key] = kvp.Value;
        }
        
        return new List<FluentDict> { mergedDict };
    }
}

public class OrMergeStrategy : IFluentDictionaryMergeStrategy
{
    public static List<FluentDict> Merge(ReadOnlyFluentDict lessSpecific, ReadOnlyFluentDict moreSpecific)
    {
        if (lessSpecific.Count > moreSpecific.Count)
        {
            throw new ArgumentException("lessSpecific has a bigger Count than moreSpecific");
        }
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