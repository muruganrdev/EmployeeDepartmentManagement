using System.Text;
using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.ValueObjects;

namespace EmployeeDepartmentManagement.App.Services;

/// <summary>
/// Service managing in-memory employee records.
/// Demonstrates:
/// - Interface Implementation (implements IEmployeeService)
/// - Polymorphism (manages both FullTimeEmployee and ContractEmployee through base Employee reference)
/// - Encapsulation and Data Validation
/// - Collections (List<Employee>, Dictionary<int, Employee>)
/// - StringBuilder for report generation
/// </summary>
public class EmployeeService : IEmployeeService
{
    // List<Employee>: Stores base Employee references polymorphically pointing to FullTimeEmployee or ContractEmployee
    private readonly List<Employee> _employeeList = new();

    // Dictionary<int, Employee>: Fast O(1) hash index for ID lookups
    private readonly Dictionary<int, Employee> _employeeIndex = new();

    public EmployeeService()
    {
        SeedSampleData();
    }

    /// <summary>
    /// Seeds realistic initial employee records illustrating both Full-Time and Contract workers.
    /// Demonstrates Liskov Substitution Principle (LSP): Both derived types are stored
    /// interchangeably in a List of the abstract base class.
    /// </summary>
    private void SeedSampleData()
    {
        var sampleEmployees = new Employee[]
        {
            new FullTimeEmployee(101, "Aarav", "Sharma", null, Department.FromName("Engineering"), 85000m, 5000m, 28, 5, new DateTime(2021, 3, 15)),
            new FullTimeEmployee(102, "Priya", "Natarajan", "R", Department.FromName("Human Resources"), 62000m, 3500m, 31, 7, new DateTime(2019, 7, 1)),
            new FullTimeEmployee(103, "Murugan", "R", null, Department.FromName("Engineering"), 95000m, 8000m, 29, 6, new DateTime(2020, 1, 10)),
            new ContractEmployee(104, "Ananya", "Deshmukh", "S", Department.FromName("Finance"), 65.00m, 160, new DateTime(2025, 12, 31), 26, 3, new DateTime(2023, 5, 20), "TechPro Staffing"),
            new ContractEmployee(105, "Vikram", "Malhotra", null, Department.FromName("Marketing"), 55.00m, 150, new DateTime(2025, 9, 30), 34, 9, new DateTime(2018, 11, 12), "Creative Consultants")
        };

        foreach (var emp in sampleEmployees)
        {
            _employeeList.Add(emp);
            _employeeIndex[emp.Id] = emp;
        }
    }

    /// <summary>
    /// Validates employee state according to domain business rules.
    /// </summary>
    public (bool IsValid, string ErrorMessage) ValidateEmployee(Employee emp)
    {
        if (emp.Id <= 0)
            return (false, "Employee ID must be a positive integer greater than zero.");

        if (string.IsNullOrWhiteSpace(emp.FirstName) || emp.FirstName.Trim().Length < 2)
            return (false, "First Name must contain at least 2 characters.");

        if (string.IsNullOrWhiteSpace(emp.LastName))
            return (false, "Last Name is mandatory and cannot be blank.");

        if (emp.Department == null || !Department.IsValid(emp.Department.Name))
        {
            string allowed = string.Join(", ", Department.ValidDepartments);
            return (false, $"Invalid Department. Allowed departments are: {allowed}");
        }

        if (emp.Age < 18 || emp.Age > 75)
            return (false, "Employee age must be between 18 and 75.");

        if (emp.YearsOfExperience < 0 || emp.YearsOfExperience > emp.Age - 16)
            return (false, "Years of Experience must be realistic for the given age.");

        if (emp.JoiningDate > DateTime.Today)
            return (false, "Joining Date cannot be in the future.");

        // Type-specific domain validation
        if (emp is FullTimeEmployee ft)
        {
            if (ft.BaseSalary <= 0m)
                return (false, "Base Salary for a full-time employee must be positive.");
            if (ft.AnnualBonus.HasValue && ft.AnnualBonus.Value < 0m)
                return (false, "Annual Bonus cannot be negative.");
        }
        else if (emp is ContractEmployee ct)
        {
            if (ct.HourlyRate <= 0m)
                return (false, "Hourly rate for a contractor must be greater than zero.");
            if (ct.HoursWorked < 0 || ct.HoursWorked > 300)
                return (false, "Hours worked must be between 0 and 300 hours.");
        }

        return (true, string.Empty);
    }

