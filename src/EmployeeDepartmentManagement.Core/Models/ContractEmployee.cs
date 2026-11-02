using EmployeeDepartmentManagement.Core.ValueObjects;

namespace EmployeeDepartmentManagement.Core.Models;

/// <summary>
/// Represents an external or temporary contractor billed at an hourly rate.
/// Demonstrates:
/// - Inheritance (extends abstract Employee)
/// - Polymorphism (distinct compensation and tax calculation)
/// - Value Object Integration (uses WorkHours struct)
/// </summary>
public class ContractEmployee : Employee
{
    private decimal _hourlyRate;
    private int _hoursWorked;

    public decimal HourlyRate
    {
        get => _hourlyRate;
        set
        {
            if (value <= 0m)
                throw new ArgumentOutOfRangeException(nameof(value), "Hourly Rate for a contractor must be greater than zero.");
            _hourlyRate = value;
        }
    }

    public int HoursWorked
    {
        get => _hoursWorked;
        set
        {
            if (value < 0 || value > 300)
                throw new ArgumentOutOfRangeException(nameof(value), "Monthly hours worked must be between 0 and 300 hours.");
            _hoursWorked = value;
        }
    }

    public DateTime ContractEndDate { get; set; }
    public string AgencyName { get; set; } = "Independent Contractor";

    public ContractEmployee()
    {
    }

    public ContractEmployee(
        int id,
        string firstName,
        string lastName,
        string? middleName,
        Department department,
        decimal hourlyRate,
        int hoursWorked,
        DateTime contractEndDate,
        int age,
        int yearsOfExperience,
        DateTime joiningDate,
        string agencyName = "Independent Contractor",
        bool isActive = true)
        : base(id, firstName, lastName, middleName, department, age, yearsOfExperience, joiningDate, isActive)
    {
        HourlyRate = hourlyRate;
        HoursWorked = hoursWorked;
        ContractEndDate = contractEndDate;
        AgencyName = agencyName;
    }

    /// <summary>
    /// Polymorphic override: Gross pay is computed from hourly rate multiplied by total hours.
    /// </summary>
    public override decimal CalculateGrossPay()
    {
        return HourlyRate * HoursWorked;
    }

    /// <summary>
    /// Polymorphic override: Contractors are subject to a flat 10% withholding tax
    /// rather than standard progressive corporate income tax.
    /// </summary>
    public override decimal CalculateTaxDeduction()
    {
        return CalculateGrossPay() * 0.10m;
    }

    public override string GetEmployeeType() => "Contract";

    public override Employee ShallowCopy() => (ContractEmployee)MemberwiseClone();

    public override string ToString() =>
        $"[Contract ID: {Id:D3}] {FullName} | Dept: {Department.Name} | Rate: {HourlyRate:C}/hr ({HoursWorked} hrs) | Net: {CalculateNetPay():C} | Agency: {AgencyName}";
}
