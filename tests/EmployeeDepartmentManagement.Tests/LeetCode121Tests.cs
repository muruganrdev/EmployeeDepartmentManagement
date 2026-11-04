using EmployeeDepartmentManagement.LeetCode;
using Xunit;

namespace EmployeeDepartmentManagement.Tests;

/// <summary>
/// Unit tests verifying LeetCode Problem 121 (Best Time to Buy and Sell Stock)
/// across all standard and edge test cases.
/// </summary>
public class LeetCode121Tests
{
    private readonly Solution _solution = new();

    [Theory]
    [InlineData(new[] { 7, 1, 5, 3, 6, 4 }, 5)] // Standard valley and peak (buy at 1, sell at 6)
    [InlineData(new[] { 7, 6, 4, 3, 1 }, 0)]    // Strictly descending (no profit)
    [InlineData(new[] { 1, 2 }, 1)]             // Minimal 2-element ascending
    [InlineData(new[] { 2, 4, 1 }, 2)]          // Minimum price appears after peak
    [InlineData(new[] { 3, 3, 3, 3 }, 0)]       // Flat prices
    [InlineData(new[] { 10 }, 0)]               // Single element
    public void MaxProfit_Optimized_ReturnsExpectedProfit(int[] prices, int expectedProfit)
    {
        // ACT
        int actualProfit = _solution.MaxProfit(prices);

        // ASSERT
        Assert.Equal(expectedProfit, actualProfit);
    }

    [Theory]
    [InlineData(new[] { 7, 1, 5, 3, 6, 4 }, 5)]
    [InlineData(new[] { 7, 6, 4, 3, 1 }, 0)]
    [InlineData(new[] { 1, 2 }, 1)]
    [InlineData(new[] { 2, 4, 1 }, 2)]
    [InlineData(new[] { 3, 3, 3, 3 }, 0)]
    [InlineData(new[] { 10 }, 0)]
    public void MaxProfit_BruteForce_ReturnsExpectedProfit(int[] prices, int expectedProfit)
    {
        // ACT
        int actualProfit = _solution.MaxProfitBruteForce(prices);

        // ASSERT
        Assert.Equal(expectedProfit, actualProfit);
    }

    [Fact]
    public void MaxProfit_NullOrEmptyArray_ReturnsZeroSafely()
    {
        Assert.Equal(0, _solution.MaxProfit(null!));
        Assert.Equal(0, _solution.MaxProfit(Array.Empty<int>()));
        Assert.Equal(0, _solution.MaxProfitBruteForce(null!));
        Assert.Equal(0, _solution.MaxProfitBruteForce(Array.Empty<int>()));
    }
}
