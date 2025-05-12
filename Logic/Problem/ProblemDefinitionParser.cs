using System.Diagnostics.CodeAnalysis;
using Logic.Problem.Models;

namespace Logic.Problem;

/// <summary>
/// 	Class that parses and validates problem definitions
/// </summary>
public sealed class ProblemDefinitionParser
{
    /// <summary>
    /// 	Parses and validates a problem definition
    /// </summary>
    /// <param name="definition">
    /// 	Problem definition
    /// </param>
    /// <param name="problem">
    /// 	Parsed problem definition if parsing was successful, null otherwise
    /// </param>
    /// <param name="errors">
    /// 	Errors that occurred during parsing, or null if parsing was successful
    /// </param>
    /// <returns>
    /// 	True if parsing was successful, false otherwise
    /// </returns>
    public bool TryParse(
        string definition, 
        [NotNullWhen(true)] out ProblemDefinition? problem, 
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        throw new NotImplementedException();
    }
}
