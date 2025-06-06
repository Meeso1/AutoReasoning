using Logic.States.Models;

namespace Logic.Problem.Models;

public sealed record SatisfiabilityStatement(ActionProgram ActionChain, Formula Effect);
