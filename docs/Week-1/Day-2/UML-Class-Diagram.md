# Domain Model UML Class Diagram

This document presents the visual object-oriented architecture of the **EmployeeDepartmentManagement** domain model library (`EmployeeDepartmentManagement.Core`) and application service layer (`EmployeeDepartmentManagement.App`).

---

## 1. Mermaid UML Class Diagram

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
        +GetPayrollStatistics() ValueTuple
        +GeneratePayrollReport() string
        +GetEmployeeSummary(int id) EmployeeSummary
    }

    %% Relationships
    IPayrollCalculable <|.. Employee : implements
    Employee <|-- FullTimeEmployee : inherits (is-a)
    Employee <|-- ContractEmployee : inherits (is-a)
    Employee *-- Department : composition (has-a)
    IEmployeeService <|.. EmployeeService : implements
    EmployeeService o-- Employee : manages collection (aggregation)
    Employee ..> EmployeeSummary : creates snapshot
    ContractEmployee ..> WorkHours : utilizes
```

---

## 2. Explanation of Class Relationships

### 1. Interface Implementation (`..|>`)
- `Employee` implements `IPayrollCalculable`: The abstract class binds itself to the payroll calculation contract. Any concrete employee subclass must satisfy these payroll calculation methods.
- `EmployeeService` implements `IEmployeeService`: Decouples the console UI from concrete storage implementation details (Dependency Inversion).

### 2. Generalization / Inheritance (`--|>`)
- `FullTimeEmployee` inherits from `Employee`: Permanent salaried employees reuse base properties (`Id`, `FullName`, `Department`, `Age`) and supply specific compensation logic (`BaseSalary + Bonus`).
- `ContractEmployee` inherits from `Employee`: Hourly contractors reuse common employee identities while supplying contractor-specific rules (`HourlyRate * HoursWorked`).

### 3. Composition (`*--`)
- `Employee` *has-a* `Department`: An employee maintains an instance of `Department`. The department object encapsulates corporate department codes and validation logic.

### 4. Aggregation / Collection (`o--`)
- `EmployeeService` maintains `List<Employee>` and `Dictionary<int, Employee>`: The service aggregates multiple employee instances in memory, operating on them polymorphically.

### 5. Dependency / Creation (`..>`)
- `Employee` creates `EmployeeSummary`: A lightweight, immutable record snapshot for data transfer and display.
- `ContractEmployee` utilizes `WorkHours`: A readonly struct value object validating hourly tracking.
