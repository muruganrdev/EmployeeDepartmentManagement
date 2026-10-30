using System.Text;
using EmployeeDepartmentManagement.App.Models;

namespace EmployeeDepartmentManagement.App.Services;

/// <summary>
/// Service managing in-memory employee records.
/// Demonstrates C# Collections: List&lt;T&gt;, Dictionary&lt;TKey, TValue&gt;,
/// Loops (foreach, for, while), StringBuilder, and Method Overloading.
/// </summary>
public class EmployeeService
{
    // List<T>: Dynamic ordered collection for sequential access and filtering
    private readonly List<Employee> _employeeList = new();

    // Dictionary<TKey, TValue>: Hash table key-value pair collection for O(1) instant ID lookup
    private readonly Dictionary<int, Employee> _employeeIndex = new();

    public EmployeeService()
    {
        SeedSampleData();
    }

    /// <summary>
    /// Seeds realistic initial employee records.
    /// Demonstrates object initialization and collection population.
    /// </summary>
    private void SeedSampleData()
    {
        var sampleEmployees = new[]
        {
            new Employee(101, "Aarav", "Sharma", null, "Engineering", 85000m, 5000m, 28, 5, new DateTime(2021, 3, 15), true),
            new Employee(102, "Priya", "Natarajan", "R", "Human Resources", 62000m, 3500m, 31, 7, new DateTime(2019, 7, 1), true),
            new Employee(103, "Murugan", "R", null, "Engineering", 95000m, 8000m, 29, 6, new DateTime(2020, 1, 10), true),
            new Employee(104, "Ananya", "Deshmukh", "S", "Finance", 78000m, null, 26, 3, new DateTime(2023, 5, 20), true),
            new Employee(105, "Vikram", "Malhotra", null, "Marketing", 71000m, 4000m, 34, 9, new DateTime(2018, 11, 12), true)
        };

        foreach (var emp in sampleEmployees)
        {
            _employeeList.Add(emp);
            _employeeIndex[emp.Id] = emp;
        }
    }

