namespace EmployeeDepartmentManagement.Core.Models;

/// <summary>
/// Domain model representing a Corporate Department.
/// Demonstrates Composition: An Employee *has-a* Department.
/// Also encapsulates department validation rules.
/// </summary>
public class Department
{
    private string _code = string.Empty;
    private string _name = string.Empty;

    // Fixed array of standard corporate departments
    public static readonly string[] ValidDepartments =
    [
        "Engineering",
        "Human Resources",
        "Finance",
        "Marketing",
        "Operations"
    ];

    public string Code
    {
        get => _code;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Department Code cannot be empty.", nameof(value));
            _code = value.Trim().ToUpperInvariant();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Department Name cannot be empty.", nameof(value));
            if (!IsValid(value))
                throw new ArgumentException($"'{value}' is not an authorized department.", nameof(value));
            _name = value.Trim();
        }
    }

    public string? Description { get; set; }

    public Department()
    {
    }

    public Department(string code, string name, string? description = null)
    {
        Code = code;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Factory helper to build a Department from a standard name.
    /// </summary>
    public static Department FromName(string name)
    {
        string canonical = GetCanonicalName(name);
        string code = canonical.Length >= 3 ? canonical[..3].ToUpperInvariant() : "GEN";
        return new Department(code, canonical);
    }

    /// <summary>
    /// Validates whether a given department name exists in the allowed list (case-insensitive).
    /// </summary>
    public static bool IsValid(string? departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
            return false;

        foreach (string dept in ValidDepartments)
        {
            if (string.Equals(dept, departmentName.Trim(), StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the exact casing for a recognized department name.
    /// </summary>
    public static string GetCanonicalName(string departmentName)
    {
        foreach (string dept in ValidDepartments)
        {
            if (string.Equals(dept, departmentName.Trim(), StringComparison.OrdinalIgnoreCase))
                return dept;
        }
        return departmentName.Trim();
    }

    public override string ToString() => $"{Name} ({Code})";

    public override bool Equals(object? obj)
    {
        if (obj is Department other)
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
}
