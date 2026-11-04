using System;
using System.Collections.Generic;

namespace LeetCode.Day03
{
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
                // HashSet.Add(x) returns false if x already exists in the set
                if (!seen.Add(num))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsDuplicateSorting(int[] nums)
        {
            if (nums == null || nums.Length <= 1)
            {
                return false;
            }

            int[] copy = (int[])nums.Clone();
            Array.Sort(copy);

            for (int i = 0; i < copy.Length - 1; i++)
            {
                if (copy[i] == copy[i + 1])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Approach 3: Brute Force (Nested Loops) - Demonstration only
        /// Compare each pair of elements.
        /// 
        /// Time Complexity: O(n^2) - Impractical for large inputs (n > 10,000).
        /// Space Complexity: O(1) - No extra memory.
        /// </summary>
        public bool ContainsDuplicateBruteForce(int[] nums)
        {
            if (nums == null || nums.Length <= 1)
            {
                return false;
            }

            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[i] == nums[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Standalone local verification harness testing all boundary scenarios.
        /// </summary>
        public static void Main(string[] args)
        {
            var solution = new Solution();

            Console.WriteLine("=================================================================");
            Console.WriteLine("         LeetCode 217: Contains Duplicate - Test Harness         ");
            Console.WriteLine("=================================================================");

            // Test Case 1: Duplicate at end
            RunTest(solution, "Test 1: Duplicate at End", new int[] { 1, 2, 3, 1 }, true);

            // Test Case 2: Duplicate at beginning
            RunTest(solution, "Test 2: Duplicate at Beginning", new int[] { 1, 1, 2, 3 }, true);

            // Test Case 3: No duplicates
            RunTest(solution, "Test 3: No Duplicates (All Distinct)", new int[] { 1, 2, 3, 4 }, false);

            // Test Case 4: Single element
            RunTest(solution, "Test 4: Single Element", new int[] { 7 }, false);

            // Test Case 5: Multiple repeated values
            RunTest(solution, "Test 5: Multiple Repeated Values", new int[] { 1, 1, 1, 3, 3, 4, 3, 2, 4, 2 }, true);

            // Test Case 6: Empty array
            RunTest(solution, "Test 6: Empty Array", new int[] { }, false);

            Console.WriteLine("\nAll test cases passed successfully!");
        }

        private static void RunTest(Solution solution, string testName, int[] input, bool expected)
        {
            bool optimalResult = solution.ContainsDuplicate(input);
            bool sortResult = solution.ContainsDuplicateSorting(input);
            bool bruteResult = solution.ContainsDuplicateBruteForce(input);

            bool allMatch = (optimalResult == expected) && (sortResult == expected) && (bruteResult == expected);

            Console.WriteLine(string.Format("\n{0}:", testName));
            Console.WriteLine(string.Format("  Input   : [{0}]", string.Join(", ", input)));
            Console.WriteLine(string.Format("  Expected: {0}", expected));
            Console.WriteLine(string.Format("  Actual  : Optimal={0}, Sort={1}, Brute={2}", optimalResult, sortResult, bruteResult));
            Console.WriteLine(string.Format("  Status  : {0}", allMatch ? "PASSED [OK]" : "FAILED [X]"));

            if (!allMatch)
            {
                throw new Exception("Test '" + testName + "' failed!");
            }
        }
    }
}
