namespace EmployeeDepartmentManagement.Core.ValueObjects;

/// <summary>
/// Immutable Data Transfer Record representing an Employee Summary snapshot.
/// Demonstrates C# Record types, positional syntax, built-in value equality,
/// and non-destructive mutation via 'with' expressions.
/// </summary>
public record EmployeeSummary(
    int Id,
    string FullName,
    string DepartmentName,
    string EmployeeType,
    decimal GrossPay,
    decimal NetPay
);
