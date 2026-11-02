# LeetCode 1342: Number of Steps to Reduce a Number to Zero – Detailed Notes

- **Problem Link**: [LeetCode 1342 - Number of Steps to Reduce a Number to Zero](https://leetcode.com/problems/number-of-steps-to-reduce-a-number-to-zero/)
- **Solution File**: [`NumberOfStepsToReduceNumberToZero.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/docs/Week-1/Day-2/NumberOfStepsToReduceNumberToZero.cs)
- **Difficulty**: Easy
- **Tags**: Math, Bit Manipulation, Simulation

---

## 1. Problem Description

Given an integer `num`, return the number of steps to reduce it to zero.

In one step:
- If the current number is **even**, divide it by `2`.
- Otherwise (if it is **odd**), subtract `1` from it.

### Constraints:
- `0 <= num <= 10^6`

---

## 2. Examples & Trace

### Example 1:
- **Input**: `num = 14`
- **Output**: `6`
- **Trace**:
  1. `14` is even $\rightarrow$ `14 / 2 = 7` (Step 1)
  2. `7` is odd $\rightarrow$ `7 - 1 = 6` (Step 2)
  3. `6` is even $\rightarrow$ `6 / 2 = 3` (Step 3)
  4. `3` is odd $\rightarrow$ `3 - 1 = 2` (Step 4)
  5. `2` is even $\rightarrow$ `2 / 2 = 1` (Step 5)
  6. `1` is odd $\rightarrow$ `1 - 1 = 0` (Step 6)
  Total steps: **6**.

### Example 2:
- **Input**: `num = 8`
- **Output**: `4` (`8 -> 4 -> 2 -> 1 -> 0`)

### Example 3 (Edge Case):
- **Input**: `num = 0`
- **Output**: `0` (Loop does not execute; already zero).

---

## 3. Algorithmic Approaches

### Approach 1: Iterative Simulation (Arithmetic)
We maintain a `steps` counter initialized to `0`. A `while (num > 0)` loop checks parity via modulo:
- `num % 2 == 0`: divide by 2.
- `else`: subtract 1.
- Increment `steps` on each operation.

```csharp
public int NumberOfSteps(int num)
{
    int steps = 0;
    while (num > 0)
    {
        if (num % 2 == 0)
        {
            num /= 2;
        }
        else
        {
            num -= 1;
        }
        steps++;
    }
    return steps;
}
```

### Approach 2: Bit Manipulation (Optimization)
In binary representation:
- Checking if a number is even: `(num & 1) == 0`.
- Dividing by 2: bitwise right-shift `num >>= 1`.
- Subtracting 1 from an odd number: bitwise XOR `num ^= 1`.

---

## 4. Complexity Analysis

| Metric | Complexity | Explanation |
| :--- | :--- | :--- |
| **Time Complexity** | **O(log n)** | Dividing by 2 in at least every other step reduces the binary length of $n$ by 1 bit. A 32-bit integer takes at most $2 \times 32 = 64$ operations. |
| **Space Complexity** | **O(1)** | Operates in-place using only a single 32-bit integer counter. |

---

## 5. Verification and Test Results

Tested in [`NumberOfStepsToReduceNumberToZero.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/docs/Week-1/Day-2/NumberOfStepsToReduceNumberToZero.cs):

1. `num = 14` $\rightarrow$ Expected: `6` $\rightarrow$ Actual: `6` **[PASSED]**
2. `num = 8` $\rightarrow$ Expected: `4` $\rightarrow$ Actual: `4` **[PASSED]**
3. `num = 123` $\rightarrow$ Expected: `12` $\rightarrow$ Actual: `12` **[PASSED]**
4. `num = 0` (Edge Case) $\rightarrow$ Expected: `0` $\rightarrow$ Actual: `0` **[PASSED]**
5. `num = 1` (Edge Case) $\rightarrow$ Expected: `1` $\rightarrow$ Actual: `1` **[PASSED]**

---

## 6. LeetCode Online Submission Format

```csharp
public class Solution {
    public int NumberOfSteps(int num) {
        int steps = 0;
        while (num > 0) {
            num = (num % 2 == 0) ? num / 2 : num - 1;
            steps++;
        }
        return steps;
    }
}
```
