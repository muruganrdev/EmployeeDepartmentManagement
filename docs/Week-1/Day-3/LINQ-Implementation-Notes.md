# Week 1 – Day 3: LINQ Implementation Notes

This document provides a technical walkthrough of the eight LINQ operations implemented in [`EmployeeQueryService.cs`](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.Core/Services/EmployeeQueryService.cs).

---

## The Eight LINQ Operations

### Query 1: Filter Employees by Minimum Salary
- **Method Signature**: `List<Employee> FilterByMinimumSalary(IEnumerable<Employee> employees, decimal minSalary)`
- **LINQ Operator Used**: `Where`
- **Purpose**: Retrieves all employees whose calculated gross compensation meets or exceeds a target amount.
- **Code**:
  ```csharp
  return employees.Where(emp => emp.CalculateGrossPay() >= minSalary).ToList();
  ```
- **Operator Role**: `Where` streams elements matching the boolean predicate `emp.CalculateGrossPay() >= minSalary`.
- **Sample Input**: `minSalary = 75000`
- **Sample Output**:
  - `Aarav Sharma` ($90,000.00)
  - `Murugan R` ($103,000.00)
- **Time Complexity**: $O(n)$ where $n$ is the total number of employees in the source collection.

---

### Query 2: Filter Employees by Department
- **Method Signature**: `List<Employee> FilterByDepartment(IEnumerable<Employee> employees, string departmentName)`
- **LINQ Operator Used**: `Where` with `string.Equals(..., StringComparison.OrdinalIgnoreCase)`
- **Purpose**: Retrieves employees belonging to a specific department case-insensitively.
- **Code**:
  ```csharp
  return employees.Where(emp => string.Equals(emp.Department.Name, departmentName.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
  ```
- **Sample Input**: `departmentName = "Engineering"`
- **Sample Output**:
  - `[Full-Time] Aarav Sharma`
  - `[Full-Time] Murugan R`
- **Time Complexity**: $O(n)$.

---

### Query 3: Sort Employees by Salary
- **Method Signature**: `List<Employee> SortBySalary(IEnumerable<Employee> employees, bool ascending = true)`
- **LINQ Operators Used**: `OrderBy`, `OrderByDescending`
- **Purpose**: Orders employees by gross compensation in ascending or descending order.
- **Code**:
  ```csharp
  return ascending
      ? employees.OrderBy(emp => emp.CalculateGrossPay()).ToList()
      : employees.OrderByDescending(emp => emp.CalculateGrossPay()).ToList();
  ```
- **Operator Role**: `OrderByDescending` creates an ordered sequence by evaluating the salary selector key.
- **Sample Output (Descending)**:
  1. `Murugan R`: $103,000.00
  2. `Aarav Sharma`: $90,000.00
  3. `Priya R Natarajan`: $65,500.00
  4. `Ananya S Deshmukh`: $10,400.00
  5. `Vikram Malhotra`: $8,250.00
- **Time Complexity**: $O(n \log n)$ due to the comparison-based sort algorithm utilized by .NET.

---

### Query 4: Sort Employees Alphabetically by Name
- **Method Signature**: `List<Employee> SortByName(IEnumerable<Employee> employees)`
- **LINQ Operator Used**: `OrderBy`
- **Purpose**: Sorts employees alphabetically by their calculated `FullName` property.
- **Code**:
  ```csharp
  return employees.OrderBy(emp => emp.FullName).ToList();
  ```
- **Sample Output**:
  - `Aarav Sharma`
  - `Ananya S Deshmukh`
  - `Murugan R`
  - `Priya R Natarajan`
  - `Vikram Malhotra`
- **Time Complexity**: $O(n \log n)$.

---

### Query 5: Group Employees by Department
- **Method Signature**: `List<DepartmentGroup> GroupByDepartment(IEnumerable<Employee> employees)`
- **LINQ Operators Used**: `GroupBy`, `Select`, `OrderBy`
- **Purpose**: Groups employee records by their department name, returning structured `DepartmentGroup` DTOs.
- **Code**:
  ```csharp
  return employees
      .GroupBy(emp => emp.Department.Name)
      .Select(group => new DepartmentGroup(
          DepartmentName: group.Key,
          EmployeeCount: group.Count(),
          Employees: group.ToList()))
      .OrderBy(group => group.DepartmentName)
      .ToList();
  ```
