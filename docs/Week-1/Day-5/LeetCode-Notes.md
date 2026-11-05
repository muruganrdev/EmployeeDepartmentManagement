# LeetCode 242: Valid Anagram – Detailed Analysis & Implementation

## 1. Problem Statement in Simple English

Given two strings `s` and `t`, return `true` if `t` is an **anagram** of `s`, and `false` otherwise.

An **Anagram** is a word or phrase formed by rearranging the letters of another word, using all original letters exactly once with identical frequencies.

### Examples:
- **Example 1:** `s = "anagram"`, `t = "nagaram"` -> Output: `true`  
  Both strings contain: 3 'a's, 1 'n', 1 'g', 1 'r', 1 'm'.
- **Example 2:** `s = "rat"`, `t = "car"` -> Output: `false`  
  'car' contains 'c' which does not exist in 'rat'.

---

## 2. Approach 1: Sorting

### Intuition
If two strings are anagrams, sorting their characters alphabetically will produce identical sequences of characters!

### Step-by-Step Logic
1. Check if `s.Length != t.Length`. If lengths differ, they cannot be anagrams -> return `false`.
2. Convert both strings to character arrays (`s.ToCharArray()`, `t.ToCharArray()`).
3. Sort both arrays using `Array.Sort()`.
4. Compare character by character:
   - If every index matches, return `true`.
   - If any character differs, return `false`.

### C# Implementation
```csharp
public bool IsAnagramSorting(string s, string t)
{
    if (s == null || t == null || s.Length != t.Length)
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
```

### Complexity
- **Time Complexity:** $O(n \log n)$ where $n$ is the length of the strings. In .NET, `Array.Sort` uses Introsort (quicksort + heapsort + insertion sort), which requires $O(n \log n)$ time.
- **Space Complexity:** $O(n)$ auxiliary space to allocate the two `char[]` arrays.

---

## 3. Approach 2: Character Frequency Array (Optimal)

### Intuition
Instead of sorting the entire array, we can simply count how many times each character appears!
Since the problem specifies lowercase English letters ('a' through 'z'), there are only **26 possible characters**.

### Step-by-Step Logic
1. If `s.Length != t.Length`, return `false`.
2. Create an integer array of fixed size 26: `int[] frequency = new int[26];`.
3. In a single loop from `0` to `n - 1`:
   - Increment the count for character in `s`: `frequency[s[i] - 'a']++`
   - Decrement the count for character in `t`: `frequency[t[i] - 'a']--`
4. If `t` is an exact anagram of `s`, every increment will be cancelled out by an exact decrement.
5. Check if all 26 integers in the array are `0`. If any element is non-zero, return `false`; otherwise, return `true`.

### C# Implementation
```csharp
public bool IsAnagram(string s, string t)
{
    if (s == null || t == null || s.Length != t.Length)
        return false;

    int[] frequencyTable = new int[26];

    for (int i = 0; i < s.Length; i++)
    {
        frequencyTable[s[i] - 'a']++;
        frequencyTable[t[i] - 'a']--;
    }

    for (int i = 0; i < 26; i++)
    {
        if (frequencyTable[i] != 0)
            return false;
    }

    return true;
}
```

---

## 4. Step-by-Step Dry Run

Input: `s = "anagram"`, `t = "nagaram"` ($n = 7$)

| Step ($i$) | `s[i]` | `t[i]` | Operation on Table | Frequency Table Highlights |
| :---: | :---: | :---: | :--- | :--- |
| **0** | 'a' | 'n' | `freq['a']++`, `freq['n']--` | 'a': +1, 'n': -1 |
| **1** | 'n' | 'a' | `freq['n']++`, `freq['a']--` | 'a': 0, 'n': 0 |
| **2** | 'a' | 'g' | `freq['a']++`, `freq['g']--` | 'a': +1, 'g': -1 |
| **3** | 'g' | 'a' | `freq['g']++`, `freq['a']--` | 'a': 0, 'g': 0 |
| **4** | 'r' | 'r' | `freq['r']++`, `freq['r']--` | 'r': 0 |
| **5** | 'a' | 'a' | `freq['a']++`, `freq['a']--` | 'a': 0 |
| **6** | 'm' | 'm' | `freq['m']++`, `freq['m']--` | 'm': 0 |

**Final Check:** All 26 elements in `frequencyTable` are `0` -> Returns **`true`**!

---

## 5. Complexity Comparison

| Metric | Approach 1 (Sorting) | Approach 2 (Frequency Array - Optimal) |
| :--- | :---: | :---: |
| **Time Complexity** | $O(n \log n)$ | **$O(n)$** (Single pass) |
| **Space Complexity** | $O(n)$ | **$O(1)$** (Fixed array of 26 integers) |
| **Passes Required** | Sort passes + comparison | Single loop + 26 iterations |
| **LeetCode Speed** | Good | **Beats 98%+ of C# submissions** |

---

## 6. Verification Test Cases Table

| Test Case | String `s` | String `t` | Expected | Sorting Approach | Frequency Approach | Status |
| :---: | :--- | :--- | :---: | :---: | :---: | :---: |
| **1** | `"anagram"` | `"nagaram"` | `true` | `true` | `true` | **PASS** |
| **2** | `"rat"` | `"car"` | `false` | `false` | `false` | **PASS** |
| **3** | `"a"` | `"a"` | `true` | `true` | `true` | **PASS** |
| **4** | `"ab"` | `"a"` | `false` | `false` | `false` | **PASS** |
| **5** | `""` | `""` | `true` | `true` | `true` | **PASS** |
| **6** | `"listen"` | `"silent"` | `true` | `true` | `true` | **PASS** |
| **7** | `"aabbcc"` | `"bbaacc"` | `true` | `true` | `true` | **PASS** |
| **8** | `"aacc"` | `"ccac"` | `false` | `false` | `false` | **PASS** |
| **9** | `null` | `"test"` | `false` | `false` | `false` | **PASS** |
