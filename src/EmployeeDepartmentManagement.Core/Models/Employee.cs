using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.ValueObjects;

namespace EmployeeDepartmentManagement.Core.Models;

/// <summary>
/// Abstract base class for all employee types.
/// Demonstrates:
/// - Encapsulation (private fields with validating public properties)
/// - Abstract Classes & Interfaces (implements IPayrollCalculable)
/// - Polymorphism (abstract and virtual methods)
/// - Access Modifiers (public, private, protected, internal)
/// - Composition (Employee has-a Department)
/// </summary>
public abstract class Employee : IPayrollCalculable
{
    // Encapsulated private backing fields
    private int _id;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private int _age;
    private int _yearsOfExperience;
    private Department _department = new("GEN", "Engineering");

    // --- Public Encapsulated Properties ---

    public int Id
    {
        get => _id;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Employee ID must be a positive integer greater than zero.");
            _id = value;
        }
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("First Name cannot be null or empty.", nameof(value));
            if (value.Trim().Length < 2)
                throw new ArgumentException("First Name must have at least 2 characters.", nameof(value));
            _firstName = value.Trim();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Last Name cannot be null or empty.", nameof(value));
            _lastName = value.Trim();
        }
    }

    public string? MiddleName { get; set; }

    public int Age
    {
        get => _age;
        set
        {
            if (value < 18 || value > 75)
                throw new ArgumentOutOfRangeException(nameof(value), "Employee age must be between 18 and 75.");
            _age = value;
        }
    }

    public int YearsOfExperience
    {
        get => _yearsOfExperience;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Years of experience cannot be negative.");
            _yearsOfExperience = value;
        }
    }

    // Composition: Employee *has-a* Department
    public Department Department
    {
        get => _department;
        set => _department = value ?? throw new ArgumentNullException(nameof(value), "Department cannot be null.");
    }

    public DateTime JoiningDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Read-only calculated property
    public string FullName => string.IsNullOrWhiteSpace(MiddleName)
        ? $"{FirstName} {LastName}"
        : $"{FirstName} {MiddleName.Trim()} {LastName}";

    // Protected constructor accessible only to derived subclasses
    protected Employee()
    {
    }

    protected Employee(
        int id,
        string firstName,
        string lastName,
        string? middleName,
        Department department,
        int age,
        int yearsOfExperience,
        DateTime joiningDate,
        bool isActive = true)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Department = department;
        Age = age;
        YearsOfExperience = yearsOfExperience;
        JoiningDate = joiningDate;
        IsActive = isActive;
    }

    // --- Polymorphic Contract Methods (IPayrollCalculable) ---

    /// <summary>
    /// Abstract method: Each employment subclass MUST provide its own gross pay calculation.
    /// Full-Time uses BaseSalary + Bonus; Contract uses HourlyRate * Hours.
    /// </summary>
    public abstract decimal CalculateGrossPay();

    /// <summary>
    /// Virtual method: Provides a progressive tax bracket calculation that subclasses can override.
    /// </summary>
    public virtual decimal CalculateTaxDeduction()
    {
        decimal gross = CalculateGrossPay();
        if (gross <= 30000m) return gross * 0.05m;
        if (gross <= 70000m) return gross * 0.12m;
        if (gross <= 120000m) return gross * 0.22m;
        return gross * 0.30m;
    }

    /// <summary>
    /// Non-virtual template method: Net pay is universally gross minus tax.
    /// </summary>
    public decimal CalculateNetPay() => CalculateGrossPay() - CalculateTaxDeduction();

    /// <summary>
    /// Abstract identifier for employee classification (Full-Time, Contract, etc.)
    /// </summary>
    public abstract string GetEmployeeType();

    /// <summary>
    /// Creates a shallow clone of the employee object.
    /// </summary>
    public abstract Employee ShallowCopy();

    /// <summary>
    /// Converts this employee to an immutable summary record snapshot.
    /// </summary>
    public EmployeeSummary ToSummary() =>
        new(Id, FullName, Department.Name, GetEmployeeType(), CalculateGrossPay(), CalculateNetPay());

    public override string ToString() =>
        $"[{Id:D3}] {FullName} | Dept: {Department.Name} | Type: {GetEmployeeType()} | Net: {CalculateNetPay():C}";
}
