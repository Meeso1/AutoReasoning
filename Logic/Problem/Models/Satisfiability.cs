using Logic.States.Models;

namespace Logic.Problem.Models;

public record SatisfiabilityStatement();
public sealed record AfterStatement(ActionProgram ActionChain, Formula Effect) : SatisfiabilityStatement;
public sealed record ObservableStatement(ActionProgram ActionChain, Formula Effect) : SatisfiabilityStatement;