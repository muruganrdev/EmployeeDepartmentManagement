namespace EmployeeDepartmentManagement.App.Models;

/// <summary>
/// Domain model representing an Employee record.
/// Demonstrates C# fundamental data types: int, string, decimal, bool, DateTime,
/// and nullable types (decimal?, string?).
/// </summary>
public class Employee
{
    // --- Value Types ---
    public int Id { get; set; }
    public int Age { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal? PerformanceBonus { get; set; } // Nullable value type
    public bool IsActive { get; set; }
    public DateTime JoiningDate { get; set; }
    public int YearsOfExperience { get; set; }

    // --- Reference Types ---
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } // Nullable reference type
    public string Department { get; set; } = string.Empty;

    // Parameterless constructor for object initializers
    public Employee()
    {
    }

    // Parameterized constructor demonstrating constructor overloading and assignment
    public Employee(
        int id,
        string firstName,
        string lastName,
        string? middleName,
        string department,
        decimal baseSalary,
        decimal? performanceBonus,
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
        BaseSalary = baseSalary;
        PerformanceBonus = performanceBonus;
        Age = age;
        YearsOfExperience = yearsOfExperience;
        JoiningDate = joiningDate;
        IsActive = isActive;
    }

    /// <summary>
    /// Calculated Full Name demonstrating String Interpolation and conditional logic.
    /// </summary>
    public string FullName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(MiddleName))
            {
                return $"{FirstName} {LastName}";
            }

            return $"{FirstName} {MiddleName.Trim()} {LastName}";
        }
    }

    /// <summary>
    /// Calculates gross total compensation (Base Salary + Optional Bonus).
    /// Demonstrates arithmetic addition and the null-coalescing operator (??).
    /// </summary>
    public decimal CalculateGrossCompensation()
    {
        decimal bonusAmount = PerformanceBonus ?? 0m;
        return BaseSalary + bonusAmount;
    }

    /// <summary>
    /// Calculates annual tax bracket estimation using comparison and arithmetic operators.
    /// Demonstrates if/else ladder and percentage arithmetic on decimal values.
    /// </summary>
    public decimal CalculateEstimatedTax()
    {
        decimal gross = CalculateGrossCompensation();

        if (gross <= 30000m)
        {
            return gross * 0.05m; // 5% tax bracket
        }
        else if (gross <= 70000m)
        {
            return gross * 0.12m; // 12% tax bracket
        }
        else if (gross <= 120000m)
        {
            return gross * 0.22m; // 22% tax bracket
        }
        else
        {
            return gross * 0.30m; // 30% tax bracket
        }
    }

    /// <summary>
    /// Calculates net pay after tax deduction.
    /// </summary>
    public decimal CalculateNetPay()
    {
        return CalculateGrossCompensation() - CalculateEstimatedTax();
    }

    /// <summary>
    /// Creates a member-wise shallow clone of this Employee object.
    /// Used to demonstrate reference-copying vs new-instance value copying.
    /// </summary>
    public Employee ShallowCopy()
    {
        return (Employee)MemberwiseClone();
    }

    /// <summary>
    /// Formats employee record into a concise summary string.
    /// Demonstrates string methods and currency formatting.
    /// </summary>
    public override string ToString()
    {
        string bonusDisplay = PerformanceBonus.HasValue
            ? PerformanceBonus.Value.ToString("C")
            : "N/A";

        return $"[ID: {Id:D3}] {FullName} | Dept: {Department} | Base: {BaseSalary:C} | Bonus: {bonusDisplay} | Active: {(IsActive ? "Yes" : "No")}";
    }
}
