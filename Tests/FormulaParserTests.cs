using Logic.States;
using Logic.States.Models;

namespace Tests;

public sealed class FormulaParserTests
{
    private readonly FormulaParser _parser = new();
    private readonly Dictionary<string, Fluent> _fluents = new()
    {
        ["A"] = new Fluent("A", true),
        ["B"] = new Fluent("B", true),
        ["C"] = new Fluent("C", true),
    };

    [Fact]
    public void TryParse_TokenizationFails_ReturnsFalse()
    {
        bool result = _parser.TryParse("", _fluents, out var formula, out var errors);

        Assert.False(result);
        Assert.Null(formula);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void TryParse_TrueLiteral_ReturnsTrue()
    {
        bool result = _parser.TryParse("true", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.IsType<True>(formula);
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_FalseLiteral_ReturnsTrue()
    {
        bool result = _parser.TryParse("false", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.IsType<False>(formula);
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_ValidFluent_ReturnsTrue()
    {
        bool result = _parser.TryParse("A", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new FluentIsSet(_fluents["A"]));
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_InvalidFluent_ReturnsFalse()
    {
        bool result = _parser.TryParse("X", _fluents, out var formula, out var errors);

        Assert.False(result);
        Assert.Null(formula);
        Assert.NotNull(errors);
        Assert.Contains(errors, e => e.Contains("Unknown fluent"));
    }

    [Fact]
    public void TryParse_NotExpression_ReturnsTrue()
    {
        bool result = _parser.TryParse("not A", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new Not(new FluentIsSet(_fluents["A"])));
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_AndExpression_ReturnsTrue()
    {
        bool result = _parser.TryParse("A and B", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new And(
            new FluentIsSet(_fluents["A"]),
            new FluentIsSet(_fluents["B"])
        ));
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_OrExpression_ReturnsTrue()
    {
        bool result = _parser.TryParse("A or B", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new Or(
            new FluentIsSet(_fluents["A"]),
            new FluentIsSet(_fluents["B"])
        ));
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_ImpliesExpression_ReturnsTrue()
    {
        bool result = _parser.TryParse("A implies B", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new Implies(
            new FluentIsSet(_fluents["A"]),
            new FluentIsSet(_fluents["B"])
        ));
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_EquivalentExpression_ReturnsTrue()
    {
        bool result = _parser.TryParse("A equivalent B", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new Equivalent(
            new FluentIsSet(_fluents["A"]),
            new FluentIsSet(_fluents["B"])
        ));
        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_OperatorPrecedence_ReturnsTrue()
    {
        bool result = _parser.TryParse("A and B or C", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new Or(
            new And(new FluentIsSet(_fluents["A"]), new FluentIsSet(_fluents["B"])),
            new FluentIsSet(_fluents["C"])
        ));

        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_ParenthesizedExpression_ReturnsTrue()
    {
        bool result = _parser.TryParse("(A or B) and C", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.Equivalent(formula, new And(
            new Or(new FluentIsSet(_fluents["A"]), new FluentIsSet(_fluents["B"])),
            new FluentIsSet(_fluents["C"])
        ));

        Assert.Null(errors);
    }

    [Fact]
    public void TryParse_ComplexExpression_ReturnsTrue()
    {
        bool result = _parser.TryParse("not A and (B or C) implies A equivalent not C", _fluents, out var formula, out var errors);

        Assert.True(result);
        Assert.NotNull(formula);
        Assert.IsType<Equivalent>(formula);
        Assert.Null(errors);
        Assert.Equivalent(formula, new Equivalent(
            new Implies(
                new And(
                    new Not(new FluentIsSet(_fluents["A"])),
                    new Or(new FluentIsSet(_fluents["B"]), new FluentIsSet(_fluents["C"]))
                ),
                new FluentIsSet(_fluents["A"])
            ),
            new Not(new FluentIsSet(_fluents["C"]))
        ));
    }

    [Fact]
    public void TryParse_UnexpectedTokensAtEnd_ReturnsFalse()
    {
        bool result = _parser.TryParse("A B", _fluents, out var formula, out var errors);

        Assert.False(result);
        Assert.Null(formula);
        Assert.NotNull(errors);
        Assert.Contains(errors, e => e.Contains("Unexpected tokens"));
    }

    [Fact]
    public void TryParse_MissingClosingParenthesis_ReturnsFalse()
    {
        bool result = _parser.TryParse("(A and B", _fluents, out var formula, out var errors);

        Assert.False(result);
        Assert.Null(formula);
        Assert.NotNull(errors);
        Assert.Contains(errors, e => e.Contains("closing parenthesis"));
    }

    [Fact]
    public void TryParse_EmptyBrackets_ReturnsFalse()
    {
        bool result = _parser.TryParse("()", _fluents, out var formula, out var errors);

        Assert.False(result);
        Assert.Null(formula);
        Assert.NotNull(errors);
        Assert.NotEmpty(errors);
        Assert.Contains(errors, e => e.Contains("Unexpected token ')'"));
    }
}
