using Logic.States.Models;

namespace Logic.Problem.Models;

/// <summary>
/// 	Problem definition (from all specifications besides queries)
/// </summary>
/// <param name="Fluents">
/// 	All fluents in the problem (indexed by name)
/// </param>
/// <param name="Actions">
/// 	All actions in the problem (indexed by name)
/// </param>
/// <param name="InitialStates">
/// 	Group representing all initial states (parsed from `initially [formula]`)
/// </param>
/// <param name="ValidStates">
/// 	Group of valid states (parsed from `always [formula]`)
/// </param>
public sealed class ProblemDefinition
{
    public required IReadOnlyDictionary<string, Fluent> Fluents { get; init; }
    public required IReadOnlyDictionary<string, Action> Actions { get; init; }
    public required StateGroup InitialStates { get; init; }
    public required StateGroup ValidStates { get; init; }
    public IReadOnlyList<Fluent> FluentUniverse => Fluents.Values.ToList();
}
