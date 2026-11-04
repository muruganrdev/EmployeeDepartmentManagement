using System.Text;
using EmployeeDepartmentManagement.App.Services;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.Services;
using Xunit;

namespace EmployeeDepartmentManagement.Tests;

/// <summary>
/// Unit tests for EmployeeAsyncService demonstrating:
/// - Topic D: Async/Await, Task, and Task<T>
/// - Non-blocking asynchronous programming
/// - Topic E: CancellationToken and CancellationTokenSource
/// - Assert.ThrowsAsync<OperationCanceledException>
/// </summary>
public class EmployeeAsyncServiceTests
{
    private readonly EmployeeAsyncService _asyncService;

    public EmployeeAsyncServiceTests()
    {
        var employeeService = new EmployeeService();
        _asyncService = new EmployeeAsyncService(employeeService);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ExistingId_ReturnsMatchingEmployee()
    {
        // ARRANGE
        const int existingId = 101;

        // ACT
        // Demonstrates non-blocking await on Task<Employee?>
        Employee? result = await _asyncService.GetEmployeeByIdAsync(existingId);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(existingId, result.Id);
        Assert.Equal("Aarav", result.FirstName);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_NonExistentId_ReturnsNull()
    {
        // ARRANGE
        const int nonExistentId = 9999;

        // ACT
        Employee? result = await _asyncService.GetEmployeeByIdAsync(nonExistentId);

        // ASSERT
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllEmployeesAsync_DefaultToken_ReturnsAllEmployees()
    {
        // ARRANGE & ACT
        IReadOnlyList<Employee> employees = await _asyncService.GetAllEmployeesAsync();

        // ASSERT
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.True(employees.Count >= 5);
    }

    [Fact]
    public async Task ExportPayrollReportAsync_WritableMemoryStream_WritesCsvAndReturnsBytes()
    {
        // ARRANGE
        using var memoryStream = new MemoryStream();

        // ACT
        // Demonstrates genuine asynchronous I/O write with streaming
        int bytesWritten = await _asyncService.ExportPayrollReportAsync(memoryStream);

        // ASSERT
        Assert.True(bytesWritten > 0);
        Assert.True(memoryStream.Length > 0);

        // Read back the written stream content to verify validity
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream, Encoding.UTF8);
        string headerLine = await reader.ReadLineAsync() ?? string.Empty;

        Assert.Contains("ID,FirstName,LastName,Department", headerLine);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        // ARRANGE
        // Create a pre-cancelled CancellationToken
        using var cts = new CancellationTokenSource();
        cts.Cancel(); // Request cancellation before executing the async call

        // ACT & ASSERT
        // Verifies that the method cooperatively respects the cancellation request
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await _asyncService.GetEmployeeByIdAsync(101, cts.Token);
        });
    }

    [Fact]
    public async Task GenerateEmployeeReportAsync_WhenCancelled_ThrowsOperationCanceledException()
    {
        // ARRANGE
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // ACT & ASSERT
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await _asyncService.GenerateEmployeeReportAsync(cts.Token);
        });
    }

    [Fact]
    public async Task ProcessBatchAnalysisAsync_CancellationRequestedDuringExecution_ThrowsOperationCanceledException()
    {
        // ARRANGE
        // Request cancellation after 50ms, while the method is configured to run 10 batches of 50ms each (500ms total)
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(millisecondsDelay: 40);

        // ACT & ASSERT
        // Verifies that Task.Delay or intermediate loop cooperatively terminates when token is signaled
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await _asyncService.ProcessBatchAnalysisAsync(totalBatches: 10, delayPerBatchMs: 50, cancellationToken: cts.Token);
        });
    }
}
