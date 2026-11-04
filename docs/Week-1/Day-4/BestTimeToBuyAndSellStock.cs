using System;

namespace EmployeeDepartmentManagement.LeetCode;

/// <summary>
/// LeetCode Problem 121: Best Time to Buy and Sell Stock
/// Difficulty: Easy
/// Category: Arrays / Dynamic Programming / Greedy Single-Pass
/// 
/// PROBLEM UNDERSTANDING IN SIMPLE ENGLISH:
/// You are given an array 'prices' where prices[i] is the price of a given stock on the i-th day.
/// You want to maximize your profit by choosing a single day to buy one stock and choosing
/// a DIFFERENT day in the FUTURE to sell that stock.
/// Return the maximum profit you can achieve from this transaction.
/// If you cannot achieve any profit, return 0.
/// 
/// CRITICAL REAL-WORLD RULE:
/// You CANNOT sell before you buy! (Time travels strictly forward: Day_sell > Day_buy).
/// </summary>
public class Solution
{
    /// <summary>
    /// Recommended Approach: Single-Pass Greedy / Running Minimum
    /// Time Complexity: O(n) - Visits each stock price exactly once.
    /// Space Complexity: O(1) - Uses only two integer variables.
    /// </summary>
    /// <param name="prices">Array of daily stock prices.</param>
    /// <returns>Maximum possible profit, or 0 if no profit is possible.</returns>
    public int MaxProfit(int[] prices)
    {
        if (prices == null || prices.Length <= 1)
        {
            return 0; // Need at least two days to buy on one day and sell on a later day
        }

        int minimumPrice = int.MaxValue;
        int maximumProfit = 0;

        foreach (int price in prices)
        {
            // 1. Keep track of the lowest buying price seen so far
            if (price < minimumPrice)
            {
                minimumPrice = price;
            }

            // 2. Calculate potential profit if we sold today after buying at the historical low
            int currentProfit = price - minimumPrice;

            // 3. Update maximum profit if today's profit is higher than previous best
            if (currentProfit > maximumProfit)
            {
                maximumProfit = currentProfit;
            }
        }

        return maximumProfit;
    }

    /// <summary>
    /// Alternative/Comparison Approach: Brute Force (Nested Loops)
    /// Checks every possible (buy, sell) pair where sell_day > buy_day.
    /// Time Complexity: O(n^2) - Quadratic time, exceeds time limit on large LeetCode inputs.
    /// Space Complexity: O(1) - Constant space.
    /// </summary>
    public int MaxProfitBruteForce(int[] prices)
    {
        if (prices == null || prices.Length <= 1)
        {
            return 0;
        }

        int maxProfit = 0;
        for (int i = 0; i < prices.Length - 1; i++)
        {
            for (int j = i + 1; j < prices.Length; j++)
            {
                int profit = prices[j] - prices[i];
                if (profit > maxProfit)
                {
                    maxProfit = profit;
                }
            }
        }
        return maxProfit;
    }
}

/// <summary>
/// Local verification harness testing all mandatory test cases and edge conditions.
/// </summary>
public static class BestTimeToBuyAndSellStockTestHarness
{
    public static void RunVerification()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("===============================================================================");
        Console.WriteLine("      LEETCODE 121: BEST TIME TO BUY AND SELL STOCK - LOCAL VERIFICATION       ");
        Console.WriteLine("===============================================================================");

        var solution = new Solution();

        var testCases = new[]
        {
            new { Prices = new[] { 7, 1, 5, 3, 6, 4 }, Expected = 5, Desc = "Standard Valley and Peak (Buy at 1, Sell at 6)" },
            new { Prices = new[] { 7, 6, 4, 3, 1 }, Expected = 0, Desc = "Strictly Decreasing Prices (No Profit Possible)" },
            new { Prices = new[] { 1, 2 }, Expected = 1, Desc = "Minimal Two-Day Upward Trend (Buy at 1, Sell at 2)" },
            new { Prices = new[] { 2, 4, 1 }, Expected = 2, Desc = "New Minimum at End after Earlier Peak (Buy at 2, Sell at 4)" },
            new { Prices = new[] { 3, 3, 3, 3 }, Expected = 0, Desc = "Flat Stock Prices (Zero Gain)" },
            new { Prices = new[] { 10 }, Expected = 0, Desc = "Single Day Array (Cannot sell on future day)" },
            new { Prices = Array.Empty<int>(), Expected = 0, Desc = "Empty Array (Boundary Guard)" }
        };

        bool allPassed = true;

        for (int i = 0; i < testCases.Length; i++)
        {
            var tc = testCases[i];
            int actualOptimized = solution.MaxProfit(tc.Prices);
            int actualBruteForce = solution.MaxProfitBruteForce(tc.Prices);

            bool passed = (actualOptimized == tc.Expected) && (actualBruteForce == tc.Expected);
            if (!passed) allPassed = false;

            string status = passed ? "[PASS]" : "[FAIL]";
            Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{status} Test {i + 1}: ");
            Console.ResetColor();
            Console.WriteLine($"{tc.Desc}");
            Console.WriteLine($"       Input: [{string.Join(", ", tc.Prices)}] | Expected: {tc.Expected} | Optimized: {actualOptimized} | Brute Force: {actualBruteForce}");
            Console.WriteLine();
        }

        Console.ForegroundColor = allPassed ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(allPassed
            ? ">>> ALL 7 LOCAL TEST CASES PASSED SUCCESSFULLY (100% VERIFIED) <<<"
            : ">>> SOME TEST CASES FAILED <<<");
        Console.ResetColor();
    }
}