    /// <summary>
    /// Enrolls a new employee into the system, enforcing uniqueness and data integrity.
    /// </summary>
    public bool AddEmployee(Employee emp, out string errorMessage)
    {
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

        _employeeList.Add(emp);
        _employeeIndex.Add(emp.Id, emp);

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Instant O(1) retrieval using dictionary hash indexing.
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
    /// Generates an immutable snapshot record for an employee.
    /// </summary>
    public EmployeeSummary GetEmployeeSummary(int id)
    {
        var emp = GetEmployeeById(id) ?? throw new KeyNotFoundException($"Employee with ID {id} was not found.");
        return emp.ToSummary();
    }

    /// <summary>
    /// Searches employees by Department (case-insensitive).
    /// </summary>
    public List<Employee> SearchEmployees(string department)
    {
        var matched = new List<Employee>();
        if (string.IsNullOrWhiteSpace(department))
            return matched;

        foreach (var emp in _employeeList)
        {
            if (emp.Department.Name.Equals(department.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                matched.Add(emp);
            }
        }
        return matched;
    }

    /// <summary>
    /// Searches employees by Name query (case-insensitive substring match).
    /// </summary>
    public List<Employee> SearchEmployeesByName(string nameQuery)
    {
        var matched = new List<Employee>();
        if (string.IsNullOrWhiteSpace(nameQuery))
            return matched;

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

    public IReadOnlyList<Employee> GetAllEmployees() => _employeeList.AsReadOnly();

    /// <summary>
    /// Computes aggregate payroll metrics polymorphically.
    /// Notice how CalculateGrossPay() dynamically dispatches to either FullTimeEmployee
    /// or ContractEmployee without type-checking if/else ladders!
    /// </summary>
    public (int Count, decimal TotalPayroll, decimal AverageSalary, decimal HighestSalary, decimal LowestSalary) GetPayrollStatistics()
    {
        if (_employeeList.Count == 0)
            return (0, 0m, 0m, 0m, 0m);

        decimal totalPayroll = 0m;
        decimal highestSalary = decimal.MinValue;
        decimal lowestSalary = decimal.MaxValue;

        for (int i = 0; i < _employeeList.Count; i++)
        {
            // Runtime Polymorphism: CalculateGrossPay() resolves to the concrete derived type
            decimal comp = _employeeList[i].CalculateGrossPay();
            totalPayroll += comp;

            if (comp > highestSalary) highestSalary = comp;
            if (comp < lowestSalary) lowestSalary = comp;
        }

        decimal averageSalary = totalPayroll / _employeeList.Count;
        return (_employeeList.Count, totalPayroll, averageSalary, highestSalary, lowestSalary);
    }

    /// <summary>
    /// Generates a comprehensive payroll summary report using StringBuilder.
    /// </summary>
    public string GeneratePayrollReport()
    {
        var stats = GetPayrollStatistics();

        var sb = new StringBuilder();
        sb.AppendLine("=================================================================================================");
        sb.AppendLine("                             EMPLOYEE PAYROLL SUMMARY REPORT (OOP)                               ");
        sb.AppendLine("=================================================================================================");
        sb.AppendLine(string.Format("{0,-6} | {1,-20} | {2,-16} | {3,-10} | {4,-12} | {5,-12}",
            "ID", "Name", "Department", "Type", "Gross Pay", "Net Pay"));
        sb.AppendLine("-------------------------------------------------------------------------------------------------");

        foreach (var emp in _employeeList)
        {
            sb.AppendLine(string.Format("{0,-6} | {1,-20} | {2,-16} | {3,-10} | {4,-12:C} | {5,-12:C}",
                emp.Id,
                emp.FullName.Length > 20 ? emp.FullName.Substring(0, 17) + "..." : emp.FullName,
                emp.Department.Name,
                emp.GetEmployeeType(),
                emp.CalculateGrossPay(),
                emp.CalculateNetPay()));
        }

        sb.AppendLine("-------------------------------------------------------------------------------------------------");
        sb.AppendLine($"Total Headcount      : {stats.Count}");
        sb.AppendLine($"Total Gross Payroll  : {stats.TotalPayroll:C}");
        sb.AppendLine($"Average Compensation : {stats.AverageSalary:C}");
        sb.AppendLine($"Highest Compensation : {stats.HighestSalary:C}");
        sb.AppendLine($"Lowest Compensation  : {stats.LowestSalary:C}");
        sb.AppendLine("=================================================================================================");

        return sb.ToString();
    }
}
