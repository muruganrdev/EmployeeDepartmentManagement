using EmployeeDepartmentManagement.Core.Models;

namespace EmployeeDepartmentManagement.Core.Interfaces;

/// <summary>
/// Data transfer record representing department-level salary metrics.
/// </summary>
public record DepartmentSalarySummary(
    string DepartmentName,
    int EmployeeCount,
    decimal AverageSalary,
    decimal TotalPayroll
);

/// <summary>
/// Data transfer record representing a department group of employees.
/// </summary>
public record DepartmentGroup(
    string DepartmentName,
    int EmployeeCount,
    IReadOnlyList<Employee> Employees
);

/// <summary>
/// Contract for reusable LINQ query operations on employee collections.
/// Demonstrates separation of query logic from presentation.
/// </summary>
public interface IEmployeeQueryService
{
    // Query 1: Filter by salary
    List<Employee> FilterByMinimumSalary(IEnumerable<Employee> employees, decimal minSalary);

    // Query 2: Filter by department
    List<Employee> FilterByDepartment(IEnumerable<Employee> employees, string departmentName);

    // Query 3: Sort by salary
    List<Employee> SortBySalary(IEnumerable<Employee> employees, bool ascending = true);

    // Query 4: Sort by name
    List<Employee> SortByName(IEnumerable<Employee> employees);

    // Query 5: Group by department
    List<DepartmentGroup> GroupByDepartment(IEnumerable<Employee> employees);

    // Query 6: Average salary by department
    List<DepartmentSalarySummary> GetAverageSalaryByDepartment(IEnumerable<Employee> employees);

    // Query 7: Highest-paid employee
    Employee? GetHighestPaidEmployee(IEnumerable<Employee> employees);

    // Query 8: Multiple-condition search
    List<Employee> SearchEmployees(
        IEnumerable<Employee> employees,
        string? department = null,
        decimal? minSalary = null,
        string? employeeType = null,
        string? nameQuery = null);
}
