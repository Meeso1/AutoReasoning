using Logic.States;

namespace Tests;

public sealed class FormulaTokenizerTests
{
    [Fact]
    public void BasicTokens_ShouldBeRecognized()
    {
        var input = "( ) true false not and or implies equivalent var1";
        var success = FormulaTokenizer.TryTokenize(input, out var tokens, out _);
        
        Assert.True(success);
        Assert.Equal(10, tokens.Count);
        
        Assert.Equal(TokenType.OpenParen, tokens[0].Type);
        Assert.Equal(TokenType.CloseParen, tokens[1].Type);
        Assert.Equal(TokenType.True, tokens[2].Type);
        Assert.Equal(TokenType.False, tokens[3].Type);
        Assert.Equal(TokenType.Not, tokens[4].Type);
        Assert.Equal(TokenType.And, tokens[5].Type);
        Assert.Equal(TokenType.Or, tokens[6].Type);
        Assert.Equal(TokenType.Implies, tokens[7].Type);
        Assert.Equal(TokenType.Equivalent, tokens[8].Type);
        Assert.Equal(TokenType.Identifier, tokens[9].Type);
    }
    
    [Fact]
    public void ValidIdentifiers_ShouldBeRecognized()
    {
        var input = "x y z _var var1 var_2 longVariableName";
        var success = FormulaTokenizer.TryTokenize(input, out var tokens, out _);
        
        Assert.True(success);
        Assert.Equal(7, tokens.Count);
        
        foreach (var token in tokens)
        {
            Assert.Equal(TokenType.Identifier, token.Type);
        }
    }
    
    [Fact]
    public void ComplexFormula_ShouldTokenizeCorrectly()
    {
        var input = "(x and y) or (not z implies w)";
        var success = FormulaTokenizer.TryTokenize(input, out var tokens, out _);
        
        Assert.True(success);
        Assert.Equal(12, tokens.Count);
        
        Assert.Equal(TokenType.OpenParen, tokens[0].Type);
        Assert.Equal(TokenType.Identifier, tokens[1].Type);
        Assert.Equal("x", tokens[1].Value);
        Assert.Equal(TokenType.And, tokens[2].Type);
        Assert.Equal(TokenType.Identifier, tokens[3].Type);
        Assert.Equal("y", tokens[3].Value);
        Assert.Equal(TokenType.CloseParen, tokens[4].Type);
        Assert.Equal(TokenType.Or, tokens[5].Type);
        Assert.Equal(TokenType.OpenParen, tokens[6].Type);
        Assert.Equal(TokenType.Not, tokens[7].Type);
        Assert.Equal(TokenType.Identifier, tokens[8].Type);
        Assert.Equal("z", tokens[8].Value);
        Assert.Equal(TokenType.Implies, tokens[9].Type);
        Assert.Equal(TokenType.Identifier, tokens[10].Type);
        Assert.Equal("w", tokens[10].Value);
        Assert.Equal(TokenType.CloseParen, tokens[11].Type);
    }
    
    [Fact]
    public void WhitespaceHandling_ShouldBeCorrect()
    {
        var input = "   (  x   and\t\ty )  \n  ";
        var success = FormulaTokenizer.TryTokenize(input, out var tokens, out _);
        
        Assert.True(success);
        Assert.Equal(5, tokens.Count);
        
        Assert.Equal(TokenType.OpenParen, tokens[0].Type);
        Assert.Equal(TokenType.Identifier, tokens[1].Type);
        Assert.Equal("x", tokens[1].Value);
        Assert.Equal(TokenType.And, tokens[2].Type);
        Assert.Equal(TokenType.Identifier, tokens[3].Type);
        Assert.Equal("y", tokens[3].Value);
        Assert.Equal(TokenType.CloseParen, tokens[4].Type);
    }
    
    [Fact]
    public void EmptyInput_ShouldReturnEmptyTokens()
    {
        var input = "";
        var success = FormulaTokenizer.TryTokenize(input, out var tokens, out _);
        
        Assert.True(success);
        Assert.Empty(tokens);
        
        input = "   \t\n   ";
        success = FormulaTokenizer.TryTokenize(input, out tokens, out _);
        
        Assert.True(success);
        Assert.Empty(tokens);
    }
    
    [Fact]
    public void InvalidTokens_ShouldReturnError()
    {
        var input = "x and #invalid";
        var success = FormulaTokenizer.TryTokenize(input, out var tokens, out var errors);
        
        Assert.False(success);
        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Contains("Unexpected character at position", errors[0]);

        Assert.Contains("#", errors[0]);
    }
    
    [Theory]
    [InlineData("with")]
    [InlineData("budget")]
    [InlineData("necessarily")]
    [InlineData("possibly")]
    [InlineData("executable")]
    [InlineData("affordable")]
    [InlineData("accessible")]
    public void ReservedIdentifiers_ShouldReturnError(string reserved)
    {
        var input = $"x and {reserved}";
        var success = FormulaTokenizer.TryTokenize(input, out _, out var errors);
        
        Assert.False(success);
        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Contains($"'{reserved}' at position", errors[0]);
        Assert.Contains("cannot be used as an identifier", errors[0]);
    }
    
    [Fact]
    public void TokenPositions_ShouldBeCorrect()
    {
        var input = "x and y";
        var success = FormulaTokenizer.TryTokenize(input, out var tokens, out _);
        
        Assert.True(success);
        Assert.Equal(3, tokens.Count);
        
        Assert.Equal(0, tokens[0].Position);
        Assert.Equal(2, tokens[1].Position);
        Assert.Equal(6, tokens[2].Position);
    }
}
