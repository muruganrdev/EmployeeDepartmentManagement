using EmployeeDepartmentManagement.App.Services;
using EmployeeDepartmentManagement.Core.Models;
using Xunit;

namespace EmployeeDepartmentManagement.Tests;

/// <summary>
/// Parameterized unit tests demonstrating:
/// - Topic B & Topic C: [Theory] and [InlineData] attributes in xUnit
/// - Data-driven testing: Executing the same test logic across multiple input boundaries
/// 
/// TEACHING NOTE - [Fact] VS [Theory]:
/// - [Fact]: Declares a standard test that always runs once with fixed, hardcoded inputs.
/// - [Theory]: Declares a parameterized test that executes multiple times—once for each [InlineData] row.
/// </summary>
public class SalaryValidationTheoryTests
{
    private readonly EmployeeService _sut = new();

    /// <summary>
    /// Demonstrates [Theory] with [InlineData] testing salary update validation across boundary conditions:
    /// - Positive values should succeed (expectedIsValid = true)
    /// - Zero and negative values should fail (expectedIsValid = false)
    /// </summary>
    [Theory]
    [InlineData(85000.00, true)]   // Valid positive salary
    [InlineData(120000.50, true)]  // Higher positive salary
    [InlineData(1.00, true)]       // Minimum positive salary
    [InlineData(0.00, false)]      // Boundary: Zero salary (invalid)
    [InlineData(-500.00, false)]   // Negative salary (invalid)
    [InlineData(-99999.00, false)] // Large negative salary (invalid)
    public void UpdateEmployeeSalary_VariousAmounts_ValidatesCorrectly(double testSalaryDouble, bool expectedIsValid)
    {
        // ARRANGE
        const int existingEmployeeId = 101;
        decimal testSalary = (decimal)testSalaryDouble;

        // ACT
        bool result = _sut.UpdateEmployeeSalary(existingEmployeeId, testSalary, out string errorMessage);

        // ASSERT
        if (expectedIsValid)
        {
            Assert.True(result, $"Expected salary update to succeed for {testSalary:C}, but failed with: {errorMessage}");
            Assert.Empty(errorMessage);
        }
        else
        {
            Assert.False(result, $"Expected salary update to be rejected for {testSalary:C}, but it succeeded.");
            Assert.NotEmpty(errorMessage);
            Assert.Contains("greater than zero", errorMessage, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Demonstrates [Theory] testing Department validation across multiple valid and invalid strings.
    /// </summary>
    [Theory]
    [InlineData("Engineering", true)]
    [InlineData("Human Resources", true)]
    [InlineData("Finance", true)]
    [InlineData("Marketing", true)]
    [InlineData("Sales", false)]      // Not in the standard list of corporate departments
    [InlineData("Operations", true)]
    [InlineData("InvalidDepartment", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void Department_IsValid_CorrectlyIdentifiesAllowedDepartments(string departmentName, bool expectedIsValid)
    {
        // ARRANGE & ACT
        bool actualIsValid = Department.IsValid(departmentName);

        // ASSERT
        Assert.Equal(expectedIsValid, actualIsValid);
    }

    /// <summary>
    /// Demonstrates [Theory] testing Employee age validation boundary logic (18 to 75 allowed).
    /// </summary>
    [Theory]
    [InlineData(17, false)] // Underage (< 18)
    [InlineData(18, true)]  // Boundary: Minimum allowed working age
    [InlineData(30, true)]  // Typical working age
    [InlineData(75, true)]  // Boundary: Maximum allowed working age
    [InlineData(76, false)] // Overage (> 75)
    [InlineData(0, false)]  // Zero
    [InlineData(-5, false)] // Negative
    public void Employee_AgeEncapsulation_EnforcesDomainBoundaries(int testAge, bool expectedIsValid)
    {
        // ARRANGE & ACT & ASSERT
        if (expectedIsValid)
        {
            var emp = new FullTimeEmployee(
                id: 888,
                firstName: "Test",
                lastName: "Candidate",
                middleName: null,
                department: Department.FromName("Engineering"),
                baseSalary: 60000m,
                annualBonus: null,
                age: testAge,
                yearsOfExperience: 0,
                joiningDate: DateTime.Today
            );

            Assert.Equal(testAge, emp.Age);
        }
        else
        {
            // Encapsulation guard in Employee.Age property throws ArgumentOutOfRangeException
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new FullTimeEmployee(
                    id: 888,
                    firstName: "Test",
                    lastName: "Candidate",
                    middleName: null,
                    department: Department.FromName("Engineering"),
                    baseSalary: 60000m,
                    annualBonus: null,
                    age: testAge,
                    yearsOfExperience: 0,
                    joiningDate: DateTime.Today
                );
            });
        }
    }
}
