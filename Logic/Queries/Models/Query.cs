namespace Logic.Queries.Models;

public abstract record Query;

public sealed record AlwaysReachableWithCostQuery(int CostLimit) : Query;

public sealed record AlwaysReachableQuery() : Query;
