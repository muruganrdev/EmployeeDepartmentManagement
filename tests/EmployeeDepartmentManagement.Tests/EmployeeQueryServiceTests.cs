using EmployeeDepartmentManagement.App.Services;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.Services;
using Xunit;

namespace EmployeeDepartmentManagement.Tests;

/// <summary>
/// Unit tests for EmployeeQueryService demonstrating:
/// - LINQ Query verification
/// - Exception assertion testing via Assert.Throws
/// - Safe empty collection handling
/// </summary>
public class EmployeeQueryServiceTests
{
    private readonly EmployeeQueryService _queryService = new();
    private readonly EmployeeService _employeeService = new();

    [Fact]
    public void FilterByMinimumSalary_GivenThreshold_ReturnsOnlyQualifyingEmployees()
    {
        // ARRANGE
        var employees = _employeeService.GetAllEmployees();
        const decimal minSalary = 80000m;

        // ACT
        var highEarners = _queryService.FilterByMinimumSalary(employees, minSalary);

        // ASSERT
        Assert.NotNull(highEarners);
        Assert.NotEmpty(highEarners);
        Assert.All(highEarners, emp => Assert.True(emp.CalculateGrossPay() >= minSalary));
    }

    [Fact]
    public void FilterByDepartment_ExistingDepartment_ReturnsMatchingEmployeesCaseInsensitive()
    {
        // ARRANGE
        var employees = _employeeService.GetAllEmployees();
        const string dept = "engineering"; // Lowercase test

        // ACT
        var result = _queryService.FilterByDepartment(employees, dept);

        // ASSERT
        Assert.NotEmpty(result);
        Assert.All(result, emp => Assert.Equal("Engineering", emp.Department.Name, ignoreCase: true));
    }

    [Fact]
    public void SortBySalary_Descending_FirstEmployeeHasHighestGrossPay()
    {
        // ARRANGE
        var employees = _employeeService.GetAllEmployees();

        // ACT
        var sorted = _queryService.SortBySalary(employees, ascending: false);

        // ASSERT
        Assert.Equal(employees.Count, sorted.Count);
        for (int i = 0; i < sorted.Count - 1; i++)
        {
            Assert.True(sorted[i].CalculateGrossPay() >= sorted[i + 1].CalculateGrossPay(),
                $"Expected {sorted[i].FullName} ({sorted[i].CalculateGrossPay():C}) to earn >= {sorted[i + 1].FullName} ({sorted[i + 1].CalculateGrossPay():C})");
        }
    }

    [Fact]
    public void GetHighestPaidEmployee_WithEmployees_ReturnsEmployeeWithMaxGrossPay()
    {
        // ARRANGE
        var employees = _employeeService.GetAllEmployees();

        // ACT
        Employee? highest = _queryService.GetHighestPaidEmployee(employees);

        // ASSERT
        Assert.NotNull(highest);
        decimal maxPay = employees.Max(e => e.CalculateGrossPay());
        Assert.Equal(maxPay, highest.CalculateGrossPay());
    }

    [Fact]
    public void GetHighestPaidEmployee_EmptyCollection_ReturnsNullSafely()
    {
        // ARRANGE
        var emptyList = new List<Employee>();

        // ACT
        Employee? highest = _queryService.GetHighestPaidEmployee(emptyList);

        // ASSERT
        Assert.Null(highest); // Verifies safe handling of empty data
    }

    [Fact]
    public void FilterByMinimumSalary_NullSource_ThrowsArgumentNullException()
    {
        // ARRANGE
        IEnumerable<Employee> nullCollection = null!;

        // ACT & ASSERT
        // Demonstrates Assert.Throws() to verify argument validation and defensive programming
        Assert.Throws<ArgumentNullException>(() =>
        {
            _queryService.FilterByMinimumSalary(nullCollection, 50000m);
        });
    }
}
