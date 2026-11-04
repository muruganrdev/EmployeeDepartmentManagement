# Week 1 – Day 3: Modern C# Implementation Notes

This document provides detailed implementation and architectural notes for the modern C# language features added in Week 1 – Day 3.

---

## 1. Generics (`GenericFilter<T>` and `OperationResult<T>`)

### What Was Built:
Located in [`src/EmployeeDepartmentManagement.Core/Common/GenericFilter.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Common/GenericFilter.cs).

### Key Architectural Choices:
1. **Generic Constraint (`where T : class`)**:
   Enforces reference-type semantics, ensuring `T` can be checked for `null` and treated as an entity object.
2. **Streaming Iteration (`yield return`)**:
   Instead of allocating temporary lists inside the filter, `Filter(source, predicate)` yields elements on demand, maintaining low memory overhead.
3. **`OperationResult<T>` Wrapper**:
   Eliminates null-reference hazards by encapsulating `IsSuccess`, `Data`, and `Message` in a standardized return envelope.

---

## 2. Lambda Expressions & Delegates

### Built-in Generic Delegates Used:
1. **`Func<Employee, decimal>`**:
   Maps an `Employee` instance to their gross salary (`emp => emp.CalculateGrossPay()`).
2. **`Predicate<Employee>`**:
   Evaluates a boolean condition against an employee (`emp => emp is ContractEmployee`).
3. **`Action<Employee>`**:
   Executes a side-effect (such as printing or event dispatch) without returning a value.

### Custom Delegate:
```csharp
public delegate decimal CompensationAdjustmentDelegate(decimal baseAmount, double performanceRating);
```
Used in [`ModernCSharpDemonstration.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Demonstrations/ModernCSharpDemonstration.cs) to demonstrate passing compensation formulas as first-class method parameters.

---

## 3. Events & Publisher / Subscriber Architecture

### Classes Implemented:
- [`EmployeeEventArgs.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Events/EmployeeEventArgs.cs): Carries `Employee`, `EventTimestamp`, and `Action`.
- [`SalaryChangedEventArgs.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Events/EmployeeEventArgs.cs): Carries `Employee`, `OldSalary`, `NewSalary`, and `EventTimestamp`.

### Publisher (`EmployeeService`):
```csharp
public event EventHandler<EmployeeEventArgs>? EmployeeAdded;
public event EventHandler<SalaryChangedEventArgs>? SalaryChanged;
```
When `AddEmployee()` or `UpdateEmployeeSalary()` completes successfully, the service invokes the event via `EmployeeAdded?.Invoke(this, args)`.

### Subscriber (`ModernCSharpDemonstration`):
Subscribes to notifications using `+=`, logs real-time updates when an event fires, and unsubscribes using `-=` to guarantee zero memory leaks.

---

## 4. Deferred Execution Demonstration

### The Experiment:
```csharp
var testList = new List<Employee> { emp1, emp2 };

// Query 1: Deferred execution
var deferredQuery = testList.Where(e => e.CalculateGrossPay() >= 65000m);

// Query 2: Materialized execution via ToList
var materializedList = testList.Where(e => e.CalculateGrossPay() >= 65000m).ToList();

// Add new element AFTER queries were declared
testList.Add(emp3);
```

### Result Observed in Console:
- `deferredQuery.Count()` evaluated to **2** because it re-enumerated the modified `testList` dynamically.
- `materializedList.Count` remained **1** because `.ToList()` had created a static snapshot at the moment of declaration.

---

## 5. Extension Methods (`EmployeeExtensions`)

### What Was Built:
Located in [`src/EmployeeDepartmentManagement.Core/Extensions/EmployeeExtensions.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Extensions/EmployeeExtensions.cs).

### Methods Implemented:
1. `bool IsHighEarner(this Employee employee, decimal threshold = 80000m)`
2. `string ToFormattedString(this Employee employee)`
3. `decimal TotalGrossPayroll(this IEnumerable<Employee> employees)`
4. `IEnumerable<Employee> FilterByDepartmentName(this IEnumerable<Employee> employees, string dept)`
5. `decimal AverageGrossSalary(this IEnumerable<Employee> employees)`

### Why It Matters:
Extends both individual `Employee` instances and `IEnumerable<Employee>` sequences with fluent, readable methods without polluting the base domain entities with formatting logic.

---

## 6. Pattern Matching (`EmployeePatternMatcher`)

### What Was Built:
Located in [`src/EmployeeDepartmentManagement.Core/Services/EmployeePatternMatcher.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Services/EmployeePatternMatcher.cs).

### Patterns Applied:
1. **Property Pattern with Relational Check**:
   ```csharp
   FullTimeEmployee { BaseSalary: >= 95000m } ftHigh
   ```
2. **Nullable Property Pattern**:
   ```csharp
   FullTimeEmployee { AnnualBonus: > 0m } ftBonus
   ```
3. **Type Pattern with Deconstruction**:
   ```csharp
   if (employee is ContractEmployee ct) { ... }
   ```
4. **Switch Expression**:
   Replaced verbose nested `if/else` ladders with a clean, exhaustive pattern-matching expression.
