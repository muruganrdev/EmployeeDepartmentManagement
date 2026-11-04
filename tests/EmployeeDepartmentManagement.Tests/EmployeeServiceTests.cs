using EmployeeDepartmentManagement.App.Services;
using EmployeeDepartmentManagement.Core.Events;
using EmployeeDepartmentManagement.Core.Models;
using Xunit;

namespace EmployeeDepartmentManagement.Tests;

/// <summary>
/// Unit tests for EmployeeService demonstrating:
/// - Topic A: Unit Testing Fundamentals and Test Isolation
/// - Topic B: xUnit Assertions (Assert.True, Assert.False, Assert.Equal, Assert.NotNull, Assert.Null, Assert.Contains, Assert.Empty)
/// - Topic C: The Arrange-Act-Assert (AAA) Pattern with distinct step separation
/// </summary>
public class EmployeeServiceTests
{
    private readonly EmployeeService _sut; // SUT = System Under Test

    public EmployeeServiceTests()
    {
        // Each test method receives a fresh instance of EmployeeService,
        // ensuring test isolation (no shared mutable state between tests).
        _sut = new EmployeeService();
    }

    [Fact]
    public void AddEmployee_ValidFullTimeEmployee_ReturnsTrueAndAddsToCollection()
    {
        // -------------------------------------------------------------
        // ARRANGE: Prepare the test input data and prerequisites
        // -------------------------------------------------------------
        var newEmployee = new FullTimeEmployee(
            id: 999,
            firstName: "Devi",
            lastName: "Sundaram",
            middleName: "M",
            department: Department.FromName("Engineering"),
            baseSalary: 80000m,
            annualBonus: 5000m,
            age: 27,
            yearsOfExperience: 4,
            joiningDate: new DateTime(2022, 6, 1)
        );

        // -------------------------------------------------------------
        // ACT: Execute the method under test
        // -------------------------------------------------------------
        bool result = _sut.AddEmployee(newEmployee, out string errorMessage);
        Employee? retrieved = _sut.GetEmployeeById(999);

        // -------------------------------------------------------------
        // ASSERT: Verify the outcome matches expectations
        // -------------------------------------------------------------
        Assert.True(result, $"Expected employee to be added successfully, but got error: {errorMessage}");
        Assert.Empty(errorMessage);
        Assert.NotNull(retrieved);
        Assert.Equal("Devi", retrieved.FirstName);
        Assert.Equal(80000m, ((FullTimeEmployee)retrieved).BaseSalary);
    }

    [Fact]
    public void AddEmployee_DuplicateId_ReturnsFalseAndProvidesMeaningfulErrorMessage()
    {
        // ARRANGE
        // ID 101 already exists in the seeded sample data
        var duplicateEmployee = new FullTimeEmployee(
            id: 101,
            firstName: "Clone",
            lastName: "Employee",
            middleName: null,
            department: Department.FromName("Engineering"),
            baseSalary: 70000m,
            annualBonus: null,
            age: 30,
            yearsOfExperience: 5,
            joiningDate: DateTime.Today
        );

        // ACT
        bool result = _sut.AddEmployee(duplicateEmployee, out string errorMessage);

        // ASSERT
        Assert.False(result);
        Assert.NotEmpty(errorMessage);
        Assert.Contains("already exists", errorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void EmployeeConstructor_UnderageEmployee_ThrowsArgumentOutOfRangeException()
    {
        // ARRANGE, ACT & ASSERT
        // Encapsulation guard: Employee model throws ArgumentOutOfRangeException when age < 18
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new FullTimeEmployee(
                id: 998,
                firstName: "Junior",
                lastName: "Candidate",
                middleName: null,
                department: Department.FromName("Engineering"),
                baseSalary: 50000m,
                annualBonus: null,
                age: 16, // Invalid: < 18
                yearsOfExperience: 0,
                joiningDate: DateTime.Today
            );
        });
    }

