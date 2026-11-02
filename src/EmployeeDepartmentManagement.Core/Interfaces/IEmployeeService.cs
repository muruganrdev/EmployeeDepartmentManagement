using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.ValueObjects;

namespace EmployeeDepartmentManagement.Core.Interfaces;

/// <summary>
/// Service contract defining operations for managing employees and generating payroll reports.
/// Demonstrates Dependency Inversion Principle (DIP) and testability through interfaces.
/// </summary>
public interface IEmployeeService
{
    bool AddEmployee(Employee emp, out string errorMessage);
    Employee? GetEmployeeById(int id);
    List<Employee> SearchEmployees(string department);
    List<Employee> SearchEmployeesByName(string nameQuery);
    IReadOnlyList<Employee> GetAllEmployees();
    (int Count, decimal TotalPayroll, decimal AverageSalary, decimal HighestSalary, decimal LowestSalary) GetPayrollStatistics();
    string GeneratePayrollReport();
    EmployeeSummary GetEmployeeSummary(int id);
}
