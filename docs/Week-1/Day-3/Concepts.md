# Week 1 – Day 3: Modern C# & LINQ Concepts Guide

## Table of Contents
1. [Introduction to Day 3](#1-introduction-to-day-3)
2. [Collections in C#](#2-collections-in-c)
3. [Generics (`<T>`)](#3-generics-t)
4. [Lambda Expressions (`=>`)](#4-lambda-expressions-)
5. [Delegates (`Func`, `Action`, `Predicate`)](#5-delegates-func-action-predicate)
6. [Events (Publisher/Subscriber Pattern)](#6-events-publishersubscriber-pattern)
7. [Language Integrated Query (LINQ)](#7-language-integrated-query-linq)
8. [Deferred vs. Immediate Execution](#8-deferred-vs-immediate-execution)
9. [Extension Methods](#9-extension-methods)
10. [Pattern Matching in Modern C#](#10-pattern-matching-in-modern-c)

---

## 1. Introduction to Day 3

In **Week 1 – Day 3**, we elevate our **EmployeeDepartmentManagement** application from foundational Object-Oriented design into **Modern C# (C# 9 to 13) and LINQ (Language Integrated Query)**.

Modern C# combines object-oriented design with functional programming paradigms, enabling developers to write expressive, declarative, type-safe, and highly maintainable code.

---

## 2. Collections in C#

### What are Collections?
Collections are specialized data structures for grouping and manipulating multiple objects. Unlike fixed-size arrays (`int[]`, `string[]`), modern collections are dynamic in size and provide rich search, sort, and mutation APIs.

### Core Collections Overview:
| Collection | Type | Internal Structure | Access Speed | When to Use |
| :--- | :--- | :--- | :--- | :--- |
| `List<T>` | Generic Class | Contiguous Array (auto-resizing) | Index: $O(1)$, Scan: $O(n)$ | Ordered items, frequent index reads. |
| `Dictionary<TKey, TValue>` | Generic Class | Hash Table with Buckets | Key Lookup: $O(1)$ | Key-value pairs (e.g., lookup employee by ID). |
| `HashSet<T>` | Generic Class | Hash Set (Keys only) | Membership: $O(1)$ | Deduplication, uniqueness verification. |
| `Queue<T>` / `Stack<T>` | Generic Class | Ring Buffer / Linked list | $O(1)$ push/pop | FIFO / LIFO processing. |
| `IEnumerable<T>` | Interface | State Machine / Enumerator | Forward-only ($O(n)$) | Deferred query streaming, universal abstraction. |

---

## 3. Generics (`<T>`)

### What are Generics?
Generics introduce **type parameters** (`<T>`), allowing classes, interfaces, and methods to be defined with placeholders for the exact data type they operate on until instantiated by consumer code.

### Why Are Generics Required?
Before Generics in .NET 1.0, developers used `ArrayList` storing `object`. This caused two severe problems:
1. **Boxing and Unboxing Overhead**: Value types (like `int`, `decimal`) were repeatedly allocated on the heap (boxed) and cast back (unboxed), destroying performance and garbage collector efficiency.
2. **Lack of Type Safety**: Any object could be added to an `ArrayList`. A list meant for `Employee` could accidentally hold a `Department` string, leading to runtime `InvalidCastException` crashes.

### How it is Implemented in Our Solution:
In [`GenericFilter.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Common/GenericFilter.cs):
```csharp
public class GenericFilter<T> where T : class
{
    public IEnumerable<T> Filter(IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (T item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
}
```
- `where T : class` is a **generic constraint**, ensuring `T` can only be a reference type.
- `Func<T, bool>` ensures compiler-checked type safety. Passing `emp => emp.BaseSalary > 80000` is verified at compile-time!

---

## 4. Lambda Expressions (`=>`)

### What is a Lambda Expression?
A **lambda expression** is an anonymous inline function written using the `=>` ("goes to") token. It defines parameters on the left and an expression or statement block on the right.

### Syntax Anatomy:
```text
(parameters) => expression_or_statement_block
```

### Examples in Employee Management:
```csharp
// 1. Single parameter, returns a boolean
emp => emp.CalculateGrossPay() > 75000m

// 2. Extracts a property value
emp => emp.Department.Name

// 3. Multi-parameter calculation
(baseSalary, rating) => baseSalary * (decimal)(1.0 + (rating * 0.05))
```

### Why Use Lambdas over Named Methods?
Instead of creating a separate named method `bool IsSalaryOver75K(Employee e)` for every tiny condition, a lambda expresses the logic directly where it is consumed, reducing boilerplate code.

---

## 5. Delegates (`Func`, `Action`, `Predicate`)

### What is a Delegate?
A **delegate** is a type-safe function pointer. It defines a method signature and can hold a reference to any method that matches that signature.

### Built-in .NET Generic Delegates:
1. **`Func<T, TResult>`**: Takes input arguments and **returns a result** value.
   - Example: `Func<Employee, decimal> getSalary = emp => emp.CalculateGrossPay();`
2. **`Action<T>`**: Takes input arguments and **returns void** (executes side-effects).
   - Example: `Action<Employee> printCard = emp => Console.WriteLine(emp.FullName);`
3. **`Predicate<T>`**: Takes a single input and **returns a bool** (used for conditions).
   - Example: `Predicate<Employee> isContract = emp => emp is ContractEmployee;`

### Custom Delegate Example:
In [`ModernCSharpDemonstration.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Demonstrations/ModernCSharpDemonstration.cs):
```csharp
public delegate decimal CompensationAdjustmentDelegate(decimal baseAmount, double performanceRating);
```

---

## 6. Events (Publisher/Subscriber Pattern)

### What is an Event?
An **event** is a specialized, encapsulated delegate wrapper that implements the **Observer (Publisher/Subscriber) Pattern**.
- The **Publisher** class defines the event and triggers (`raises` / `invokes`) it when a significant lifecycle action occurs.
- The **Subscriber** class attaches (`+=`) a handler method to listen for and react to the notification.
- Unlike public delegates, external classes **cannot** directly invoke an event or overwrite its invocation list with `=`; they can only subscribe (`+=`) or unsubscribe (`-=`).

### Demonstrated in Our Application:
In [`IEmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Interfaces/IEmployeeService.cs) & [`EmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs):
```csharp
// Publisher (EmployeeService)
public event EventHandler<EmployeeEventArgs>? EmployeeAdded;

public bool AddEmployee(Employee emp, out string errorMessage)
{
    // ... validation and storage ...
    EmployeeAdded?.Invoke(this, new EmployeeEventArgs(emp, "Enrolled"));
    return true;
}
```
When `AddEmployee` succeeds, the event automatically fires, notifying logging listeners and auditing systems without hardcoding dependencies inside `AddEmployee`.

---

## 7. Language Integrated Query (LINQ)

### What is LINQ?
**LINQ** provides a uniform, declarative query syntax integrated natively into C#. It allows querying collections (`IEnumerable<T>`), databases (EF Core), XML, and JSON with the exact same fluent operators.

### Core LINQ Operators Implemented in `EmployeeQueryService`:
- **`Where`**: Filters elements matching a boolean predicate.
- **`Select`**: Projects each element into a new shape (DTO or value).
- **`OrderBy` / `OrderByDescending`**: Sorts elements by key.
- **`GroupBy`**: Groups elements sharing a common key.
- **`Average` / `Sum`**: Aggregates numerical quantities.
- **`MaxBy` / `MinBy`**: Selects the element with maximum/minimum key (.NET 6+).
- **`ToList`**: Forces immediate evaluation and allocates an in-memory list.

---

## 8. Deferred vs. Immediate Execution

### Deferred Execution (Lazy Evaluation)
In LINQ, operators that return `IEnumerable<T>` (such as `Where`, `Select`, `Take`) do **not** run when the query is declared.
Instead, they build an execution plan (an iterator state machine). The query only executes **when enumerated** (e.g., by a `foreach` loop, `Count()`, or `ToList()`).

### Real-World Benefit:
- Queries can be constructed dynamically in stages without multiple full passes over the collection.

### Potential Pitfall (Multiple Enumeration):
If a deferred query is looped over twice, the entire query executes twice. If the underlying data changed between loops, results will differ.

### Immediate Execution (Eager Evaluation)
Operators such as `ToList()`, `ToArray()`, `Count()`, `Average()`, and `MaxBy()` force the query to execute immediately and materialize the results into memory.

---

## 9. Extension Methods

### What are Extension Methods?
Extension methods enable adding new methods to existing types without modifying their source code, recompiling them, or creating subclass hierarchies.

### Syntax Rules:
1. Must be declared inside a **`static class`**.
2. Must be declared as a **`static method`**.
3. The **first parameter** must have the **`this`** keyword preceding its type.

### Implemented in [`EmployeeExtensions.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Extensions/EmployeeExtensions.cs):
```csharp
public static class EmployeeExtensions
{
    public static bool IsHighEarner(this Employee employee, decimal threshold = 80000m)
    {
        return employee.CalculateGrossPay() >= threshold;
    }
}
```
Caller code invokes it as an instance method:
```csharp
if (emp.IsHighEarner(85000m)) { ... }
```

---

## 10. Pattern Matching in Modern C#

### What is Pattern Matching?
Pattern matching tests an expression against a shape or structure and extracts data when a match succeeds.

### Patterns Demonstrated in [`EmployeePatternMatcher.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Services/EmployeePatternMatcher.cs):

1. **Type Pattern**:
   ```csharp
   if (employee is FullTimeEmployee ft) { ... }
   ```
2. **Property Pattern**:
   ```csharp
   employee is FullTimeEmployee { AnnualBonus: > 5000m } ftHighBonus
   ```
3. **Switch Expression with Relational Patterns**:
   ```csharp
   public static string ClassifyEmployeeTier(Employee employee) => employee switch
   {
       FullTimeEmployee { BaseSalary: >= 95000m } ftHigh => $"Senior Executive (Base: {ftHigh.BaseSalary:C})",
       FullTimeEmployee => "Standard Corporate Staff",
       ContractEmployee { HourlyRate: >= 60m } ctHigh => $"Senior Specialist ({ctHigh.AgencyName})",
       ContractEmployee => "Standard Contractor",
       null => "Unknown",
       _ => "General Personnel"
   };
   ```
This replaces fragile, repetitive `if-else` and `typeof()` casting ladders with concise, readable, compiler-verified pattern branches.
