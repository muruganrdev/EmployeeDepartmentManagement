using EmployeeDepartmentManagement.Core.Events;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.ValueObjects;

namespace EmployeeDepartmentManagement.Core.Interfaces;

/// <summary>
/// Service contract defining operations for managing employees and generating payroll reports.
/// Demonstrates Dependency Inversion Principle (DIP), testability through interfaces,
/// and Event-driven publisher notifications.
/// </summary>
public interface IEmployeeService
{
    event EventHandler<EmployeeEventArgs>? EmployeeAdded;
    event EventHandler<SalaryChangedEventArgs>? SalaryChanged;

    bool AddEmployee(Employee emp, out string errorMessage);
    bool UpdateEmployeeSalary(int id, decimal newSalary, out string errorMessage);
    Employee? GetEmployeeById(int id);
    List<Employee> SearchEmployees(string department);
    List<Employee> SearchEmployeesByName(string nameQuery);
    IReadOnlyList<Employee> GetAllEmployees();
    (int Count, decimal TotalPayroll, decimal AverageSalary, decimal HighestSalary, decimal LowestSalary) GetPayrollStatistics();
    string GeneratePayrollReport();
    EmployeeSummary GetEmployeeSummary(int id);
}
