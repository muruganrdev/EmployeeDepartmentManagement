# Week 1 – Day 2: OOP Implementation Notes

This document captures architectural design decisions and practical notes regarding the Day 2 Object-Oriented expansion of **EmployeeDepartmentManagement**.

---

## 1. Architectural Restructuring

In Day 1, our console application had models and service classes co-located in the same project (`EmployeeDepartmentManagement.App`). 
For Day 2, we transitioned to a **Clean Two-Project Architecture**:

1. **`EmployeeDepartmentManagement.Core` (Class Library, .NET 9)**:
   - Houses the business domain, independent of any UI or presentation layer.
   - Contains `Interfaces/`, `Models/`, and `ValueObjects/`.
   - Has zero dependencies on `Console` or I/O.
2. **`EmployeeDepartmentManagement.App` (Console Application, .NET 9)**:
   - Contains the presentation logic, user prompt sanitization, and terminal rendering.
   - References `EmployeeDepartmentManagement.Core`.
   - Depends only on the `IEmployeeService` abstraction.

---

## 2. Preservation of Day 1 Functionality

A vital requirement of Day 2 was: **"Do not delete or unnecessarily rewrite working Day 1 functionality."**

### How Day 1 Features Were Preserved:
1. **Interactive Menu**: All 7 menu options remain identical in command codes:
   - Option 1: View all employees (now enhanced with polymorphic employment types).
   - Option 2: Add employee (now allows choosing between Full-Time and Contract).
   - Option 3: Search employee by ID (now returns polymorphic compensation and record snapshots).
   - Option 4: Search employees by department.
   - Option 5: View payroll statistics (now computes polymorphic payroll).
   - Option 6: Live C# memory and OOP demonstration.
   - Option 7: Exit application.
2. **Zero Breaking Changes**: Seeded data from Day 1 (`Aarav Sharma`, `Priya Natarajan`, `Murugan R`) was retained as `FullTimeEmployee` records, while adding `Ananya Deshmukh` and `Vikram Malhotra` as `ContractEmployee` records to demonstrate polymorphism.

---

## 3. Code Evidence for Key OOP Concepts

### A. Abstract Class & Abstract Methods
[`Models/Employee.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Models/Employee.cs):
```csharp
public abstract class Employee : IPayrollCalculable
{
    public abstract decimal CalculateGrossPay();
    public abstract string GetEmployeeType();
}
```

### B. Method Overriding in Subclasses
[`Models/FullTimeEmployee.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Models/FullTimeEmployee.cs):
```csharp
public override decimal CalculateGrossPay()
{
    return BaseSalary + (AnnualBonus ?? 0m);
}
```
[`Models/ContractEmployee.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Models/ContractEmployee.cs):
```csharp
public override decimal CalculateGrossPay()
{
    return HourlyRate * HoursWorked;
}
```

### C. Interface Implementation
[`Models/Employee.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Models/Employee.cs) & [`Services/EmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs):
- `Employee` implements `IPayrollCalculable`.
- `EmployeeService` implements `IEmployeeService`.

### D. Record for Value Equality
[`ValueObjects/EmployeeSummary.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/ValueObjects/EmployeeSummary.cs):
```csharp
public record EmployeeSummary(
    int Id,
    string FullName,
    string DepartmentName,
    string EmployeeType,
    decimal GrossPay,
    decimal NetPay
);
```

### E. Readonly Struct for Stack Allocation
[`ValueObjects/WorkHours.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/ValueObjects/WorkHours.cs):
```csharp
public readonly struct WorkHours
{
    public int RegularHours { get; }
    public int OvertimeHours { get; }
    public int TotalHours => RegularHours + OvertimeHours;
}
```