- **Operator Role**: `GroupBy` partitions the collection into sub-sequences sharing the same key (`emp.Department.Name`).
- **Sample Output**:
  - Engineering (2 staff): Aarav Sharma, Murugan R
  - Finance (1 staff): Ananya S Deshmukh
  - Human Resources (1 staff): Priya R Natarajan
  - Marketing (1 staff): Vikram Malhotra
- **Time Complexity**: $O(n + k \log k)$ where $k$ is the number of distinct departments.

---

### Query 6: Average & Total Salary by Department
- **Method Signature**: `List<DepartmentSalarySummary> GetAverageSalaryByDepartment(IEnumerable<Employee> employees)`
- **LINQ Operators Used**: `GroupBy`, `Average`, `Sum`, `Select`, `OrderByDescending`
- **Purpose**: Computes departmental headcount, aggregate payroll, and average compensation.
- **Code**:
  ```csharp
  return employees
      .GroupBy(emp => emp.Department.Name)
      .Select(group => new DepartmentSalarySummary(
          DepartmentName: group.Key,
          EmployeeCount: group.Count(),
          AverageSalary: Math.Round(group.Average(emp => emp.CalculateGrossPay()), 2),
          TotalPayroll: group.Sum(emp => emp.CalculateGrossPay())))
      .OrderByDescending(summary => summary.AverageSalary)
      .ToList();
  ```
- **Sample Output**:
  - `Engineering`       | Staff: 2 | Average: $96,500.00 | Total: $193,000.00
  - `Human Resources`   | Staff: 1 | Average: $65,500.00 | Total: $65,500.00
  - `Finance`           | Staff: 1 | Average: $10,400.00 | Total: $10,400.00
  - `Marketing`         | Staff: 1 | Average: $8,250.00  | Total: $8,250.00
- **Time Complexity**: $O(n + k \log k)$.

---

### Query 7: Find the Highest-Paid Employee
- **Method Signature**: `Employee? GetHighestPaidEmployee(IEnumerable<Employee> employees)`
- **LINQ Operator Used**: `MaxBy`
- **Purpose**: Finds the employee with maximum compensation in a single pass, safely returning `null` if the input is empty.
- **Code**:
  ```csharp
  return employees.MaxBy(emp => emp.CalculateGrossPay());
  ```
- **Operator Role**: Unlike `OrderByDescending().FirstOrDefault()` (which sorts the entire list in $O(n \log n)$), `MaxBy` traverses the list once in $O(n)$ time while tracking the maximum element.
- **Sample Output**: `Murugan R` ($103,000.00).
- **Time Complexity**: $O(n)$ time and $O(1)$ auxiliary space.

---

### Query 8: Multiple-Condition Employee Search
- **Method Signature**:
  ```csharp
  List<Employee> SearchEmployees(
      IEnumerable<Employee> employees,
      string? department = null,
      decimal? minSalary = null,
      string? employeeType = null,
      string? nameQuery = null)
  ```
- **LINQ Operators Used**: Chained deferred `Where` queries and final `ToList()` materialization.
- **Purpose**: Supports flexible filtering combining any subset of department, minimum compensation, employment classification, and partial name search.
- **Code**:
  ```csharp
  IEnumerable<Employee> query = employees;
  if (!string.IsNullOrWhiteSpace(department))
      query = query.Where(emp => string.Equals(emp.Department.Name, department.Trim(), StringComparison.OrdinalIgnoreCase));
  if (minSalary.HasValue)
      query = query.Where(emp => emp.CalculateGrossPay() >= minSalary.Value);
  if (!string.IsNullOrWhiteSpace(employeeType))
      query = query.Where(emp => string.Equals(emp.GetEmployeeType(), employeeType.Trim(), StringComparison.OrdinalIgnoreCase));
  if (!string.IsNullOrWhiteSpace(nameQuery))
      query = query.Where(emp => emp.FullName.Contains(nameQuery.Trim(), StringComparison.OrdinalIgnoreCase));

  return query.ToList();
  ```
- **Sample Output** (Criteria: Dept = "Engineering", MinSalary = $80,000, Type = "Full-Time"):
  - `[101] Aarav Sharma` ($90,000.00)
  - `[103] Murugan R` ($103,000.00)
- **Time Complexity**: $O(n)$ where $n$ is the employee collection count.
