namespace Logic.States.Models;

public abstract record Formula;

public sealed record True : Formula;

public sealed record False : Formula;

public sealed record FluentIsSet(Fluent Fluent);

public sealed record Not(Formula Formula);

public sealed record And(Formula First, Formula Second);

public sealed record Or(Formula First, Formula Second);

public sealed record Implies(Formula Prior, Formula Posterior);

public sealed record Equivalent(Formula First, Formula Second);
