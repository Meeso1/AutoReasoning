using System.Diagnostics;
using Logic.Problem.Models;
using Logic.Queries.Models;

namespace Logic.Queries;

/// <summary>
/// 	Class that evaluates queries for one given problem
/// </summary>
/// <remarks>
/// 	Can cache problem-specific data - if a new problem is specified, a new instance of the evaluator will be created
/// </remarks>
/// <param name="problem">
/// 	Problem definition
/// </param>
public sealed class QueryEvaluator(ProblemDefinition problem)
{
    public bool Evaluate(Query query)
    {
        return query switch
        {
            ExecutableQuery q => EvaluateExecutable(q),
            AccessibleQuery q => EvaluateAccessible(q),
            AffordableQuery q => EvaluateAffordable(q),
            _ => throw new UnreachableException($"Query type not implemented: {query.GetType()}")
        };
    }

    private bool EvaluateExecutable(ExecutableQuery query)
    {
        throw new NotImplementedException();
    }

    private bool EvaluateAccessible(AccessibleQuery query)
    {
        throw new NotImplementedException();
    }

    private bool EvaluateAffordable(AffordableQuery query)
    {
        throw new NotImplementedException();
    }
}
