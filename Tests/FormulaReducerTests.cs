using Logic.States;
using Logic.States.Models;

namespace Tests;

public sealed class FormulaReducerTests
{
    // Test Fluents for use across tests
    private static readonly Fluent FluentA = new Fluent("A", true);
    private static readonly Fluent FluentB = new Fluent("B", true);
    private static readonly Fluent FluentC = new Fluent("C", true);
    private static readonly Fluent FluentD = new Fluent("D", true);

    private readonly FormulaReducer _reducer = new FormulaReducer();

    #region Basic Formula Tests

    [Fact]
    public void Reduce_TrueFormula_ReturnsStateGroupWithEmptyDictionary()
    {
        // Arrange
        var formula = new True();

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Empty(result.SpecifiedFluentGroups[0]);
    }

    [Fact]
    public void Reduce_FalseFormula_ReturnsEmptyStateGroup()
    {
        // Arrange
        var formula = new False();

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Empty(result.SpecifiedFluentGroups);
    }

    [Fact]
    public void Reduce_FluentIsSetFormula_ReturnsStateGroupWithFluentSetToTrue()
    {
        // Arrange
        var formula = new FluentIsSet(FluentA);

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
    }

    #endregion

    #region Not Formula Tests

    [Fact]
    public void Reduce_NotTrue_ReturnsFalse()
    {
        // Arrange
        var formula = new Not(new True());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Empty(result.SpecifiedFluentGroups);
    }

    [Fact]
    public void Reduce_NotFalse_ReturnsTrue()
    {
        // Arrange
        var formula = new Not(new False());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Empty(result.SpecifiedFluentGroups[0]);
    }

    [Fact]
    public void Reduce_NotFluentIsSet_ReturnsFluentSetToFalse()
    {
        // Arrange
        var formula = new Not(new FluentIsSet(FluentA));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.False(result.SpecifiedFluentGroups[0][FluentA]);
    }

    [Fact]
    public void Reduce_NotOrFormula_ReturnsAndOfNegatedParts()
    {
        // Arrange
        var formula = new Not(new Or(new FluentIsSet(FluentA), new FluentIsSet(FluentB)));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Equal(2, result.SpecifiedFluentGroups[0].Count);
        Assert.False(result.SpecifiedFluentGroups[0][FluentA]);
        Assert.False(result.SpecifiedFluentGroups[0][FluentB]);
    }

