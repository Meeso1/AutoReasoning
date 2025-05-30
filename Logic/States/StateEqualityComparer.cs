using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.States.Models;

namespace Logic.States;

public class StateEqualityComparer : IEqualityComparer<State>
{
    public bool Equals(State? x, State? y)
    {
        if (x is null && y is null) return true;
        if (x is null || y is null) return false;

        if (x.FluentValues.Count != y.FluentValues.Count) return false;

        return x.FluentValues.All(kvp =>
            y.FluentValues.TryGetValue(kvp.Key, out var value) &&
            kvp.Value == value);
    }

    public int GetHashCode(State obj)
    {
        var hash = new HashCode();
        foreach (var kvp in obj.FluentValues.OrderBy(x => x.Key.Name)) // Assuming 'Name' is a stable property
        {
            hash.Add(kvp.Key);
            hash.Add(kvp.Value);
        }
        return hash.ToHashCode();
    }
}
