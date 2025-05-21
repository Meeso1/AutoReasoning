namespace Logic.States.Models;

public sealed record Fluent(string Name, bool IsInertial)
{
    public override string ToString()
    {
        var isInertial = IsInertial ? "INERTIAL" : "NOT INERTIAL";
        return $"{Name} {isInertial}";
    }
}
