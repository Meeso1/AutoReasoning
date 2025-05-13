using Logic.Problem.Models;
using Logic.Queries;
using Logic.Queries.Models;
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

    [Fact]
    public void TryParse_EmptyQuery_ReturnsFalse()
    {
        var parser = new QueryParser(_problemDefinition);
        var result = parser.TryParse("", out var query, out var errors);

        Assert.False(result);
        Assert.Null(query);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
		Assert.Equal("Query is empty", errors[0]);
    }

    [Fact]
    public void TryParse_MultipleLines_ReturnsFalse()
    {
        var parser = new QueryParser(_problemDefinition);
        var result = parser.TryParse("query\nquery", out var query, out var errors);

		Assert.False(result);
		Assert.Null(query);
		Assert.NotNull(errors);
		Assert.NotEmpty(errors);
		Assert.Equal("Query must contain only one line", errors[0]);
    }

	[Fact]
	public void TryParse_InvalidQueryType_ReturnsFalse()
	{
		var parser = new QueryParser(_problemDefinition);
		var result = parser.TryParse("query", out var query, out var errors);
		
		Assert.False(result);
		Assert.Null(query);
		Assert.NotNull(errors);
		Assert.NotEmpty(errors);
		Assert.Matches("Invalid query type.*", errors[0]);
	}

	[Fact]
	public void TryParse_UnspecifiedQueryKind_ReturnsFalse()
	{
		var parser = new QueryParser(_problemDefinition);
		var result = parser.TryParse("necessarily", out var query, out var errors);
		
		Assert.False(result);
		Assert.Null(query);
		Assert.NotNull(errors);
		Assert.NotEmpty(errors);
		Assert.Matches("Query kind not specified.*", errors[0]);
	}

	[Fact]
	public void TryParse_InvalidQueryKind_ReturnsFalse()
	{
		var parser = new QueryParser(_problemDefinition);
		var result = parser.TryParse("necessarily query", out var query, out var errors);

		Assert.False(result);
		Assert.Null(query);
		Assert.NotNull(errors);
		Assert.NotEmpty(errors);
		Assert.Matches("Invalid query kind.*", errors[0]);
	}

	[Fact]
	public void TryParse_ExecutableQueryWithNoProgram_ReturnsFalse()
	{
		var parser = new QueryParser(_problemDefinition);
		var result = parser.TryParse("necessarily executable", out var query, out var errors);

		Assert.False(result);
		Assert.Null(query);
		Assert.NotNull(errors);
		Assert.NotEmpty(errors);
		Assert.Equal("Action program must contain at least one action", errors[0]);
	}
	
	[Fact]
	public void TryParse_ExecutableQueryWithInvalidAction_ReturnsFalse()
	{
		var parser = new QueryParser(_problemDefinition);
		var result = parser.TryParse("necessarily executable act1, unknown1, unknown2", out var query, out var errors);
		
		Assert.False(result);
		Assert.Null(query);
		Assert.NotNull(errors);
		Assert.NotEmpty(errors);
		Assert.Equal("Invalid action names: \"unknown1\", \"unknown2\"", errors[0]);
	}
	
	[Fact]
	public void TryParse_ExecutableQueryWithValidProgram_ReturnsTrue()
	{
		var parser = new QueryParser(_problemDefinition);
		var result = parser.TryParse("necessarily executable act1, act2", out var query, out var errors);

		Assert.True(result);
		Assert.NotNull(query);
		var executableQuery = Assert.IsType<ExecutableQuery>(query);
		Assert.Equal(QueryType.Necessarily, query.Type);
		Assert.Equivalent(
			new ActionProgram([_problemDefinition.Actions["act1"], _problemDefinition.Actions["act2"]]), 
			executableQuery.Program, 
			strict: true);
	}
}
