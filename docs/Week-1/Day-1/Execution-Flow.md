# Complete Application Execution Flow

This document details the exact runtime lifecycle of the **EmployeeDepartmentManagement** application from startup to exit, showing how each file, class, and method interacts.

---

## 1. High-Level Architecture Diagram

```
+----------------------------------------------------------------------------------------------------+
|                                      APPLICATION RUNTIME LIFECYCLE                                 |
+----------------------------------------------------------------------------------------------------+

                          [ User launches application (dotnet run) ]
                                              │
                                              ▼
                                     [ Program.Main() ]
                                              │
                                              ▼
                           [ Instantiate EmployeeService ]
                                              │
                                              ▼
                              [ EmployeeService: Seed Data ]
                            (Populates List<T> & Dictionary)
                                              │
                                              ▼
                              [ Display Welcome Banner & Menu ]
                                              │
                     ┌────────────────────────┴────────────────────────┐
                     │                                                 │
                     ▼                                                 ▼
            [ Option 1 - 6 ]                                      [ Option 7 ]
       Process user selection                                    Set isRunning = false
                     │                                                 │
                     ▼                                                 ▼
      ┌──────────────┴──────────────┐                           [ Display Goodbye ]
      │                             │                                  │
      ▼                             ▼                                  ▼
[ Collect & Clean Input ]    [ Validation Failed ]             [ Process Exits (0) ]
      │                      Show warning & retry
      ▼                             │
[ Execute Business Logic ]           │
      │                             │
      ▼                             │
[ Render Formatted Result ]          │
      │                             │
      ▼                             │
[ Prompt: Press Enter to Return ]   │
      │                             │
      └─────────────────────────────┘
                     │
                     ▼
           [ Loop to Menu Display ]
```

---

## 2. Step-by-Step Execution Lifecycle

### Phase 1: Startup & Initialization
1. **Operating System & Runtime Host**:
   - The OS loads `dotnet.exe`, which bootstraps the .NET 9 CLR (Common Language Runtime).
   - JIT (Just-In-Time) compiler compiles MSIL instructions in `EmployeeDepartmentManagement.App.dll` into native machine code.
2. **`Program.Main(string[] args)` Entry Point**:
   - Sets console output encoding to UTF-8 (`Console.OutputEncoding = Encoding.UTF8`) for clean currency and symbol formatting.
   - Calls `DisplayWelcomeBanner()`, executing `SafeClear()` to handle both interactive and redirected environments.
3. **Instantiating `EmployeeService`**:
   - The static field `_employeeService` is initialized.
   - The constructor invokes `SeedSampleData()`, which allocates 5 realistic `Employee` objects and populates:
     - `_employeeList` (`List<Employee>`): for sequential operations and tabular views.
     - `_employeeIndex` (`Dictionary<int, Employee>`): for O(1) instant key-value ID lookups.

---

### Phase 2: Interactive Menu Loop
4. **Interactive `while (isRunning)` Loop**:
   - Displays the numbered menu (`1` to `7`).
   - Prompts the user: `Enter your selection (1-7): `.
   - Sanitizes input via `CleanInput()` (strips whitespace, newlines, and UTF-8 Byte Order Marks `\uFEFF`).
   - Dispatches the request through a C# `switch (input)` statement:

```csharp
switch (input)
{
    case "1": HandleViewAllEmployees(); break;
    case "2": HandleAddEmployee(); break;
    case "3": HandleSearchEmployeeById(); break;
    case "4": HandleSearchEmployeesByDepartment(); break;
    case "5": HandlePayrollStatistics(); break;
    case "6": HandleValueVsReferenceTypesDemo(); break;
    case "7": isRunning = false; break;
    default: DisplayInvalidSelectionWarning(); break;
}
```

---

### Phase 3: Detailed Menu Option Execution

#### Option 1: View All Employees
- `Program.HandleViewAllEmployees()` calls `_employeeService.GetAllEmployees()`.
- Returns an `IReadOnlyList<Employee>`.
- Formats each employee into aligned table columns (`{0,-5} | {1,-20} | ...`).
- Displays total headcount and returns to menu.

#### Option 2: Add New Employee (Defensive Validation Pipeline)
- Prompts for each employee property sequentially with dedicated retry loops:
  1. `ReadPositiveInt("Enter Employee ID")`: Checks for positive whole numbers.
  2. Duplicate check: Immediately tests `_employeeService.GetEmployeeById(id)`. If ID exists, halts early with a warning.
  3. `ReadRequiredString("Enter First Name")`: Enforces non-blank, at least 2 characters.
  4. `middleInput`: Optional; allows blank Enter for `null`.
  5. `ReadRequiredString("Enter Last Name")`.
  6. `ReadDepartment()`: Validates against `Department.ValidDepartments` array.
  7. `ReadPositiveDecimal("Enter Base Annual Salary")`: Validates positive decimal amount.
  8. `ReadOptionalDecimal("Enter Performance Bonus")`: Allows null or positive decimal.
  9. `ReadIntInRange("Enter Age", 18, 75)`.
  10. `ReadIntInRange("Enter Years of Experience", 0, age - 16)`.
  11. `ReadPastOrTodayDate("Enter Joining Date")`: Validates `yyyy-MM-dd` and ensures date is not in the future.
- Instantiates a new `Employee` object and calls `_employeeService.AddEmployee(newEmployee, out error)`.
- Synchronously updates both `_employeeList` and `_employeeIndex`.

#### Option 3: Search Employee by ID
- Reads ID via `ReadPositiveInt()`.
- Calls `_employeeService.GetEmployeeById(id)`.
- Inside `EmployeeService`:
  ```csharp
  if (_employeeIndex.TryGetValue(id, out Employee? emp))
  {
      return emp;
  }
  return null;
  ```
- If found: Calculates gross compensation, estimated progressive tax bracket, and net pay in real-time and prints an employee details card.
- If not found: Displays `[Not Found] No employee found with ID {id}.`

#### Option 4: Search Employees by Department
- Prompts user for department name.
- Calls `_employeeService.SearchEmployees(department)`.
- Demonstrates `foreach` iteration and case-insensitive string matching (`StringComparison.OrdinalIgnoreCase`).
- Prints matching records or a message if none found.

#### Option 5: Payroll & Compensation Statistics
- Calls `_employeeService.GeneratePayrollReport()`.
- Computes aggregate metrics in `GetPayrollStatistics()` using arithmetic operators and loops:
  - `totalPayroll += comp;`
  - `highestSalary = Math.Max(...);`
  - `lowestSalary = Math.Min(...);`
  - `averageSalary = totalPayroll / count;`
- Uses `StringBuilder` to format the report into an aligned table.

#### Option 6: Live C# Fundamentals Demo
- Executes live memory experiments:
  1. **Value Type copying**: Modifying a copied `int` leaves the original untouched.
  2. **Reference Type copying**: Mutating an `Employee` reference mutates the same object on the Heap.
  3. **Instance Cloning**: Modifying a `ShallowCopy()` leaves the original intact.
  4. **Nullable Types**: Demonstrates `.HasValue`, `.Value`, and the `??` null-coalescing operator.

#### Option 7: Exit Application
- Sets `isRunning = false`.
- Prints a goodbye message.
- Exits the `while` loop cleanly with process exit code `0`.

---

### Phase 4: Defensive Exception Handling
All menu operations are wrapped inside a structured `try / catch` block:
- `catch (FormatException ex)`: Captures unexpected parsing faults.
- `catch (ArgumentException ex)`: Catches argument violations.
- `catch (Exception ex)`: Catch-all safety net that logs the issue without crashing the application.
