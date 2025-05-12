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
    public StateGroup Reduce(Formula formula)
    {
        throw new NotImplementedException();
    }
}
