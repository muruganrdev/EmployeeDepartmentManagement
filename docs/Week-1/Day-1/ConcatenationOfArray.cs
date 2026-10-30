using System;

namespace LeetCode.Day01
{
    /// <summary>
    /// LeetCode 1929: Concatenation of Array
    /// Problem URL: https://leetcode.com/problems/concatenation-of-array/
    /// 
    /// Problem Understanding:
    /// Given an integer array nums of length n, create an array ans of length 2n where
    /// ans[i] == nums[i] and ans[i + n] == nums[i] for 0 <= i < n (0-indexed).
    /// Specifically, ans is the concatenation of two nums arrays.
    /// 
    /// Example 1:
    /// Input: nums = [1, 2, 1]
    /// Output: [1, 2, 1, 1, 2, 1]
    /// 
    /// Example 2:
    /// Input: nums = [1, 3, 2, 1]
    /// Output: [1, 3, 2, 1, 1, 3, 2, 1]
    /// </summary>
    public class Solution
    {
        /// <summary>
        /// Constructs an array of length 2n containing two concatenated copies of nums.
        /// 
        /// Approach:
        /// 1. Let n be the length of the input array nums.
        /// 2. Allocate a new integer array `ans` of size 2 * n.
        /// 3. Loop through index i from 0 to n - 1:
        ///    - Assign ans[i] = nums[i] (first half)
        ///    - Assign ans[i + n] = nums[i] (second half)
        /// 4. Return the populated ans array.
        /// 
        /// Time Complexity:
        /// O(n) - We perform a single loop running exactly n times, doing constant-time O(1) operations.
        /// 
        /// Space Complexity:
        /// O(n) - We allocate a new array of size 2n to store the answer. No extra auxiliary data structures.
        /// </summary>
        public int[] GetConcatenation(int[] nums)
        {
            if (nums == null)
            {
                throw new ArgumentNullException("nums");
            }

            int n = nums.Length;
            int[] ans = new int[2 * n];

            for (int i = 0; i < n; i++)
            {
                ans[i] = nums[i];       // First occurrence
                ans[i + n] = nums[i];   // Second occurrence
            }

            return ans;
        }

        /// <summary>
        /// Verification harness with comprehensive test cases including edge cases.
        /// </summary>
        public static void Main(string[] args)
        {
            var solution = new Solution();

            Console.WriteLine("=================================================");
            Console.WriteLine(" LeetCode 1929: Concatenation of Array - Tests   ");
            Console.WriteLine("=================================================");

            // Test Case 1: Standard case
            int[] test1 = new int[] { 1, 2, 1 };
            int[] expected1 = new int[] { 1, 2, 1, 1, 2, 1 };
            RunTest(solution, "Test 1 (Standard)", test1, expected1);

            // Test Case 2: Multi-element array
            int[] test2 = new int[] { 1, 3, 2, 1 };
            int[] expected2 = new int[] { 1, 3, 2, 1, 1, 3, 2, 1 };
            RunTest(solution, "Test 2 (Multi-element)", test2, expected2);

            // Test Case 3 (Edge Case): Single-element array (minimum constraint: 1 <= nums.length <= 1000)
            int[] test3 = new int[] { 7 };
            int[] expected3 = new int[] { 7, 7 };
            RunTest(solution, "Test 3 (Single Element)", test3, expected3);

            // Test Case 4 (Edge Case): All identical elements
            int[] test4 = new int[] { 0, 0, 0 };
            int[] expected4 = new int[] { 0, 0, 0, 0, 0, 0 };
            RunTest(solution, "Test 4 (All Zeros)", test4, expected4);

            Console.WriteLine("\nAll test cases passed successfully!");
        }

        private static void RunTest(Solution solution, string testName, int[] input, int[] expected)
        {
            int[] result = solution.GetConcatenation(input);
            bool passed = AreArraysEqual(result, expected);

            Console.WriteLine(string.Format("\n{0}:", testName));
            Console.WriteLine(string.Format("  Input   : [{0}]", string.Join(", ", input)));
            Console.WriteLine(string.Format("  Expected: [{0}]", string.Join(", ", expected)));
            Console.WriteLine(string.Format("  Actual  : [{0}]", string.Join(", ", result)));
            Console.WriteLine(string.Format("  Status  : {0}", (passed ? "PASSED [OK]" : "FAILED [X]")));

            if (!passed)
            {
                throw new Exception("Test '" + testName + "' failed!");
            }
        }

        private static bool AreArraysEqual(int[] a, int[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
    }
}
