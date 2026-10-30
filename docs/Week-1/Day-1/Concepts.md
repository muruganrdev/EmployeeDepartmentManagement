# Week 1 – Day 1: C# Fundamentals

## Table of Contents
1. [Introduction & Domain Overview](#introduction--domain-overview)
2. [Variables and Fundamental Data Types](#variables-and-fundamental-data-types)
   - [What is a Variable?](#what-is-a-variable)
   - [Type Selection Criteria](#type-selection-criteria)
   - [Why `decimal` is Used for Salaries](#why-decimal-is-used-for-salaries)
   - [Nullable Types (`decimal?`, `string?`)](#nullable-types)
3. [Operators and Control Flow](#operators-and-control-flow)
   - [Arithmetic Operators](#arithmetic-operators)
   - [Comparison and Logical Operators](#comparison-and-logical-operators)
   - [Branching: `if / else` and `switch`](#branching-if--else-and-switch)
   - [Loops: `while`, `for`, `foreach`](#loops-while-for-foreach)
4. [Methods, Parameters, and String Handling](#methods-parameters-and-string-handling)
   - [Method Signatures & Return Types](#method-signatures--return-types)
   - [Method Overloading](#method-overloading)
   - [String Interpolation vs `StringBuilder`](#string-interpolation-vs-stringbuilder)
5. [Arrays and Collections](#arrays-and-collections)
   - [Arrays (`T[]`)](#arrays-t)
   - [Dynamic Lists (`List<T>`)](#dynamic-lists-listt)
   - [Hash Tables (`Dictionary<TKey, TValue>`)](#hash-tables-dictionarytkey-tvalue)
6. [Exceptions and Defensive Input Validation](#exceptions-and-defensive-input-validation)
   - [Validation vs Exception Handling](#validation-vs-exception-handling)
   - [Structured `try / catch` Blocks](#structured-try--catch-blocks)

---

## 1. Introduction & Domain Overview

In **Week 1 – Day 1**, we build the foundation of our enterprise application: **EmployeeDepartmentManagement**. 
Rather than creating disconnected syntax examples, all C# fundamentals are implemented within a realistic human resources and payroll management domain.

Every concept introduced here is directly utilized in our solution:
- [`Employee.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Models/Employee.cs): Models the core entity with value types, reference types, and calculations.
- [`Department.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Models/Department.cs): Encapsulates department array constants and case-insensitive validation.
- [`EmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs): Manages state via collections (`List<Employee>`, `Dictionary<int, Employee>`), implements business logic, searches, and payroll reports.
- [`Program.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Program.cs): Implements the user-facing CLI menu, input sanitization, and live memory demonstration.

---

## 2. Variables and Fundamental Data Types

### What is a Variable?
A **variable** is a named location in computer memory allocated to hold a value of a specific data type. In C#, a strongly typed language, every variable has an explicit type checked by the compiler at compile time.

### Type Selection Criteria
We select data types according to domain constraints and memory efficiency:

| Data Type | .NET Type | Size | Purpose in Application | Code Location |
| :--- | :--- | :--- | :--- | :--- |
| `int` | `System.Int32` | 32 bits (4 bytes) | Identifiers (`Id`), `Age`, `YearsOfExperience`. Integer quantities without fractional components. | `Employee.cs` |
| `decimal` | `System.Decimal` | 128 bits (16 bytes) | Base salary (`BaseSalary`), bonuses, taxes, compensation. High-precision financial arithmetic. | `Employee.cs` |
| `string` | `System.String` | Variable (heap) | Textual data (`FirstName`, `LastName`, `Department`). Immutable reference type. | `Employee.cs` |
| `bool` | `System.Boolean` | 8 bits (1 byte) | State flags (`IsActive`). Represents truth values `true` or `false`. | `Employee.cs` |
| `DateTime` | `System.DateTime` | 64 bits (8 bytes) | Calendar dates (`JoiningDate`). Stores date and time information. | `Employee.cs` |

### Why `decimal` is Used for Salaries
In financial systems, using `float` (32-bit) or `double` (64-bit) leads to **floating-point rounding inaccuracies** because binary floating-point representations cannot precisely represent base-10 decimal fractions (e.g., `0.1 + 0.2 != 0.3` in binary floating-point).

`decimal` uses a 128-bit base-10 representation offering 28–29 significant digits of precision:
```csharp
// From Employee.cs
public decimal BaseSalary { get; set; }
public decimal CalculateGrossCompensation()
{
    decimal bonusAmount = PerformanceBonus ?? 0m;
    return BaseSalary + bonusAmount;
}
```
All monetary calculations (taxes, net pay, total payroll) are computed in `decimal` with zero rounding loss.

### Nullable Types
Value types cannot hold `null` by default because they store binary data directly. C# provides `Nullable<T>` (shorthand `T?`) to represent a value type that may be absent:
```csharp
// Optional annual bonus: an employee may not have an assigned bonus
public decimal? PerformanceBonus { get; set; }

// Checking for presence:
if (PerformanceBonus.HasValue)
{
    Console.WriteLine($"Bonus: {PerformanceBonus.Value:C}");
}
else
{
    Console.WriteLine("No bonus assigned.");
}

// Null-coalescing operator '??' provides a fallback default:
decimal gross = BaseSalary + (PerformanceBonus ?? 0m);
```

For reference types, C# 8+ enables **Nullable Reference Types** (`#nullable enable`). 
- `string FirstName`: Mandatory; compiler warns if null.
- `string? MiddleName`: Explicitly optional; informs developers and tools that `null` is expected.

---

## 3. Operators and Control Flow

### Arithmetic Operators
Demonstrated in `Employee.CalculateNetPay()` and `EmployeeService.GetPayrollStatistics()`:
- `+` (Addition): Gross salary calculation (`BaseSalary + bonus`).
- `-` (Subtraction): Net pay deduction (`gross - tax`).
- `*` (Multiplication): Tax bracket calculation (`gross * 0.22m`).
- `/` (Division): Average salary computation (`totalPayroll / count`).

### Comparison and Logical Operators
Demonstrated in `EmployeeService.ValidateEmployee()`:
```csharp
// Logical AND (&&), OR (||), NOT (!)
if (emp.Age < 18 || emp.Age > 75)
{
    return (false, "Employee age must be between 18 and 75.");
}

if (emp.YearsOfExperience < 0 || emp.YearsOfExperience > emp.Age - 16)
{
    return (false, "Years of Experience must be realistic for the given age.");
}
```

### Branching: `if / else` and `switch`
- `if / else` ladder: Applied in progressive tax bracket calculations (`CalculateEstimatedTax` in `Employee.cs`).
- `switch` statement: Applied in `Program.cs` to cleanly dispatch user menu selections (`1` through `7`).

### Loops: `while`, `for`, `foreach`
1. `while`: Keeps the console menu running interactively until the user chooses option `7` (Exit). Also powers prompt retry loops in `CleanInput()` helpers.
2. `for`: Demonstrates index-based sequential iteration when calculating aggregate statistics in `GetPayrollStatistics()`.
3. `foreach`: Iterates through collections (`_employeeList`, `Department.ValidDepartments`) cleanly without managing index counters.

---

## 4. Methods, Parameters, and String Handling

### Method Signatures & Return Types
- **Value-returning methods**: `Employee? GetEmployeeById(int id)` returns a nullable employee reference.
- **Void methods**: `DisplayWelcomeBanner()` performs side-effects (console writing) without returning data.
- **Out parameters / Tuples**: `AddEmployee(Employee emp, out string error)` and `(bool IsValid, string ErrorMessage) ValidateEmployee(Employee emp)` return multi-value outcomes.

### Method Overloading
Method overloading allows multiple methods to share the same name with distinct parameter lists:
```csharp
// In EmployeeService.cs:
// Overload 1: Filters by exact Department name
public List<Employee> SearchEmployees(string department);

// Overload 2: Searches by partial name query
public List<Employee> SearchEmployeesByName(string nameQuery);
```

### String Interpolation vs `StringBuilder`
- **String Interpolation (`$"..."`)**: Ideal for simple, concise formatting:
  ```csharp
  return $"{FirstName} {LastName} (ID: {Id})";
  ```
- **`StringBuilder`**: Crucial when constructing large, multi-line reports inside loops (`GeneratePayrollReport()` in `EmployeeService.cs`). Since strings are immutable, repeated `+` concatenation inside loops allocates many intermediate discardable strings on the managed heap. `StringBuilder` maintains an internal expandable buffer, minimizing garbage collection pressure.

---

## 5. Arrays and Collections

### Arrays (`T[]`)
Fixed-size, contiguous block of memory allocated on the heap.
```csharp
// From Department.cs
public static readonly string[] ValidDepartments =
[
    "Engineering",
    "Human Resources",
    "Finance",
    "Marketing",
    "Operations"
];
```
Use arrays when the number of elements is fixed and known at design time.

### Dynamic Lists (`List<T>`)
Under the hood, `List<T>` encapsulates a dynamically resizing array:
- Dynamically grows as items are added via `.Add()`.
- Provides indexed access (`list[i]`), sequential enumeration (`foreach`), and LINQ extension methods.

### Hash Tables (`Dictionary<TKey, TValue>`)
A dictionary organizes data in buckets computed via hash codes:
```csharp
private readonly Dictionary<int, Employee> _employeeIndex = new();
```
- **Lookup Complexity**: **O(1)** average-time lookup via `_employeeIndex.TryGetValue(id, out var emp)`.
- **Comparison**: Searching through a `List<T>` takes **O(n)** time because each item must be inspected sequentially. The dictionary provides instant retrieval even with thousands of employees.

---

## 6. Exceptions and Defensive Input Validation

### Validation vs Exception Handling
A critical senior architectural principle: **Never use exception handling for normal control flow.**

| Category | Purpose | Example in Application | Cost |
| :--- | :--- | :--- | :--- |
| **Input Validation** | Handles expected, user-correctable input mistakes. | `int.TryParse()`, `string.IsNullOrWhiteSpace()`, range checks. | Very fast; zero stack unwinding. |
| **Exception Handling** | Recovers from unexpected, catastrophic, or external failures. | `try / catch` in `Program.Main()` catching `FormatException`, `IOException`. | Expensive; creates stack trace and pauses execution. |

In `Program.cs`, all user inputs are defensively checked using `TryParse` loops before instantiating domain models. The top-level `try / catch` guarantees the application never abruptly terminates if an unforeseen runtime fault occurs.
