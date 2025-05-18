using Logic.States;
using Logic.States.Models;

namespace Tests;

public sealed class FluentDictionaryMergeStrategyTests
{
    // Test Fluents for use across tests
    private static readonly Fluent FluentA = new Fluent("A", true);
    private static readonly Fluent FluentB = new Fluent("B", true);
    private static readonly Fluent FluentC = new Fluent("C", true);
    private static readonly Fluent FluentD = new Fluent("D", true);

    #region AndMergeStrategy Tests

    [Fact]
    public void AndMergeStrategy_EmptyDictionaries_ReturnsEmptyDictionary()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>();
        var moreSpecific = new Dictionary<Fluent, bool>();

        // Act
        var result = AndMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Empty(result[0]);
    }

    [Fact]
    public void AndMergeStrategy_EmptyLessSpecific_ReturnsMoreSpecific()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>();
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };

        // Act
        var result = AndMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Count);
        Assert.True(result[0][FluentA]);
        Assert.False(result[0][FluentB]);
    }

    [Fact]
    public void AndMergeStrategy_MoreSpecificHasLessKeys_ThrowsArgumentException()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };
        var moreSpecific = new Dictionary<Fluent, bool>();

        // Act and Assert
        Assert.Throws<ArgumentException>(() => AndMergeStrategy.Merge(lessSpecific, moreSpecific));
    }

    [Fact]
    public void AndMergeStrategy_ConsistentValues_ReturnsMergedDictionary()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentC, true }
        };

        // Act
        var result = AndMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[0].Count);
        Assert.True(result[0][FluentA]);
        Assert.False(result[0][FluentB]);
        Assert.True(result[0][FluentC]);
    }

    [Fact]
    public void AndMergeStrategy_InconsistentValues_ReturnsEmptyList()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, false }, // Inconsistent with lessSpecific
            { FluentC, true }
        };

        // Act
        var result = AndMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void AndMergeStrategy_MultipleInconsistencies_ReturnsEmptyList()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false },
            { FluentC, true }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, false }, // Inconsistent
            { FluentB, true },  // Inconsistent
            { FluentD, true }
        };

        // Act
        var result = AndMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region OrMergeStrategy Tests

    [Fact]
    public void OrMergeStrategy_EmptyDictionaries_ReturnsEmptyDictionary()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>();
        var moreSpecific = new Dictionary<Fluent, bool>();

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Empty(result[0]);
    }

    [Fact]
    public void OrMergeStrategy_EmptyLessSpecific_ReturnsEmptyDictionary()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>();
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Empty(result[0]);
    }

    [Fact]
    public void OrMergeStrategy_MoreSpecificHasLessKeys_ThrowsArgumentException()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };
        var moreSpecific = new Dictionary<Fluent, bool>();

        // Act and Assert
        Assert.Throws<ArgumentException>(() => OrMergeStrategy.Merge(lessSpecific, moreSpecific));
    }

    [Fact]
    public void OrMergeStrategy_DifferentKeyCounts_MoreSpecificIsSubset_ReturnsLessSpecific()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false },
            { FluentC, true } // Additional constraint
        };

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Count);
        Assert.True(result[0][FluentA]);
        Assert.False(result[0][FluentB]);
    }

    [Fact]
    public void OrMergeStrategy_DifferentKeyCounts_NotSubset_ReturnsBoth()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, false }, // Different value
            { FluentB, false },
            { FluentC, true }
        };

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Equal(2, result.Count);
        // Check that both dictionaries are present
        Assert.Contains(result, dict => dict.Count == 2 && dict[FluentA] == true);
        Assert.Contains(result, dict => dict.Count == 3 && dict[FluentA] == false);
    }

    [Fact]
    public void OrMergeStrategy_SameCountDifferentKeys_ReturnsBoth()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentC, true },
            { FluentD, false }
        };

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, dict => dict.ContainsKey(FluentA) && dict.ContainsKey(FluentB));
        Assert.Contains(result, dict => dict.ContainsKey(FluentC) && dict.ContainsKey(FluentD));
    }

    [Fact]
    public void OrMergeStrategy_SameKeysNoChanges_ReturnsOne()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false },
            { FluentC, true }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false },
            { FluentC, true }
        };

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[0].Count);
        Assert.True(result[0][FluentA]);
        Assert.False(result[0][FluentB]);
        Assert.True(result[0][FluentC]);
    }

    [Fact]
    public void OrMergeStrategy_SameKeysOneDifference_ReturnsIntersection()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false },
            { FluentC, true }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, false }, // Only difference
            { FluentB, false },
            { FluentC, true }
        };

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Count); // Intersection without FluentA
        Assert.False(result[0][FluentB]);
        Assert.True(result[0][FluentC]);
        Assert.False(result[0].ContainsKey(FluentA));
    }

    [Fact]
    public void OrMergeStrategy_SameKeysMultipleDifferences_ReturnsBoth()
    {
        // Arrange
        var lessSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false },
            { FluentC, true }
        };
        var moreSpecific = new Dictionary<Fluent, bool>
        {
            { FluentA, false }, // Different
            { FluentB, true },  // Different
            { FluentC, true }
        };

        // Act
        var result = OrMergeStrategy.Merge(lessSpecific, moreSpecific);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, dict => dict[FluentA] == true && dict[FluentB] == false);
        Assert.Contains(result, dict => dict[FluentA] == false && dict[FluentB] == true);
    }
    #endregion

    #region Edge Cases

    [Fact]
    public void AndMergeStrategy_SameReference_ReturnsCopy()
    {
        // Arrange
        var dictionary = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };

        // Act
        var result = AndMergeStrategy.Merge(dictionary, dictionary);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Count);
        Assert.True(result[0][FluentA]);
        Assert.False(result[0][FluentB]);
        // Ensure it's a copy, not the same reference
        Assert.NotSame(dictionary, result[0]);
    }

    [Fact]
    public void OrMergeStrategy_SameReference_ReturnsOne()
    {
        // Arrange
        var dictionary = new Dictionary<Fluent, bool>
        {
            { FluentA, true },
            { FluentB, false }
        };

        // Act
        var result = OrMergeStrategy.Merge(dictionary, dictionary);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Count);
        Assert.True(result[0][FluentA]);
        Assert.False(result[0][FluentB]);
    }

    #endregion
}