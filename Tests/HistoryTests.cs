using Logic.Problem.Models;
using Logic.Queries;
using Logic.States;
using Logic.States.Models;
using Xunit.Abstractions;
using Action = Logic.Problem.Models.Action;

namespace Tests;

public sealed class HistoryTests(ITestOutputHelper output)
{
    private static ProblemDefinition CreateYaleShootingProblem()
    {
        var fluents = new[] { "alive", "loaded", "walking" }.Select(name => new Fluent(name, true)).ToList();
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

        return new ProblemDefinition
        {
            Fluents = fluents.ToDictionary(f => f.Name, f => f),
            ValueStatements = [new AfterStatement(new ActionProgram([]), new FluentIsSet(fluents[0]))], // initially alive
            ValidStates = new StateGroup([
                new Dictionary<Fluent, bool>(){ [fluents[2]] = false },
                new Dictionary<Fluent, bool>(){ [fluents[0]] = true }
            ]), // always (walking => alive)
            Actions = actions.ToDictionary(a => a.Name, a => a)
        };
    }

    private static ProblemDefinition CreateYaleShootingProblemWithReleases()
    {
        var fluents = new[] { "alive", "loaded", "walking" }.Select(name => new Fluent(name, true)).ToList();
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
                [new ActionCondition(new Not(new FluentIsSet(fluents[0])))]), // impossible walk if not alive
        };

        return new ProblemDefinition
        {
            Fluents = fluents.ToDictionary(f => f.Name, f => f),
            ValueStatements = [new AfterStatement(new ActionProgram([]), new FluentIsSet(fluents[0]))], // initially alive
            ValidStates = StateGroup.All,
            Actions = actions.ToDictionary(a => a.Name, a => a)
        };
    }

    private static ProblemDefinition CreateYaleShootingProblemWithNoninertialFluents()
    {
        var fluents = new[] { new Fluent("alive", true), new Fluent("loaded", true), new Fluent("walking", false) };
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
                [], []),
        };

        return new ProblemDefinition
        {
            Fluents = fluents.ToDictionary(f => f.Name, f => f),
            ValueStatements = [new AfterStatement(new ActionProgram([]), new FluentIsSet(fluents[0]))], // initially alive
            ValidStates = new StateGroup([
                new Dictionary<Fluent, bool>(){ [fluents[2]] = false },
                new Dictionary<Fluent, bool>(){ [fluents[0]] = true }
            ]), // always (walking => alive)
            Actions = actions.ToDictionary(a => a.Name, a => a)
        };
    }

    private static Dictionary<Fluent, bool> CreateState(IReadOnlyList<Fluent> fluents, params (string name, bool value)[] setValues)
    {
        return setValues.ToDictionary(f => fluents.First(fluent => fluent.Name == f.name), f => f.value);
    }

    [Fact]
    public void ComputeHistories_EmptyProgram_ReturnsInitialState()
    {
        var problem = CreateYaleShootingProblem();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse,
                ("alive", true),
                ("loaded", false),
                ("walking", false))),
            []);

        var singleTrajectory = Assert.Single(histories);
        var singleState = Assert.Single(singleTrajectory);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), singleState);
    }

    [Fact]
    public void ComputeHistories_DeterministicAction_ReturnsSingleTrajectory()
    {
        var problem = CreateYaleShootingProblem();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse,
                ("alive", true),
                ("loaded", false),
                ("walking", false))),
            [problem.Actions["load"]]).ToList();

        var singleTrajectory = Assert.Single(histories);

        Assert.Equal(2, singleTrajectory.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), singleTrajectory[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), singleTrajectory[1]);
    }

    [Fact]
    public void ComputeHistories_ManyDeterministicActions_ReturnsManyTrajectories()
    {
        var problem = CreateYaleShootingProblem();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))),
            [problem.Actions["walk"], problem.Actions["load"], problem.Actions["shoot"]]).ToList();

        var singleTrajectory = Assert.Single(histories);
        Assert.Equal(4, singleTrajectory.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), singleTrajectory[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", true))), singleTrajectory[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", true))), singleTrajectory[2]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), singleTrajectory[3]);
    }

    [Fact]
    public void ComputeHistories_ImpossibleAction_ReturnsTruncatedTrajectory()
    {
        var problem = CreateYaleShootingProblem();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))),
            [problem.Actions["walk"], problem.Actions["load"], problem.Actions["load"]]).ToList();

        var singleTrajectory = Assert.Single(histories);
        Assert.Equal(3, singleTrajectory.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), singleTrajectory[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", true))), singleTrajectory[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", true))), singleTrajectory[2]);
    }

    [Fact]
    public void ComputeHistories_NondeterministicAction_ReturnsManyTrajectories()
    {
        var problem = CreateYaleShootingProblemWithReleases();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))),
            [problem.Actions["shoot"]]).ToList();

        Assert.Equal(2, histories.Count);

        var firstIndex = histories[0][^1].FluentValues.Values.Any(v => v) ? 0 : 1; // first index: end state with alive
        var firstTrajectory = histories[firstIndex];
        Assert.Equal(2, firstTrajectory.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), firstTrajectory[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), firstTrajectory[1]);

        var secondTrajectory = histories[1 - firstIndex];
        Assert.Equal(2, secondTrajectory.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), secondTrajectory[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), secondTrajectory[1]);
    }

    [Fact]
    public void ComputeHistories_ManyNondeterministicActions_ReturnsManyTrajectories()
    {
         var problem = CreateYaleShootingProblemWithReleases();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))),
            [problem.Actions["shoot"], problem.Actions["load"], problem.Actions["shoot"]]).ToList();

        Assert.Equal(3, histories.Count);

        var aliveAtEnd = histories.First(h => h[^1].FluentValues.Values.Any(v => v));
        var aliveBeforeSecondShot = histories.First(h => h[^3].FluentValues.Values.Any(v => v) && !h[^1].FluentValues.Values.Any(v => v));
        var deadAfterFirstShot = histories.First(h => !h[^3].FluentValues.Values.Any(v => v) && !h[^1].FluentValues.Values.Any(v => v));

        Assert.Equal(4, aliveAtEnd.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveAtEnd[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), aliveAtEnd[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveAtEnd[2]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), aliveAtEnd[3]);

        Assert.Equal(4, aliveBeforeSecondShot.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveBeforeSecondShot[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), aliveBeforeSecondShot[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveBeforeSecondShot[2]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), aliveBeforeSecondShot[3]);

        Assert.Equal(4, deadAfterFirstShot.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), deadAfterFirstShot[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), deadAfterFirstShot[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", true), ("walking", false))), deadAfterFirstShot[2]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), deadAfterFirstShot[3]);
    }

    [Fact]
    public void ComputeHistories_ConditionallyImpossibleActions_ReturnsSomeTruncatedTrajectories()
    {
        var problem = CreateYaleShootingProblemWithReleases();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))),
            [problem.Actions["shoot"], problem.Actions["load"], problem.Actions["shoot"], problem.Actions["walk"]]).ToList();

        Assert.Equal(3, histories.Count);

        var aliveAtEnd = histories.First(h => h[3].FluentValues.Values.Any(v => v));
        var aliveBeforeSecondShot = histories.First(h => h[1].FluentValues.Values.Any(v => v) && !h[3].FluentValues.Values.Any(v => v));
        var deadAfterFirstShot = histories.First(h => !h[1].FluentValues.Values.Any(v => v) && !h[3].FluentValues.Values.Any(v => v));

        Assert.Equal(5, aliveAtEnd.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveAtEnd[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), aliveAtEnd[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveAtEnd[2]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), aliveAtEnd[3]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", true))), aliveAtEnd[4]);

        Assert.Equal(4, aliveBeforeSecondShot.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveBeforeSecondShot[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), aliveBeforeSecondShot[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), aliveBeforeSecondShot[2]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), aliveBeforeSecondShot[3]);

        Assert.Equal(4, deadAfterFirstShot.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", false))), deadAfterFirstShot[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), deadAfterFirstShot[1]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", true), ("walking", false))), deadAfterFirstShot[2]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), deadAfterFirstShot[3]);
    }

    [Fact]
    public void ComputeHistories_NoninertialFluents_ShouldNotMinimizeNoninertialFluents()
    {
        var problem = CreateYaleShootingProblemWithNoninertialFluents();
        var history = new History(problem, new FormulaReducer());
        var histories = history.ComputeHistories(
            new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", true))),
            [problem.Actions["shoot"]]).ToList();

        Assert.Equal(3, histories.Count);

        var aliveAndWalking = histories.First(h => h[1].FluentValues.Values.Count(v => v) == 2);
        var aliveAndNotWalking = histories.First(h => h[1].FluentValues.Values.Count(v => v) == 1);
        var deadAndNotWalking = histories.First(h => h[1].FluentValues.Values.Count(v => v) == 0);

        Assert.Equal(2, aliveAndWalking.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", true))), aliveAndWalking[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", true))), aliveAndWalking[1]);

        Assert.Equal(2, aliveAndNotWalking.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", true))), aliveAndNotWalking[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", false), ("walking", false))), aliveAndNotWalking[1]);

        Assert.Equal(2, deadAndNotWalking.Count);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", true), ("loaded", true), ("walking", true))), deadAndNotWalking[0]);
        Assert.Equal(new State(CreateState(problem.FluentUniverse, ("alive", false), ("loaded", false), ("walking", false))), deadAndNotWalking[1]);
    }

    // Helper to see what's going on when stuff inevitably breaks
    private void PrintTrajectories(IEnumerable<IReadOnlyList<State>> trajectories)
    {
        foreach (var trajectory in trajectories)
        {
            output.WriteLine("Trajectory:");
            foreach (var state in trajectory)
            {
                output.WriteLine($"  ---");
                foreach (var (fluent, value) in state.FluentValues.OrderBy(f => f.Key.Name))
                {
                    output.WriteLine($"  {fluent.Name}: {value}");
                }
            }
        }
    }
}
