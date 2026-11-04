# LeetCode 121: Best Time to Buy and Sell Stock – Analysis and Notes

## 1. Problem Understanding in Simple English

**Problem Statement:**
You are given an integer array `prices` where `prices[i]` represents the price of a given stock on the $i$-th day.
You want to maximize your financial profit by choosing **one day to buy** the stock and choosing a **different day in the future to sell** that stock.

Return the **maximum profit** you can achieve from this single transaction. If no profitable transaction is possible (for example, if prices continuously drop), return `0`.

### The Critical Real-World Rule: Chronological Order
You cannot sell a stock before you buy it!
- If you buy on Day $i$, you can only sell on Day $j$ where $j > i$.
- Example: In `[7, 1, 5, 3, 6, 4]`, the highest price is `7` (Day 0) and the lowest price is `1` (Day 1). You **cannot** buy at `1` and sell at `7`, because Day 0 has already passed in time! The correct transaction is to buy at `1` (Day 1) and sell at `6` (Day 4), yielding profit $6 - 1 = 5$.

---

## 2. Approach 1: Brute Force (Nested Loops)

### Logic
Check every possible buying day $i$ from $0$ to $n - 2$, and for each buying day, check every subsequent selling day $j$ from $i + 1$ to $n - 1$. Calculate `prices[j] - prices[i]` and track the maximum profit seen.

### C# Implementation
```csharp
public int MaxProfitBruteForce(int[] prices)
{
    if (prices == null || prices.Length <= 1) return 0;

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
```

### Complexity
- **Time Complexity:** $O(n^2)$. For an array of size $n$, the total comparisons are $\frac{n(n - 1)}{2}$. For $n = 10^5$, this requires approximately $5 \times 10^9$ operations, which **Time Out (TLE)** on LeetCode!
- **Space Complexity:** $O(1)$. No auxiliary data structures used.

---

## 3. Approach 2: Optimized Single-Pass (Greedy / Running Minimum) - Recommended

### Logic
As we iterate through the array day by day, we maintain two simple pieces of state:
1. `minimumPrice`: The lowest buying price observed up to today.
2. `maximumProfit`: The greatest difference between any day's price and the minimum buying price seen before or on that day.

For every price in the array:
- If today's price is cheaper than `minimumPrice`, update `minimumPrice = price`.
- Otherwise, calculate `currentProfit = price - minimumPrice`. If `currentProfit > maximumProfit`, update `maximumProfit`.

### C# LeetCode-Compatible Solution
```csharp
public class Solution
{
    public int MaxProfit(int[] prices)
    {
        if (prices == null || prices.Length <= 1)
        {
            return 0;
        }

        int minimumPrice = int.MaxValue;
        int maximumProfit = 0;

        foreach (int price in prices)
        {
            if (price < minimumPrice)
            {
                minimumPrice = price;
            }

            int currentProfit = price - minimumPrice;
            if (currentProfit > maximumProfit)
            {
                maximumProfit = currentProfit;
            }
        }

        return maximumProfit;
    }
}
```

---

## 4. Step-by-Step Dry Run

Input: `prices = [7, 1, 5, 3, 6, 4]`

| Day ($i$) | Price | `minimumPrice` Before | Updated `minimumPrice` | `currentProfit` (`price - min`) | `maximumProfit` | Notes |
| :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| **0** | **7** | $\infty$ | **7** | $7 - 7 = 0$ | **0** | Initial day, lowest so far is 7 |
| **1** | **1** | 7 | **1** | $1 - 1 = 0$ | **0** | Found new valley! New best buy day |
| **2** | **5** | 1 | 1 | $5 - 1 = 4$ | **4** | Selling today yields profit 4 |
| **3** | **3** | 1 | 1 | $3 - 1 = 2$ | 4 | Profit 2 < 4, keep max = 4 |
| **4** | **6** | 1 | 1 | $6 - 1 = 5$ | **5** | Found new peak! Max profit updated to 5 |
| **5** | **4** | 1 | 1 | $4 - 1 = 3$ | 5 | Profit 3 < 5, keep max = 5 |

**Final Result:** `5` (Buy at 1 on Day 1, Sell at 6 on Day 4).

---

## 5. Complexity Comparison

| Metric | Brute Force | Optimized Single-Pass |
| :--- | :---: | :---: |
| **Time Complexity** | $O(n^2)$ | **$O(n)$** |
| **Space Complexity** | $O(1)$ | **$O(1)$** |
| **LeetCode Submission** | Exceeds Time Limit (TLE) | **Accepted (beats 95%+ in speed)** |
| **Number of Passes** | Nested (Pairwise) | **Single Linear Scan** |

---

## 6. Test Cases Verification Table

| Test Case | Input | Expected Output | Actual Output | Result |
| :---: | :--- | :---: | :---: | :---: |
| **1** | `[7, 1, 5, 3, 6, 4]` | `5` | `5` | **PASS** |
| **2** | `[7, 6, 4, 3, 1]` | `0` | `0` | **PASS** |
| **3** | `[1, 2]` | `1` | `1` | **PASS** |
| **4** | `[2, 4, 1]` | `2` | `2` | **PASS** |
| **5** | `[3, 3, 3, 3]` | `0` | `0` | **PASS** |
| **6** | `[10]` | `0` | `0` | **PASS** |
| **7** | `[]` | `0` | `0` | **PASS** |
