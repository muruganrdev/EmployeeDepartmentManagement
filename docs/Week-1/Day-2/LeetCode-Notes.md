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

## 2. Why We Use `% 2`, `/ 2`, and `- 1`

Understanding the arithmetic operators in this problem:

1. **`num % 2 == 0` (Modulo Parity Check)**:
   - In integer arithmetic, `num % 2` calculates the remainder when `num` is divided by `2`.
   - Any even number divided by 2 leaves a remainder of `0` (`14 % 2 == 0`).
   - Any odd number leaves a remainder of `1` (`7 % 2 == 1`).
   - Thus, `num % 2 == 0` accurately determines if the number is even in constant time $O(1)$.

2. **`num /= 2` (Integer Division)**:
   - When `num` is even, dividing by 2 is the fastest way to shrink the magnitude of the number towards zero.
   - In binary representation, dividing an even number by 2 corresponds to shifting all bits one position to the right (`num >> 1`), eliminating the trailing zero bit.

3. **`num -= 1` (Subtraction by One)**:
   - An odd number cannot be evenly divided by 2 without losing precision in integer arithmetic.
   - Subtracting 1 immediately converts the odd number into an even number (e.g., $7 - 1 = 6$, $3 - 1 = 2$, $1 - 1 = 0$), preparing it for subsequent division by 2 on the next step.

---

## 3. Algorithm Walkthrough & Examples

### Example 1: `num = 14`
1. Step 1: `14` is even $\rightarrow$ `14 / 2 = 7`
2. Step 2: `7` is odd $\rightarrow$ `7 - 1 = 6`
3. Step 3: `6` is even $\rightarrow$ `6 / 2 = 3`
4. Step 4: `3` is odd $\rightarrow$ `3 - 1 = 2`
5. Step 5: `2` is even $\rightarrow$ `2 / 2 = 1`
6. Step 6: `1` is odd $\rightarrow$ `1 - 1 = 0`
- Total steps: **6**

### Example 2: `num = 8`
- `8` $\rightarrow$ `4` $\rightarrow$ `2` $\rightarrow$ `1` $\rightarrow$ `0`
- Total steps: **4**

### Example 3: `num = 123`
- Odd/Even alternation reduces 123 to 0 in **12** steps.

### Example 4 (Boundary / Edge Case): `num = 0`
- Condition `while (num > 0)` is immediately false; loop executes 0 times.
- Result: **0** steps.

### Example 5 (Boundary / Edge Case): `num = 1`
- `1` is odd $\rightarrow$ `1 - 1 = 0`.
- Result: **1** step.

---

## 4. Complexity Analysis

| Metric | Complexity | Explanation |
| :--- | :--- | :--- |
| **Time Complexity** | **O(log n)** | Each division by 2 cuts the number in half. In binary, subtracting 1 flips the lowest bit from 1 to 0, and dividing by 2 shifts bits right. At most $2 \times \lfloor\log_2(n)\rfloor$ operations occur. For $n \le 10^6$, $\le 40$ steps are needed. |
| **Space Complexity** | **O(1)** | Requires only a single integer `steps` counter. No memory allocation occurs. |

---

## 5. Local Verification vs. Online LeetCode Acceptance

### Local Test Harness Results
All five test cases are verified via the standalone test runner inside [`NumberOfStepsToReduceNumberToZero.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/docs/Week-1/Day-2/NumberOfStepsToReduceNumberToZero.cs):

| Test Case | Input | Expected Output | Actual Output | Local Status |
| :--- | :---: | :---: | :---: | :---: |
| Test 1 | 14 | 6 | 6 | **PASSED** |
| Test 2 | 8 | 4 | 4 | **PASSED** |
| Test 3 | 123 | 12 | 12 | **PASSED** |
| Test 4 (Edge: Zero) | 0 | 0 | 0 | **PASSED** |
| Test 5 (Edge: One) | 1 | 1 | 1 | **PASSED** |

### Online LeetCode Submission Note
> [!IMPORTANT]
> **Distinction Between Local Verification and Online Acceptance:**
> - Local test execution validates algorithmic correctness, boundary edge cases, and runtime behavior against the local .NET compiler.
> - **Online LeetCode Acceptance** requires logging into [LeetCode 1342](https://leetcode.com/problems/number-of-steps-to-reduce-a-number-to-zero/) in your web browser, pasting the exact code block below into the C# editor, and clicking **Submit**.
> - In accordance with strict audit rules, we do not claim online acceptance until you have submitted the code through your authenticated LeetCode account.

### Exact LeetCode Web Submission Code
```csharp
public class Solution
{
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
}
```