    [Fact]
    public void Reduce_NotAndFormula_ReturnsOrOfNegatedParts()
    {
        // Arrange
        var formula = new Not(new And(new FluentIsSet(FluentA), new FluentIsSet(FluentB)));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(2, result.SpecifiedFluentGroups.Count);
        // Should contain either A=false or B=false
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == false && dict.Count == 1);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == false && dict.Count == 1);
    }

    #endregion

    #region And Formula Tests

    [Fact]
    public void Reduce_AndWithTwoFluents_ReturnsCombinedState()
    {
        // Arrange
        var formula = new And(new FluentIsSet(FluentA), new FluentIsSet(FluentB));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Equal(2, result.SpecifiedFluentGroups[0].Count);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentB]);
    }

    [Fact]
    public void Reduce_AndWithFalse_ReturnsEmptyStateGroup()
    {
        // Arrange
        var formula = new And(new FluentIsSet(FluentA), new False());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Empty(result.SpecifiedFluentGroups);
    }

    [Fact]
    public void Reduce_AndWithTrue_ReturnsOtherOperand()
    {
        // Arrange
        var formula = new And(new FluentIsSet(FluentA), new True());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
    }

    [Fact]
    public void Reduce_AndWithConflictingFluents_ReturnsEmptyStateGroup()
    {
        // Arrange
        var formula = new And(
            new FluentIsSet(FluentA),
            new Not(new FluentIsSet(FluentA))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Empty(result.SpecifiedFluentGroups);
    }

    [Fact]
    public void Reduce_ComplexAndFormula_ReturnsCorrectCombination()
    {
        // Arrange - (A AND B) AND (A AND C) should result in A=true, B=true, C=true
        var formula = new And(
            new And(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
            new And(new FluentIsSet(FluentA), new FluentIsSet(FluentC))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Equal(3, result.SpecifiedFluentGroups[0].Count);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentB]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentC]);
    }

    #endregion

    #region Or Formula Tests

    [Fact]
    public void Reduce_OrWithTwoFluents_ReturnsTwoSeparateStates()
    {
        // Arrange
        var formula = new Or(new FluentIsSet(FluentA), new FluentIsSet(FluentB));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(2, result.SpecifiedFluentGroups.Count);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == true && dict.Count == 1);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == true && dict.Count == 1);
    }

    [Fact]
    public void Reduce_OrWithFalse_ReturnsOtherOperand()
    {
        // Arrange
        var formula = new Or(new FluentIsSet(FluentA), new False());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
    }

    [Fact]
    public void Reduce_OrWithTrue_ReturnsTrue()
    {
        // Arrange
        var formula = new Or(new FluentIsSet(FluentA), new True());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Empty(result.SpecifiedFluentGroups[0]);
    }

    [Fact]
    public void Reduce_OrWithSameFluent_ReturnsSimplifiedResult()
    {
        // Arrange - A OR A should simplify to just A
        var formula = new Or(new FluentIsSet(FluentA), new FluentIsSet(FluentA));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
    }

    [Fact]
    public void Reduce_ComplexOrFormula_ReturnsCorrectCombination()
    {
        // Arrange - (A OR B) OR (C OR D) should result in four possible states
        var formula = new Or(
            new Or(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
            new Or(new FluentIsSet(FluentC), new FluentIsSet(FluentD))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(4, result.SpecifiedFluentGroups.Count);
        Assert.Contains(result.SpecifiedFluentGroups, dict => dict.ContainsKey(FluentA) && dict.Count == 1);
        Assert.Contains(result.SpecifiedFluentGroups, dict => dict.ContainsKey(FluentB) && dict.Count == 1);
        Assert.Contains(result.SpecifiedFluentGroups, dict => dict.ContainsKey(FluentC) && dict.Count == 1);
        Assert.Contains(result.SpecifiedFluentGroups, dict => dict.ContainsKey(FluentD) && dict.Count == 1);
    }

    #endregion

    #region Implies Formula Tests

    [Fact]
    public void Reduce_ImpliesFormula_ReturnsEquivalentOrFormula()
    {
        // Arrange - A implies B is equivalent to (NOT A) OR B
        var formula = new Implies(new FluentIsSet(FluentA), new FluentIsSet(FluentB));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(2, result.SpecifiedFluentGroups.Count);
        // Should contain either A=false or B=true (or both)
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == false && dict.Count == 1);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == true && dict.Count == 1);
    }

    [Fact]
    public void Reduce_ImpliesTrueFormula_ReturnsTrue()
    {
        // Arrange - A implies True is always True
        var formula = new Implies(new FluentIsSet(FluentA), new True());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Empty(result.SpecifiedFluentGroups[0]);
    }

    [Fact]
    public void Reduce_TrueImpliesFormula_ReturnsConsequent()
    {
        // Arrange - True implies B is equivalent to B
        var formula = new Implies(new True(), new FluentIsSet(FluentB));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentB]);
    }

    [Fact]
    public void Reduce_FalseImpliesFormula_ReturnsTrue()
    {
        // Arrange - False implies anything is always True
        var formula = new Implies(new False(), new FluentIsSet(FluentB));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Empty(result.SpecifiedFluentGroups[0]);
    }

    #endregion

    #region Equivalent Formula Tests

    [Fact]
    public void Reduce_EquivalentFormula_ReturnsCorrectStates()
    {
        // Arrange - A equivalent to B means (A implies B) AND (B implies A)
        var formula = new Equivalent(new FluentIsSet(FluentA), new FluentIsSet(FluentB));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(2, result.SpecifiedFluentGroups.Count);
        // Should contain either both true or both false
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict.ContainsKey(FluentB) &&
            dict[FluentA] == true && dict[FluentB] == true);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict.ContainsKey(FluentB) &&
            dict[FluentA] == false && dict[FluentB] == false);
    }

    [Fact]
    public void Reduce_EquivalentWithTrue_ReturnsOtherOperand()
    {
        // Arrange - A equivalent to True means A must be true
        var formula = new Equivalent(new FluentIsSet(FluentA), new True());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
    }

    [Fact]
    public void Reduce_EquivalentWithFalse_ReturnsNegatedOperand()
    {
        // Arrange - A equivalent to False means A must be false
        var formula = new Equivalent(new FluentIsSet(FluentA), new False());

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.False(result.SpecifiedFluentGroups[0][FluentA]);
    }

    #endregion

    #region Complex Formula Tests

    [Fact]
    public void Reduce_ComplexFormula_AndOfOrs_ReturnsCorrectResult()
    {
        // Arrange - (A OR B) AND (C OR D) should result in 4 possible combinations
        var formula = new And(
            new Or(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
            new Or(new FluentIsSet(FluentC), new FluentIsSet(FluentD))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(4, result.SpecifiedFluentGroups.Count);

        // Should contain all combinations: AC, AD, BC, BD
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == true &&
            dict.ContainsKey(FluentC) && dict[FluentC] == true);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == true &&
            dict.ContainsKey(FluentD) && dict[FluentD] == true);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == true &&
            dict.ContainsKey(FluentC) && dict[FluentC] == true);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == true &&
            dict.ContainsKey(FluentD) && dict[FluentD] == true);
    }

    [Fact]
    public void Reduce_ComplexFormula_OrOfAnds_ReturnsCorrectResult()
    {
        // Arrange - (A AND B) OR (C AND D) should result in 2 possible states
        var formula = new Or(
            new And(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
            new And(new FluentIsSet(FluentC), new FluentIsSet(FluentD))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(2, result.SpecifiedFluentGroups.Count);

        // Should contain AB and CD
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == true &&
            dict.ContainsKey(FluentB) && dict[FluentB] == true &&
            dict.Count == 2);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentC) && dict[FluentC] == true &&
            dict.ContainsKey(FluentD) && dict[FluentD] == true &&
            dict.Count == 2);
    }

    [Fact]
    public void Reduce_DeepNestingFormula_ReturnsCorrectResult()
    {
        // Arrange - ((A AND B) OR C) AND (D OR (NOT A))
        var formula = new And(
            new Or(
                new And(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
                new FluentIsSet(FluentC)
            ),
            new Or(
                new FluentIsSet(FluentD),
                new Not(new FluentIsSet(FluentA))
            )
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Equal(3, result.SpecifiedFluentGroups.Count);

        // Should contain: A=true, B=true, D=true
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == true &&
            dict.ContainsKey(FluentB) && dict[FluentB] == true &&
            dict.ContainsKey(FluentD) && dict[FluentD] == true &&
            dict.Count == 3);

        // Should contain: C=true, D=true
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentC) && dict[FluentC] == true &&
            dict.ContainsKey(FluentD) && dict[FluentD] == true &&
            dict.Count == 2);

        // Should contain: C=true, A=false
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentC) && dict[FluentC] == true &&
            dict.ContainsKey(FluentA) && dict[FluentA] == false &&
            dict.Count == 2);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Reduce_NestedNotFormulas_ReturnsSimplifiedResult()
    {
        // Arrange - NOT(NOT(A)) should simplify to A
        var formula = new Not(new Not(new FluentIsSet(FluentA)));

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Single(result.SpecifiedFluentGroups[0]);
        Assert.True(result.SpecifiedFluentGroups[0][FluentA]);
    }

    [Fact]
    public void Reduce_TautologyFormula_ReturnsTrue()
    {
        // Arrange - A OR (NOT A) is always true
        var formula = new Or(
            new FluentIsSet(FluentA),
            new Not(new FluentIsSet(FluentA))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Single(result.SpecifiedFluentGroups);
        Assert.Empty(result.SpecifiedFluentGroups[0]);
    }

    [Fact]
    public void Reduce_ContradictionFormula_ReturnsFalse()
    {
        // Arrange - A AND (NOT A) is always false
        var formula = new And(
            new FluentIsSet(FluentA),
            new Not(new FluentIsSet(FluentA))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.Empty(result.SpecifiedFluentGroups);
    }

    [Fact]
    public void Reduce_SameFluentsInDifferentSubformulas_HandlesCorrectly()
    {
        // Arrange - (A OR B) AND (A OR C) should optimize correctly
        var formula = new And(
            new Or(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
            new Or(new FluentIsSet(FluentA), new FluentIsSet(FluentC))
        );

        // Act
        var result = _reducer.Reduce(formula);

        // Assert
        Assert.NotEmpty(result.SpecifiedFluentGroups);
        Assert.All(result.SpecifiedFluentGroups, dict => Assert.NotEmpty(dict));

        // Should contain A=true as one possibility, and combinations of B,C as others
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == true);
        Assert.Contains(result.SpecifiedFluentGroups, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == true &&
            dict.ContainsKey(FluentC) && dict[FluentC] == true);
    }

    #endregion
}