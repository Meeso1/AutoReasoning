using Logic.Problem.Models;
using Logic.Queries;
using Logic.Queries.Models;
using Logic.States;
using Logic.States.Models;
using Action = Logic.Problem.Models.Action;

namespace Tests;

public sealed class QueryEvaluatorTests
{
    private static ProblemDefinition CreateYaleShootingProblem(params (string Fluent, bool Value)[][] initialStates)
    {
        var fluents = new[] {"alive", "loaded", "walking"}.Select(name => new Fluent(name, true)).ToList();
        var actions = new List<Action>
        {
            new Action("load", 
                [new ActionEffect(new True(), new FluentIsSet(fluents[1]), 1)], // load causes loaded
                [], 
                [new ActionCondition(new Not(new FluentIsSet(fluents[1])))]), // impossible load if loaded
            new Action("shoot", 
                [new ActionEffect(new FluentIsSet(fluents[1]), new Not(new FluentIsSet(fluents[0])), 1), // shoot causes not alive if loaded
                 new ActionEffect(new True(), new Not(new FluentIsSet(fluents[1])), 1)], // shoot causes not loaded
                [], []),
            new Action("walk", 
                [new ActionEffect(new True(), new FluentIsSet(fluents[2]), 1)], // walk causes walking
                [], []),
        };

        return new ProblemDefinition
        {
            Fluents = fluents.ToDictionary(f => f.Name, f => f),
            InitialStates = new StateGroup(
				initialStates.Select(states => 
					states.ToDictionary(s => fluents.First(f => f.Name == s.Fluent), s => s.Value)).ToList()),
            ValidStates = new StateGroup([
                new Dictionary<Fluent, bool>(){ [fluents[2]] = false }, 
                new Dictionary<Fluent, bool>(){ [fluents[0]] = true }
            ]), // always (walking => alive)
            Actions = actions.ToDictionary(a => a.Name, a => a)
        };
    }

    private static ProblemDefinition CreateYaleShootingProblemWithReleases()
    {
        var fluents = new[] {"alive", "loaded", "walking"}.Select(name => new Fluent(name, true)).ToList();
        var actions = new List<Action>
        {
            new Action("load", 
                [new ActionEffect(new True(), new FluentIsSet(fluents[1]), 1)], // load causes loaded
                [], 
                [new ActionCondition(new Not(new FluentIsSet(fluents[1])))]), // impossible load if loaded
            new Action("shoot", 
                [new ActionEffect(new True(), new Not(new FluentIsSet(fluents[1])), 1)], // shoot causes not loaded
                [new ActionRelease(new And(new FluentIsSet(fluents[1]), new FluentIsSet(fluents[0])), fluents[0], 1)], // shoot releases alive if loaded and alive
                []),
            new Action("walk", 
                [new ActionEffect(new True(), new FluentIsSet(fluents[2]), 1)], // walk causes walking
                [], 
                [new ActionCondition(new FluentIsSet(fluents[0]))]), // impossible walk if not alive
        };

        return new ProblemDefinition
        {
            Fluents = fluents.ToDictionary(f => f.Name, f => f),
            InitialStates = new StateGroup([new Dictionary<Fluent, bool>(){ [fluents[0]] = true }]), // initially alive
            ValidStates = StateGroup.All,
            Actions = actions.ToDictionary(a => a.Name, a => a)
        };
    }

    private static ProblemDefinition CreateYaleShootingProblemWithNoninertialFluents()
    {
        var fluents = new[] {new Fluent("alive", true), new Fluent("loaded", true), new Fluent("walking", false)};
        var actions = new List<Action>
        {
            new Action("load", 
                [new ActionEffect(new True(), new FluentIsSet(fluents[1]), 1)], // load causes loaded
                [], 
                [new ActionCondition(new Not(new FluentIsSet(fluents[1])))]), // impossible load if loaded
            new Action("shoot", 
                [new ActionEffect(new True(), new Not(new FluentIsSet(fluents[1])), 1)], // shoot causes not loaded
                [new ActionRelease(new And(new FluentIsSet(fluents[1]), new FluentIsSet(fluents[0])), fluents[0], 1)], // shoot releases alive if loaded and alive
                []),
            new Action("walk", 
                [new ActionEffect(new True(), new FluentIsSet(fluents[2]), 1)], // walk causes walking
                [], []), // impossible walk if not alive
        };

        return new ProblemDefinition
        {
            Fluents = fluents.ToDictionary(f => f.Name, f => f),
            InitialStates = new StateGroup([new Dictionary<Fluent, bool>(){ [fluents[0]] = true }]), // initially alive
            ValidStates = new StateGroup([
                new Dictionary<Fluent, bool>(){ [fluents[2]] = false }, 
                new Dictionary<Fluent, bool>(){ [fluents[0]] = true }
            ]), // always (walking => alive)
            Actions = actions.ToDictionary(a => a.Name, a => a)
        };
    }

	[Fact]
	public void EvaluateExecutable_EmptyProgram_ReturnsTrue()
	{
		var problem = CreateYaleShootingProblem([("alive", true)]);
		var evaluator = new QueryEvaluator(problem, new FormulaReducer());

		var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, new ActionProgram([]));
		Assert.True(evaluator.Evaluate(necessaryQuery));

		var possibleQuery = new ExecutableQuery(QueryType.Possibly, new ActionProgram([]));
		Assert.True(evaluator.Evaluate(possibleQuery));
	}

	[Fact]
	public void EvaluateExecutable_PossibleProgram_ReturnsTrue()
	{
		var problem = CreateYaleShootingProblem([("alive", true), ("loaded", false)]);
		var evaluator = new QueryEvaluator(problem, new FormulaReducer());

		var program = new ActionProgram([
			problem.Actions["load"],
			problem.Actions["shoot"],
		]);

		var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, program);
		Assert.True(evaluator.Evaluate(necessaryQuery));

		var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
		Assert.True(evaluator.Evaluate(possibleQuery));
	}

	[Fact]
	public void EvaluateExecutable_ImpossibleProgram_ReturnsFalse()
	{
		var problem = CreateYaleShootingProblem([("alive", true), ("loaded", true)]);
		var evaluator = new QueryEvaluator(problem, new FormulaReducer());

		var program = new ActionProgram([
			problem.Actions["load"],
			problem.Actions["shoot"],
		]);

		var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, program);
		Assert.False(evaluator.Evaluate(necessaryQuery));

		var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
		Assert.False(evaluator.Evaluate(possibleQuery));
	}

	[Fact]
	public void EvaluateExecutable_ProgramExecutableFromOnlySomeInitialStates_ReturnsFalse()
	{
		var problem = CreateYaleShootingProblem([("alive", true)]);
		var evaluator = new QueryEvaluator(problem, new FormulaReducer());
		
		var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, new ActionProgram([problem.Actions["load"]]));
		Assert.False(evaluator.Evaluate(necessaryQuery));

		var possibleQuery = new ExecutableQuery(QueryType.Possibly, new ActionProgram([problem.Actions["load"]]));
		Assert.False(evaluator.Evaluate(possibleQuery));
	}

	// TODO: Check executable with releases and non-inertial fluents

	// TODO: Check accessible
}
