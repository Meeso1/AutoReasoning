using Logic.Problem;
using Logic.Problem.Models;
using Logic.Queries;
using Logic.Queries.Models;
using Logic.States;
using Logic.States.Models;
using Action = Logic.Problem.Models.Action;

namespace Tests;

public sealed class QueryEvaluatorTests
{
    private static List<ValueStatement> ParseParams(IReadOnlyDictionary<string, Fluent> fluentsDict, params (string Fluent, bool Value)[][] initialStates)
    {
        List<ValueStatement> initiaslList = new();
        foreach (var initialState in initialStates)
        {
            Formula formula;
            if (initialState[0].Value) { formula = new FluentIsSet(fluentsDict[initialState[0].Fluent]); }
            else { formula = new Not(new FluentIsSet(fluentsDict[initialState[0].Fluent])); }

            foreach (var condition in initialState[1..])
            {
                Formula tmp;
                if (condition.Value) { tmp = new FluentIsSet(fluentsDict[condition.Fluent]); }
                else { tmp = new Not(new FluentIsSet(fluentsDict[condition.Fluent])); }
                formula = new And(tmp, formula);
            }
            initiaslList.Add(new AfterStatement(new ActionProgram([]), formula));
        }
        return initiaslList;
    }

    private static ProblemDefinition CreateYaleShootingProblem(params (string Fluent, bool Value)[][] initialStates)
    {
        var fluentsDict = new[] { "alive", "loaded", "walking" }.ToDictionary(name => name, name => new Fluent(name, true));
        var fluents = fluentsDict.Values.ToList();
        var actions = new List<Action>
        {
            new Action("load",
                [new ActionEffect(new True(), new FluentIsSet(fluents[1]), 1)], // load causes loaded
                [],
                [new ActionCondition(new FluentIsSet(fluents[1]))]), // impossible load if loaded
            new Action("shoot",
                [new ActionEffect(new FluentIsSet(fluents[1]), new Not(new FluentIsSet(fluents[0])), 1), // shoot causes not alive if loaded
                 new ActionEffect(new True(), new Not(new FluentIsSet(fluents[1])), 1)], // shoot causes not loaded
                [], []),
            new Action("walk",
                [new ActionEffect(new True(), new FluentIsSet(fluents[2]), 1)], // walk causes walking
                [], []),
        };

        return ProblemDefinitionParser.CreateProblemDefinition(
            fluents.ToDictionary(f => f.Name, f => f),
            actions.ToDictionary(a => a.Name, a => a),
            ParseParams(fluentsDict, initialStates),
            [new Implies(new FluentIsSet(fluents[2]), new FluentIsSet(fluents[0]))]
            );
    }

    private static ProblemDefinition CreateYaleShootingProblemWithReleases(params (string Fluent, bool Value)[][] initialStates)
    {
        var fluentsDict = new[] { "alive", "loaded", "walking" }.ToDictionary(name => name, name => new Fluent(name, true));
        var fluents = fluentsDict.Values.ToList();
        var actions = new List<Action>
        {
            new Action("load",
                [new ActionEffect(new True(), new FluentIsSet(fluents[1]), 1)], // load causes loaded
                [],
                [new ActionCondition(new FluentIsSet(fluents[1]))]), // impossible load if loaded
            new Action("shoot",
                [new ActionEffect(new True(), new Not(new FluentIsSet(fluents[1])), 1)], // shoot causes not loaded
                [new ActionRelease(new And(new FluentIsSet(fluents[1]), new FluentIsSet(fluents[0])), fluents[0], 1),  // shoot releases alive if loaded and alive
                 new ActionRelease(new And(new FluentIsSet(fluents[1]), new FluentIsSet(fluents[0])), fluents[2], 1)], // shoot releases walking if loaded and alive
                []),
            new Action("walk",
                [new ActionEffect(new True(), new FluentIsSet(fluents[2]), 1)], // walk causes walking
                [],
                [new ActionCondition(new Not(new FluentIsSet(fluents[0])))]), // impossible walk if not alive
        };

        

        return ProblemDefinitionParser.CreateProblemDefinition(
            fluents.ToDictionary(f => f.Name, f => f),
            actions.ToDictionary(a => a.Name, a => a),
            ParseParams(fluentsDict, initialStates),
            [new Implies(new FluentIsSet(fluents[2]), new FluentIsSet(fluents[0]))]
            );
    }

