using System.Diagnostics.CodeAnalysis;
using Logic.Problem.Models;
using Logic.Queries.Models;

namespace Logic.Queries;

/// <summary>
/// 	Class that parses and validates queries for a given problem
/// </summary>
/// <remarks>
///     Can cache problem-specific data - if a new problem is specified, a new instance of the parser will be created
/// </remarks>
/// <param name="problem">
/// 	Problem definition
/// </param>
public sealed class QueryParser(ProblemDefinition problem)
{
    /// <summary>
    /// 	Parses and validates a query
    /// </summary>
    /// <param name="input">
    /// 	Query to parse
    /// </param>
    /// <param name="query">
    /// 	Parsed query if parsing was successful, null otherwise
    /// </param>
    /// <param name="errors">
    /// 	Errors that occurred during parsing, or null if parsing was successful
    /// </param>
    /// <returns>
    /// 	True if parsing was successful, false otherwise
    /// </returns>
    public bool TryParse(
        string input, 
        [NotNullWhen(true)] out Query? query, 
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        throw new NotImplementedException();
    }   
}
