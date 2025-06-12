using Logic.States.Models;

namespace Logic.Problem.Models;

public record ValueStatement(ActionProgram ActionChain, Formula Effect);
public sealed record AfterStatement(ActionProgram ActionChain, Formula Effect) : ValueStatement(ActionChain, Effect);
public sealed record ObservableStatement(ActionProgram ActionChain, Formula Effect) : ValueStatement(ActionChain, Effect);