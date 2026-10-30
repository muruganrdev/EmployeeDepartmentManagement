# LeetCode 1929: Concatenation of Array – Detailed Notes

- **Problem Link**: [LeetCode 1929 - Concatenation of Array](https://leetcode.com/problems/concatenation-of-array/)
- **Solution File**: [`ConcatenationOfArray.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/docs/Week-1/Day-1/ConcatenationOfArray.cs)
- **Difficulty**: Easy
- **Tags**: Array, Simulation, Memory Management

---

## 1. Problem Description

Given an integer array `nums` of length `n`, create an array `ans` of length `2n` where:
- `ans[i] == nums[i]` for `0 <= i < n`
- `ans[i + n] == nums[i]` for `0 <= i < n`

In simple terms, `ans` is the concatenation of two copies of the input array `nums`.

### Constraints:
- `n == nums.length`
- `1 <= n <= 1000`
- `1 <= nums[i] <= 1000`

---

## 2. Examples

### Example 1:
- **Input**: `nums = [1, 2, 1]`
- **Length**: `n = 3`
- **Output**: `[1, 2, 1, 1, 2, 1]`
- **Explanation**: 
  - `ans[0] = nums[0] = 1`, `ans[3] = nums[0] = 1`
  - `ans[1] = nums[1] = 2`, `ans[4] = nums[1] = 2`
  - `ans[2] = nums[2] = 1`, `ans[5] = nums[2] = 1`

### Example 2:
- **Input**: `nums = [1, 3, 2, 1]`
- **Length**: `n = 4`
- **Output**: `[1, 3, 2, 1, 1, 3, 2, 1]`

---

## 3. Algorithmic Approach

### Strategy: Single-Pass Direct Indexing
1. Let `n` be the length of `nums`.
2. Allocate a new integer array `ans` of length `2 * n`. In C#, arrays are allocated on the Managed Heap as a contiguous sequence of elements.
3. Iterate from `i = 0` to `n - 1`:
   - Set the first half: `ans[i] = nums[i]`
   - Set the second half: `ans[i + n] = nums[i]`
4. Return `ans`.

### C# Implementation:
```csharp
public int[] GetConcatenation(int[] nums)
{
    if (nums == null)
    {
        throw new ArgumentNullException(nameof(nums));
    }

    int n = nums.Length;
    int[] ans = new int[2 * n];

    for (int i = 0; i < n; i++)
    {
        ans[i] = nums[i];       // First copy (indices 0 to n-1)
        ans[i + n] = nums[i];   // Second copy (indices n to 2n-1)
    }

    return ans;
}
```

---

## 4. Complexity Analysis

| Metric | Complexity | Explanation |
| :--- | :--- | :--- |
| **Time Complexity** | **O(n)** | The `for` loop runs exactly `n` iterations. Inside the loop, array reads and writes take constant time **O(1)**. Total time: `n * O(1) = O(n)`. |
| **Space Complexity** | **O(n)** | We allocate an output array of size `2n` on the heap to store the result. Auxiliary memory (excluding the returned array) is **O(1)** as we only use a single integer loop counter `i`. |

---

## 5. Test Cases and Edge Cases

We verified this implementation against multiple test cases in `ConcatenationOfArray.cs`:

1. **Standard Case**: `[1, 2, 1]` → Expected `[1, 2, 1, 1, 2, 1]` *(Verified [OK])*
2. **Multi-Element Case**: `[1, 3, 2, 1]` → Expected `[1, 3, 2, 1, 1, 3, 2, 1]` *(Verified [OK])*
3. **Edge Case (Minimum Length)**: `[7]` → Expected `[7, 7]` *(Verified [OK])*
   - *Significance*: Confirms that boundary condition `n = 1` correctly produces a 2-element array without off-by-one errors.
4. **Edge Case (Uniform Elements)**: `[0, 0, 0]` → Expected `[0, 0, 0, 0, 0, 0]` *(Verified [OK])*
   - *Significance*: Confirms proper value copying for identical and zero values.

---

## 6. Connection to C# Fundamentals

This LeetCode problem directly reinforces the Day 1 fundamentals:
1. **Arrays in Memory**: Fixed-size, contiguous memory blocks allocated on the Managed Heap.
2. **Zero-Based Indexing**: Elements are accessed via `nums[0]` through `nums[n - 1]`.
3. **Loops (`for`)**: Demonstrates predictable, bounded iteration using index variables.
4. **Defensive Programming**: Validating against `null` inputs using `ArgumentNullException`.
