using Logic.States.Models;

namespace Logic.Problem.Models;

public record ActionElement();

public sealed record ActionStatement(string name, ActionElement element);

public sealed record ActionEffect(Formula Condition, Fluent Fluent, bool Value, int CostIfChanged): ActionElement;

public sealed record ActionRelease(Formula Condition, Fluent ReleasedFluent, int CostIfChanged) : ActionElement;

public sealed record ActionCondition(Formula Condition) : ActionElement;

/// <summary>
/// 	Action
/// </summary>
/// <param name="Name">Name of the action</param>
/// <param name="Effects">
/// 	Fluent change caused by the action, and cost if the fluent value changes 
/// 	(from `[action] causes [fluent value] if [condition] costs [cost]`)
/// </param>
/// <param name="Releases">
/// 	Nondeterministic fluent change statement, and cost if the fluent value changes
/// 	(from `[action] releases [fluent value] if [condition] costs [cost]`)
/// </param>
/// <param name="Conditions">
/// 	Conditions that must be met for the action to be applicable
/// 	(from `impossible [action] if [not condition]`)
/// </param>
public sealed record Action(
    string Name,
    IReadOnlyList<ActionEffect> Effects,
    IReadOnlyList<ActionRelease> Releases,
    IReadOnlyList<ActionCondition> Conditions
);

public sealed record ActionProgram(IReadOnlyList<Action> Actions);
