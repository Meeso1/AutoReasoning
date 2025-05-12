using Logic.Problem.Models;
using Logic.States.Models;

namespace Logic.Queries.Models;

public enum QueryType
{
    Necessarily,
    Possibly
}

public abstract record Query(QueryType Type, ActionProgram Program);

public sealed record ExecutableQuery(QueryType Type, ActionProgram Program) : Query(Type, Program);

public sealed record AccessibleQuery(QueryType Type, ActionProgram Program, StateGroup States) : Query(Type, Program);

public sealed record AffordableQuery(QueryType Type, ActionProgram Program, int CostLimit) : Query(Type, Program);
