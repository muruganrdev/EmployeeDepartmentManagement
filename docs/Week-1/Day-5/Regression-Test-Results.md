# Regression Test Results – Week 1 Final Verification

## 1. Solution Build Output

Command:
```powershell
dotnet clean
dotnet restore
dotnet build -m:1 /p:UseSharedCompilation=false --configuration Release
```

### Actual Build Output
```text
  Determining projects to restore...
  All projects are up-to-date for restore.
  EmployeeDepartmentManagement.Core -> C:\Local Disk D\Murugan Project\Projects\test\EmployeeDepartmentManagement\src\EmployeeDepartmentManagement.Core\bin\Release\net9.0\EmployeeDepartmentManagement.Core.dll
  EmployeeDepartmentManagement.App -> C:\Local Disk D\Murugan Project\Projects\test\EmployeeDepartmentManagement\src\EmployeeDepartmentManagement.App\bin\Release\net9.0\EmployeeDepartmentManagement.App.dll
  EmployeeDepartmentManagement.Tests -> C:\Local Disk D\Murugan Project\Projects\test\EmployeeDepartmentManagement\tests\EmployeeDepartmentManagement.Tests\bin\Release\net9.0\EmployeeDepartmentManagement.Tests.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:03.12
```

---

## 2. Automated xUnit Test Suite Results

Command:
```powershell
dotnet test --configuration Release
```

### Actual Test Execution Output
```text
Test run for C:\Local Disk D\Murugan Project\Projects\test\EmployeeDepartmentManagement\tests\EmployeeDepartmentManagement.Tests\bin\Release\net9.0\EmployeeDepartmentManagement.Tests.dll (.NETCoreApp,Version=v9.0)
Microsoft (R) Test Execution Command Line Tool Version 17.12.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    83, Skipped:     0, Total:    83, Duration: 1 s - EmployeeDepartmentManagement.Tests.dll (net9.0)
```

---

## 3. Comprehensive Test Breakdown by Class

| Test Class | Focus Area | Executions | Status |
| :--- | :--- | :---: | :---: |
| **`EmployeeServiceTests`** | Core CRUD, Invariants, Events, Null Guards, Boundary Regression | 16 | **PASS** |
| **`SalaryValidationTheoryTests`** | `[Theory]` & `[InlineData]` Parameterized Boundary Tests | 22 | **PASS** |
| **`EmployeeQueryServiceTests`** | LINQ Reporting, `Where`, `OrderBy`, `MaxBy`, `Assert.Throws` | 6 | **PASS** |
| **`EmployeeAsyncServiceTests`** | Non-blocking async, Stream I/O, `CancellationToken` cancellation | 7 | **PASS** |
| **`EmployeeMockingTests`** | Moq test doubles (`Setup`, `Returns`, `Verify`, `Times.Once`) | 2 | **PASS** |
| **`LeetCode121Tests`** | LeetCode 121 (Stock Profit: Single-Pass vs Brute-Force) | 13 | **PASS** |
| **`LeetCode242Tests`** | LeetCode 242 (Valid Anagram: Frequency Array vs Sorting) | 17 | **PASS** |
| **TOTAL** | | **83** | **100% PASS** |

---

## 4. Manual Menu Regression Verification

All existing Day 1–4 console options were executed interactively:
- **Option 1 (View Roster):** Renders all 5 employees with accurate currency formatting.
- **Option 2 (Add Employee):** Defensively guards against null, duplicates, under-age candidates, and future joining dates.
- **Option 3 (Search by ID):** $O(1)$ dictionary lookup returns existing employee or friendly not-found message.
- **Option 4 (Search by Dept):** LINQ-refactored case-insensitive matching functions seamlessly.
- **Option 5 (Payroll Statistics):** Correct aggregate figures computed without arithmetic overflow.
- **Option 6 (Day 2 OOP & Orders):** Polymorphism, record value equality, struct value semantics, and Order management inventory deductions verified.
- **Option 8 (Day 3 LINQ Deep Dive):** 8 queries, Generics, Lambdas, Delegates, Events, Deferred Execution, Extensions, Pattern matching verified.
- **Option 9 (Day 4 Async & Cancellation):** Stream export, CancellationToken cancellation, and LeetCode 121 verified.
- **Options 7 & 10 (Exit):** Clean exit without errors.
