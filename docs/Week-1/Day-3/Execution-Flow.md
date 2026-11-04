# Week 1 – Day 3: Execution Flow Architecture

This document describes the runtime execution flow when selecting **Option 8: Run Modern C# & LINQ Deep Dive** from the application main menu.

---

## 1. High-Level Runtime Architecture

```text
               ┌────────────────────────────────────────────────────────┐
               │              Program.cs (Console Host)                 │
               │   - User selects '8' (Modern C# & LINQ Deep Dive)      │
               └───────────────────────────┬────────────────────────────┘
                                           │
                                           ▼
┌───────────────────────────────────────────────────────────────────────────────────────┐
│              ModernCSharpDemonstration.Run(employeeService, queryService)             │
└───────┬───────────────────┬───────────────────┬───────────────────┬───────────────────┘
        │                   │                   │                   │
        ▼                   ▼                   ▼                   ▼
┌──────────────┐    ┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│  Employee    │    │   Generic    │    │   Events     │    │   Pattern    │
│QueryService  │    │  Filter<T>   │    │ Publisher &  │    │   Matcher    │
│  (8 LINQ Ops)│    │ & Result<T>  │    │  Subscriber  │    │ & Extensions │
└──────────────┘    └──────────────┘    └──────────────┘    └──────────────┘
```

---

## 2. Step-by-Step Execution Sequence

### Phase 1: Initialization & Dispatch
1. `Program.Main()` initializes the application and displays the 9-option menu.
2. User types `"8"` and presses `[Enter]`.
3. `Program.CleanInput()` sanitizes input and dispatches to `ModernCSharpDemonstration.Run(_employeeService, _queryService)`.

### Phase 2: Retrieval of In-Memory Employees
1. `employeeService.GetAllEmployees()` retrieves the seeded employee collection from `EmployeeService._employeeList` as `IReadOnlyList<Employee>`.
2. This list contains both `FullTimeEmployee` and `ContractEmployee` records.

### Phase 3: The 8 LINQ Operations
1. **Query 1 (Salary Filter)**: Passes `employees` into `queryService.FilterByMinimumSalary(employees, 75000m)`. Evaluates `Where(...)` and renders matching employees.
2. **Query 2 (Dept Filter)**: Passes `departmentName = "Engineering"`. Evaluates case-insensitive ordinal comparison.
3. **Query 3 (Salary Sort)**: Executes `OrderByDescending` to rank staff from highest gross earner ($103k) to lowest ($8.2k).
4. **Query 4 (Name Sort)**: Executes `OrderBy(emp => emp.FullName)` for alphabetical ordering.
5. **Query 5 (Department Grouping)**: Executes `GroupBy(emp => emp.Department.Name)`. Iterates sub-groups and formats departmental rosters.
6. **Query 6 (Average Salary)**: Aggregates each department's headcount, average pay, and total payroll.
7. **Query 7 (Highest Paid)**: Executes `MaxBy(emp => emp.CalculateGrossPay())` in a single pass.
8. **Query 8 (Multi-condition Search)**: Chains deferred `Where` clauses based on optional parameters (`Dept`, `MinSalary`, `Type`, `NameQuery`), executing the combined query pipeline with `.ToList()`.

### Phase 4: Generics & Functional Delegates
1. Instantiates `GenericFilter<Employee>()`.
2. Tests `Func<Employee, decimal>`, `Predicate<Employee>`, and a custom `CompensationAdjustmentDelegate`.

### Phase 5: Event Notifications (Observer Pattern)
1. Registers local subscriber handlers:
   ```csharp
   employeeService.EmployeeAdded += OnEmployeeAddedHandler;
   employeeService.SalaryChanged += OnSalaryChangedHandler;
   ```
2. Calls `employeeService.AddEmployee(...)` with new employee `Vikrant Gupta` (ID 999).
3. `EmployeeService` invokes `EmployeeAdded?.Invoke(...)`. The subscriber instantly prints the real-time notification to the console.
4. Calls `employeeService.UpdateEmployeeSalary(999, 98000m)`.
5. `EmployeeService` invokes `SalaryChanged?.Invoke(...)`. The subscriber logs the old and new compensation.
6. Unsubscribes handlers with `-=`.

### Phase 6: Deferred Execution Demonstration
1. Declares `deferredQuery` without materialization.
2. Declares `materializedList` with `.ToList()`.
3. Modifies the source collection (`testList.Add(...)`).
4. Re-evaluates both: demonstrates that the deferred query adapts to the newly added item, while the materialized list remains a frozen snapshot.

### Phase 7: Extension Methods & Pattern Matching
1. Calls extension methods: `sampleEmp.IsHighEarner()`, `sampleEmp.ToFormattedString()`, and `employees.TotalGrossPayroll()`.
2. Passes employees to `EmployeePatternMatcher.ClassifyEmployeeTier(emp)` and `EvaluateBudgetImpact(emp)`.
3. Returns control to `Program.cs`, prompting user to press `[Enter]` to return to the main menu.
