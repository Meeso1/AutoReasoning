using Logic.States.Models;

namespace Logic.Problem.Models;

public record SatisfiabilityStatement(ActionProgram ActionChain, Formula Effect);
public sealed record AfterStatement(ActionProgram ActionChain, Formula Effect) : SatisfiabilityStatement(ActionChain, Effect);
public sealed record ObservableStatement(ActionProgram ActionChain, Formula Effect) : SatisfiabilityStatement(ActionChain, Effect);