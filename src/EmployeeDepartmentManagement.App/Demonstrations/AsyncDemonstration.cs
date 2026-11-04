using System.Text;
using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.Services;
using EmployeeDepartmentManagement.LeetCode;

namespace EmployeeDepartmentManagement.App.Demonstrations;

/// <summary>
/// Interactive demonstration runner for Week 1 - Day 4:
/// 1. Async/Await and Task<T> fundamentals
/// 2. Genuine Asynchronous I/O via stream export
/// 3. CancellationToken, CancellationTokenSource, and cooperative cancellation
/// 4. Graceful OperationCanceledException handling
/// 5. LeetCode Problem 121 (Best Time to Buy and Sell Stock) verification
/// </summary>
public static class AsyncDemonstration
{
    public static async Task RunAsync(IEmployeeService employeeService)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================================================");
        Console.WriteLine("        WEEK 1 - DAY 4: ASYNC/AWAIT & CANCELLATIONTOKEN DEEP DIVE                ");
        Console.WriteLine("=================================================================================");
        Console.ResetColor();
        Console.WriteLine("Demonstrating modern asynchronous programming patterns in .NET 9:");
        Console.WriteLine("• Non-blocking Task & Task<T> execution");
        Console.WriteLine("• Cooperative Cancellation with CancellationTokenSource");
        Console.WriteLine("• OperationCanceledException handling");
        Console.WriteLine("• Genuine asynchronous I/O with streams\n");

        var asyncService = new EmployeeAsyncService(employeeService);

        // -------------------------------------------------------------
        // PART 1: ASYNC / AWAIT NON-BLOCKING RETRIEVAL
        // -------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[1] Asynchronous Non-Blocking Retrieval (Task<T> & await):");
        Console.ResetColor();
        Console.WriteLine("Calling: await asyncService.GetEmployeeByIdAsync(101)");
        Console.WriteLine("Thread ID before await : " + Environment.CurrentManagedThreadId);

        Employee? employee = await asyncService.GetEmployeeByIdAsync(101);

        Console.WriteLine("Thread ID after await  : " + Environment.CurrentManagedThreadId);
        if (employee != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [Found] ID: {employee.Id} | Name: {employee.FullName} | Dept: {employee.Department.Name} | Gross: {employee.CalculateGrossPay():C}");
            Console.ResetColor();
        }
        Console.WriteLine("Notice: 'await' pauses method execution without blocking the calling thread!\n");

        // -------------------------------------------------------------
        // PART 2: GENUINE ASYNCHRONOUS I/O STREAM EXPORT
        // -------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[2] Genuine Asynchronous Non-Blocking I/O (Export to Stream):");
        Console.ResetColor();
        Console.WriteLine("Exporting employee payroll data asynchronously using StreamWriter and MemoryStream...");

        using (var memoryStream = new MemoryStream())
        {
            int bytesWritten = await asyncService.ExportPayrollReportAsync(memoryStream);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  -> Asynchronously written {bytesWritten} bytes to stream without blocking thread.");
            Console.ResetColor();

            memoryStream.Position = 0;
            using var reader = new StreamReader(memoryStream, Encoding.UTF8);
            Console.WriteLine("  First 2 exported CSV lines from stream:");
            for (int i = 0; i < 2; i++)
            {
                string? line = await reader.ReadLineAsync();
                if (line != null)
                {
                    Console.WriteLine($"     {line}");
                }
            }
        }
        Console.WriteLine();

        // -------------------------------------------------------------
        // PART 3: COOPERATIVE CANCELLATION (CancellationTokenSource)
        // -------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[3] Cooperative Cancellation with CancellationTokenSource:");
        Console.ResetColor();
        Console.WriteLine("Why 'Cooperative'? In modern .NET, cancellation is NOT forceful (unlike legacy Thread.Abort).");
        Console.WriteLine("The caller sends a cancellation request, and the executing method cooperatively checks");
        Console.WriteLine("token.ThrowIfCancellationRequested() or forwards the token to async I/O calls.\n");

        Console.WriteLine("Scenario A: Successful operation when NOT cancelled:");
        using (var ctsSuccessful = new CancellationTokenSource())
        {
            try
            {
                int batches = await asyncService.ProcessBatchAnalysisAsync(totalBatches: 3, delayPerBatchMs: 30, cancellationToken: ctsSuccessful.Token);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  -> Completed {batches} batches successfully.");
                Console.ResetColor();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("  -> Operation was cancelled.");
            }
        }

        Console.WriteLine("\nScenario B: Operation timed out / cancelled via CancellationTokenSource.CancelAfter(50ms):");
        using (var ctsCancelled = new CancellationTokenSource())
        {
            // Request cancellation after 50ms while task needs 250ms (5 * 50ms)
            ctsCancelled.CancelAfter(50);

            try
            {
                Console.WriteLine("  Starting 5 batches (50ms each)... CancelAfter set to 50ms.");
                await asyncService.ProcessBatchAnalysisAsync(totalBatches: 5, delayPerBatchMs: 50, cancellationToken: ctsCancelled.Token);
                Console.WriteLine("  -> Finished without cancellation (unexpected).");
            }
            catch (OperationCanceledException ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"  [COOPERATIVE CANCELLATION TRIGGERED] Caught OperationCanceledException!");
                Console.WriteLine($"  Message: {ex.Message}");
                Console.WriteLine($"  CancellationToken.IsCancellationRequested = {ctsCancelled.IsCancellationRequested}");
                Console.ResetColor();
                Console.WriteLine("  The application handled the cancellation gracefully without crashing or resource leaks!");
            }
        }
        Console.WriteLine();

        // -------------------------------------------------------------
        // PART 4: LEETCODE 121 (BEST TIME TO BUY AND SELL STOCK)
        // -------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[4] LeetCode Problem 121: Best Time to Buy and Sell Stock:");
        Console.ResetColor();
        Console.WriteLine("Running 7 verification test cases comparing O(n) Single-Pass vs O(n^2) Brute-Force...\n");
        BestTimeToBuyAndSellStockTestHarness.RunVerification();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=================================================================================");
        Console.WriteLine("               END OF DAY 4 ASYNC & CANCELLATION DEEP DIVE                       ");
        Console.WriteLine("=================================================================================");
        Console.ResetColor();
    }
}
