namespace Logic.Model.Models;

public sealed record Action;

public sealed record ActionProgram(IReadOnlyList<Action> Actions);
