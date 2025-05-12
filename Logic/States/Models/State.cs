namespace Logic.States.Models;

public sealed record State(IReadOnlyDictionary<Fluent, bool> FluentValues);

public sealed record StateGroup(IReadOnlyDictionary<Fluent, bool> SpecifiedFluentValues);
