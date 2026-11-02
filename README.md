# EmployeeDepartmentManagement

[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)

A clean, production-structured C# console application developed as part of **Week 1 – Day 1: C# Fundamentals** of comprehensive .NET training.

This application implements an in-memory **Employee and Department Management System** while demonstrating essential C# concepts: variables, fundamental types, operators, control flow, methods, value vs. reference type memory mechanics, collections, string handling, defensive input validation, and structured exception handling.

---

## Table of Contents
- [Project Overview](#project-overview)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Features Implemented](#features-implemented)
- [C# Concepts Demonstrated](#c-concepts-demonstrated)
- [How to Build and Run](#how-to-build-and-run)
- [Sample Input/Output Scenarios](#sample-inputoutput-scenarios)
- [Day 1 LeetCode Solution](#day-1-leetcode-solution)
- [Known Limitations](#known-limitations)
- [Documentation Roadmap](#documentation-roadmap)

---

## Project Overview

The **EmployeeDepartmentManagement** application serves as the core foundation for a 3-week progressive .NET training curriculum. Rather than throwing away code daily, this solution will be incrementally extended across upcoming training modules (OOP, LINQ, Unit Testing, SQL Server, Entity Framework Core, ASP.NET Core Web API, and frontends).

---

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (or compatible .NET 8 / 10 SDK)
- [Git](https://git-scm.com/)
- [GitHub CLI (`gh`)](https://cli.github.com/) (for repository management)
- Any modern IDE: Visual Studio 2022+, VS Code with C# Dev Kit, or JetBrains Rider.

---

## Project Structure

```
EmployeeDepartmentManagement/
├── EmployeeDepartmentManagement.sln               # Primary .NET Solution file
├── .gitignore                                      # Comprehensive .NET exclusion rules
├── README.md                                       # Solution documentation & run guide
│
├── src/
│   └── EmployeeDepartmentManagement.App/           # Console Application Project
│       ├── EmployeeDepartmentManagement.App.csproj # Project configuration (.NET 9.0)
│       ├── Program.cs                              # Console UI, menu loop, validation & live demo
│       ├── Models/
│       │   ├── Employee.cs                         # Core domain model (types, properties, calculations)
│       │   └── Department.cs                       # Department constants & array validation
│       └── Services/
│           └── EmployeeService.cs                  # In-memory store (List, Dictionary), search, payroll
│
└── docs/
    └── Week-1/
        └── Day-1/                                  # Day 1 learning notes & LeetCode solution
            ├── Concepts.md                         # Detailed C# fundamentals guide
            ├── Value-vs-Reference-Types.md         # Stack/Heap memory analysis & cloning
            ├── Execution-Flow.md                   # Runtime lifecycle & architectural diagrams
            ├── LeetCode-Notes.md                   # LeetCode 1929 complexity & test cases
            └── ConcatenationOfArray.cs             # Runnable LeetCode 1929 verification harness
```

---

## Features Implemented

1. **Tabular Employee Roster**: View seeded and newly added employees formatted into clean tabular columns with currency rendering.
2. **Interactive Employee Registration**: Add new employee records with multi-stage defensive validation (positive IDs, unique ID checks, valid corporate department matching, non-negative salary/bonus, age 18–75, and valid past joining dates).
3. **O(1) Employee Search by ID**: High-performance instant record lookup using an in-memory `Dictionary<int, Employee>`.
4. **Case-Insensitive Department Search**: Filter employees by department using `foreach` traversal and `StringComparison.OrdinalIgnoreCase`.
5. **Payroll & Compensation Statistics**: Calculate gross payroll, average salary, highest/lowest compensation, and estimated progressive tax deductions using arithmetic operators and `StringBuilder`.
6. **Live Memory Demonstration**: Live interactive demonstration of Value Type copying (Stack) vs. Reference Type copying (Heap aliasing), object cloning (`ShallowCopy`), and Nullable Types (`T?`).
7. **Crash-Resilient Console UI**: Resilient input parsing using `TryParse` and top-level `try / catch` blocks to gracefully prevent crashes on unexpected user input.

---

## C# Concepts Demonstrated

| Concept | Demonstrated In | Description |
| :--- | :--- | :--- |
| **Variables & Types** | `Employee.cs` | Appropriate use of `int`, `string`, `decimal`, `bool`, `DateTime`. |
| **Financial Precision** | `Employee.cs` | 128-bit `decimal` used for salaries and taxes to eliminate floating-point rounding errors. |
| **Nullable Types** | `Employee.cs`, `Program.cs` | Nullable value types (`decimal? PerformanceBonus`) and nullable reference types (`string? MiddleName`). |
| **Control Flow** | `Program.cs` | `while` loop (menu lifecycle), `switch` statement (option dispatch), `if/else` (validation). |
| **Operators** | `Employee.cs`, `EmployeeService.cs` | Arithmetic (`+`, `-`, `*`, `/`), Comparison (`==`, `<`, `>`), Logical (`&&`, `\|\|`, `!`), and Null-coalescing (`??`). |
| **Methods & Overloading** | `EmployeeService.cs` | Overloaded `SearchEmployees(string dept)` and `SearchEmployeesByName(string query)`. |
| **String Optimization** | `EmployeeService.cs` | `StringBuilder` for tabular reports vs string interpolation (`$"..."`) for concise formatting. |
| **Collections** | `EmployeeService.cs` | Fixed `Array` (departments), dynamic `List<Employee>`, and hash table `Dictionary<int, Employee>`. |
| **Memory Semantics** | `Program.cs` (Option 6) | Stack vs Heap, value copying vs reference aliasing, and independent shallow cloning. |
| **Exception Handling** | `Program.cs` | Structured `try / catch` catching `FormatException`, `ArgumentException`, and general `Exception`. |

---

## How to Build and Run

### Step 1: Restore Dependencies & Build
From the solution root directory (`EmployeeDepartmentManagement/`):

```powershell
dotnet restore
dotnet build EmployeeDepartmentManagement.sln
```

### Step 2: Run the Console Application
```powershell
dotnet run --project src/EmployeeDepartmentManagement.App/EmployeeDepartmentManagement.App.csproj
```
*(Or publish and execute the binary directly):*
```powershell
dotnet publish src/EmployeeDepartmentManagement.App/EmployeeDepartmentManagement.App.csproj -c Release
./src/EmployeeDepartmentManagement.App/bin/Release/net9.0/publish/EmployeeDepartmentManagement.App.exe
```

---

## Sample Input/Output Scenarios

### Scenario 1: Viewing Employee Roster (Menu Option 1)
```text
=== All Employee Records ===
ID    | Full Name            | Department       | Base Salary  | Bonus      | Age    | Exp   
-------------------------------------------------------------------------------------
101   | Aarav Sharma         | Engineering      | $85,000      | $5,000     | 28     | 5 yrs 
102   | Priya R Natarajan    | Human Resources  | $62,000      | $3,500     | 31     | 7 yrs 
103   | Murugan R            | Engineering      | $95,000      | $8,000     | 29     | 6 yrs 
104   | Ananya S Deshmukh    | Finance          | $78,000      | None       | 26     | 3 yrs 
105   | Vikram Malhotra      | Marketing        | $71,000      | $4,000     | 34     | 9 yrs 
-------------------------------------------------------------------------------------
Total Employees: 5
```

### Scenario 2: Adding a New Employee with Validation (Menu Option 2)
```text
=== Add New Employee ===
Enter Employee ID (e.g., 106): 106
Enter First Name: Anita
Enter Middle Name (Optional, press Enter to skip): K
Enter Last Name: Verma
Allowed Departments: Engineering, Human Resources, Finance, Marketing, Operations
Enter Department: Engineering
Enter Base Annual Salary in USD (e.g., 85000): 82000
Enter Performance Bonus (Optional, press Enter for None): 4000
Enter Age (18 - 75): 27
Enter Years of Experience (0 - 11): 4
Enter Joining Date (yyyy-MM-dd, e.g., 2023-04-15): 2022-06-15

[Success] Employee 'Anita K Verma' (ID: 106) was successfully added!
```

### Scenario 3: Fast Search by ID with Compensation Breakdown (Menu Option 3)
```text
=== Search Employee by ID ===
Enter Employee ID to search: 101

--- Employee Found ---
  ID                  : 101
  Full Name           : Aarav Sharma
  Department          : Engineering
  Age                 : 28 years
  Experience          : 5 years
  Joining Date        : 2021-03-15
  Base Salary         : $85,000.00
  Performance Bonus   : $5,000.00
  Gross Compensation  : $90,000.00
  Estimated Tax       : $19,800.00
  Net Take-Home Pay   : $70,200.00
  Employment Status   : Active
```

---

## Day 1 LeetCode Solution

- **Problem**: [LeetCode 1929: Concatenation of Array](https://leetcode.com/problems/concatenation-of-array/)
- **Location**: [`docs/Week-1/Day-1/ConcatenationOfArray.cs`](docs/Week-1/Day-1/ConcatenationOfArray.cs)
- **Time Complexity**: **O(n)** (single-pass array allocation and assignment)
- **Space Complexity**: **O(n)** (output array) / **O(1)** auxiliary
- **Test Harness**: Contains 4 automated test cases including boundary and edge cases (single element, zero arrays).

Run the standalone verification harness via PowerShell:
```powershell
Add-Type -Path "docs/Week-1/Day-1/ConcatenationOfArray.cs"
[LeetCode.Day01.Solution]::Main(@())
```

---

## Known Limitations

- **In-Memory Storage Only**: Data is stored in memory (`List<T>` and `Dictionary<int, Employee>`) and is reset whenever the application exits. Persistent storage via SQL Server, Entity Framework Core, and Repository patterns will be introduced in subsequent training modules.
- **Single-Node Execution**: No distributed caching or external database dependencies on Day 1.

---

## Documentation Roadmap

| Document | Description |
| :--- | :--- |
| [`docs/Week-1/Day-1/Concepts.md`](docs/Week-1/Day-1/Concepts.md) | Exhaustive C# fundamentals guide with code references. |
| [`docs/Week-1/Day-1/Value-vs-Reference-Types.md`](docs/Week-1/Day-1/Value-vs-Reference-Types.md) | Deep dive into Stack vs Heap, copying semantics, and cloning. |
| [`docs/Week-1/Day-1/Execution-Flow.md`](docs/Week-1/Day-1/Execution-Flow.md) | Application runtime lifecycle and data flow diagrams. |
| [`docs/Week-1/Day-1/LeetCode-Notes.md`](docs/Week-1/Day-1/LeetCode-Notes.md) | LeetCode 1929 problem analysis, complexity, and tests. |
| [`docs/Week-1/Day-2/Concepts.md`](docs/Week-1/Day-2/Concepts.md) | Comprehensive Object-Oriented C# guide (Classes, Interfaces, Structs, Records). |
| [`docs/Week-1/Day-2/OOP-Implementation-Notes.md`](docs/Week-1/Day-2/OOP-Implementation-Notes.md) | Architectural notes on Domain Class Library and polymorphism. |
| [`docs/Week-1/Day-2/SOLID-Principles.md`](docs/Week-1/Day-2/SOLID-Principles.md) | Detailed analysis of all 5 SOLID principles with code examples. |
| [`docs/Week-1/Day-2/UML-Class-Diagram.md`](docs/Week-1/Day-2/UML-Class-Diagram.md) | Visual Mermaid UML diagram detailing classes, interfaces, and associations. |
| [`docs/Week-1/Day-2/LeetCode-Notes.md`](docs/Week-1/Day-2/LeetCode-Notes.md) | LeetCode 1342 problem analysis, bit manipulation, and test suite. |

---

## Week 1 – Day 2: Object-Oriented C# Implementation

### Architecture Overview
Day 2 transitioned the solution into a **multi-project domain architecture**:
1. **`EmployeeDepartmentManagement.Core`**: Class library encapsulating `Employee` (abstract base), `FullTimeEmployee`, `ContractEmployee`, `Department` (composition), `IPayrollCalculable` (interface), `IEmployeeService`, `EmployeeSummary` (record), and `WorkHours` (struct).
2. **`EmployeeDepartmentManagement.App`**: Console application consuming `Core` through interface abstractions.

### Day 2 LeetCode Solution
- **Problem**: [LeetCode 1342: Number of Steps to Reduce a Number to Zero](https://leetcode.com/problems/number-of-steps-to-reduce-a-number-to-zero/)
- **Location**: [`docs/Week-1/Day-2/NumberOfStepsToReduceNumberToZero.cs`](docs/Week-1/Day-2/NumberOfStepsToReduceNumberToZero.cs)
- **Complexity**: Time: **O(log n)**, Space: **O(1)**.
- **Verification**: 5 automated test cases passing in standalone runner.

