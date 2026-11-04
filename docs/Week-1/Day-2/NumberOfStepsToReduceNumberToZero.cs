using System;

namespace LeetCode.Day02
{
    public class Solution
    {
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
