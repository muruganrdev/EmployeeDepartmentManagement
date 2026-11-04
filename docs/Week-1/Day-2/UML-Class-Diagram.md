# Domain Model UML Class Diagrams

This document presents the visual object-oriented architecture of the **EmployeeDepartmentManagement** application, covering both the **Employee & Department Domain** and the **Order Management Domain** in `EmployeeDepartmentManagement.Core`.

---

## 1. Employee & Department Management UML Diagram

```mermaid
classDiagram
    %% Interfaces
    class IPayrollCalculable {
        <<interface>>
        +CalculateGrossPay() decimal
        +CalculateTaxDeduction() decimal
        +CalculateNetPay() decimal
    }

    class IEmployeeService {
        <<interface>>
        +AddEmployee(Employee emp, out string error) bool
        +GetEmployeeById(int id) Employee
        +SearchEmployees(string department) List~Employee~
        +SearchEmployeesByName(string query) List~Employee~
        +GetAllEmployees() IReadOnlyList~Employee~
        +GetPayrollStatistics() ValueTuple
        +GeneratePayrollReport() string
        +GetEmployeeSummary(int id) EmployeeSummary
    }

    %% Abstract Base Class
    class Employee {
        <<abstract>>
        -int _id
        -string _firstName
        -string _lastName
        -int _age
        -int _yearsOfExperience
        -Department _department
        +int Id
        +string FirstName
        +string LastName
        +string MiddleName
        +int Age
        +int YearsOfExperience
        +Department Department
        +DateTime JoiningDate
        +bool IsActive
        +string FullName
        +CalculateGrossPay()* decimal
        +CalculateTaxDeduction() decimal
        +CalculateNetPay() decimal
        +GetEmployeeType()* string
        +ShallowCopy()* Employee
        +ToSummary() EmployeeSummary
    }

    %% Concrete Subclasses
    class FullTimeEmployee {
        -decimal _baseSalary
        -decimal _annualBonus
        +decimal BaseSalary
        +decimal AnnualBonus
        +string HealthInsurancePlan
        +CalculateGrossPay() decimal
        +GetEmployeeType() string
        +ShallowCopy() Employee
    }

    class ContractEmployee {
        -decimal _hourlyRate
        -int _hoursWorked
        +decimal HourlyRate
        +int HoursWorked
        +DateTime ContractEndDate
        +string AgencyName
        +CalculateGrossPay() decimal
        +CalculateTaxDeduction() decimal
        +GetEmployeeType() string
        +ShallowCopy() Employee
    }

    %% Value Objects
    class Department {
        -string _code
        -string _name
        +string Code
        +string Name
        +string Description
        +IsValid(string name)$ bool
        +FromName(string name)$ Department
    }

    class EmployeeSummary {
        <<record>>
        +int Id
        +string FullName
        +string DepartmentName
        +string EmployeeType
        +decimal GrossPay
        +decimal NetPay
    }

    class WorkHours {
        <<struct>>
        +int RegularHours
        +int OvertimeHours
        +int TotalHours
    }

    %% Service Implementation
    class EmployeeService {
        -List~Employee~ _employeeList
        -Dictionary~int,Employee~ _employeeIndex
        +AddEmployee(Employee emp, out string error) bool
        +GetEmployeeById(int id) Employee
        +SearchEmployees(string department) List~Employee~
        +SearchEmployeesByName(string query) List~Employee~
        +GetAllEmployees() IReadOnlyList~Employee~
        +GetPayrollStatistics() ValueTuple
        +GeneratePayrollReport() string
        +GetEmployeeSummary(int id) EmployeeSummary
    }

    %% Relationships
    IPayrollCalculable <|.. Employee : implements
    Employee <|-- FullTimeEmployee : inherits (is-a)
    Employee <|-- ContractEmployee : inherits (is-a)
    Employee o-- Department : aggregation (has-a)
    IEmployeeService <|.. EmployeeService : implements
    EmployeeService o-- Employee : manages collection
    Employee ..> EmployeeSummary : creates snapshot
    ContractEmployee ..> WorkHours : utilizes
```

