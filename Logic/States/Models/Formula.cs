namespace Logic.States.Models;

public abstract record Formula;

public sealed record True : Formula;

public sealed record False : Formula;

public sealed record FluentIsSet(Fluent Fluent) : Formula;

public sealed record Not(Formula Formula) : Formula;

public sealed record And(Formula First, Formula Second) : Formula;

public sealed record Or(Formula First, Formula Second) : Formula;

public sealed record Implies(Formula Prior, Formula Posterior) : Formula;

public sealed record Equivalent(Formula First, Formula Second) : Formula;
