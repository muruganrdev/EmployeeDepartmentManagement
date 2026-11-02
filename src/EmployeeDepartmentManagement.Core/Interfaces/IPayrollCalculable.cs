namespace EmployeeDepartmentManagement.Core.Interfaces;

/// <summary>
/// Defines a contract for entities whose compensation and taxes can be calculated.
/// Demonstrates Interface-Based Design and the Interface Segregation Principle (ISP).
/// </summary>
public interface IPayrollCalculable
{
    /// <summary>
    /// Calculates gross compensation before tax deductions.
    /// </summary>
    decimal CalculateGrossPay();

    /// <summary>
    /// Calculates estimated tax withholdings based on employment type and compensation rules.
    /// </summary>
    decimal CalculateTaxDeduction();

    /// <summary>
    /// Calculates net take-home pay after tax deductions.
    /// </summary>
    decimal CalculateNetPay();
}
