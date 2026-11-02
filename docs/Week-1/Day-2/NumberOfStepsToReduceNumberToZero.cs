using System;

namespace LeetCode.Day02
{
    /// <summary>
    /// LeetCode 1342: Number of Steps to Reduce a Number to Zero
    /// Problem URL: https://leetcode.com/problems/number-of-steps-to-reduce-a-number-to-zero/
    /// 
    /// Problem Understanding:
    /// Given an integer num, return the number of steps to reduce it to zero.
    /// In one step, if the current number is even, divide it by 2;
    /// otherwise, subtract 1 from it.
    /// 
    /// Example 1:
    /// Input: num = 14
    /// Output: 6
    /// Explanation: 
    /// Step 1) 14 is even; divide by 2 and obtain 7. 
    /// Step 2) 7 is odd; subtract 1 and obtain 6.
    /// Step 3) 6 is even; divide by 2 and obtain 3. 
    /// Step 4) 3 is odd; subtract 1 and obtain 2. 
    /// Step 5) 2 is even; divide by 2 and obtain 1. 
    /// Step 6) 1 is odd; subtract 1 and obtain 0.
    /// 
    /// Example 2:
    /// Input: num = 8
    /// Output: 4
    /// 
    /// Example 3 (Edge Case):
    /// Input: num = 0
    /// Output: 0
    /// </summary>
    public class Solution
    {
        /// <summary>
        /// Approach 1: Iterative Simulation (Arithmetic)
        /// Time Complexity: O(log n) - In each step, we either divide by 2 or subtract 1 (which turns an odd number even).
        /// Space Complexity: O(1) - Only a single counter variable is used.
        /// </summary>
        public int NumberOfSteps(int num)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException("num", "Input number must be non-negative.");
            }

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

        /// <summary>
        /// Approach 2: Bit Manipulation (Alternative high-performance approach)
        /// Even check: (num & 1) == 0
        /// Divide by 2: num >>= 1
        /// </summary>
        public int NumberOfStepsBitwise(int num)
        {
            int steps = 0;
            while (num > 0)
            {
                if ((num & 1) == 0)
                {
                    num >>= 1; // Bit shift right divides by 2
                }
                else
                {
                    num ^= 1;  // Flip lowest bit to subtract 1
                }
                steps++;
            }
            return steps;
        }

        /// <summary>
        /// Standalone verification harness with automated test cases.
        /// </summary>
        public static void Main(string[] args)
        {
            var solution = new Solution();

            Console.WriteLine("=================================================================");
            Console.WriteLine(" LeetCode 1342: Number of Steps to Reduce a Number to Zero Tests ");
            Console.WriteLine("=================================================================");

            // Test Case 1: Standard even number
            RunTest(solution, "Test 1 (num = 14)", 14, 6);

            // Test Case 2: Power of two
            RunTest(solution, "Test 2 (num = 8)", 8, 4);

            // Test Case 3: Odd number
            RunTest(solution, "Test 3 (num = 123)", 123, 12);

            // Test Case 4 (Edge Case): Zero
            RunTest(solution, "Test 4 (Edge Case: num = 0)", 0, 0);

            // Test Case 5 (Edge Case): One
            RunTest(solution, "Test 5 (Edge Case: num = 1)", 1, 1);

            Console.WriteLine("\nAll test cases passed successfully!");
        }

        private static void RunTest(Solution solution, string testName, int input, int expected)
        {
            int result = solution.NumberOfSteps(input);
            int bitwiseResult = solution.NumberOfStepsBitwise(input);
            bool passed = (result == expected) && (bitwiseResult == expected);

            Console.WriteLine(string.Format("\n{0}:", testName));
            Console.WriteLine(string.Format("  Input   : {0}", input));
            Console.WriteLine(string.Format("  Expected: {0}", expected));
            Console.WriteLine(string.Format("  Actual  : {0} (Bitwise: {1})", result, bitwiseResult));
            Console.WriteLine(string.Format("  Status  : {0}", (passed ? "PASSED [OK]" : "FAILED [X]")));

            if (!passed)
            {
                throw new Exception("Test '" + testName + "' failed!");
            }
        }
    }
}