---

## 2. Order Management Domain UML Diagram

```mermaid
classDiagram
    %% Interfaces
    class IOrderRepository {
        <<interface>>
        +Add(Order order) void
        +GetById(int orderId) Order
        +GetAll() IReadOnlyList~Order~
        +Exists(int orderId) bool
    }

    class IOrderService {
        <<interface>>
        +CreateOrder(Customer customer, IEnumerable items) Order
        +GetOrderById(int orderId) Order
        +GetAllOrders() IReadOnlyList~Order~
        +CalculateOrderTotal(int orderId) decimal
    }

    %% Domain Models
    class Customer {
        -int _customerId
        -string _name
        -string _email
        +int CustomerId
        +string Name
        +string Email
    }

    class Product {
        -int _productId
        -string _name
        -decimal _price
        -int _availableStock
        +int ProductId
        +string Name
        +decimal Price
        +int AvailableStock
        +DeductStock(int quantity) void
        +Restock(int quantity) void
    }

    class OrderItem {
        -int _orderItemId
        -Product _product
        -int _quantity
        -decimal _unitPrice
        +int OrderItemId
        +Product Product
        +int Quantity
        +decimal UnitPrice
        +CalculateSubtotal() decimal
    }

    class Order {
        -int _orderId
        -Customer _customer
        -List~OrderItem~ _orderItems
        -DateTime _orderDate
        +int OrderId
        +Customer Customer
        +DateTime OrderDate
        +IReadOnlyList~OrderItem~ OrderItems
        +AddOrderItem(OrderItem item) void
        +AddItem(Product product, int quantity) OrderItem
        +CalculateTotal() decimal
    }

    %% Service and Repository Implementations
    class InMemoryOrderRepository {
        -Dictionary~int,Order~ _orders
        +Add(Order order) void
        +GetById(int orderId) Order
        +GetAll() IReadOnlyList~Order~
        +Exists(int orderId) bool
    }

    class OrderService {
        -IOrderRepository _orderRepository
        -int _nextOrderId
        +CreateOrder(Customer customer, IEnumerable items) Order
        +GetOrderById(int orderId) Order
        +GetAllOrders() IReadOnlyList~Order~
        +CalculateOrderTotal(int orderId) decimal
    }

    %% Relationships
    Customer "1" <-- "many" Order : places (association)
    Order "1" *-- "many" OrderItem : contains (composition)
    OrderItem "many" --> "1" Product : references (association)

    IOrderRepository <|.. InMemoryOrderRepository : implements
    IOrderService <|.. OrderService : implements
    OrderService --> IOrderRepository : depends upon (DIP)
```

---

## 3. Explanation of Order Management Relationships

### 1. Customer and Order (Association)
- A `Customer` places an `Order`.
- The `Order` holds a reference to the `Customer` who placed it.
- **Lifetime Independence**: If an `Order` is completed or deleted, the `Customer` entity remains valid in the system.

### 2. Order and OrderItem (Composition `*--`)
- An `Order` owns a collection of `OrderItem` instances.
- **Coincident Lifetime**: Line items only exist within the context of their owning order. If an order is discarded, its items are discarded with it.

### 3. OrderItem and Product (Association `-->`)
- An `OrderItem` references a `Product` and records the purchased quantity and historical `UnitPrice`.
- The `Product` exists independently in the catalog and maintains its own `AvailableStock`.

### 4. Interface Implementation (`<|..`)
- `InMemoryOrderRepository` implements `IOrderRepository`.
- `OrderService` implements `IOrderService`.

### 5. Dependency Inversion (`-->`)
- `OrderService` depends on the abstraction `IOrderRepository`, injected via its constructor, rather than coupling directly to `InMemoryOrderRepository`.
