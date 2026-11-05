# Week 1 – Day 5: Code Review, Refactoring, Debugging & Quality Assurance – Concepts

## 1. What is Code Review?
Code review is the disciplined practice where developers inspect source code written by themselves or peers before it is merged into production branches.

### Why Do We Need Code Review?
1. **Defect Prevention:** Catches logic errors, unhandled null references, and edge-case boundary flaws before they reach users.
2. **Knowledge Sharing:** Fosters pair learning between junior and senior developers.
3. **Consistency & Standards:** Ensures naming conventions, formatting, and architectural patterns (e.g., SOLID) are upheld.
4. **Maintainability:** Prevents technical debt by questioning redundant code and over-engineered abstractions.

---

## 2. What is Code Refactoring?
**Refactoring** is the disciplined process of restructuring existing computer code—changing the internal structure—**without changing its external behavior**.

### Martin Fowler's Two Hats Metaphor
When developing software, a developer wears two different hats:
- **Feature Hat:** Adding new functionality, creating new methods, changing existing public behavior.
- **Refactoring Hat:** Restructuring existing code, improving readability, eliminating duplication, enhancing performance, while keeping all existing unit tests 100% green!

### Core Refactoring Smells Addressed:
1. **Dead Code:** Removing variables calculated but never used (`decimal oldGross`).
2. **Defensive Programming:** Adding explicit null guards (`if (emp is null)`) to prevent runtime crashes.
3. **Premature Allocation:** Replacing repeated `.ToLowerInvariant()` string allocations with zero-allocation `StringComparison.OrdinalIgnoreCase`.
4. **Style Inconsistency:** Replacing manual loop accumulators with declarative LINQ expressions (`Where`, `ToList`).

---

## 3. What is Software Debugging?
**Debugging** is the methodical, scientific process of detecting, isolating, and fixing defects (bugs) in software.

### The 9-Step Professional Debugging Cycle:
1. **Understand the Report:** Read the bug report or user complaint carefully. What was expected? What actually happened?
2. **Reproduce the Issue:** Create a deterministic minimal test case that triggers the defect every single time.
3. **Isolate the Component:** Narrow down which class and method is responsible.
4. **Set Breakpoints:** Pause program execution right before the suspect code runs.
5. **Inspect Variable State:** Examine locals, parameters, and expressions in the Watch window.
6. **Identify the Root Cause:** Distinguish symptoms from the actual root cause (e.g. an off-by-one operator `>=` vs `>`).
7. **Apply the Minimal Correct Fix:** Correct the flaw without introducing unintended side effects.
8. **Add a Regression Test:** Write an automated unit test that would have caught the bug.
9. **Run the Full Test Suite:** Verify that the fix did not break any other parts of the system.

---

## 4. Defensive Programming and Edge-Case Handling
Defensive programming is an approach to software design where code anticipates unexpected or malicious inputs and handles them safely without crashing.

### Key Edge Cases in Employee Management:
- **Null References:** `null` objects passed to methods (`emp == null`).
- **String Edge Cases:** `null`, empty `""`, whitespace-only `"   "`, or strings with leading/trailing spaces.
- **Numeric Boundaries:** Negative salaries (`-500`), zero compensation (`0.0`), extreme values.
- **Temporal Boundaries:** Future joining dates, contract end dates in the past.
- **Relational Invariants:** Experience exceeding legal age ($Age - 16$).
- **Collection Boundaries:** Empty lists (`Count == 0`), single-element lists, and duplicate IDs.

---

## 5. Regression Testing
Regression testing is the practice of running an existing suite of tests after code changes to confirm that previously working features have not "regressed" (broken).

In our project:
- **Day 1** Roster & CRUD operations
- **Day 2** OOP Polymorphism & Order Management
- **Day 3** LINQ Queries & Language Features
- **Day 4** Async/Await & CancellationToken
- **Day 5** Refactored methods and LeetCode solutions

All 83 automated unit tests execute in under 1 second, giving instantaneous confirmation of zero regressions.
