namespace EmployeeDepartmentManagement.Core.Models;

/// <summary>
/// Represents a permanent, salaried full-time corporate employee.
/// Demonstrates:
/// - Inheritance (extends abstract Employee)
/// - Polymorphism (overrides CalculateGrossPay, GetEmployeeType, ShallowCopy)
/// - Encapsulation (validated BaseSalary and AnnualBonus)
/// </summary>
public class FullTimeEmployee : Employee
{
    private decimal _baseSalary;
    private decimal? _annualBonus;

    public decimal BaseSalary
    {
        get => _baseSalary;
        set
        {
            if (value <= 0m)
                throw new ArgumentOutOfRangeException(nameof(value), "Base Salary for a full-time employee must be positive.");
            _baseSalary = value;
        }
    }

    public decimal? AnnualBonus
    {
        get => _annualBonus;
        set
        {
            if (value.HasValue && value.Value < 0m)
                throw new ArgumentOutOfRangeException(nameof(value), "Annual Bonus cannot be negative.");
            _annualBonus = value;
        }
    }

    public string HealthInsurancePlan { get; set; } = "Standard Corporate Healthcare";

    public FullTimeEmployee()
    {
    }

    public FullTimeEmployee(
        int id,
        string firstName,
        string lastName,
        string? middleName,
        Department department,
        decimal baseSalary,
        decimal? annualBonus,
        int age,
        int yearsOfExperience,
        DateTime joiningDate,
        string healthInsurancePlan = "Standard Corporate Healthcare",
        bool isActive = true)
        : base(id, firstName, lastName, middleName, department, age, yearsOfExperience, joiningDate, isActive)
    {
        BaseSalary = baseSalary;
        AnnualBonus = annualBonus;
        HealthInsurancePlan = healthInsurancePlan;
    }

    /// <summary>
    /// Polymorphic override: Gross pay is annual base salary + optional bonus.
    /// </summary>
    public override decimal CalculateGrossPay()
    {
        return BaseSalary + (AnnualBonus ?? 0m);
    }

    public override string GetEmployeeType() => "Full-Time";

    public override Employee ShallowCopy() => (FullTimeEmployee)MemberwiseClone();

    public override string ToString()
    {
        string bonusDisplay = AnnualBonus.HasValue ? AnnualBonus.Value.ToString("C") : "None";
        return $"[Full-Time ID: {Id:D3}] {FullName} | Dept: {Department.Name} | Base: {BaseSalary:C} | Bonus: {bonusDisplay} | Net: {CalculateNetPay():C}";
    }
}
