using EmployeeDepartmentManagement.Core.Models;

namespace EmployeeDepartmentManagement.Core.Interfaces;

/// <summary>
/// Service contract defining asynchronous operations for employee data access,
/// report generation, and stream export with cooperative cancellation support.
/// Demonstrates Week 1 - Day 4: Task, Task<T>, async/await, and CancellationToken.
/// </summary>
public interface IEmployeeAsyncService
{
    /// <summary>
    /// Asynchronously retrieves an employee by their unique identifier.
    /// Supports cooperative cancellation via CancellationToken.
    /// </summary>
    /// <param name="id">The employee identifier.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A Task containing the Employee if found, or null if not found.</returns>
    Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all employees.
    /// Supports cooperative cancellation via CancellationToken.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A Task containing a read-only list of all employees.</returns>
    Task<IReadOnlyList<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously compiles an extensive employee payroll summary report.
    /// Periodically checks the cancellation token between employee records.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A Task containing the formatted report text.</returns>
    Task<string> GenerateEmployeeReportAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously streams employee data to a destination Stream (e.g., FileStream or MemoryStream).
    /// Demonstrates genuine asynchronous non-blocking I/O with CancellationToken passing.
    /// </summary>
    /// <param name="destinationStream">The writable destination stream.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A Task containing the total number of bytes written.</returns>
    Task<int> ExportPayrollReportAsync(Stream destinationStream, CancellationToken cancellationToken = default);
}
