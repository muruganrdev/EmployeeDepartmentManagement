using System.Globalization;
using EmployeeDepartmentManagement.App.Models;
using EmployeeDepartmentManagement.App.Services;

namespace EmployeeDepartmentManagement.App;

/// <summary>
/// Main Console Entry Point.
/// Demonstrates C# Fundamentals: Control Flow (switch, while, if/else),
/// Operators, String Methods, Exception Handling, and Console I/O.
/// </summary>
public static class Program
{
    private static readonly EmployeeService _employeeService = new();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        DisplayWelcomeBanner();

        bool isRunning = true;

        // While loop driving the main interactive application menu
        while (isRunning)
        {
            try
            {
                DisplayMenu();
                Console.Write("Enter your selection (1-7): ");
                string? rawInput = Console.ReadLine();

                if (rawInput == null)
                {
                    // End of stream reached (e.g., redirected input finished)
                    isRunning = false;
                    break;
                }

                string input = rawInput.Trim().Trim('\uFEFF');

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                Console.WriteLine();

                // Switch statement demonstrating option dispatch
                switch (input)
                {
                    case "1":
                        HandleViewAllEmployees();
                        break;
                    case "2":
                        HandleAddEmployee();
                        break;
                    case "3":
                        HandleSearchEmployeeById();
                        break;
                    case "4":
                        HandleSearchEmployeesByDepartment();
                        break;
                    case "5":
                        HandlePayrollStatistics();
                        break;
                    case "6":
                        HandleValueVsReferenceTypesDemo();
                        break;
                    case "7":
                        isRunning = false;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Thank you for using the Employee & Department Management System. Goodbye!");
                        Console.ResetColor();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Invalid selection '{input}' (length: {input?.Length}). Please choose an option between 1 and 7.");
                        Console.ResetColor();
                        break;
                }
            }
            catch (FormatException ex)
            {
                // Handling parsing and formatting errors
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Format Error] An input format exception occurred: {ex.Message}");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                // Handling argument and validation errors
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Argument Error] Invalid argument: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Top-level catch block ensuring the console application never abruptly crashes
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Unexpected Error] An unexpected error occurred: {ex.Message}");
                Console.ResetColor();
            }

            if (isRunning)
            {
                Console.WriteLine();
                Console.Write("Press [Enter] to return to the main menu...");
                Console.ReadLine();
                SafeClear();
            }
        }
    }

    private static void SafeClear()
    {
        if (!Console.IsInputRedirected && !Console.IsOutputRedirected)
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // Ignore console clear failure when running in redirected streams
            }
        }
    }

    private static void DisplayWelcomeBanner()
    {
        SafeClear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===============================================================================");
        Console.WriteLine("             EMPLOYEE & DEPARTMENT MANAGEMENT SYSTEM (.NET 9)                 ");
        Console.WriteLine("                    Week 1 - Day 1: C# Fundamentals                           ");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();
        Console.WriteLine("Welcome! This application manages employee records while demonstrating");
        Console.WriteLine("C# variables, types, operators, collections, strings, and memory semantics.");
        Console.WriteLine();
    }

    private static void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("-------------------------------- MENU -----------------------------------------");
        Console.ResetColor();
        Console.WriteLine("  1. View All Employees (Tabular View)");
        Console.WriteLine("  2. Add New Employee (Validation & Error Handling)");
        Console.WriteLine("  3. Search Employee by ID (Fast Dictionary Lookup)");
        Console.WriteLine("  4. Search Employees by Department");
        Console.WriteLine("  5. View Payroll & Compensation Statistics (Arithmetic Operators)");
        Console.WriteLine("  6. Run C# Fundamentals Demo (Value vs Reference Types Live Memory)");
        Console.WriteLine("  7. Exit Application");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("-------------------------------------------------------------------------------");
        Console.ResetColor();
    }

    private static void HandleViewAllEmployees()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== All Employee Records ===");
        Console.ResetColor();

        var employees = _employeeService.GetAllEmployees();

        if (employees.Count == 0)
        {
            Console.WriteLine("No employee records found.");
            return;
        }

        Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-16} | {3,-12} | {4,-10} | {5,-6} | {6,-6}",
            "ID", "Full Name", "Department", "Base Salary", "Bonus", "Age", "Exp"));
        Console.WriteLine(new string('-', 85));

        foreach (var emp in employees)
        {
            string bonus = emp.PerformanceBonus.HasValue ? emp.PerformanceBonus.Value.ToString("C0") : "None";
            Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-16} | {3,-12:C0} | {4,-10} | {5,-6} | {6,-6}",
                emp.Id,
                emp.FullName.Length > 20 ? emp.FullName.Substring(0, 17) + "..." : emp.FullName,
                emp.Department,
                emp.BaseSalary,
                bonus,
                emp.Age,
                $"{emp.YearsOfExperience} yrs"));
        }

        Console.WriteLine(new string('-', 85));
        Console.WriteLine($"Total Employees: {employees.Count}");
    }

    private static void HandleAddEmployee()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== Add New Employee ===");
        Console.ResetColor();

        // 1. Employee ID (int)
        int id = ReadPositiveInt("Enter Employee ID (e.g., 106): ");

        // Check if ID already exists before prompting for other fields
        if (_employeeService.GetEmployeeById(id) != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Validation Error] Employee ID {id} is already taken. Please use a unique ID.");
            Console.ResetColor();
            return;
        }

        // 2. First Name (string)
        string firstName = ReadRequiredString("Enter First Name: ");

        // 3. Middle Name (string? - Nullable Reference Type)
        Console.Write("Enter Middle Name (Optional, press Enter to skip): ");
        string? middleInput = Console.ReadLine();
        string? middleName = string.IsNullOrWhiteSpace(middleInput) ? null : middleInput.Trim();

        // 4. Last Name (string)
        string lastName = ReadRequiredString("Enter Last Name: ");

        // 5. Department selection (Array validation)
        Console.WriteLine("\nAllowed Departments: " + string.Join(", ", Department.ValidDepartments));
        string department = ReadDepartment();

        // 6. Base Salary (decimal)
        decimal baseSalary = ReadPositiveDecimal("Enter Base Annual Salary in USD (e.g., 85000): ");

        // 7. Performance Bonus (decimal? - Nullable Value Type)
        decimal? bonus = ReadOptionalDecimal("Enter Performance Bonus (Optional, press Enter for None): ");

        // 8. Age (int)
        int age = ReadIntInRange("Enter Age (18 - 75): ", 18, 75);

        // 9. Years of Experience (int)
        int maxExp = Math.Max(0, age - 16);
        int experience = ReadIntInRange($"Enter Years of Experience (0 - {maxExp}): ", 0, maxExp);

        // 10. Joining Date (DateTime)
        DateTime joiningDate = ReadPastOrTodayDate("Enter Joining Date (yyyy-MM-dd, e.g., 2023-04-15): ");

        var newEmployee = new Employee(
            id,
            firstName,
            lastName,
            middleName,
            department,
            baseSalary,
            bonus,
            age,
            experience,
            joiningDate,
            isActive: true);

        if (_employeeService.AddEmployee(newEmployee, out string error))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[Success] Employee '{newEmployee.FullName}' (ID: {newEmployee.Id}) was successfully added!");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[Error] Failed to add employee: {error}");
            Console.ResetColor();
        }
    }

    private static void HandleSearchEmployeeById()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== Search Employee by ID ===");
        Console.ResetColor();

        int id = ReadPositiveInt("Enter Employee ID to search: ");
        Employee? emp = _employeeService.GetEmployeeById(id);

        if (emp != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- Employee Found ---");
            Console.ResetColor();
            Console.WriteLine($"  ID                  : {emp.Id}");
            Console.WriteLine($"  Full Name           : {emp.FullName}");
            Console.WriteLine($"  Department          : {emp.Department}");
            Console.WriteLine($"  Age                 : {emp.Age} years");
            Console.WriteLine($"  Experience          : {emp.YearsOfExperience} years");
            Console.WriteLine($"  Joining Date        : {emp.JoiningDate:yyyy-MM-dd}");
            Console.WriteLine($"  Base Salary         : {emp.BaseSalary:C}");
            Console.WriteLine($"  Performance Bonus   : {(emp.PerformanceBonus.HasValue ? emp.PerformanceBonus.Value.ToString("C") : "None")}");
            Console.WriteLine($"  Gross Compensation  : {emp.CalculateGrossCompensation():C}");
            Console.WriteLine($"  Estimated Tax       : {emp.CalculateEstimatedTax():C}");
            Console.WriteLine($"  Net Take-Home Pay   : {emp.CalculateNetPay():C}");
            Console.WriteLine($"  Employment Status   : {(emp.IsActive ? "Active" : "Inactive")}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[Not Found] No employee found with ID {id}. Please verify the ID and try again.");
            Console.ResetColor();
        }
    }

    private static void HandleSearchEmployeesByDepartment()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== Search Employees by Department ===");
        Console.ResetColor();

        Console.WriteLine("Allowed Departments: " + string.Join(", ", Department.ValidDepartments));
        Console.Write("Enter Department Name: ");
        string? dept = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dept))
        {
            Console.WriteLine("Department name cannot be empty.");
            return;
        }

        var results = _employeeService.SearchEmployees(dept);

        if (results.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nFound {results.Count} employee(s) in department '{dept}':");
            Console.ResetColor();

            foreach (var emp in results)
            {
                Console.WriteLine($"  • [ID: {emp.Id}] {emp.FullName} | Salary: {emp.BaseSalary:C} | Exp: {emp.YearsOfExperience} yrs");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nNo employees found in department '{dept}'.");
            Console.ResetColor();
        }
    }

    private static void HandlePayrollStatistics()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== Payroll & Compensation Statistics ===");
        Console.ResetColor();

        string report = _employeeService.GeneratePayrollReport();
        Console.WriteLine(report);
    }

    /// <summary>
    /// Interactive demonstration of C# Value Types vs Reference Types.
    /// Explains memory allocation, stack vs heap, copying semantics, and mutation.
    /// </summary>
    private static void HandleValueVsReferenceTypesDemo()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===============================================================================");
        Console.WriteLine("        DEEP DIVE: VALUE TYPES VS REFERENCE TYPES IN C# (.NET)                 ");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // --- PART 1: VALUE TYPE COPYING DEMONSTRATION ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[1] Value Types (int, decimal, bool, DateTime, struct):");
        Console.ResetColor();
        Console.WriteLine("Value types hold their actual data directly where declared (e.g., on the stack).");
        Console.WriteLine("When you copy a value type, a completely INDEPENDENT duplicate is created in memory.");

        int originalSalary = 50000;
        int copiedSalary = originalSalary; // Value copy
        copiedSalary = 90000; // Modifying the copy

        Console.WriteLine("\nCode executed:");
        Console.WriteLine("  int originalSalary = 50000;");
        Console.WriteLine("  int copiedSalary = originalSalary;");
        Console.WriteLine("  copiedSalary = 90000;");
        Console.WriteLine("\nMemory Result:");
        Console.WriteLine($"  originalSalary = {originalSalary:C0}  <-- Notice: ORIGINAL WAS NOT CHANGED!");
        Console.WriteLine($"  copiedSalary   = {copiedSalary:C0}  <-- Only the copy changed.");

        // --- PART 2: REFERENCE TYPE COPYING DEMONSTRATION ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[2] Reference Types (class, string, List<T>, Dictionary):");
        Console.ResetColor();
        Console.WriteLine("Reference type variables store a MEMORY ADDRESS (reference) pointing to an object");
        Console.WriteLine("allocated on the Managed Heap. When you copy a reference type variable, you only");
        Console.WriteLine("copy the pointer, NOT the underlying object!");

        Employee emp1 = new Employee(999, "Test", "Employee", null, "Engineering", 60000m, null, 25, 2, DateTime.Today);
        Employee emp2 = emp1; // Reference copy (both point to the SAME object on heap)

        Console.WriteLine("\nCode executed:");
        Console.WriteLine("  Employee emp1 = new Employee(999, \"Test\", ..., BaseSalary = 60000);");
        Console.WriteLine("  Employee emp2 = emp1; // Reference copy");
        Console.WriteLine("  emp2.BaseSalary = 120000; // Modify via emp2 reference");

        emp2.BaseSalary = 120000m;

        Console.WriteLine("\nMemory Result:");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"  emp1.BaseSalary = {emp1.BaseSalary:C0}  <-- EMP1's SALARY CHANGED TOO!");
        Console.WriteLine($"  emp2.BaseSalary = {emp2.BaseSalary:C0}");
        Console.ResetColor();
        Console.WriteLine("  Explanation: emp1 and emp2 point to the EXACT same memory address on the Heap.");

        // --- PART 3: PROPER CLONING / INDEPENDENT INSTANCE ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[3] Creating an Independent Instance (Cloning):");
        Console.ResetColor();
        Employee emp3 = emp1.ShallowCopy(); // Creates a new object on heap
        emp3.BaseSalary = 75000m;

        Console.WriteLine("Code executed:");
        Console.WriteLine("  Employee emp3 = emp1.ShallowCopy(); // Allocates NEW object on heap");
        Console.WriteLine("  emp3.BaseSalary = 75000;");
        Console.WriteLine("\nMemory Result:");
        Console.WriteLine($"  emp1.BaseSalary = {emp1.BaseSalary:C0}  <-- UNCHANGED!");
        Console.WriteLine($"  emp3.BaseSalary = {emp3.BaseSalary:C0}  <-- Independent copy modified.");

        // --- PART 4: NULLABLE TYPES ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[4] Nullable Types (decimal?, DateTime?, string?):");
        Console.ResetColor();
        Console.WriteLine("Value types cannot normally be null. Nullable<T> (T?) wraps the value type with");
        Console.WriteLine("a boolean flag (.HasValue) indicating whether a value is present.");

        decimal? bonusA = null;
        decimal? bonusB = 5000m;

        Console.WriteLine($"  bonusA: HasValue = {bonusA.HasValue}, Fallback via '??' = {bonusA ?? 0m:C}");
        Console.WriteLine($"  bonusB: HasValue = {bonusB.HasValue}, Value = {bonusB.Value:C}");
        Console.WriteLine("===============================================================================");
    }

    #region Input Validation Helpers

    private static string CleanInput(string? str) => str?.Trim().Trim('\uFEFF') ?? string.Empty;

    private static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());

            if (int.TryParse(input, out int result) && result > 0)
            {
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Invalid Input] Please enter a valid positive whole number.");
            Console.ResetColor();
        }
    }

    private static int ReadIntInRange(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());

            if (int.TryParse(input, out int result) && result >= min && result <= max)
            {
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Invalid Input] Please enter a number between {min} and {max}.");
            Console.ResetColor();
        }
    }

    private static decimal ReadPositiveDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());

            if (decimal.TryParse(input, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture, out decimal result) && result > 0)
            {
                return result;
            }

            // Also try current culture
            if (decimal.TryParse(input, out result) && result > 0)
            {
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Invalid Input] Please enter a valid positive decimal number (e.g. 75000).");
            Console.ResetColor();
        }
    }

    private static decimal? ReadOptionalDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (decimal.TryParse(input, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture, out decimal result) && result >= 0)
            {
                return result;
            }

            if (decimal.TryParse(input, out result) && result >= 0)
            {
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Invalid Input] Please enter a valid non-negative decimal amount, or press Enter to skip.");
            Console.ResetColor();
        }
    }

    private static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());

            if (!string.IsNullOrWhiteSpace(input) && input.Length >= 2)
            {
                return input;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Invalid Input] Field is required and must contain at least 2 non-whitespace characters.");
            Console.ResetColor();
        }
    }

    private static string ReadDepartment()
    {
        while (true)
        {
            Console.Write("Enter Department: ");
            string input = CleanInput(Console.ReadLine());

            if (Department.IsValid(input))
            {
                // Return properly cased canonical name
                foreach (var dept in Department.ValidDepartments)
                {
                    if (string.Equals(dept, input, StringComparison.OrdinalIgnoreCase))
                    {
                        return dept;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Invalid Department] Must be one of: {string.Join(", ", Department.ValidDepartments)}");
            Console.ResetColor();
        }
    }

    private static DateTime ReadPastOrTodayDate(string prompt)
    {
        string[] formats = { "yyyy-MM-dd", "yyyy/MM/dd", "dd-MM-yyyy", "dd/MM/yyyy" };

        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());

            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                if (parsedDate.Date <= DateTime.Today)
                {
                    return parsedDate.Date;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Invalid Date] Joining date cannot be in the future.");
                Console.ResetColor();
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Invalid Date] Please enter a valid date in yyyy-MM-dd format (e.g. 2023-01-15).");
            Console.ResetColor();
        }
    }

    #endregion
}
