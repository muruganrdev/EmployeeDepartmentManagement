using EmployeeDepartmentManagement.Core.Models;

namespace EmployeeDepartmentManagement.Core.Services;

/// <summary>
/// Demonstrates Modern C# Pattern Matching (Type Patterns, Property Patterns, Relational Patterns,
/// and Switch Expressions).
/// Provides clear classification of employee records based on domain attributes.
/// </summary>
public static class EmployeePatternMatcher
{
    /// <summary>
    /// Demonstrates Switch Expressions combined with Type and Property Patterns.
    /// Classifies employees into compensation and organizational tiers.
    /// </summary>
    public static string ClassifyEmployeeTier(Employee employee) => employee switch
    {
        // 1. Property pattern on FullTimeEmployee with relational pattern (>= 95000)
        FullTimeEmployee { BaseSalary: >= 95000m } ftHigh =>
            $"Senior Executive (Full-Time | Base: {ftHigh.BaseSalary:C} + Bonus: {ftHigh.AnnualBonus:C})",

        // 2. Property pattern on FullTimeEmployee checking if AnnualBonus has a positive value
        FullTimeEmployee { AnnualBonus: > 0m } ftBonus =>
            $"Performance Tier (Full-Time | Bonus: {ftBonus.AnnualBonus:C})",

        // 3. Simple Type pattern for any other FullTimeEmployee
        FullTimeEmployee ft =>
            $"Standard Corporate Staff (Full-Time | Base: {ft.BaseSalary:C})",

        // 4. Property pattern on ContractEmployee checking high hourly rate
        ContractEmployee { HourlyRate: >= 60m } ctHigh =>
            $"Senior Specialist Contractor ({ctHigh.AgencyName} | Rate: {ctHigh.HourlyRate:C}/hr)",

        // 5. Simple Type pattern for any other ContractEmployee
        ContractEmployee ct =>
            $"Standard Contractor ({ct.AgencyName} | Rate: {ct.HourlyRate:C}/hr)",

        // 6. Null pattern check
        null => "Unknown (Null Reference)",

        // 7. Discard pattern for any future unsupported employee subclass
        _ => $"General Personnel ({employee.GetEmployeeType()})"
    };

    /// <summary>
    /// Demonstrates Type Pattern with 'is' declaration pattern and 'when' guards.
    /// Computes a projected annual retention cost for budgeting.
    /// </summary>
    public static (string Category, decimal AnnualProjectedCost) EvaluateBudgetImpact(Employee employee)
    {
        if (employee is null)
            return ("None", 0m);

        // Pattern matching with 'is' and property extraction
        if (employee is FullTimeEmployee ft)
        {
            decimal benefitsOverhead = ft.BaseSalary * 0.15m; // 15% estimated corporate health & 401k
            decimal totalCost = ft.BaseSalary + (ft.AnnualBonus ?? 0m) + benefitsOverhead;
            return ("Permanent Headcount (W2)", totalCost);
        }

        if (employee is ContractEmployee ct)
        {
            // Monthly contractor annualized (HourlyRate * HoursWorked * 12)
            decimal annualBilled = ct.HourlyRate * ct.HoursWorked * 12m;
            return ("Vendor Procurement (1099)", annualBilled);
        }

        return ("General", employee.CalculateGrossPay());
    }
}
