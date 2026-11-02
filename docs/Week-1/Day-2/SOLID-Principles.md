# SOLID Design Principles in C# (.NET)

The **SOLID** principles are five foundational object-oriented design principles formulated by Robert C. Martin ("Uncle Bob") to create maintainable, flexible, and scalable software architectures.

---

## 1. Single Responsibility Principle (SRP)

> *"A class should have one, and only one, reason to change."*

### What it Means:
Each class should be responsible for a single part of the application's functionality, and that responsibility should be entirely encapsulated by the class.

### Why it is Important:
If a class handles multiple responsibilities (e.g., business calculations, console rendering, and data persistence), modifying how reports are printed risks breaking core calculation logic.

### Real-World Analogy:
A Swiss Army knife has a blade, scissors, and a screwdriver. But a master chef uses a dedicated chef's knife for cutting and a screwdriver only for fixing cabinets. Dedicated tools do one job reliably.

### How Demonstrated in Our Application:
- [`Employee.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Models/Employee.cs): Responsible *only* for employee data, calculations, and domain invariants. It does not know how to print to the console or manage lists.
- [`EmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs): Responsible *only* for in-memory employee collection operations, validation, and payroll reporting.
- [`Program.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Program.cs): Responsible *only* for user interface input/output, menu routing, and top-level error handling.

---

## 2. Open/Closed Principle (OCP)

> *"Software entities (classes, modules, functions) should be open for extension, but closed for modification."*

### What it Means:
You should be able to introduce new behavior or features without altering existing, tested source code.

### Why it is Important:
Modifying existing production classes to support new features introduces regression bugs in previously tested logic.

### Real-World Analogy:
Your smartphone has an operating system. To add a flashlight feature, you don't crack open the phone to solder new wires into the motherboard; you download an app that integrates with the OS camera API.

### How Demonstrated in Our Application:
When introducing `ContractEmployee`, we did **not** modify `FullTimeEmployee.cs` or add `if (type == "Contract")` conditional branches throughout our codebase. 
Instead:
1. `Employee` defines the abstract method `CalculateGrossPay()`.
2. `FullTimeEmployee` overrides it with salary + bonus.
3. `ContractEmployee` extends `Employee` and overrides it with hourly rate * hours.
4. Adding an `InternEmployee` tomorrow only requires subclassing `Employee`; `EmployeeService.GetPayrollStatistics()` works without modification!

---

## 3. Liskov Substitution Principle (LSP)

> *"Objects of a superclass should be replaceable with objects of a subclass without affecting the correctness of the program."*

### What it Means:
Derived classes must adhere to the contract established by their base class. A derived class should never throw `NotImplementedException` or drastically change expected behavior for base methods.

### Real-World Analogy:
If it looks like a duck and quacks like a duck, but needs batteries to quack, it is NOT a proper substitute for a biological duck.

### How Demonstrated in Our Application:
In [`EmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs), `_employeeList` is typed as `List<Employee>`. 
```csharp
Employee ft = new FullTimeEmployee(...);
Employee ct = new ContractEmployee(...);

_employeeList.Add(ft);
_employeeList.Add(ct);

foreach (Employee emp in _employeeList)
{
    // Both FullTimeEmployee and ContractEmployee can substitute Employee seamlessly
    decimal net = emp.CalculateNetPay(); 
}
```
Both subclasses fulfill the contract of `CalculateGrossPay()` and `CalculateNetPay()` without breaking callers.

---

## 4. Interface Segregation Principle (ISP)

> *"Clients should not be forced to depend on interfaces they do not use."*

### What it Means:
Many small, specific interfaces are better than one bloated, "fat" general-purpose interface.

### Real-World Analogy:
A restaurant menu should have separate sections: Food Menu and Drinks Menu. You shouldn't be forced to buy a catering package just to order a coffee.

### How Demonstrated in Our Application:
Instead of creating a massive `IEnterpriseEntity` containing printing, saving, logging, and calculating, we created a focused, single-purpose interface:
[`IPayrollCalculable.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Interfaces/IPayrollCalculable.cs):
```csharp
public interface IPayrollCalculable
{
    decimal CalculateGrossPay();
    decimal CalculateTaxDeduction();
    decimal CalculateNetPay();
}
```
Any future entity requiring payroll processing (e.g., `ThirdPartyVendor`, `SoftwareLicense`) can implement `IPayrollCalculable` without being forced to implement employee properties like `Age` or `JoiningDate`.

---

## 5. Dependency Inversion Principle (DIP)

> *"High-level modules should not depend on low-level modules. Both should depend on abstractions. Abstractions should not depend on details; details should depend on abstractions."*

### What it Means:
Decouple high-level business coordinators from low-level implementation details by introducing interfaces between them.

### Real-World Analogy:
A lamp does not solder its electrical wires directly into the power plant; it plugs into a standardized wall socket. The socket is the abstraction.

### How Demonstrated in Our Application:
In [`Program.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Program.cs):
```csharp
// Program depends on the IEmployeeService interface, not the concrete implementation details
private static readonly IEmployeeService _employeeService = new EmployeeService();
```
When we introduce unit testing on Day 4, we can easily swap `EmployeeService` with a `MockEmployeeService` without changing `Program.cs`.
