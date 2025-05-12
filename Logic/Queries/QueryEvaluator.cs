using System.Diagnostics;
using Logic.Model.Models;
using Logic.Queries.Models;

namespace Logic;

public sealed class QueryEvaluator(ModelDefinition model)
{
    public bool Evaluate(Query query)
    {
        return query switch
        {
            AlwaysReachableWithCostQuery q => EvaluateAlwaysReachableWithCost(q),
            AlwaysReachableQuery q => EvaluateAlwaysReachable(q),
            _ => throw new UnreachableException($"Query type not implemented: {query.GetType()}")
        };
    }

    private bool EvaluateAlwaysReachableWithCost(AlwaysReachableWithCostQuery query)
    {
        throw new NotImplementedException();
    }

    private bool EvaluateAlwaysReachable(AlwaysReachableQuery query)
    {
        throw new NotImplementedException();
    }
}
