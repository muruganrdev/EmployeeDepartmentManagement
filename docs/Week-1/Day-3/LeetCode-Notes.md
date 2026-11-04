# LeetCode 217: Contains Duplicate – Detailed Notes

- **Problem Link**: [LeetCode 217 - Contains Duplicate](https://leetcode.com/problems/contains-duplicate/)
- **Solution File**: [`ContainsDuplicate.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/docs/Week-1/Day-3/ContainsDuplicate.cs)
- **Difficulty**: Easy
- **Tags**: Array, Hash Table, Sorting

---

## 1. Problem Description (Simple English)

You are given an array of integers called `nums`.
- If any integer appears **at least twice** in the array, return `true`.
- If every element in the array is distinct (each number appears only once), return `false`.

### Examples:
- **Example 1**: `nums = [1, 2, 3, 1]` $\rightarrow$ `true` (The number `1` appears twice).
- **Example 2**: `nums = [1, 2, 3, 4]` $\rightarrow$ `false` (All elements are unique).
- **Example 3**: `nums = [1, 1, 1, 3, 3, 4, 3, 2, 4, 2]` $\rightarrow$ `true` (Multiple values repeat).

### Constraints:
- `1 <= nums.Length <= 10^5`
- `-10^9 <= nums[i] <= 10^9`

---

## 2. Approach Comparison

We evaluated three classic algorithmic approaches:

| Approach | Data Structure | Time Complexity | Auxiliary Space | Verdict |
| :--- | :--- | :---: | :---: | :--- |
| **1. Brute Force** | None (Nested Loops) | **$O(n^2)$** | **$O(1)$** | **Too Slow**: Times out on LeetCode for $n = 10^5$. |
| **2. Sorting** | Array Clone | **$O(n \log n)$** | **$O(1)$** or **$O(n)$** | **Acceptable**: Sorts array and checks adjacent pairs. |
| **3. Hash Set** | `HashSet<int>` | **$O(n)$** | **$O(n)$** | **Recommended (Optimal)**: Linear time with fast hash lookups. |

---

## 3. Recommended Solution: Hash Set Single-Pass

### Why HashSet is Recommended:
A `HashSet<T>` in .NET uses a bucketed hash table.
- Lookup: **$O(1)$** average time.
- Insertion: **$O(1)$** average time.
- In C#, `set.Add(num)` returns a `bool`:
  - `true` if the item was newly added.
  - `false` if the item **already existed** in the set.

By testing `if (!seen.Add(num)) return true;`, we detect duplicates immediately on first collision without needing a separate `Contains()` call!

### Clean C# Implementation:
```csharp
public class Solution
{
    public bool ContainsDuplicate(int[] nums)
    {
        if (nums == null || nums.Length <= 1)
        {
            return false;
        }

        var seen = new HashSet<int>();

        foreach (int num in nums)
        {
            // If Add returns false, 'num' is a duplicate!
            if (!seen.Add(num))
            {
                return true;
            }
        }

        return false;
    }
}
```

---

## 4. Line-by-Line Code Explanation

```csharp
// Line 1: Check edge cases. An array with 0 or 1 element can never contain duplicates.
if (nums == null || nums.Length <= 1) return false;

// Line 2: Allocate an empty hash set to track numbers encountered so far.
var seen = new HashSet<int>();

// Line 3: Iterate through each number in the array sequentially.
foreach (int num in nums)
{
    // Line 4: Attempt to insert 'num'. If it already exists, Add() returns false.
    if (!seen.Add(num))
    {
        // Line 5: Duplicate found! Short-circuit and exit immediately.
        return true;
    }
}

// Line 6: If loop finishes with zero collisions, all elements are unique.
return false;
```

---

## 5. Dry Run Traces

### Trace 1: `nums = [1, 2, 3, 1]`
1. `num = 1`: `seen.Add(1)` $\rightarrow$ `true`. Set: `{ 1 }`.
2. `num = 2`: `seen.Add(2)` $\rightarrow$ `true`. Set: `{ 1, 2 }`.
3. `num = 3`: `seen.Add(3)` $\rightarrow$ `true`. Set: `{ 1, 2, 3 }`.
4. `num = 1`: `seen.Add(1)` $\rightarrow$ **`false`** (Already present!). Returns **`true`**.

### Trace 2: `nums = [1, 2, 3, 4]`
1. `1, 2, 3, 4` each return `true` on `Add`.
2. Loop terminates. Returns **`false`**.

---

## 6. Test Cases & Verification Results

All test cases are verified via the standalone test runner inside [`ContainsDuplicate.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/docs/Week-1/Day-3/ContainsDuplicate.cs):

| Test Case | Input | Expected Output | Actual Output | Local Status |
| :--- | :--- | :---: | :---: | :---: |
| **Test 1: Duplicate at End** | `[1, 2, 3, 1]` | `true` | `true` | **PASSED [OK]** |
| **Test 2: Duplicate at Start** | `[1, 1, 2, 3]` | `true` | `true` | **PASSED [OK]** |
| **Test 3: No Duplicates** | `[1, 2, 3, 4]` | `false` | `false` | **PASSED [OK]** |
| **Test 4: Single Element** | `[7]` | `false` | `false` | **PASSED [OK]** |
| **Test 5: Multiple Repeats** | `[1, 1, 1, 3, 3, 4, 3, 2, 4, 2]` | `true` | `true` | **PASSED [OK]** |
| **Test 6: Empty Array** | `[]` | `false` | `false` | **PASSED [OK]** |

---

## 7. Local Verification vs. Online LeetCode Acceptance

> [!IMPORTANT]
> **Distinction Between Local Verification and Online Acceptance:**
> - **Local Verification:** Confirms algorithmic accuracy, compilation, edge case handling, and test harness execution on the local machine using the .NET 9 runtime.
> - **Online LeetCode Acceptance:** Requires logging into your account at [LeetCode 217](https://leetcode.com/problems/contains-duplicate/), pasting the code from section 3, and clicking **Submit**.
> - In accordance with strict audit rules, we do not claim online acceptance until you have submitted the code through your authenticated LeetCode web account.
