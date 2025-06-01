using Logic.Problem;
using Logic.Problem.Models;
using Logic.States;
using Logic.States.Models;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using Xunit;

namespace Tests;

public sealed class ProblemDefinitionParserTests
{
    // Test Fluents for use across tests
    private static readonly Fluent FluentA = new Fluent("A", true);
    private static readonly Fluent FluentB = new Fluent("B", true);
    private static readonly Fluent FluentC = new Fluent("C", true);

    private readonly ProblemDefinitionParser _parser = new ProblemDefinitionParser();

    #region CreateProblemDefinition Tests

    [Fact]
    public void CreateProblemDefinition_WithBasicInputs_ReturnsProblemDefinition()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
        {
            { "A", FluentA },
            { "B", FluentB }
        };

        var actionStatements = new List<ActionStatement>
        {
            new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1)),
            new ActionStatement("move", new ActionCondition(new FluentIsSet(FluentB)))
        };

        var initials = new And(new Not(new FluentIsSet(FluentA)), new FluentIsSet(FluentB));

        var always = new List<Formula>
        {
            new True()
        };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fluents, result.Fluents);
        Assert.Equal(2, result.Fluents.Count);
        Assert.Single(result.Actions);
        Assert.Equal("move", result.Actions["move"].Name);
        Assert.Single(result.ValidStates.SpecifiedFluentGroups);
        Assert.Single(result.InitialStates.SpecifiedFluentGroups);
        Assert.Contains(result.InitialStates.SpecifiedFluentGroups, dict => dict.Count == 2 &&
            dict.ContainsKey(FluentA) && !dict[FluentA] && dict.ContainsKey(FluentB) && dict[FluentB]);
    }

    [Fact]
    public void CreateProblemDefinition_WithMultipleActionsAndEffects_ProcessesCorrectly()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
        {
            { "A", FluentA },
            { "B", FluentB },
            { "C", FluentC }
        };

        var actionStatements = new List<ActionStatement>
        {
            new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1)),
            new ActionStatement("move", new ActionCondition(new FluentIsSet(FluentB))),
            new ActionStatement("jump", new ActionEffect(new True(), new FluentIsSet(FluentC), 2)),
            new ActionStatement("jump", new ActionRelease(new FluentIsSet(FluentA), FluentB, 1))
        };

        var initials = new And(new Not(new FluentIsSet(FluentA)), new FluentIsSet(FluentB));

        var always = new List<Formula>
        {
            new True()
        };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Actions.Count);

        // Check "move" action
        Assert.Equal("move", result.Actions["move"].Name);
        Assert.Single(result.Actions["move"].Effects);
        Assert.Empty(result.Actions["move"].Releases);
        Assert.Single(result.Actions["move"].Conditions);

        // Check "jump" action
        Assert.Equal("jump", result.Actions["jump"].Name);
        Assert.Single(result.Actions["jump"].Effects);
        Assert.Single(result.Actions["jump"].Releases);
        Assert.Empty(result.Actions["jump"].Conditions);
    }

    [Fact]
    public void CreateProblemDefinition_WithConstraints_ProcessesValidStatesCorrectly()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
        {
            { "A", FluentA },
            { "B", FluentB }
        };

        var actionStatements = new List<ActionStatement>
        {
            new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1))
        };

        var initials = new True();

        // Constraint: A implies B (if A is true, B must also be true)
        var always = new List<Formula>
        {
            new Implies(new FluentIsSet(FluentA), new FluentIsSet(FluentB))
        };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);

        // Valid states should include states where either A is false or B is true
        var validStates = result.ValidStates.SpecifiedFluentGroups;
        Assert.Equal(2, validStates.Count);

        // Check that we have the state where A=false
        Assert.Contains(validStates, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == false && dict.Count == 1);

        // Check that we have the state where B=true
        Assert.Contains(validStates, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == true && dict.Count == 1);
    }

    [Fact]
    public void CreateProblemDefinition_WithInitialAndConstraints_ProcessesInitialStatesCorrectly()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
        {
            { "A", FluentA },
            { "B", FluentB }
        };

        var actionStatements = new List<ActionStatement>
        {
            new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1))
        };

        // Initial: A is true
        var initials = new FluentIsSet(FluentA);

        // Constraint: A implies B (if A is true, B must also be true)
        var always = new List<Formula>
        {
            new Implies(new FluentIsSet(FluentA), new FluentIsSet(FluentB))
        };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);

        // Initial states should only include the state where A=true and B=true
        var initialStates = result.InitialStates.SpecifiedFluentGroups;
        Assert.Single(initialStates);
        Assert.Contains(initialStates, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == true &&
            dict.ContainsKey(FluentB) && dict[FluentB] == true && dict.Count == 2);
    }

    [Fact]
    public void CreateProblemDefinition_WithMultipleConstraints_HandlesThemCorrectly()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
    {
        { "A", FluentA },
        { "B", FluentB },
        { "C", FluentC }
    };

        var actionStatements = new List<ActionStatement>
    {
        new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1))
    };

        var initials = new And(new Not(new FluentIsSet(FluentA)), new FluentIsSet(FluentB));

        // Constraints: A implies B, and B implies C
        var always = new List<Formula>
    {
        new Implies(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
        new Implies(new FluentIsSet(FluentB), new FluentIsSet(FluentC))
    };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);

        var validStates = result.ValidStates.SpecifiedFluentGroups;
        Assert.Equal(3, validStates.Count);

        // Valid states should include:
        // 1. A=false, B=false (C unspecified)
        Assert.Contains(validStates, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == false &&
            dict.ContainsKey(FluentB) && dict[FluentB] == false &&
            dict.Count == 2);

        // 2. A=false, C=true (B unspecified)
        Assert.Contains(validStates, dict =>
            dict.ContainsKey(FluentA) && dict[FluentA] == false &&
            dict.ContainsKey(FluentC) && dict[FluentC] == true &&
            dict.Count == 2);

        // 3. B=true, C=true (A unspecified)
        Assert.Contains(validStates, dict =>
            dict.ContainsKey(FluentB) && dict[FluentB] == true &&
            dict.ContainsKey(FluentC) && dict[FluentC] == true &&
            dict.Count == 2);

        // Since B=true in initial state, C must be true in the initial state
        var initialStates = result.InitialStates.SpecifiedFluentGroups;
        Assert.Single(initialStates);
        Assert.True(initialStates[0].ContainsKey(FluentA) && initialStates[0][FluentA] == false);
        Assert.True(initialStates[0].ContainsKey(FluentB) && initialStates[0][FluentB] == true);
        Assert.True(initialStates[0].ContainsKey(FluentC) && initialStates[0][FluentC] == true);
    }

    [Fact]
    public void CreateProblemDefinition_WithContradictoryConstraints_HandlesCorrectly()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
        {
            { "A", FluentA }
        };

        var actionStatements = new List<ActionStatement>
        {
            new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1))
        };

        var initials = new True();

        // Contradictory constraints: A and not A
        var always = new List<Formula>
        {
            new FluentIsSet(FluentA),
            new Not(new FluentIsSet(FluentA))
        };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);

        // Valid states should be empty (no state can satisfy the contradictory constraints)
        Assert.Empty(result.ValidStates.SpecifiedFluentGroups);

        // Initial states should also be empty
        Assert.Empty(result.InitialStates.SpecifiedFluentGroups);
    }

    [Fact]
    public void CreateProblemDefinition_IncompatibleInitialAndValidStates_ResultsInEmptyInitialStates()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
    {
        { "A", FluentA }
    };

        var actionStatements = new List<ActionStatement>
    {
        new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1))
    };

        // Initial state: A is false
        var initials = new Not(new FluentIsSet(FluentA));

        // Constraint: A must always be true
        var always = new List<Formula>
    {
        new FluentIsSet(FluentA)
    };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);

        // Valid states should only contain states where A is true
        var validStates = result.ValidStates.SpecifiedFluentGroups;
        Assert.Single(validStates);
        Assert.True(validStates[0].ContainsKey(FluentA) && validStates[0][FluentA] == true);

        // Initial states should be empty since A=false is incompatible with the constraint A=true
        Assert.Empty(result.InitialStates.SpecifiedFluentGroups);
    }

    [Fact]
    public void CreateProblemDefinition_PartiallySpecifiedInitialStates_ExtendedCorrectly()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
    {
        { "A", FluentA },
        { "B", FluentB },
        { "C", FluentC }
    };

        var actionStatements = new List<ActionStatement>
    {
        new ActionStatement("move", new ActionEffect(new True(), new FluentIsSet(FluentA), 1))
    };

        // Initial state only specifies A=true
        var initials = new FluentIsSet(FluentA);

        // Constraint: A implies B, and B implies C (if A is true, both B and C must be true)
        var always = new List<Formula>
    {
        new Implies(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
        new Implies(new FluentIsSet(FluentB), new FluentIsSet(FluentC))
    };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);

        // Initial states should contain a single state with A=true, B=true, C=true
        var initialStates = result.InitialStates.SpecifiedFluentGroups;
        Assert.Single(initialStates);
        Assert.True(initialStates[0].ContainsKey(FluentA) && initialStates[0][FluentA] == true);
        Assert.True(initialStates[0].ContainsKey(FluentB) && initialStates[0][FluentB] == true);
        Assert.True(initialStates[0].ContainsKey(FluentC) && initialStates[0][FluentC] == true);
    }

    [Fact]
    public void CreateProblemDefinition_WithComplexActionStatements_ProcessesCorrectly()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>
        {
            { "A", FluentA },
            { "B", FluentB },
            { "C", FluentC }
        };

        // Create complex action statements with conditions
        var actionStatements = new List<ActionStatement>
        {
            // "move" has effect: if B is true, set A to true with cost 1
            new ActionStatement("move", new ActionEffect(new FluentIsSet(FluentB), new FluentIsSet(FluentA), 1)),
            
            // "move" has condition: C must be true
            new ActionStatement("move", new ActionCondition(new FluentIsSet(FluentC))),
            
            // "move" has another effect: if A is false, set B to false with cost 2
            new ActionStatement("move", new ActionEffect(new Not(new FluentIsSet(FluentA)), new Not(new FluentIsSet(FluentB)), 2)),
            
            // "move" releases C if A and B are true with cost 3
            new ActionStatement("move", new ActionRelease(
                new And(new FluentIsSet(FluentA), new FluentIsSet(FluentB)),
                FluentC,
                3))
        };

        var initials = new FluentIsSet(FluentC);

        var always = new List<Formula>
        {
            new True()
        };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Actions);

        var moveAction = result.Actions["move"];
        Assert.Equal(2, moveAction.Effects.Count);
        Assert.Single(moveAction.Releases);
        Assert.Single(moveAction.Conditions);

        // Check specific action elements
        Assert.Contains(moveAction.Effects, e =>
            e.Effect == new FluentIsSet(FluentA) && e.CostIfChanged == 1);
        Assert.Contains(moveAction.Effects, e =>
            e.Effect == new Not(new FluentIsSet(FluentB)) && e.CostIfChanged == 2);
        Assert.Contains(moveAction.Releases, r =>
            r.ReleasedFluent == FluentC && r.CostIfChanged == 3);
    }

    [Fact]
    public void CreateProblemDefinition_WithEmptyInputs_ReturnsMinimalProblemDefinition()
    {
        // Arrange
        var fluents = new Dictionary<string, Fluent>();
        var actionStatements = new List<ActionStatement>();
        var initials = new True();
        var always = new List<Formula> { new True() };

        // Act
        var result = _parser.CreateProblemDefinition(fluents, actionStatements, [initials], always);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Fluents);
        Assert.Empty(result.Actions);
        Assert.Single(result.ValidStates.SpecifiedFluentGroups);
        Assert.Empty(result.ValidStates.SpecifiedFluentGroups[0]);
        Assert.Single(result.InitialStates.SpecifiedFluentGroups);
        Assert.Empty(result.InitialStates.SpecifiedFluentGroups[0]);
    }

    #endregion
}