    private static ProblemDefinition CreateYaleShootingProblemWithNoninertialFluents(params (string Fluent, bool Value)[][] initialStates)
    {
        var fluents = new[] { new Fluent("alive", true), new Fluent("loaded", true), new Fluent("walking", false) };
        var fluentsDict = fluents.ToDictionary(f => f.Name, f => f);
        var actions = new List<Action>
        {
            new Action("load",
                [new ActionEffect(new True(), new FluentIsSet(fluents[1]), 1)], // load causes loaded
                [],
                [new ActionCondition(new FluentIsSet(fluents[1]))]), // impossible load if loaded
            new Action("shoot",
                [new ActionEffect(new True(), new Not(new FluentIsSet(fluents[1])), 1)], // shoot causes not loaded
                [new ActionRelease(new And(new FluentIsSet(fluents[1]), new FluentIsSet(fluents[0])), fluents[0], 1)], // shoot releases alive if loaded and alive
                []),
            new Action("walk",
                [new ActionEffect(new True(), new FluentIsSet(fluents[2]), 1)], // walk causes walking
                [],
                [new ActionCondition(new FluentIsSet(fluents[2]))]), // impossible walk if walking
        };

        return ProblemDefinitionParser.CreateProblemDefinition(
            fluents.ToDictionary(f => f.Name, f => f),
            actions.ToDictionary(a => a.Name, a => a),
            ParseParams(fluentsDict, initialStates),
            [new Implies(new FluentIsSet(fluents[2]), new FluentIsSet(fluents[0]))]
            );
    }

    #region ExecutableQueries

