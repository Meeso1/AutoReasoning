using Logic.States;
using Logic.States.Models;

namespace Tests;

public sealed class StateEqualityTests
{
    private readonly Fluent FluentA = new("A", false);
    private readonly Fluent FluentB = new("B", false);
    private readonly Fluent FluentC = new("C", false);

    [Fact]
    public void Equals_BothNull_ReturnsTrue()
    {
        State? state1 = null;
        State? state2 = null;

        Assert.True(state1 == state2);
    }

    [Fact]
    public void Equals_OneNull_ReturnsFalse()
    {
        var state = new State(new Dictionary<Fluent, bool> { { FluentA, true } });

        Assert.False(state.Equals(null));
        Assert.False(state == null);
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        var state = new State(new Dictionary<Fluent, bool> { { FluentA, true } });

        Assert.True(state.Equals(state));
    }

    [Fact]
    public void Equals_SameFluentValues_ReturnsTrue()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });

        Assert.True(state1.Equals(state2));
    }

    [Fact]
    public void Equals_SameFluentValuesDifferentOrder_ReturnsTrue()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentB, false }, { FluentA, true } });

        Assert.True(state1.Equals(state2));
    }

    [Fact]
    public void Equals_DifferentFluentValues_ReturnsFalse()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, true } });

        Assert.False(state1.Equals(state2));
    }

    [Fact]
    public void Equals_DifferentFluentKeys_ReturnsFalse()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentB, true } });

        Assert.False(state1.Equals(state2));
    }

    [Fact]
    public void Equals_DifferentNumberOfFluents_ReturnsFalse()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });

        Assert.False(state1.Equals(state2));
    }

    [Fact]
    public void Equals_EmptyStates_ReturnsTrue()
    {
        var state1 = new State(new Dictionary<Fluent, bool>());
        var state2 = new State(new Dictionary<Fluent, bool>());

        Assert.True(state1.Equals(state2));
    }

    [Fact]
    public void GetHashCode_SameFluentValues_ReturnsSameHashCode()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });

        var hash1 = state1.GetHashCode();
        var hash2 = state2.GetHashCode();

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_SameFluentValuesDifferentOrder_ReturnsSameHashCode()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentB, false }, { FluentA, true } });

        var hash1 = state1.GetHashCode();
        var hash2 = state2.GetHashCode();

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_DifferentFluentValues_ReturnsDifferentHashCodes()
    {
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, true } });

        var hash1 = state1.GetHashCode();
        var hash2 = state2.GetHashCode();

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_EmptyState_ReturnsConsistentHashCode()
    {
        var state1 = new State(new Dictionary<Fluent, bool>());
        var state2 = new State(new Dictionary<Fluent, bool>());

        var hash1 = state1.GetHashCode();
        var hash2 = state2.GetHashCode();

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void HashSet_CorrectlyDeduplicatesStates()
    {
        var hashSet = new HashSet<State>();
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state3 = new State(new Dictionary<Fluent, bool> { { FluentB, false }, { FluentA, true } }); // Same content, different order

        hashSet.Add(state1);
        hashSet.Add(state2);
        hashSet.Add(state3);

        Assert.Single(hashSet); // Should only contain one unique state
    }

    [Fact]
    public void HashSet_KeepsDifferentStates()
    {
        var hashSet = new HashSet<State>();
        var state1 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, false } });
        var state2 = new State(new Dictionary<Fluent, bool> { { FluentA, false }, { FluentB, true } });
        var state3 = new State(new Dictionary<Fluent, bool> { { FluentA, true }, { FluentB, true } });

        hashSet.Add(state1);
        hashSet.Add(state2);
        hashSet.Add(state3);

        Assert.Equal(3, hashSet.Count); // Should contain all three different states
    }
}