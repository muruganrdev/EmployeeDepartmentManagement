using EmployeeDepartmentManagement.Core.Models;

namespace EmployeeDepartmentManagement.Core.Extensions;

/// <summary>
/// Demonstrates Extension Methods in C#.
/// Allows "adding" methods to existing types without modifying the original class source code
/// or creating derived types. Extension methods are declared in a static class as static methods,
/// but called using instance syntax via the 'this' keyword on the first parameter.
/// </summary>
public static class EmployeeExtensions
{
    /// <summary>
    /// Determines whether an employee is classified as a high earner based on gross compensation.
    /// Usage: employee.IsHighEarner() or employee.IsHighEarner(95000m)
    /// </summary>
    public static bool IsHighEarner(this Employee employee, decimal threshold = 80000m)
    {
        ArgumentNullException.ThrowIfNull(employee);
        return employee.CalculateGrossPay() >= threshold;
    }

    /// <summary>
    /// Formats an employee's key career metrics into a concise single-line summary string.
    /// Usage: employee.ToFormattedString()
    /// </summary>
    public static string ToFormattedString(this Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);
        return $"[{employee.Id:D3}] {employee.FullName} | Dept: {employee.Department.Name,-15} | Type: {employee.GetEmployeeType(),-10} | Gross: {employee.CalculateGrossPay():C}";
    }

    /// <summary>
    /// Extension on IEnumerable&lt;Employee&gt; computing total gross payroll for any collection of employees.
    /// Usage: employees.TotalGrossPayroll()
    /// </summary>
    public static decimal TotalGrossPayroll(this IEnumerable<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);
        return employees.Sum(emp => emp.CalculateGrossPay());
    }

    /// <summary>
    /// Extension on IEnumerable&lt;Employee&gt; filtering employees by department name.
    /// Usage: employees.FilterByDepartmentName("Engineering")
    /// </summary>
    public static IEnumerable<Employee> FilterByDepartmentName(this IEnumerable<Employee> employees, string departmentName)
    {
        ArgumentNullException.ThrowIfNull(employees);
        if (string.IsNullOrWhiteSpace(departmentName))
            return employees;

        return employees.Where(emp =>
            string.Equals(emp.Department.Name, departmentName.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Extension on IEnumerable&lt;Employee&gt; calculating average compensation safely.
    /// </summary>
    public static decimal AverageGrossSalary(this IEnumerable<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);
        var list = employees as IList<Employee> ?? employees.ToList();
        return list.Count == 0 ? 0m : list.Average(emp => emp.CalculateGrossPay());
    }
}
