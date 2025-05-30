using Logic.States.Models;

namespace Tests;

public class StateGroupTests
{

    private readonly Fluent FluentA = new("A", false);
    private readonly Fluent FluentB = new("B", false);
    private readonly Fluent FluentC = new("C", false);

    #region GenerateAllStatesInGroupTests
    [Fact]
    public void GenerateAllStatesInGroup_EmptySpecifiedFluentGroups_ReturnsEmpty()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB };
        var stateGroup = new StateGroup(new List<IReadOnlyDictionary<Fluent, bool>>());

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GenerateAllStatesInGroup_SingleEmptyConstraint_ReturnsAllPossibleStates()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB };
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
    {
        new Dictionary<Fluent, bool>() // Empty constraint = no restrictions
    };
        var stateGroup = new StateGroup(constraints);

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse);

        // Assert
        Assert.Equal(4, result.Count()); // 2^2 = 4 possible states

        // Check that we have all 4 possible combinations
        Assert.Contains(result, state => state.FluentValues[FluentA] == false && state.FluentValues[FluentB] == false);
        Assert.Contains(result, state => state.FluentValues[FluentA] == true && state.FluentValues[FluentB] == false);
        Assert.Contains(result, state => state.FluentValues[FluentA] == false && state.FluentValues[FluentB] == true);
        Assert.Contains(result, state => state.FluentValues[FluentA] == true && state.FluentValues[FluentB] == true);
    }

    [Fact]
    public void GenerateAllStatesInGroup_SingleConstraintOneFluentFixed_ReturnsCorrectStates()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB };
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool> { {FluentA, true} } // A must be true
            };
        var stateGroup = new StateGroup(constraints);

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse);

        // Assert
        Assert.Equal(2, result.Count()); // 2^1 = 2 states (B can be true/false)

        // All states should have A = true
        Assert.All(result, state => Assert.True(state.FluentValues[FluentA]));

        // Should contain both B = true and B = false
        Assert.Contains(result, state => state.FluentValues[FluentB] == true);
        Assert.Contains(result, state => state.FluentValues[FluentB] == false);
    }

    [Fact]
    public void GenerateAllStatesInGroup_SingleConstraintAllFluentsFixed_ReturnsSingleState()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB };
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool> { {FluentA, true}, {FluentB, false} }
            };
        var stateGroup = new StateGroup(constraints);

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse).ToList();

        // Assert
        Assert.Single(result);
        Assert.True(result[0].FluentValues[FluentA]);
        Assert.False(result[0].FluentValues[FluentB]);
    }

    [Fact]
    public void GenerateAllStatesInGroup_MultipleConstraints_ReturnsUnionOfStates()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB };
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool> { {FluentA, true} },  // A = true, B can be anything
                new Dictionary<Fluent, bool> { {FluentB, true} }   // B = true, A can be anything
            };
        var stateGroup = new StateGroup(constraints);

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse).ToList();

        // Assert
        Assert.Equal(3, result.Count); // Should get: (T,T), (T,F), (F,T)

        // Check that we have the expected states
        Assert.Contains(result, state => state.FluentValues[FluentA] == true && state.FluentValues[FluentB] == true);
        Assert.Contains(result, state => state.FluentValues[FluentA] == true && state.FluentValues[FluentB] == false);
        Assert.Contains(result, state => state.FluentValues[FluentA] == false && state.FluentValues[FluentB] == true);

        // Should NOT contain (F,F)
        Assert.DoesNotContain(result, state => state.FluentValues[FluentA] == false && state.FluentValues[FluentB] == false);
    }

    [Fact]
    public void GenerateAllStatesInGroup_OverlappingConstraints_NoDuplicates()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB };
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool> { {FluentA, true}, {FluentB, true} },
                new Dictionary<Fluent, bool> { {FluentA, true}, {FluentB, true} } // Same constraint twice
            };
        var stateGroup = new StateGroup(constraints);

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse).ToList();

        // Assert
        Assert.Single(result); // Should only have one unique state
        Assert.True(result[0].FluentValues[FluentA]);
        Assert.True(result[0].FluentValues[FluentB]);
    }

    [Fact]
    public void GenerateAllStatesInGroup_ThreeFluents_CorrectCombinations()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB, FluentC };
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool> { {FluentA, true} } // A = true, B and C can be anything
            };
        var stateGroup = new StateGroup(constraints);

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse).ToList();

        // Assert
        Assert.Equal(4, result.Count); // 2^2 = 4 (B and C can each be true/false)

        // All states should have A = true
        Assert.All(result, state => Assert.True(state.FluentValues[FluentA]));

        // Should have all combinations of B and C
        Assert.Contains(result, state => state.FluentValues[FluentB] == false && state.FluentValues[FluentC] == false);
        Assert.Contains(result, state => state.FluentValues[FluentB] == true && state.FluentValues[FluentC] == false);
        Assert.Contains(result, state => state.FluentValues[FluentB] == false && state.FluentValues[FluentC] == true);
        Assert.Contains(result, state => state.FluentValues[FluentB] == true && state.FluentValues[FluentC] == true);
    }

    [Fact]
    public void GenerateAllStatesInGroup_ConstraintWithFluentNotInUniverse_ThrowsException()
    {
        // Arrange
        var fluentUniverse = new[] { FluentA, FluentB };
        var fluentD = new Fluent("D", false);
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool> { {fluentD, true} } // D is not in universe
            };
        var stateGroup = new StateGroup(constraints);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => stateGroup.EnumerateStates(fluentUniverse).ToList());
    }

    [Fact]
    public void GenerateAllStatesInGroup_EmptyFluentUniverse_ReturnsEmpty()
    {
        // Arrange
        var fluentUniverse = Array.Empty<Fluent>();
        var constraints = new List<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool>() // Empty constraint
            };
        var stateGroup = new StateGroup(constraints);

        // Act
        var result = stateGroup.EnumerateStates(fluentUniverse).ToList();

        // Assert
        Assert.Single(result); // One state with no fluents
        Assert.Empty(result[0].FluentValues);
    }
    #endregion GenerateAllStatesInGroupTests
}