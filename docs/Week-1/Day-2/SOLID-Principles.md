# SOLID Design Principles in C# (.NET)

The **SOLID** principles are five foundational object-oriented design principles formulated by Robert C. Martin ("Uncle Bob") to create maintainable, flexible, and scalable software architectures.

---

## 1. Single Responsibility Principle (SRP)

> *"A class should have one, and only one, reason to change."*

### What it Means:
Each class should be responsible for a single part of the application's functionality, and that responsibility should be entirely encapsulated by the class.

### How Demonstrated in Our Application:
- **Employee Domain**:
  - [`Employee.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Models/Employee.cs): Responsible *only* for employee state, domain validation, and core gross/net pay calculation. It does not handle persistence or UI rendering.
  - [`EmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs): Responsible *only* for collection management, search indexing, and report formatting.
- **Order Management Domain**:
  - [`Customer.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Models/Customer.cs): Encapsulates customer identity and email validation.
  - [`Product.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Models/Product.cs): Encapsulates catalog pricing and inventory stock adjustments.
  - [`OrderItem.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Models/OrderItem.cs): Encapsulates line-item quantity, historical unit price, and subtotal calculation.
  - [`Order.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Models/Order.cs): Manages order item composition and grand total aggregation.
  - [`InMemoryOrderRepository.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Repositories/InMemoryOrderRepository.cs): Responsible *only* for storing and retrieving orders.
  - [`OrderService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Services/OrderService.cs): Responsible *only* for business orchestration (stock deduction + order placement).

---

## 2. Open/Closed Principle (OCP)

> *"Software entities should be open for extension, but closed for modification."*

### What it Means:
You should be able to introduce new behavior or features without altering existing, tested source code.

### How Demonstrated in Our Application:
- **Polymorphic Employee Hierarchy**:
  - `Employee` defines the abstract method `CalculateGrossPay()`.
  - Adding `ContractEmployee` did **not** require modifying `FullTimeEmployee` or `Employee.cs`.
  - Adding an `InternEmployee` tomorrow only requires subclassing `Employee`; `EmployeeService.GetPayrollStatistics()` will calculate payroll without changing a single line of code.
- **Order Repository Extensibility**:
  - `OrderService` depends on `IOrderRepository`. When we introduce a database (SQL Server / Entity Framework Core) in future modules, we can create `SqlOrderRepository : IOrderRepository` without modifying `OrderService.cs`.

---

## 3. Liskov Substitution Principle (LSP)

> *"Objects of a superclass should be replaceable with objects of a subclass without affecting the correctness of the program."*

### What it Means:
Derived classes must adhere to the contract established by their base class without throwing unexpected exceptions or violating preconditions.

### How Demonstrated in Our Application:
- In [`EmployeeService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs), `_employeeList` is typed as `List<Employee>`.
- Both `FullTimeEmployee` and `ContractEmployee` can substitute `Employee` interchangeably:
  ```csharp
  foreach (Employee emp in _employeeList)
  {
      decimal net = emp.CalculateNetPay(); // Both subtypes satisfy base contracts
  }
  ```
- Neither subclass throws `NotImplementedException` or alters the expected behavior of base methods.

---

## 4. Interface Segregation Principle (ISP)

> *"Clients should not be forced to depend on interfaces they do not use."*

### What it Means:
Many small, client-specific interfaces are better than one bloated, "fat" general-purpose interface.

### How Demonstrated in Our Application:
- Instead of a single monolithic `IEnterpriseStore` containing employee payroll, customer ordering, catalog management, and console printing:
  - [`IPayrollCalculable`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Interfaces/IPayrollCalculable.cs): Focused exclusively on payroll calculations (`CalculateGrossPay`, `CalculateTaxDeduction`, `CalculateNetPay`).
  - [`IEmployeeService`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Interfaces/IEmployeeService.cs): Focused exclusively on employee lifecycle operations.
  - [`IOrderRepository`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Interfaces/IOrderRepository.cs): Focused exclusively on order data persistence.
  - [`IOrderService`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Interfaces/IOrderService.cs): Focused exclusively on order coordination.

---

## 5. Dependency Inversion Principle (DIP)

> *"High-level modules should not depend on low-level modules. Both should depend on abstractions. Abstractions should not depend on details; details should depend on abstractions."*

### How Demonstrated in Our Application:
1. **`Program.cs` $\rightarrow$ `IEmployeeService`**:
   ```csharp
   private static readonly IEmployeeService _employeeService = new EmployeeService();
   ```
2. **`OrderService` $\rightarrow$ `IOrderRepository`**:
   [`OrderService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/OrderManagement/Services/OrderService.cs) does **not** create a concrete repository directly with `new InMemoryOrderRepository()`. Instead, it requires `IOrderRepository` in its constructor:
   ```csharp
   public class OrderService : IOrderService
   {
       private readonly IOrderRepository _orderRepository;

       public OrderService(IOrderRepository orderRepository)
       {
           _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
       }
   }
   ```
   This fully decouples the business coordinator from the storage medium, enabling simple mocking during unit testing.
