# Domain Model UML Class Diagrams – Week 1 (Day 1, 2, and 3)

This document presents the visual object-oriented and functional architecture across all three days of development.

---

## 1. Day 3: Modern C# & LINQ Architecture Diagram

```mermaid
classDiagram
    %% Core Interfaces & DTOs
    class IEmployeeQueryService {
        <<interface>>
        +FilterByMinimumSalary(IEnumerable employees, decimal minSalary) List~Employee~
        +FilterByDepartment(IEnumerable employees, string dept) List~Employee~
        +SortBySalary(IEnumerable employees, bool ascending) List~Employee~
        +SortByName(IEnumerable employees) List~Employee~
        +GroupByDepartment(IEnumerable employees) List~DepartmentGroup~
        +GetAverageSalaryByDepartment(IEnumerable employees) List~DepartmentSalarySummary~
        +GetHighestPaidEmployee(IEnumerable employees) Employee
        +SearchEmployees(IEnumerable employees, ...) List~Employee~
    }

    class DepartmentSalarySummary {
        <<record>>
        +string DepartmentName
        +int EmployeeCount
        +decimal AverageSalary
        +decimal TotalPayroll
    }

    class DepartmentGroup {
        <<record>>
        +string DepartmentName
        +int EmployeeCount
        +IReadOnlyList~Employee~ Employees
    }

    %% Query Service Implementation
    class EmployeeQueryService {
        +FilterByMinimumSalary(...) List~Employee~
        +FilterByDepartment(...) List~Employee~
        +SortBySalary(...) List~Employee~
        +SortByName(...) List~Employee~
        +GroupByDepartment(...) List~DepartmentGroup~
        +GetAverageSalaryByDepartment(...) List~DepartmentSalarySummary~
        +GetHighestPaidEmployee(...) Employee
        +SearchEmployees(...) List~Employee~
    }

    %% Generic Components
    class GenericFilter~T~ {
        +Filter(IEnumerable~T~ source, Func~T,bool~ predicate) IEnumerable~T~
        +FindFirst(IEnumerable~T~ source, Func~T,bool~ predicate) T
        +CountMatching(IEnumerable~T~ source, Func~T,bool~ predicate) int
    }

    class OperationResult~T~ {
        +bool IsSuccess
        +T Data
        +string Message
        +string ErrorCode
        +Success(T data)$ OperationResult~T~
        +Failure(string msg)$ OperationResult~T~
    }

    %% Extensions & Pattern Matcher
    class EmployeeExtensions {
        <<static>>
        +IsHighEarner(this Employee, decimal) bool
        +ToFormattedString(this Employee) string
        +TotalGrossPayroll(this IEnumerable) decimal
        +FilterByDepartmentName(this IEnumerable, string) IEnumerable
        +AverageGrossSalary(this IEnumerable) decimal
    }

    class EmployeePatternMatcher {
        <<static>>
        +ClassifyEmployeeTier(Employee employee)$ string
        +EvaluateBudgetImpact(Employee employee)$ ValueTuple
    }

    %% Event Arguments
    class EmployeeEventArgs {
        +Employee Employee
        +DateTime EventTimestamp
        +string Action
    }

    class SalaryChangedEventArgs {
        +Employee Employee
        +decimal OldSalary
        +decimal NewSalary
        +DateTime EventTimestamp
    }

    %% Relationships
    IEmployeeQueryService <|.. EmployeeQueryService : implements
    EmployeeQueryService ..> DepartmentGroup : returns
    EmployeeQueryService ..> DepartmentSalarySummary : returns
    EmployeeExtensions ..> Employee : extends
    EmployeePatternMatcher ..> Employee : inspects
    IEmployeeService ..> EmployeeEventArgs : publishes
    IEmployeeService ..> SalaryChangedEventArgs : publishes
```

---

## 2. Preserved Day 2: Employee & Department Hierarchy

```mermaid
classDiagram
    class IPayrollCalculable {
        <<interface>>
        +CalculateGrossPay() decimal
        +CalculateTaxDeduction() decimal
        +CalculateNetPay() decimal
    }

    class IEmployeeService {
        <<interface>>
        +EmployeeAdded event
        +SalaryChanged event
        +AddEmployee(Employee, out string) bool
        +UpdateEmployeeSalary(int, decimal, out string) bool
        +GetEmployeeById(int) Employee
        +SearchEmployees(string) List~Employee~
        +GetAllEmployees() IReadOnlyList~Employee~
        +GetPayrollStatistics() ValueTuple
        +GeneratePayrollReport() string
    }

    class Employee {
        <<abstract>>
        +int Id
        +string FirstName
        +string LastName
        +Department Department
        +CalculateGrossPay()* decimal
        +CalculateNetPay() decimal
        +GetEmployeeType()* string
    }

    class FullTimeEmployee {
        +decimal BaseSalary
        +decimal AnnualBonus
        +CalculateGrossPay() decimal
    }

    class ContractEmployee {
        +decimal HourlyRate
        +int HoursWorked
        +CalculateGrossPay() decimal
    }

    class Department {
        +string Code
        +string Name
    }

    IPayrollCalculable <|.. Employee : implements
    Employee <|-- FullTimeEmployee : inherits (is-a)
    Employee <|-- ContractEmployee : inherits (is-a)
    Employee o-- Department : aggregation
    IEmployeeService <|.. EmployeeService : implements
```

---

## 3. Preserved Day 2: Order Management Domain

```mermaid
classDiagram
    class Customer {
        +int CustomerId
        +string Name
        +string Email
    }

    class Product {
        +int ProductId
        +string Name
        +decimal Price
        +int AvailableStock
        +DeductStock(int) void
        +Restock(int) void
    }

    class OrderItem {
        +int OrderItemId
        +Product Product
        +int Quantity
        +decimal UnitPrice
        +CalculateSubtotal() decimal
    }

    class Order {
        +int OrderId
        +Customer Customer
        +IReadOnlyList~OrderItem~ OrderItems
        +CalculateTotal() decimal
    }

    class IOrderRepository {
        <<interface>>
        +Add(Order) void
        +GetById(int) Order
        +GetAll() IReadOnlyList~Order~
    }

    class IOrderService {
        <<interface>>
        +CreateOrder(Customer, IEnumerable) Order
        +GetOrderById(int) Order
    }

    Customer "1" <-- "many" Order : places
    Order "1" *-- "many" OrderItem : contains
    OrderItem "many" --> "1" Product : references
    IOrderRepository <|.. InMemoryOrderRepository : implements
    IOrderService <|.. OrderService : implements
    OrderService --> IOrderRepository : depends upon
```
