using System.Text;
using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.Models;

namespace EmployeeDepartmentManagement.Core.Services;

/// <summary>
/// Service implementing asynchronous employee operations with cooperative CancellationToken support.
/// Demonstrates:
/// 1. Task and Task<T> return types
/// 2. async and await keywords
/// 3. Cooperative cancellation via CancellationToken.ThrowIfCancellationRequested()
/// 4. Genuine asynchronous non-blocking I/O using StreamWriter with cancellation tokens
/// 5. Task.FromResult for already-completed synchronous results (avoiding fake async / Task.Run anti-patterns)
/// </summary>
public class EmployeeAsyncService : IEmployeeAsyncService
{
    private readonly IEmployeeService _employeeService;

    /// <summary>
    /// Initializes a new instance of the EmployeeAsyncService.
    /// Demonstrates Dependency Inversion: Depends on the IEmployeeService abstraction.
    /// </summary>
    /// <param name="employeeService">The underlying employee data service.</param>
    public EmployeeAsyncService(IEmployeeService employeeService)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    }

    /// <summary>
    /// Asynchronously retrieves an employee by ID with cooperative cancellation.
    /// 
    /// TEACHING NOTE ON IN-MEMORY ASYNC:
    /// In an in-memory application, dictionary lookups execute in nanoseconds (CPU work).
    /// Wrapping CPU-bound in-memory work in Task.Run() would needlessly occupy a ThreadPool
    /// thread with context-switching overhead. Instead, we use Task.FromResult(), which
    /// returns an already-completed Task without thread-pool hopping!
    /// </summary>
    public Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // 1. Cooperative cancellation check: Verify the caller hasn't already aborted
        cancellationToken.ThrowIfCancellationRequested();

        // 2. Perform in-memory retrieval
        Employee? employee = _employeeService.GetEmployeeById(id);

        // 3. Return already-completed Task<Employee?>
        return Task.FromResult(employee);
    }

    /// <summary>
    /// Asynchronously retrieves all employees.
    /// Supports cooperative cancellation via CancellationToken.
    /// </summary>
    public Task<IReadOnlyList<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var employees = _employeeService.GetAllEmployees();
        return Task.FromResult(employees);
    }

    /// <summary>
    /// Asynchronously compiles an extensive employee payroll summary report.
    /// Demonstrates cooperative cancellation across loop iterations.
    /// 
    /// TEACHING NOTE:
    /// In long-running report generation, checking cancellation at each record allows the
    /// operation to gracefully stop immediately when the user or Web API request cancels,
    /// rather than wasting CPU cycles finishing work whose caller has already disconnected.
    /// </summary>
    public Task<string> GenerateEmployeeReportAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var employees = _employeeService.GetAllEmployees();
        var sb = new StringBuilder();

        sb.AppendLine("===============================================================================");
        sb.AppendLine("                ASYNCHRONOUS EMPLOYEE AUDIT & PAYROLL REPORT                   ");
        sb.AppendLine($"                     Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}                     ");
        sb.AppendLine("===============================================================================");
        sb.AppendLine(string.Format("{0,-6} | {1,-20} | {2,-16} | {3,-12} | {4,-12}",
            "ID", "Full Name", "Department", "Classification", "Gross Pay"));
        sb.AppendLine(new string('-', 79));

        for (int i = 0; i < employees.Count; i++)
        {
            // Check cancellation cooperatively at each record
            cancellationToken.ThrowIfCancellationRequested();

            var emp = employees[i];
            sb.AppendLine(string.Format("{0,-6} | {1,-20} | {2,-16} | {3,-12} | {4,12:C}",
                emp.Id, emp.FullName, emp.Department.Name, emp.GetEmployeeType(), emp.CalculateGrossPay()));
        }

        sb.AppendLine(new string('-', 79));
        var stats = _employeeService.GetPayrollStatistics();
        sb.AppendLine($"Total Employees: {stats.Count} | Total Payroll: {stats.TotalPayroll:C} | Average: {stats.AverageSalary:C}");
        sb.AppendLine("===============================================================================");

        return Task.FromResult(sb.ToString());
    }

    /// <summary>
    /// Asynchronously streams employee data to a destination Stream (e.g. MemoryStream, FileStream).
    /// Demonstrates GENUINE asynchronous non-blocking I/O in .NET:
    /// - Uses async methods on StreamWriter (WriteLineAsync, FlushAsync)
    /// - Passes the CancellationToken directly down to the operating system I/O completion port
    /// - Does NOT block the calling thread while data is written
    /// </summary>
    public async Task<int> ExportPayrollReportAsync(Stream destinationStream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(destinationStream);
        if (!destinationStream.CanWrite)
        {
            throw new ArgumentException("Destination stream must be writable.", nameof(destinationStream));
        }

        cancellationToken.ThrowIfCancellationRequested();

        var employees = _employeeService.GetAllEmployees();
        long initialPosition = destinationStream.CanSeek ? destinationStream.Position : 0;

        // Leave stream open so the caller can read or position it afterwards
        using (var writer = new StreamWriter(destinationStream, Encoding.UTF8, bufferSize: 1024, leaveOpen: true))
        {
            await writer.WriteLineAsync("ID,FirstName,LastName,Department,Type,GrossPay,NetPay".AsMemory(), cancellationToken);

            foreach (var emp in employees)
            {
                // Cooperative check before each asynchronous stream write
                cancellationToken.ThrowIfCancellationRequested();

                string csvLine = $"{emp.Id},{emp.FirstName},{emp.LastName},{emp.Department.Name},{emp.GetEmployeeType()},{emp.CalculateGrossPay():F2},{emp.CalculateNetPay():F2}";
                await writer.WriteLineAsync(csvLine.AsMemory(), cancellationToken);
            }

            // Flush remaining buffered bytes asynchronously to the underlying stream
            await writer.FlushAsync(cancellationToken);
        }

        long finalPosition = destinationStream.CanSeek ? destinationStream.Position : 0;
        return (int)(finalPosition - initialPosition);
    }

    /// <summary>
    /// Demonstrates an asynchronous batch workflow that simulates periodic chunks with cooperative delay.
    /// Useful for educational demonstration of CancellationTokenSource cancellation and OperationCanceledException handling.
    /// </summary>
    public async Task<int> ProcessBatchAnalysisAsync(int totalBatches, int delayPerBatchMs, CancellationToken cancellationToken = default)
    {
        int completedBatches = 0;

        for (int i = 1; i <= totalBatches; i++)
        {
            // 1. Cooperative check before work
            cancellationToken.ThrowIfCancellationRequested();

            // 2. Non-blocking asynchronous delay honoring the token
            await Task.Delay(delayPerBatchMs, cancellationToken);

            completedBatches++;
        }

        return completedBatches;
    }
}
