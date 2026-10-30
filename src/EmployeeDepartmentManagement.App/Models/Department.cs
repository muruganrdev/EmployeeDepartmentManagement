namespace EmployeeDepartmentManagement.App.Models;

/// <summary>
/// Represents department constants and validation rules.
/// Demonstrates fixed-size Arrays and string search methods.
/// </summary>
public static class Department
{
    // Fixed array of standard corporate departments
    public static readonly string[] ValidDepartments =
    [
        "Engineering",
        "Human Resources",
        "Finance",
        "Marketing",
        "Operations"
    ];

    /// <summary>
    /// Checks whether a given department name exists in the allowed list (case-insensitive).
    /// </summary>
    public static bool IsValid(string? departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
        {
            return false;
        }

        foreach (string dept in ValidDepartments)
        {
            if (string.Equals(dept, departmentName.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
