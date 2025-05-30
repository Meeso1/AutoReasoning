namespace Logic.States.Models;

public abstract record Formula
{
    public abstract bool IsSatisfiedBy(State state);
}

public sealed record True : Formula
{
    public override bool IsSatisfiedBy(State state) => true;
}

public sealed record False : Formula
{
    public override bool IsSatisfiedBy(State state) => false;
}

public sealed record FluentIsSet(Fluent Fluent) : Formula
{
    public override bool IsSatisfiedBy(State state) => state.FluentValues.TryGetValue(Fluent, out bool value) && value;
}

public sealed record Not(Formula Formula) : Formula
{
    public override bool IsSatisfiedBy(State state) => !Formula.IsSatisfiedBy(state);
}

public sealed record And(Formula First, Formula Second) : Formula
{
    public override bool IsSatisfiedBy(State state) => First.IsSatisfiedBy(state) && Second.IsSatisfiedBy(state);
}

public sealed record Or(Formula First, Formula Second) : Formula
{
    public override bool IsSatisfiedBy(State state) => First.IsSatisfiedBy(state) || Second.IsSatisfiedBy(state);
}

public sealed record Implies(Formula Prior, Formula Posterior) : Formula
{
    public override bool IsSatisfiedBy(State state) => !Prior.IsSatisfiedBy(state) || Posterior.IsSatisfiedBy(state);
}

public sealed record Equivalent(Formula First, Formula Second) : Formula
{
    public override bool IsSatisfiedBy(State state) => First.IsSatisfiedBy(state) == Second.IsSatisfiedBy(state);
}
