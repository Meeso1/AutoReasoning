using Logic.Problem.Models;
using Logic.Queries;
using Logic.Queries.Models;
using Logic.States;
using Logic.States.Models;
using Action = Logic.Problem.Models.Action;

namespace Tests;

public sealed class QueryParserTests
{
    private readonly ProblemDefinition _problemDefinition = new()
    {
        Fluents = new Dictionary<string, Fluent>
        {
            ["A"] = new Fluent("A", true),
            ["B"] = new Fluent("B", true),
            ["C"] = new Fluent("C", true),
        },
        Actions = new Dictionary<string, Action>
        {
            ["act1"] = new Action("act1", [], [], []),
            ["act2"] = new Action("act2", [], [], []),
        },
        InitialStates = new StateGroup([]),
        ValidStates = new StateGroup([]),
    };
    private readonly QueryParser _parser;

    public QueryParserTests()
    {
        _parser = new QueryParser(
            _problemDefinition,
            new FormulaReducer(), new FormulaParser(new FormulaTokenizer()));
    }

    [Fact]
    public void TryParse_EmptyQuery_ReturnsFalse()
    {
        var result = _parser.TryParse("", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Equal("Query is empty", errors[0]);
    }

    [Fact]
    public void TryParse_MultipleLines_ReturnsFalse()
    {
        var result = _parser.TryParse("query\nquery", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Equal("Query must contain only one line", errors[0]);
    }

    [Fact]
    public void TryParse_InvalidQueryType_ReturnsFalse()
    {
        var result = _parser.TryParse("query", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Invalid query type.*", errors[0]);
    }

    [Fact]
    public void TryParse_UnspecifiedQueryKind_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Query kind not specified.*", errors[0]);
    }

    [Fact]
    public void TryParse_InvalidQueryKind_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily query", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Invalid query kind.*", errors[0]);
    }

    [Fact]
    public void TryParse_ExecutableQueryWithNoProgram_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily executable", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Equal("Action program must contain at least one action", errors[0]);
    }

    [Fact]
    public void TryParse_ExecutableQueryWithInvalidAction_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily executable act1, unknown1, unknown2", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Equal("Invalid action names: \"unknown1\", \"unknown2\"", errors[0]);
    }

    [Fact]
    public void TryParse_ExecutableQueryWithValidProgram_ReturnsTrue()
    {
        var result = _parser.TryParse("necessarily executable act1, act2", out var query, out var errors);

        Assert.True(result);
        Assert.NotNull(query);
        var executableQuery = Assert.IsType<ExecutableQuery>(query);
        Assert.Equal(QueryType.Necessarily, query.Type);
        Assert.Equivalent(
            new ActionProgram([_problemDefinition.Actions["act1"], _problemDefinition.Actions["act2"]]),
            executableQuery.Program,
            strict: true);
    }

    [Fact]
    public void TryParse_AccessibleQueryWithInvalidStructure_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily accessible (A and B) act1, act2", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Accessible query must be of form.*", errors[0]);
    }

    [Fact]
    public void TryParse_AccessibleQueryWithEmptyFormula_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily accessible with act1, act2", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Accessible query must be of form.*", errors[0]);
    }

    [Fact]
    public void TryParse_AccessibleQueryWithInvalidFormula_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily accessible (A and B with act1, act2", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Expected closing parenthesis.*", errors[0]);
    }

    [Fact]
    public void TryParse_AccessibleQueryWithEmptyProgram_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily accessible (A and B) with", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Accessible query must be of form.*", errors[0]);
    }

    [Fact]
    public void TryParse_AccessibleQueryWithInvalidAction_ReturnsFalse()
    {
        var result = _parser.TryParse(
            "necessarily accessible (A and B) with act1, unknown1, unknown2",
            out var query,
            out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Equal("Invalid action names: \"unknown1\", \"unknown2\"", errors[0]);
    }

    [Fact]
    public void TryParse_AccessibleQueryWithValidProgram_ReturnsTrue()
    {
        var result = _parser.TryParse("possibly accessible (A and B) with act1, act2", out var query, out var errors);

        Assert.True(result);
        Assert.NotNull(query);
        var accessibleQuery = Assert.IsType<AccessibleQuery>(query);
        Assert.Equal(QueryType.Possibly, query.Type);
        Assert.Equivalent(
            new ActionProgram([_problemDefinition.Actions["act1"], _problemDefinition.Actions["act2"]]),
            accessibleQuery.Program,
            strict: true);

        // TODO: Check state group
    }

    [Fact]
    public void TryParse_AffordableQueryWithInvalidStructure_ReturnsFalse()
    {
        var result = _parser.TryParse(
            "necessarily affordable act1, act2 with 100",
            out var query,
            out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Affordable query must contain a budget.*", errors[0]);
    }

    [Fact]
    public void TryParse_AffordableQueryWithInvalidCost_ReturnsFalse()
    {
        var result = _parser.TryParse(
            "necessarily affordable act1, act2 with budget something",
            out var query,
            out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Budget specification is not an unsigned integer.*", errors[0]);
    }

    [Fact]
    public void TryParse_AffordableQueryWithEmptyProgram_ReturnsFalse()
    {
        var result = _parser.TryParse("necessarily affordable with budget 100", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Matches("Affordable query must contain a budget.*", errors[0]);
    }

    [Fact]
    public void TryParse_AffordableQueryWithInvalidAction_ReturnsFalse()
    {
        var result = _parser.TryParse(
            "necessarily affordable act1, unknown1, unknown2 with budget 100",
            out var query,
            out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Equal("Invalid action names: \"unknown1\", \"unknown2\"", errors[0]);
    }

    [Fact]
    public void TryParse_AffordableQueryWithValidProgram_ReturnsTrue()
    {
        var result = _parser.TryParse(
            "necessarily affordable act1, act2 with budget 100",
            out var query,
            out var errors);

        Assert.True(result);
        Assert.NotNull(query);
        var affordableQuery = Assert.IsType<AffordableQuery>(query);
        Assert.Equal(QueryType.Necessarily, query.Type);
        Assert.Equivalent(
            new ActionProgram([_problemDefinition.Actions["act1"], _problemDefinition.Actions["act2"]]),
            affordableQuery.Program,
            strict: true);

        Assert.Equal(100u, affordableQuery.CostLimit);
    }

    [Fact]
    public void TryParse_AffordableQueryWithZeroBudget_ReturnsTrue()
    {
        var result = _parser.TryParse("necessarily affordable act1, act2 with budget 0", out var query, out var errors);

        Assert.True(result);
        Assert.NotNull(query);
        var affordableQuery = Assert.IsType<AffordableQuery>(query);
        Assert.Equal(QueryType.Necessarily, query.Type);
        Assert.Equivalent(
            new ActionProgram([_problemDefinition.Actions["act1"], _problemDefinition.Actions["act2"]]),
            affordableQuery.Program,
            strict: true);

        Assert.Equal(0u, affordableQuery.CostLimit);
    }
}
