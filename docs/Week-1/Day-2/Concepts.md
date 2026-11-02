# Week 1 – Day 2: Object-Oriented Programming (OOP) in C#

## Table of Contents
1. [Introduction to Day 2](#introduction-to-day-2)
2. [Classes, Objects, and Constructors](#classes-objects-and-constructors)
3. [Encapsulation & Data Invariants](#encapsulation--data-invariants)
4. [Inheritance](#inheritance)
5. [Polymorphism (Dynamic Dispatch & Method Overriding)](#polymorphism-dynamic-dispatch--method-overriding)
6. [Interfaces vs Abstract Classes](#interfaces-vs-abstract-classes)
7. [Access Modifiers in C#](#access-modifiers-in-c)
8. [Classes vs Records vs Structs](#classes-vs-records-vs-structs)
9. [Composition vs Inheritance](#composition-vs-inheritance)

---

## 1. Introduction to Day 2

In **Week 1 – Day 2**, we evolved our **EmployeeDepartmentManagement** application from procedural fundamentals into a **clean Object-Oriented Domain Model Class Library** (`EmployeeDepartmentManagement.Core`).

We established a clean architectural separation:
- **`EmployeeDepartmentManagement.Core`**: Class library containing domain abstractions, entities, value objects, and business contracts.
- **`EmployeeDepartmentManagement.App`**: Console application consuming the Core library, driving the user interface, and executing workflows.

---

## 2. Classes, Objects, and Constructors

### What is a Class vs an Object?
- A **Class** is a blueprint or template defining data attributes (fields, properties) and behaviors (methods).
- An **Object** is a concrete instance of a class allocated in computer memory (on the Managed Heap).

### Constructors & Initialization
A constructor initializes an object when created with the `new` keyword. 
In `FullTimeEmployee`:
```csharp
public FullTimeEmployee(
    int id,
    string firstName,
    string lastName,
    string? middleName,
    Department department,
    decimal baseSalary,
    decimal? annualBonus,
    int age,
    int yearsOfExperience,
    DateTime joiningDate,
    string healthInsurancePlan = "Standard Corporate Healthcare",
    bool isActive = true)
    : base(id, firstName, lastName, middleName, department, age, yearsOfExperience, joiningDate, isActive)
{
    BaseSalary = baseSalary;
    AnnualBonus = annualBonus;
    HealthInsurancePlan = healthInsurancePlan;
}
```
Notice how `: base(...)` invokes the parent constructor in `Employee`, promoting clean initialization chaining.

---

## 3. Encapsulation & Data Invariants

### What is Encapsulation?
**Encapsulation** bundles data and methods operating on that data within a class, while restricting direct access to internal state. 

### Why is it Essential?
Without encapsulation, external code could set an employee's salary to `-50000`, age to `5`, or an empty string for their name. Properties guard these invariants:
```csharp
// From Employee.cs
private int _age;

public int Age
{
    get => _age;
    set
    {
        if (value < 18 || value > 75)
            throw new ArgumentOutOfRangeException(nameof(value), "Employee age must be between 18 and 75.");
        _age = value;
    }
}
```
If caller code attempts to assign an invalid age, property validation immediately throws an `ArgumentOutOfRangeException`, safeguarding domain integrity.

---

## 4. Inheritance

### What is Inheritance?
Inheritance enables a derived class (child) to inherit properties and behaviors from a base class (parent), facilitating code reuse and establishing an **"is-a"** relationship.

```
                  ┌───────────────────────────────┐
                  │    abstract class Employee    │
                  └───────────────┬───────────────┘
                                  │
                 ┌────────────────┴────────────────┐
                 │                                 │
                 ▼                                 ▼
┌─────────────────────────────────┐   ┌─────────────────────────────────┐
│     class FullTimeEmployee      │   │      class ContractEmployee     │
│   (is-a salaried employee)      │   │     (is-a temporary worker)     │
└─────────────────────────────────┘   └─────────────────────────────────┘
```

Both `FullTimeEmployee` and `ContractEmployee` inherit:
- `Id`, `FirstName`, `LastName`, `Department`, `Age`, `YearsOfExperience`, `JoiningDate`, `IsActive`.

Derived classes add their own specific data:
- `FullTimeEmployee` adds `BaseSalary`, `AnnualBonus`, `HealthInsurancePlan`.
- `ContractEmployee` adds `HourlyRate`, `HoursWorked`, `ContractEndDate`, `AgencyName`.

---

## 5. Polymorphism (Dynamic Dispatch & Method Overriding)

### What is Polymorphism?
**Polymorphism** ("many forms") allows objects of different derived classes to be treated through a single base-class reference, while executing the derived class's specific behavior at runtime.

### How it is Implemented in Our Solution:
1. In `Employee.cs`, we declared an **abstract method**:
   ```csharp
   public abstract decimal CalculateGrossPay();
   ```
2. In `FullTimeEmployee.cs`, we **override** it:
   ```csharp
   public override decimal CalculateGrossPay() => BaseSalary + (AnnualBonus ?? 0m);
   ```
3. In `ContractEmployee.cs`, we **override** it differently:
   ```csharp
   public override decimal CalculateGrossPay() => HourlyRate * HoursWorked;
   ```

### The Power of Runtime Polymorphism:
In `EmployeeService.GetPayrollStatistics()`:
```csharp
foreach (Employee emp in _employeeList)
{
    // The CLR dynamically resolves the correct method without any if/else checks!
    totalPayroll += emp.CalculateGrossPay(); 
}
```
If we add an `InternEmployee` tomorrow, `EmployeeService` does not require changing a single line of code!

---

## 6. Interfaces vs Abstract Classes

| Feature | Interface (`IPayrollCalculable`) | Abstract Class (`Employee`) |
| :--- | :--- | :--- |
| **Definition** | Pure contract specifying *what* must be done. | Incomplete blueprint specifying *what* and *how*. |
| **State / Fields** | Cannot have instance fields. | Can declare fields (`_id`, `_age`, etc.). |
| **Multiple Inheritance** | A class can implement **multiple** interfaces. | A class can inherit from **only one** base class. |
| **Constructors** | No constructors. | Has constructors (`protected Employee(...)`). |
| **Best Used For** | Cross-cutting capabilities (`IPayable`, `IDisposable`). | Core domain identity sharing state and implementation. |

---

## 7. Access Modifiers in C#

| Modifier | Accessibility Scope | Demonstrated In Our Project |
| :--- | :--- | :--- |
| **`public`** | Accessible anywhere by any assembly. | Public properties (`FullName`, `Id`), methods (`CalculateNetPay`). |
| **`private`** | Accessible only within the declaring class. | Private backing fields (`_baseSalary`, `_age`). |
| **`protected`** | Accessible within the declaring class and derived classes. | Protected constructor in `Employee`. |
| **`internal`** | Accessible anywhere within the **same assembly** (`Core`). | Internal helpers and domain invariants. |
| **`protected internal`** | Accessible within the same assembly OR derived classes in other assemblies. | Framework extension points. |

---

## 8. Classes vs Records vs Structs

### 1. `class` (Reference Type)
- Allocated on the Managed Heap.
- Compares by **reference equality** (memory address) by default.
- Used for long-lived mutable domain entities (`Employee`, `Department`).

### 2. `record` (Value-Based Equality Reference Type)
- C# 9+ feature creating immutable data carriers with minimal boilerplate.
- Compares by **value equality** (checks if all property values are equal).
- Used for `EmployeeSummary`:
  ```csharp
  EmployeeSummary rec1 = new(101, "Aarav", "Engineering", "Full-Time", 90000m, 70200m);
  EmployeeSummary rec2 = new(101, "Aarav", "Engineering", "Full-Time", 90000m, 70200m);
  Console.WriteLine(rec1 == rec2); // True!
  ```

### 3. `struct` (Value Type)
- Allocated on the Call Stack (or inline inside heap objects).
- Zero garbage collection overhead.
- Used for small, lightweight value objects like `WorkHours`:
  ```csharp
  public readonly struct WorkHours(int regularHours, int overtimeHours)
  ```

---

## 9. Composition vs Inheritance

- **Inheritance ("is-a")**: `FullTimeEmployee` *is-an* `Employee`.
- **Composition ("has-a")**: An `Employee` *has-a* `Department`.

### Why Prefer Composition?
If we made `Employee` inherit from `Department`, an employee could never change departments, multiple inheritance would cause diamond dependency issues, and the model would be unnaturally coupled. 
With composition:
```csharp
public class Employee
{
    public Department Department { get; set; } // Composition!
}
```
An employee can transfer departments freely without reconstructing the object hierarchy.