    /// <summary>
    /// Validates employee input fields before persisting.
    /// Demonstrates input validation and string inspection methods.
    /// </summary>
    public (bool IsValid, string ErrorMessage) ValidateEmployee(Employee emp)
    {
        if (emp.Id <= 0)
        {
            return (false, "Employee ID must be a positive integer greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(emp.FirstName))
        {
            return (false, "First Name is mandatory and cannot be blank.");
        }

        if (emp.FirstName.Trim().Length < 2)
        {
            return (false, "First Name must contain at least 2 characters.");
        }

        if (string.IsNullOrWhiteSpace(emp.LastName))
        {
            return (false, "Last Name is mandatory and cannot be blank.");
        }

        if (string.IsNullOrWhiteSpace(emp.Department))
        {
            return (false, "Department is mandatory.");
        }

        if (!Department.IsValid(emp.Department))
        {
            string allowed = string.Join(", ", Department.ValidDepartments);
            return (false, $"Invalid Department '{emp.Department}'. Allowed departments are: {allowed}");
        }

        if (emp.BaseSalary <= 0m)
        {
            return (false, "Base Salary must be a positive decimal value.");
        }

        if (emp.PerformanceBonus.HasValue && emp.PerformanceBonus.Value < 0m)
        {
            return (false, "Performance Bonus cannot be negative.");
        }

        if (emp.Age < 18 || emp.Age > 75)
        {
            return (false, "Employee age must be between 18 and 75.");
        }

        if (emp.YearsOfExperience < 0 || emp.YearsOfExperience > emp.Age - 16)
        {
            return (false, "Years of Experience must be non-negative and realistic for the given age.");
        }

        if (emp.JoiningDate > DateTime.Today)
        {
            return (false, "Joining Date cannot be in the future.");
        }

        return (true, string.Empty);
    }

    /// <summary>
    /// Adds a new employee into both List and Dictionary.
    /// Demonstrates duplicate ID check and collection synchronization.
    /// </summary>
    public bool AddEmployee(Employee emp, out string errorMessage)
    {
        // Check for duplicate ID using Dictionary's O(1) ContainsKey check
        if (_employeeIndex.ContainsKey(emp.Id))
        {
            errorMessage = $"An employee with ID {emp.Id} already exists.";
            return false;
        }

        var (isValid, validationError) = ValidateEmployee(emp);
        if (!isValid)
        {
            errorMessage = validationError;
            return false;
        }

        // Clean up strings using String.Trim()
        emp.FirstName = emp.FirstName.Trim();
        emp.LastName = emp.LastName.Trim();
        emp.MiddleName = string.IsNullOrWhiteSpace(emp.MiddleName) ? null : emp.MiddleName.Trim();
        emp.Department = emp.Department.Trim();

        _employeeList.Add(emp);
        _employeeIndex.Add(emp.Id, emp);

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Retrieves an employee by ID using O(1) Dictionary lookup.
    /// Demonstrates TryGetValue and nullable return type Employee?.
    /// </summary>
    public Employee? GetEmployeeById(int id)
    {
        if (_employeeIndex.TryGetValue(id, out Employee? emp))
        {
            return emp;
        }

        return null;
    }

    /// <summary>
    /// Method Overload 1: Searches employees by Department (case-insensitive).
    /// Demonstrates method overloading, foreach loop, and string comparison.
    /// </summary>
    public List<Employee> SearchEmployees(string department)
    {
        var matched = new List<Employee>();

        if (string.IsNullOrWhiteSpace(department))
        {
            return matched;
        }

        foreach (var emp in _employeeList)
        {
            if (emp.Department.Equals(department.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                matched.Add(emp);
            }
        }

        return matched;
    }

    /// <summary>
    /// Method Overload 2: Searches employees by Full Name search term.
    /// Demonstrates String.Contains() case-insensitive matching.
    /// </summary>
    public List<Employee> SearchEmployeesByName(string nameQuery)
    {
        var matched = new List<Employee>();

        if (string.IsNullOrWhiteSpace(nameQuery))
        {
            return matched;
        }

        string query = nameQuery.Trim().ToLowerInvariant();

        foreach (var emp in _employeeList)
        {
            if (emp.FullName.ToLowerInvariant().Contains(query))
            {
                matched.Add(emp);
            }
        }

        return matched;
    }

    /// <summary>
    /// Returns all employee records in order.
    /// </summary>
    public IReadOnlyList<Employee> GetAllEmployees()
    {
        return _employeeList.AsReadOnly();
    }

    /// <summary>
    /// Calculates aggregate salary statistics using arithmetic operators and loops.
    /// Returns a named Tuple containing payroll metrics.
    /// </summary>
    public (int Count, decimal TotalPayroll, decimal AverageSalary, decimal HighestSalary, decimal LowestSalary) GetPayrollStatistics()
    {
        if (_employeeList.Count == 0)
        {
            return (0, 0m, 0m, 0m, 0m);
        }

        decimal totalPayroll = 0m;
        decimal highestSalary = decimal.MinValue;
        decimal lowestSalary = decimal.MaxValue;

        // Using for loop to demonstrate index-based iteration
        for (int i = 0; i < _employeeList.Count; i++)
        {
            decimal comp = _employeeList[i].CalculateGrossCompensation();
            totalPayroll += comp;

            if (comp > highestSalary)
            {
                highestSalary = comp;
            }

            if (comp < lowestSalary)
            {
                lowestSalary = comp;
            }
        }

        decimal averageSalary = totalPayroll / _employeeList.Count;

        return (_employeeList.Count, totalPayroll, averageSalary, highestSalary, lowestSalary);
    }

    /// <summary>
    /// Generates a comprehensive payroll summary report using StringBuilder.
    /// Demonstrates StringBuilder efficiency over string concatenation.
    /// </summary>
    public string GeneratePayrollReport()
    {
        var stats = GetPayrollStatistics();

        var sb = new StringBuilder();
        sb.AppendLine("=========================================================================================");
        sb.AppendLine("                            EMPLOYEE PAYROLL SUMMARY REPORT                             ");
        sb.AppendLine("=========================================================================================");
        sb.AppendLine(string.Format("{0,-6} | {1,-22} | {2,-16} | {3,-12} | {4,-10} | {5,-10}",
            "ID", "Name", "Department", "Base Salary", "Bonus", "Net Pay"));
        sb.AppendLine("-----------------------------------------------------------------------------------------");

        foreach (var emp in _employeeList)
        {
            string bonusStr = emp.PerformanceBonus.HasValue ? emp.PerformanceBonus.Value.ToString("C") : "None";
            sb.AppendLine(string.Format("{0,-6} | {1,-22} | {2,-16} | {3,-12:C} | {4,-10} | {5,-10:C}",
                emp.Id,
                emp.FullName.Length > 22 ? emp.FullName.Substring(0, 19) + "..." : emp.FullName,
                emp.Department,
                emp.BaseSalary,
                bonusStr,
                emp.CalculateNetPay()));
        }

        sb.AppendLine("-----------------------------------------------------------------------------------------");
        sb.AppendLine($"Total Active Headcount : {stats.Count}");
        sb.AppendLine($"Total Gross Payroll    : {stats.TotalPayroll:C}");
        sb.AppendLine($"Average Compensation   : {stats.AverageSalary:C}");
        sb.AppendLine($"Highest Compensation   : {stats.HighestSalary:C}");
        sb.AppendLine($"Lowest Compensation    : {stats.LowestSalary:C}");
        sb.AppendLine("=========================================================================================");

        return sb.ToString();
    }
}
