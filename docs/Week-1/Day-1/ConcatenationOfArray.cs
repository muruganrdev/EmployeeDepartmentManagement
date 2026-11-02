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
                ans[i] = nums[i];      
                ans[i + n] = nums[i];   
            }

            return ans;
        }

        public static void Main(string[] args)
        {
            var solution = new Solution();

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
