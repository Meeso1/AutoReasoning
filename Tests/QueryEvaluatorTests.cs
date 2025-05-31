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
        var fluents = new[] { "alive", "loaded", "walking" }.Select(name => new Fluent(name, true)).ToList();
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

    private static ProblemDefinition CreateYaleShootingProblemWithReleases(params (string Fluent, bool Value)[][] initialStates)
    {
        var fluents = new[] { "alive", "loaded", "walking" }.Select(name => new Fluent(name, true)).ToList();
        var actions = new List<Action>
        {
            new Action("load",
                [new ActionEffect(new True(), new FluentIsSet(fluents[1]), 1)], // load causes loaded
                [],
                [new ActionCondition(new Not(new FluentIsSet(fluents[1])))]), // impossible load if loaded
            new Action("shoot",
                [new ActionEffect(new True(), new Not(new FluentIsSet(fluents[1])), 1)], // shoot causes not loaded
                [new ActionRelease(new And(new FluentIsSet(fluents[1]), new FluentIsSet(fluents[0])), fluents[0], 1),  // shoot releases alive if loaded and alive
                 new ActionRelease(new And(new FluentIsSet(fluents[1]), new FluentIsSet(fluents[0])), fluents[2], 1)], // shoot releases walking if loaded and alive
                []),
            new Action("walk",
                [new ActionEffect(new True(), new FluentIsSet(fluents[2]), 1)], // walk causes walking
                [],
                [new ActionCondition(new FluentIsSet(fluents[0]))]), // impossible walk if not alive
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

    private static ProblemDefinition CreateYaleShootingProblemWithNoninertialFluents(params (string Fluent, bool Value)[][] initialStates)
    {
        var fluents = new[] { new Fluent("alive", true), new Fluent("loaded", true), new Fluent("walking", false) };
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
                [new ActionCondition(new Not(new FluentIsSet(fluents[2])))]), // impossible walk if walking
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

    [Fact]
    public void EvaluateExecutable_SometimesExecutableNonDeterministicProgram_ReturnsTrueForPossiblyAndFalseForNecessarily()
    {
        var problem = CreateYaleShootingProblemWithReleases([("alive", true), ("loaded", false)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["shoot"],
            problem.Actions["walk"]
        ]);

        var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, program);
        Assert.False(evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.True(evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateExecutable_AlwaysExecutableNonDeterministicProgram_ReturnsTrueForNecessarilyAndPossibly()
    {
        var problem = CreateYaleShootingProblemWithReleases([("alive", true), ("loaded", false)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["shoot"]
        ]);

        var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, program);
        Assert.True(evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.True(evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateExecutable_ProblemWithNoninertialFluents_ReturnsFalseForNecessarilyAndTrueForPossibly()
    {
        var problem = CreateYaleShootingProblemWithNoninertialFluents([("alive", true), ("loaded", false), ("walking", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"], // For walk to be executable, walking must become false after load
			problem.Actions["walk"]
        ]);

        var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, program);
        Assert.False(evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.True(evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateExecutable_ProblemWithNoInitialStates_ReturnsTrueForAnyProgram()
    {
        var problem = CreateYaleShootingProblem([("alive", false), ("walking", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["load"]
        ]);

        var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, program);
        Assert.True(evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.True(evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateAccessible_EmptyProgram_ReturnsTrueForConditionsSatisfiedInAllInitialStates()
    {
        var problem = CreateYaleShootingProblem([("alive", true)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var satisfiedCondition = formulaReducer.Reduce(new FluentIsSet(problem.Fluents["alive"]));
        Assert.True(evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, new ActionProgram([]), satisfiedCondition)));
        Assert.True(evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, new ActionProgram([]), satisfiedCondition)));

        var unsatisfiedCondition = formulaReducer.Reduce(new FluentIsSet(problem.Fluents["loaded"]));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, new ActionProgram([]), unsatisfiedCondition)));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, new ActionProgram([]), unsatisfiedCondition)));
    }

    [Fact]
    public void EvaluateAccessible_ImpossibleProgram_ReturnsFalse()
    {
        var problem = CreateYaleShootingProblem([("alive", true)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["load"]
        ]);

        var condition = formulaReducer.Reduce(new FluentIsSet(problem.Fluents["loaded"]));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
    }

    [Fact]
    public void EvaluateAccessible_ConditionallyImpossibleProgram_ReturnsFalseForNecessarilyAndTrueForPossibly()
    {
        var problem = CreateYaleShootingProblemWithReleases([("alive", true), ("loaded", false)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["shoot"],
            problem.Actions["walk"]
        ]);

        var condition = formulaReducer.Reduce(new Not(new FluentIsSet(problem.Fluents["loaded"])));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.True(evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
    }

    [Fact]
    public void EvaluateAccessible_ConditionSatisfiedFromSomeInitialStates_ReturnsFalse()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", false)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var program = new ActionProgram([
            problem.Actions["load"]
        ]);

        var condition = formulaReducer.Reduce(new FluentIsSet(problem.Fluents["walking"]));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
    }

    [Fact]
    public void EvaluateAccessible_ConditionSatisfiedFromAllInitialStates_ReturnsTrue()
    {
        var problem = CreateYaleShootingProblemWithReleases([("loaded", false)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var program = new ActionProgram([
            problem.Actions["load"]
        ]);

        var condition = formulaReducer.Reduce(new FluentIsSet(problem.Fluents["loaded"]));
        Assert.True(evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.True(evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
    }

    [Fact]
    public void EvaluateAccessible_ConditionSometimesSatisfiedFromAllInitialStates_ReturnsTrueForPossiblyAndFalseForNecessarily()
    {
        var problem = CreateYaleShootingProblemWithReleases([("alive", true), ("loaded", false)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["shoot"]
        ]);

        var condition = formulaReducer.Reduce(new Not(new FluentIsSet(problem.Fluents["walking"])));
        Assert.False(evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.True(evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
    }

    [Fact]
    public void EvaluateAccessible_ProblemWithNoInitialStates_ReturnsTrueForAnyProgramAndCondition()
    {
        var problem = CreateYaleShootingProblem([("alive", false), ("walking", true)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["load"]
        ]);

        var necessaryQuery = new AccessibleQuery(QueryType.Necessarily, program, formulaReducer.Reduce(new False()));
        Assert.True(evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AccessibleQuery(QueryType.Possibly, program, formulaReducer.Reduce(new False()));
        Assert.True(evaluator.Evaluate(possibleQuery));
    }
}