    [Fact]
    public void AddEmployee_FutureJoiningDate_ReturnsFalseWithValidationError()
    {
        // ARRANGE
        // Domain business rule in EmployeeService: Joining date cannot be in the future
        var futureEmployee = new FullTimeEmployee(
            id: 996,
            firstName: "Future",
            lastName: "Joiner",
            middleName: null,
            department: Department.FromName("Engineering"),
            baseSalary: 55000m,
            annualBonus: null,
            age: 25,
            yearsOfExperience: 2,
            joiningDate: DateTime.Today.AddDays(7) // In the future
        );

        // ACT
        bool result = _sut.AddEmployee(futureEmployee, out string errorMessage);

        // ASSERT
        Assert.False(result);
        Assert.Contains("future", errorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetEmployeeById_ExistingId_ReturnsCorrectEmployeeRecord()
    {
        // ARRANGE
        const int existingId = 101; // Seeded Aarav Sharma

        // ACT
        Employee? employee = _sut.GetEmployeeById(existingId);

        // ASSERT
        Assert.NotNull(employee);
        Assert.Equal(existingId, employee.Id);
        Assert.Equal("Aarav", employee.FirstName);
        Assert.Equal("Sharma", employee.LastName);
    }

    [Fact]
    public void GetEmployeeById_NonExistentId_ReturnsNull()
    {
        // ARRANGE
        const int nonExistentId = 99999;

        // ACT
        Employee? employee = _sut.GetEmployeeById(nonExistentId);

        // ASSERT
        Assert.Null(employee);
    }

    [Fact]
    public void UpdateEmployeeSalary_ValidNewSalary_UpdatesCompensationAndReturnsTrue()
    {
        // ARRANGE
        const int employeeId = 101; // Aarav Sharma, initially 85,000 base
        const decimal newSalary = 105000m;

        // ACT
        bool result = _sut.UpdateEmployeeSalary(employeeId, newSalary, out string errorMessage);
        var updated = _sut.GetEmployeeById(employeeId) as FullTimeEmployee;

        // ASSERT
        Assert.True(result);
        Assert.Empty(errorMessage);
        Assert.NotNull(updated);
        Assert.Equal(newSalary, updated.BaseSalary);
    }

    [Fact]
    public void UpdateEmployeeSalary_NegativeSalary_ReturnsFalseAndKeepsExistingSalary()
    {
        // ARRANGE
        const int employeeId = 101;
        var initial = _sut.GetEmployeeById(employeeId) as FullTimeEmployee;
        Assert.NotNull(initial);
        decimal originalSalary = initial.BaseSalary;

        // ACT
        bool result = _sut.UpdateEmployeeSalary(employeeId, -15000m, out string errorMessage);
        var afterAttempt = _sut.GetEmployeeById(employeeId) as FullTimeEmployee;

        // ASSERT
        Assert.False(result);
        Assert.Contains("greater than zero", errorMessage, StringComparison.OrdinalIgnoreCase);
        Assert.NotNull(afterAttempt);
        Assert.Equal(originalSalary, afterAttempt.BaseSalary); // Verify state was preserved
    }

    [Fact]
    public void AddEmployee_ValidEmployee_RaisesEmployeeAddedEvent()
    {
        // ARRANGE
        bool eventFired = false;
        Employee? eventEmployeePayload = null;

        // Subscribe to the event
        _sut.EmployeeAdded += (sender, args) =>
        {
            eventFired = true;
            eventEmployeePayload = args.Employee;
        };

        var newEmployee = new FullTimeEmployee(
            id: 997,
            firstName: "Lakshmi",
            lastName: "Narayanan",
            middleName: null,
            department: Department.FromName("Finance"),
            baseSalary: 72000m,
            annualBonus: 4000m,
            age: 29,
            yearsOfExperience: 6,
            joiningDate: new DateTime(2021, 1, 15)
        );

        // ACT
        bool result = _sut.AddEmployee(newEmployee, out _);

        // ASSERT
        Assert.True(result);
        Assert.True(eventFired, "The EmployeeAdded event should have been raised when employee was enrolled.");
        Assert.NotNull(eventEmployeePayload);
        Assert.Equal(997, eventEmployeePayload.Id);
    }

    [Fact]
    public void SearchEmployees_NonExistentDepartment_ReturnsEmptyCollection()
    {
        // ARRANGE
        string unknownDept = "NonExistentDept999";

        // ACT
        var results = _sut.SearchEmployees(unknownDept);

        // ASSERT
        Assert.NotNull(results);
        Assert.Empty(results); // Demonstrates Assert.Empty()
    }
}
