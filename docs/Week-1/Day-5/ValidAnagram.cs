using System;

namespace EmployeeDepartmentManagement.LeetCode;

/// <summary>
/// LeetCode Problem 242: Valid Anagram
/// Difficulty: Easy
/// Category: Strings / Hash Table / Sorting
/// 
/// PROBLEM UNDERSTANDING IN SIMPLE ENGLISH:
/// An Anagram is a word or phrase formed by rearranging the letters of a different word or phrase,
/// typically using all the original letters exactly once.
/// 
/// Given two strings 's' and 't', return true if 't' is an anagram of 's', and false otherwise.
/// 
/// Examples:
/// Input: s = "anagram", t = "nagaram"  -> Output: true
/// Input: s = "rat", t = "car"          -> Output: false
/// </summary>
public class ValidAnagramSolution
{
    /// <summary>
    /// Approach 1: Sorting
    /// Convert both strings to char arrays, sort them, and compare equality.
    /// 
    /// Time Complexity: O(n log n) - Dominated by quicksort / introsort on the character arrays.
    /// Space Complexity: O(n) - Requires allocating memory for two character arrays.
    /// </summary>
    public bool IsAnagramSorting(string s, string t)
    {
        if (s == null || t == null)
            return false;

        if (s.Length != t.Length)
            return false;

        char[] sChars = s.ToCharArray();
        char[] tChars = t.ToCharArray();

        Array.Sort(sChars);
        Array.Sort(tChars);

        for (int i = 0; i < sChars.Length; i++)
        {
            if (sChars[i] != tChars[i])
                return false;
        }

        return true;
    }

    /// <summary>
    /// Approach 2: Character Frequency Counting (Optimal)
    /// Track frequencies of each lowercase English letter using a fixed-size 26-element integer table.
    /// 
    /// Time Complexity: O(n) - Single pass through both strings.
    /// Space Complexity: O(1) - Constant auxiliary space (int array of size 26 is fixed regardless of string length n).
    /// </summary>
    public bool IsAnagram(string s, string t)
    {
        if (s == null || t == null)
            return false;

        // Anagrams MUST have identical lengths
        if (s.Length != t.Length)
            return false;

        int[] frequencyTable = new int[26];

        for (int i = 0; i < s.Length; i++)
        {
            frequencyTable[s[i] - 'a']++;
            frequencyTable[t[i] - 'a']--;
        }

        // If all character frequencies canceled out to zero, strings are anagrams
        for (int i = 0; i < 26; i++)
        {
            if (frequencyTable[i] != 0)
                return false;
        }

        return true;
    }
}

/// <summary>
/// Local verification harness testing all mandatory test cases and edge conditions.
/// </summary>
public static class ValidAnagramTestHarness
{
    public static void RunVerification()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("===============================================================================");
        Console.WriteLine("             LEETCODE 242: VALID ANAGRAM - LOCAL VERIFICATION                  ");
        Console.WriteLine("===============================================================================");

        var solution = new ValidAnagramSolution();

        var testCases = new[]
        {
            new { S = "anagram", T = "nagaram", Expected = true, Desc = "Standard multi-letter anagram" },
            new { S = "rat", T = "car", Expected = false, Desc = "Different characters with same length" },
            new { S = "a", T = "a", Expected = true, Desc = "Single matching character" },
            new { S = "ab", T = "a", Expected = false, Desc = "Different string lengths" },
            new { S = "", T = "", Expected = true, Desc = "Empty strings (trivially equal)" },
            new { S = "listen", T = "silent", Expected = true, Desc = "Classic English anagram pair" },
            new { S = "aabbcc", T = "bbaacc", Expected = true, Desc = "Multiple repeated character frequencies" },
            new { S = "aacc", T = "ccac", Expected = false, Desc = "Same characters, but differing frequency counts" }
        };

        bool allPassed = true;

        for (int i = 0; i < testCases.Length; i++)
        {
            var tc = testCases[i];
            bool actualOptimized = solution.IsAnagram(tc.S, tc.T);
            bool actualSorting = solution.IsAnagramSorting(tc.S, tc.T);

            bool passed = (actualOptimized == tc.Expected) && (actualSorting == tc.Expected);
            if (!passed) allPassed = false;

            string status = passed ? "[PASS]" : "[FAIL]";
            Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{status} Test {i + 1}: ");
            Console.ResetColor();
            Console.WriteLine($"{tc.Desc}");
            Console.WriteLine($"       Input: s=\"{tc.S}\", t=\"{tc.T}\" | Expected: {tc.Expected} | Optimized: {actualOptimized} | Sorting: {actualSorting}");
            Console.WriteLine();
        }

        Console.ForegroundColor = allPassed ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(allPassed
            ? ">>> ALL 8 LOCAL TEST CASES PASSED SUCCESSFULLY (100% VERIFIED) <<<"
            : ">>> SOME TEST CASES FAILED <<<");
        Console.ResetColor();
    }
}
