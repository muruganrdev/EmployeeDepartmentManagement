using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.Services;
using Moq;
using Xunit;

namespace EmployeeDepartmentManagement.Tests;

/// <summary>
/// Unit tests demonstrating:
/// - Topic A: Test Isolation and Mocking with Moq
/// - Creating test doubles for dependencies
/// - Setting up mock behavior (Setup) and verifying interactions (Verify)
/// 
/// TEACHING NOTE - WHAT IS MOCKING & WHY DO WE NEED IT?
/// In real-world software, a service often depends on external dependencies like databases,
/// web APIs, payment gateways, or file systems.
/// 
/// If we test a class using real external dependencies:
/// 1. Tests become slow and flaky (network drops, database down).
/// 2. Tests modify real data (pollution).
/// 3. Testing failure scenarios (e.g., simulating a database timeout) is very difficult.
/// 
/// Mocking creates a lightweight "fake" (test double) implementing the interface.
/// We tell the mock exactly what to return, test our component in complete isolation,
/// and verify that the expected methods were called!
/// </summary>
public class EmployeeMockingTests
{
    [Fact]
    public async Task GetEmployeeByIdAsync_UsingMockedService_ReturnsConfiguredMockEmployeeAndVerifiesCall()
    {
        // -------------------------------------------------------------
        // ARRANGE: Set up the Mock test double
        // -------------------------------------------------------------
        var mockEmployeeService = new Mock<IEmployeeService>();

        var fakeEmployee = new FullTimeEmployee(
            id: 501,
            firstName: "Sita",
            lastName: "Ram",
            middleName: null,
            department: Department.FromName("Engineering"),
            baseSalary: 92000m,
            annualBonus: 6000m,
            age: 28,
            yearsOfExperience: 5,
            joiningDate: new DateTime(2022, 1, 10)
        );

        // Tell the mock: Whenever GetEmployeeById(501) is called, return fakeEmployee
        mockEmployeeService
            .Setup(s => s.GetEmployeeById(501))
            .Returns(fakeEmployee);

        // Inject the mock object into the class under test
        var asyncService = new EmployeeAsyncService(mockEmployeeService.Object);

        // -------------------------------------------------------------
        // ACT: Execute the asynchronous method under test
        // -------------------------------------------------------------
        Employee? result = await asyncService.GetEmployeeByIdAsync(501);

        // -------------------------------------------------------------
        // ASSERT: Verify the outcome and verify the mock interaction
        // -------------------------------------------------------------
        Assert.NotNull(result);
        Assert.Equal(501, result.Id);
        Assert.Equal("Sita", result.FirstName);

        // Verification: Ensure the consumer called the dependency exactly once with ID 501
        mockEmployeeService.Verify(s => s.GetEmployeeById(501), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_WhenMockReturnsNull_ReturnsNullWithoutThrowing()
    {
        // ARRANGE
        var mockEmployeeService = new Mock<IEmployeeService>();

        // Set up mock to return null for ID 999
        mockEmployeeService
            .Setup(s => s.GetEmployeeById(999))
            .Returns((Employee?)null);

        var asyncService = new EmployeeAsyncService(mockEmployeeService.Object);

        // ACT
        Employee? result = await asyncService.GetEmployeeByIdAsync(999);

        // ASSERT
        Assert.Null(result);
        mockEmployeeService.Verify(s => s.GetEmployeeById(999), Times.Once);
    }
}
