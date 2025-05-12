using System.Diagnostics.CodeAnalysis;
using Logic.States.Models;

namespace Logic.States;

/// <summary>
/// 	Class that parses and validates formulas
/// </summary>
public sealed class FormulaParser
{
    /// <summary>
    /// 	Parse and validate a formula
    /// </summary>
    /// <param name="input">
    /// 	Formula to parse (from any condition specification)
    /// </param>
    /// <param name="fluentNames">
    /// 	Names of all valid fluents in the problem
    /// </param>
    /// <param name="formula">
    /// 	Parsed formula if parsing was successful, null otherwise
    /// </param>
    /// <param name="errors">
    /// 	Errors that occurred during parsing, or null if parsing was successful
    /// </param>
    /// <returns>
    /// 	True if parsing was successful, false otherwise
    /// </returns>
    public bool TryParse(
        string input,
        IReadOnlyList<string> fluentNames,
        [NotNullWhen(true)] out Formula? formula,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        formula = null;
        errors = null;

        throw new NotImplementedException();
    }
}
