using Logic.Queries.Models;
using Logic.States.Models;

namespace Logic.Model.Models;

public sealed class ModelDefinition
{
    public IReadOnlyDictionary<string, Fluent> Fluents { get; init; } = new Dictionary<string, Fluent>();
    public IReadOnlyList<Action> Actions { get; init; } = [];
    public StateGroup InitialState { get; init; }
}
