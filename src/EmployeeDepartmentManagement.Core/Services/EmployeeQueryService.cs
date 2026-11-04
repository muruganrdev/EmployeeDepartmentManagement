using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.Models;

namespace EmployeeDepartmentManagement.Core.Services;

/// <summary>
/// Reusable, unit-testable service executing the 8 core LINQ operations.
/// Demonstrates Where, Select, OrderBy, OrderByDescending, GroupBy, Average, MaxBy, Count, Any, and ToList.
/// Contains ZERO console dependencies.
/// </summary>
public class EmployeeQueryService : IEmployeeQueryService
{
    /// <summary>
    /// Query 1: Filter employees whose gross compensation is greater than or equal to minSalary.
    /// Uses: LINQ Where operator with lambda expression predicate.
    /// </summary>
    public List<Employee> FilterByMinimumSalary(IEnumerable<Employee> employees, decimal minSalary)
    {
        ArgumentNullException.ThrowIfNull(employees);

        return employees
            .Where(emp => emp.CalculateGrossPay() >= minSalary)
            .ToList();
    }

    /// <summary>
    /// Query 2: Filter employees belonging to a selected department (case-insensitive).
    /// Uses: LINQ Where operator with ordinal case-insensitive string comparison.
    /// </summary>
    public List<Employee> FilterByDepartment(IEnumerable<Employee> employees, string departmentName)
    {
        ArgumentNullException.ThrowIfNull(employees);

        if (string.IsNullOrWhiteSpace(departmentName))
            return employees.ToList();

        string trimmed = departmentName.Trim();
        return employees
            .Where(emp => string.Equals(emp.Department.Name, trimmed, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Query 3: Sort employees by gross compensation in ascending or descending order.
    /// Uses: LINQ OrderBy or OrderByDescending operators.
    /// </summary>
    public List<Employee> SortBySalary(IEnumerable<Employee> employees, bool ascending = true)
    {
        ArgumentNullException.ThrowIfNull(employees);

        return ascending
            ? employees.OrderBy(emp => emp.CalculateGrossPay()).ToList()
            : employees.OrderByDescending(emp => emp.CalculateGrossPay()).ToList();
    }

    /// <summary>
    /// Query 4: Sort employees alphabetically by their full name.
    /// Uses: LINQ OrderBy operator.
    /// </summary>
    public List<Employee> SortByName(IEnumerable<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);

        return employees
            .OrderBy(emp => emp.FullName)
            .ToList();
    }

    /// <summary>
    /// Query 5: Group employees based on their department.
    /// Uses: LINQ GroupBy operator combined with Select projection.
    /// </summary>
    public List<DepartmentGroup> GroupByDepartment(IEnumerable<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);

        return employees
            .GroupBy(emp => emp.Department.Name)
            .Select(group => new DepartmentGroup(
                DepartmentName: group.Key,
                EmployeeCount: group.Count(),
                Employees: group.ToList()))
            .OrderBy(group => group.DepartmentName)
            .ToList();
    }

    /// <summary>
    /// Query 6: Calculate the average and total gross employee salary for each department.
    /// Uses: LINQ GroupBy, Average, and Sum aggregate operators.
    /// </summary>
    public List<DepartmentSalarySummary> GetAverageSalaryByDepartment(IEnumerable<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);

        return employees
            .GroupBy(emp => emp.Department.Name)
            .Select(group => new DepartmentSalarySummary(
                DepartmentName: group.Key,
                EmployeeCount: group.Count(),
                AverageSalary: Math.Round(group.Average(emp => emp.CalculateGrossPay()), 2),
                TotalPayroll: group.Sum(emp => emp.CalculateGrossPay())))
            .OrderByDescending(summary => summary.AverageSalary)
            .ToList();
    }

    /// <summary>
    /// Query 7: Retrieve the employee with the highest salary. Handles empty collections safely.
    /// Uses: LINQ MaxBy operator (introduced in .NET 6/9) returning null if collection is empty.
    /// </summary>
    public Employee? GetHighestPaidEmployee(IEnumerable<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);

        // MaxBy returns the element with the maximum key value, or null if sequence contains no elements
        return employees.MaxBy(emp => emp.CalculateGrossPay());
    }

    /// <summary>
    /// Query 8: Multi-condition dynamic employee search combining department, salary, type, and name.
    /// Uses: Chained LINQ Where operators building a deferred query pipeline before materialization.
    /// </summary>
    public List<Employee> SearchEmployees(
        IEnumerable<Employee> employees,
        string? department = null,
        decimal? minSalary = null,
        string? employeeType = null,
        string? nameQuery = null)
    {
        ArgumentNullException.ThrowIfNull(employees);

        // Start with deferred queryable sequence
        IEnumerable<Employee> query = employees;

        if (!string.IsNullOrWhiteSpace(department))
        {
            string dept = department.Trim();
            query = query.Where(emp => string.Equals(emp.Department.Name, dept, StringComparison.OrdinalIgnoreCase));
        }

        if (minSalary.HasValue && minSalary.Value > 0m)
        {
            decimal min = minSalary.Value;
            query = query.Where(emp => emp.CalculateGrossPay() >= min);
        }

        if (!string.IsNullOrWhiteSpace(employeeType))
        {
            string type = employeeType.Trim();
            query = query.Where(emp => string.Equals(emp.GetEmployeeType(), type, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(nameQuery))
        {
            string search = nameQuery.Trim();
            query = query.Where(emp => emp.FullName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        // Materialize deferred query
        return query.ToList();
    }
}
