using EmployeeDepartmentManagement.Core.Models;

namespace EmployeeDepartmentManagement.Core.Events;

/// <summary>
/// Event arguments providing metadata when an employee lifecycle event occurs.
/// Demonstrates C# Events, EventArgs inheritance, and decoupled notifications.
/// </summary>
public class EmployeeEventArgs : EventArgs
{
    public Employee Employee { get; }
    public DateTime EventTimestamp { get; }
    public string Action { get; }

    public EmployeeEventArgs(Employee employee, string action)
    {
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        Action = action ?? string.Empty;
        EventTimestamp = DateTime.UtcNow;
    }
}

/// <summary>
/// Event arguments providing data when an employee's compensation is updated.
/// </summary>
public class SalaryChangedEventArgs : EventArgs
{
    public Employee Employee { get; }
    public decimal OldSalary { get; }
    public decimal NewSalary { get; }
    public DateTime EventTimestamp { get; }

    public SalaryChangedEventArgs(Employee employee, decimal oldSalary, decimal newSalary)
    {
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        OldSalary = oldSalary;
        NewSalary = newSalary;
        EventTimestamp = DateTime.UtcNow;
    }
}
