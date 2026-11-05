# Week 1 Final Execution Flow & Architectural Lifecycle

## 1. Complete End-to-End Application Lifecycle

```mermaid
sequenceDiagram
    autonumber
    actor User as User / Terminal
    participant Program as Program.cs (Main)
    participant UI as Menu & Input Validators
    participant EmpSvc as EmployeeService (IEmployeeService)
    participant QuerySvc as EmployeeQueryService (IEmployeeQueryService)
    participant AsyncSvc as EmployeeAsyncService (IEmployeeAsyncService)
    participant OrderSvc as OrderService (IOrderService)

    User->>Program: dotnet run (Launches console)
    Program->>UI: DisplayWelcomeBanner() & DisplayMenu()
    UI-->>User: Renders 10 Menu Options
    
    User->>UI: Types Option Selection (e.g. "9")
    UI->>UI: CleanInput() & validate menu range
    
    alt Option 1: View Roster
        Program->>EmpSvc: GetAllEmployees()
        EmpSvc-->>Program: IReadOnlyList<Employee>
        Program-->>User: Formatted Tabular Roster
    else Option 6: OOP & Order Management
        Program->>OrderSvc: CreateOrder(customer, items)
        OrderSvc-->>Program: Order confirmation with inventory deduction
        Program-->>User: Polymorphic Pay & Order status
    else Option 8: Modern C# & LINQ
        Program->>QuerySvc: Execute 8 LINQ Operations
        QuerySvc-->>Program: Filtered, Grouped, Sorted DTOs
        Program-->>User: Console LINQ Deep Dive Output
    else Option 9: Async & Cancellation
        Program->>AsyncSvc: ExportPayrollReportAsync(memoryStream, cts.Token)
        AsyncSvc-->>Program: Stream written asynchronously
        Program-->>User: Async completion & cancellation demo
    else Option 7 or 10: Exit
        Program-->>User: Friendly Goodbye message & Exit
    end
```

---

## 2. File-by-File Responsibility and Connection Map

### 1. `Program.cs` (`App`)
- **Responsibility:** Top-level console entry point (`public static async Task Main`). Renders menus, defensively parses user input, and dispatches to appropriate handlers.
- **Dependencies:** `IEmployeeService`, `IEmployeeQueryService`, `IOrderService`, and demonstration runners.
- **Connects To:** Invokes business methods on `EmployeeService`, `ModernCSharpDemonstration`, and `AsyncDemonstration`.

### 2. `EmployeeService.cs` (`App/Services`)
- **Responsibility:** Manages in-memory employee roster and primary dictionary hash index. Enforces domain validation invariants (`ValidateEmployee`) and raises observer events (`EmployeeAdded`, `SalaryChanged`).
- **Dependencies:** Implements `IEmployeeService` from `Core.Interfaces`.
- **Connects To:** Serves as the data authority for `Program.cs` and `EmployeeAsyncService`.

### 3. `Employee.cs`, `FullTimeEmployee.cs`, `ContractEmployee.cs` (`Core/Models`)
- **Responsibility:** Domain entity hierarchy modeling corporate employees.
- **Polymorphism:** `Employee` defines abstract `CalculateGrossPay()`; `FullTimeEmployee` computes base + bonus, whereas `ContractEmployee` computes hourly rate $\times$ hours worked.
- **Encapsulation:** Properties protect invariants (age 18–75, positive salaries).

### 4. `EmployeeQueryService.cs` (`Core/Services`)
- **Responsibility:** Houses pure, unit-testable LINQ operations (`Where`, `Select`, `OrderBy`, `GroupBy`, `MaxBy`, `Average`, `Sum`).
- **Dependencies:** Implements `IEmployeeQueryService`.
- **Connects To:** Receives `IEnumerable<Employee>` from callers, returning filtered or grouped record snapshots.

### 5. `EmployeeAsyncService.cs` (`Core/Services`)
- **Responsibility:** Implements asynchronous data access, report compilation, and non-blocking stream export with cooperative cancellation support.
- **Dependencies:** Implements `IEmployeeAsyncService`; wraps `IEmployeeService`.
- **Connects To:** Consumed by `AsyncDemonstration.cs` and `EmployeeAsyncServiceTests.cs`.

### 6. `OrderManagement/` (`Core/OrderManagement`)
- **Responsibility:** Models an e-commerce order management system (`Customer`, `Product`, `OrderItem`, `Order`, `IOrderRepository`, `IOrderService`).
- **Connects To:** Demonstrates interface decoupling and repository patterns during Menu Option 6.

### 7. Unit Test Suite (`EmployeeDepartmentManagement.Tests`)
- **Responsibility:** 83 automated xUnit tests validating CRUD, boundaries, null safety, parameterized theories, LINQ queries, async execution, cancellation timeouts, Moq test doubles, and LeetCode solutions.