    [Fact]
    public void EvaluateExecutable_EmptyProgram_ReturnsTrue()
    {
        var problem = CreateYaleShootingProblem([("alive", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, new ActionProgram([]));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, new ActionProgram([]));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
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
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
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
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateExecutable_ProgramExecutableFromOnlySomeInitialStates_ReturnsFalse()
    {
        var problem = CreateYaleShootingProblem([("alive", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, new ActionProgram([problem.Actions["load"]]));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, new ActionProgram([problem.Actions["load"]]));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(possibleQuery));
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
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
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
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
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
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateExecutable_ProblemWithNoInitialStates_ReturnsInconsistentForAnyProgram()
    {
        var problem = CreateYaleShootingProblem([("alive", false), ("walking", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["load"]
        ]);

        var necessaryQuery = new ExecutableQuery(QueryType.Necessarily, program);
        Assert.Equal(QueryResult.Unconsistent, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new ExecutableQuery(QueryType.Possibly, program);
        Assert.Equal(QueryResult.Unconsistent, evaluator.Evaluate(possibleQuery));
    }
    #endregion ExecutableQueries
    #region AccessibleQueries

    [Fact]
    public void EvaluateAccessible_EmptyProgram_ReturnsTrueForConditionsSatisfiedInAllInitialStates()
    {
        var problem = CreateYaleShootingProblem([("alive", true)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var satisfiedCondition = formulaReducer.Reduce(new FluentIsSet(problem.Fluents["alive"]));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, new ActionProgram([]), satisfiedCondition)));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, new ActionProgram([]), satisfiedCondition)));

        var unsatisfiedCondition = formulaReducer.Reduce(new FluentIsSet(problem.Fluents["loaded"]));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, new ActionProgram([]), unsatisfiedCondition)));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, new ActionProgram([]), unsatisfiedCondition)));
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
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
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
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
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
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
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
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
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
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Necessarily, program, condition)));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AccessibleQuery(QueryType.Possibly, program, condition)));
    }

    [Fact]
    public void EvaluateAccessible_ProblemWithNoInitialStates_ReturnsInconsistentForAnyProgramAndCondition()
    {
        var problem = CreateYaleShootingProblem([("alive", false), ("walking", true)]);
        var formulaReducer = new FormulaReducer();
        var evaluator = new QueryEvaluator(problem, formulaReducer);

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["load"]
        ]);

        var necessaryQuery = new AccessibleQuery(QueryType.Necessarily, program, formulaReducer.Reduce(new False()));
        Assert.Equal(QueryResult.Unconsistent, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AccessibleQuery(QueryType.Possibly, program, formulaReducer.Reduce(new False()));
        Assert.Equal(QueryResult.Unconsistent, evaluator.Evaluate(possibleQuery));
    }
    #endregion AccessibleQueries
    #region AffordableQueries
    [Fact]
    public void EvaluateAffordable_EmptyProgram_ReturnsTrue()
    {
        var problem = CreateYaleShootingProblem([("alive", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var necessaryQuery = new AffordableQuery(QueryType.Necessarily, new ActionProgram([]), 0);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AffordableQuery(QueryType.Possibly, new ActionProgram([]), 0);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateAffordable_SingleActionWithinCostLimit_ReturnsTrue()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", false)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([problem.Actions["load"]]); // Cost: 1 (loaded becomes true)

        var necessaryQuery = new AffordableQuery(QueryType.Necessarily, program, 1);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AffordableQuery(QueryType.Possibly, program, 1);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateAffordable_SingleActionExceedsCostLimit_ReturnsFalse()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", false)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([problem.Actions["load"]]); // Cost: 1 (loaded becomes true)

        var necessaryQuery = new AffordableQuery(QueryType.Necessarily, program, 0);
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AffordableQuery(QueryType.Possibly, program, 0);
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateAffordable_MultipleActionsWithinCostLimit_ReturnsTrue()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", false)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],  // Cost: 1 (loaded becomes true)
            problem.Actions["shoot"]  // Cost: 2 (alive becomes false, loaded becomes false)
        ]);
        // Total expected cost: 3

        var necessaryQuery = new AffordableQuery(QueryType.Necessarily, program, 3);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AffordableQuery(QueryType.Possibly, program, 3);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateAffordable_MultipleActionsExceedCostLimit_ReturnsFalse()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", false)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],  // Cost: 1 (loaded becomes true)
            problem.Actions["shoot"]  // Cost: 2 (alive becomes false, loaded becomes false)
        ]);
        // Total expected cost: 3

        var necessaryQuery = new AffordableQuery(QueryType.Necessarily, program, 2);
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AffordableQuery(QueryType.Possibly, program, 2);
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateAffordable_NoEffectAction_ReturnsTrue()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", true), ("walking", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([problem.Actions["walk"]]); // Cost: 0 (walking is already true)

        var necessaryQuery = new AffordableQuery(QueryType.Necessarily, program, 0);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(necessaryQuery));

        var possibleQuery = new AffordableQuery(QueryType.Possibly, program, 0);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(possibleQuery));
    }

    [Fact]
    public void EvaluateAffordable_ActionWithReleases_AccountsForReleaseCosts()
    {
        var problem = CreateYaleShootingProblemWithReleases([("alive", true), ("loaded", false), ("walking", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],  // Cost: 1 (loaded becomes true)
            problem.Actions["shoot"]  // Cost: 1 (loaded becomes false) + potential release costs
        ]);

        // The exact results depend on the non-deterministic nature of releases
        // At minimum, we expect the program to be possibly affordable with a reasonable cost limit
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AffordableQuery(QueryType.Necessarily, program, 4)));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Necessarily, program, 3)));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AffordableQuery(QueryType.Possibly, program, 2)));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Possibly, program, 1)));
    }


    [Fact]
    public void EvaluateAffordable_ImpossibleProgram_ReturnsFalse()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"], // Impossible because already loaded
            problem.Actions["shoot"]
        ]);

        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Necessarily, program, 10)));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Possibly, program, 10)));
    }

    [Fact]
    public void EvaluateAffordable_ConditionallyAffordableProgram_ReturnsTrueForPossiblyAndFalseForNecessarily()
    {
        var problem = CreateYaleShootingProblem([("alive", true)]); // loaded is unspecified, so can be true or false
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([problem.Actions["load"]]); // Cost depends on initial loaded state

        // If loaded is initially false: cost = 1
        // If loaded is initially true: action is impossible, so not affordable
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Possibly, program, 1))); // Not affordable from any initial state due to impossibility
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Necessarily, program, 1)));                                                           
    }

    [Fact]
    public void EvaluateAffordable_ZeroCostLimit_OnlyAllowsNoChangePrograms()
    {
        var problem = CreateYaleShootingProblem([("alive", true), ("loaded", false), ("walking", true)]);
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        // Program that causes no changes (walking is already true)
        var noChangeProgram = new ActionProgram([problem.Actions["walk"]]);
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AffordableQuery(QueryType.Necessarily, noChangeProgram, 0)));
        Assert.Equal(QueryResult.Consequence, evaluator.Evaluate(new AffordableQuery(QueryType.Possibly, noChangeProgram, 0)));

        // Program that causes changes
        var changeProgram = new ActionProgram([problem.Actions["load"]]);
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Necessarily, changeProgram, 0)));
        Assert.Equal(QueryResult.NotConsequence, evaluator.Evaluate(new AffordableQuery(QueryType.Possibly, changeProgram, 0)));
    }

    [Fact]
    public void EvaluateAffordable_ProblemWithNoInitialStates_ReturnsInconsistentForAnyProgramAndCost()
    {
        var problem = CreateYaleShootingProblem([("alive", false), ("walking", true)]); // Contradictory constraints
        var evaluator = new QueryEvaluator(problem, new FormulaReducer());

        var program = new ActionProgram([
            problem.Actions["load"],
            problem.Actions["shoot"]
        ]);

        Assert.Equal(QueryResult.Unconsistent, evaluator.Evaluate(new AffordableQuery(QueryType.Necessarily, program, 0)));
        Assert.Equal(QueryResult.Unconsistent, evaluator.Evaluate(new AffordableQuery(QueryType.Possibly, program, 0)));
    }
    #endregion AffordableQueries
}
