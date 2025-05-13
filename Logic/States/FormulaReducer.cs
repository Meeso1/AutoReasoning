using Logic.States.Models;

namespace Logic.States;

/// <summary>
/// 	Class that reduces logic formulas that specify a group of states 
/// 	into a <see cref="StateGroup"/> that satisfies it
/// </summary>
public sealed class FormulaReducer
{
    /// <summary>
    /// 	Reduces a formula to its simplest form
    /// </summary>
    /// <param name="formula">
    /// 	Formula to reduce
    /// </param>
    /// <returns>
    /// 	<see cref="StateGroup"/> that represents all states that satisfy the formula
    /// </returns>
    public StateGroup Reduce(StateGroup validStates, Formula formula)
    {
        return formula switch
        {
            True => validStates,

            False => new StateGroup(new HashSet<IReadOnlyDictionary<Fluent, bool>>()),

            FluentIsSet f => new StateGroup(new HashSet<IReadOnlyDictionary<Fluent, bool>>
            {
                new Dictionary<Fluent, bool> { [f.Fluent] = true }
            }),

            Not n => ReduceNot(validStates, n),

            And a => ReduceAnd(validStates, a),

            Or o => ReduceOr(validStates, o),

            Implies i => ReduceImplies(validStates, i),

            Equivalent e => ReduceEquivalent(validStates, e),

            _ => throw new ArgumentException($"Unknown formula type: {formula.GetType().Name}")
        };
    }

    private StateGroup ReduceNot(StateGroup validStates, Not not)
    {
        throw new NotImplementedException();
    }

    private StateGroup ReduceAnd(StateGroup validStates, And and)
    { 
        throw new NotImplementedException(); 
    }

    private StateGroup ReduceOr(StateGroup validStates, Or or)
    {
        var leftGroup = Reduce(validStates, or.First);
        var rightGroup = Reduce(validStates, or.Second);

        return StateGroup.Union(leftGroup, rightGroup);
    }

    private StateGroup ReduceImplies(StateGroup validStates, Implies implies)
    {
        return Reduce(validStates, new Or(new Not(implies.Prior), implies.Posterior));
    }

    private StateGroup ReduceEquivalent(StateGroup validStates, Equivalent equivalent)
    {
        return Reduce(validStates, new And(
            new Implies(equivalent.First, equivalent.Second),
            new Implies(equivalent.Second, equivalent.First)
            ));
    }
}
