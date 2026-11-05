using EmployeeDepartmentManagement.LeetCode;
using Xunit;

namespace EmployeeDepartmentManagement.Tests;

/// <summary>
/// Unit tests for LeetCode Problem 242: Valid Anagram
/// Verifies both the Character Frequency (Optimal) and Sorting implementations
/// across normal, boundary, and edge conditions.
/// </summary>
public class LeetCode242Tests
{
    private readonly ValidAnagramSolution _solution = new();

    [Theory]
    [InlineData("anagram", "nagaram", true)]   // Standard anagram
    [InlineData("rat", "car", false)]          // Different letters
    [InlineData("a", "a", true)]               // Single character match
    [InlineData("ab", "a", false)]             // Differing lengths
    [InlineData("", "", true)]                 // Empty strings
    [InlineData("listen", "silent", true)]     // Classic anagram
    [InlineData("aabbcc", "bbaacc", true)]     // Duplicate characters
    [InlineData("aacc", "ccac", false)]        // Same characters, mismatched counts
    public void IsAnagram_OptimalFrequencyApproach_ReturnsExpected(string s, string t, bool expected)
    {
        // ACT
        bool result = _solution.IsAnagram(s, t);

        // ASSERT
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("anagram", "nagaram", true)]
    [InlineData("rat", "car", false)]
    [InlineData("a", "a", true)]
    [InlineData("ab", "a", false)]
    [InlineData("", "", true)]
    [InlineData("listen", "silent", true)]
    [InlineData("aabbcc", "bbaacc", true)]
    [InlineData("aacc", "ccac", false)]
    public void IsAnagramSorting_SortingApproach_ReturnsExpected(string s, string t, bool expected)
    {
        // ACT
        bool result = _solution.IsAnagramSorting(s, t);

        // ASSERT
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsAnagram_NullInputs_ReturnsFalseSafely()
    {
        Assert.False(_solution.IsAnagram(null!, "test"));
        Assert.False(_solution.IsAnagram("test", null!));
        Assert.False(_solution.IsAnagram(null!, null!));

        Assert.False(_solution.IsAnagramSorting(null!, "test"));
        Assert.False(_solution.IsAnagramSorting("test", null!));
        Assert.False(_solution.IsAnagramSorting(null!, null!));
    }
}
