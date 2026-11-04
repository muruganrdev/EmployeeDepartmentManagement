using System.Globalization;
using EmployeeDepartmentManagement.App.Demonstrations;
using EmployeeDepartmentManagement.App.Services;
using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.OrderManagement.Interfaces;
using EmployeeDepartmentManagement.Core.OrderManagement.Models;
using EmployeeDepartmentManagement.Core.OrderManagement.Repositories;
using EmployeeDepartmentManagement.Core.OrderManagement.Services;
using EmployeeDepartmentManagement.Core.Services;
using EmployeeDepartmentManagement.Core.ValueObjects;

namespace EmployeeDepartmentManagement.App;

/// <summary>
/// Console Presentation Layer.
/// Demonstrates Week 1:
/// - Day 1: C# Fundamentals & Value vs Reference Memory
/// - Day 2: Object-Oriented Programming (Polymorphism, Encapsulation, Records, Structs, Orders)
/// - Day 3: Modern C# and LINQ (Collections, Generics, Lambdas, Delegates, Events, Deferred Execution, Extensions, Pattern Matching)
/// </summary>
public static class Program
{
    // Dependency Inversion: Program depends on abstractions
    private static readonly IEmployeeService _employeeService = new EmployeeService();
    private static readonly IEmployeeQueryService _queryService = new EmployeeQueryService();

    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        DisplayWelcomeBanner();

        bool isRunning = true;

        while (isRunning)
        {
            try
            {
                DisplayMenu();
                Console.Write("Enter your selection (1-9, 7/10 to Exit): ");
                string? rawInput = Console.ReadLine();

                if (rawInput == null)
                {
                    isRunning = false;
                    break;
                }

                string input = CleanInput(rawInput);
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                Console.WriteLine();

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
                        HandleOopDemonstration();
                        break;
                    case "7":
                    case "10":
                        isRunning = false;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Thank you for using the Employee & Department Management System. Goodbye!");
                        Console.ResetColor();
                        break;
                    case "8":
                        ModernCSharpDemonstration.Run(_employeeService, _queryService);
                        break;
                    case "9":
                        await AsyncDemonstration.RunAsync(_employeeService);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Invalid selection '{input}'. Please choose an option between 1 and 9 (or 7/10 to exit).");
                        Console.ResetColor();
                        break;
                }
            }
            catch (FormatException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Format Error] An input format exception occurred: {ex.Message}");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Validation Error] Invalid argument: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
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
            try { Console.Clear(); } catch (IOException) { }
        }
    }

    private static void DisplayWelcomeBanner()
    {
        SafeClear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================================================");
        Console.WriteLine("             EMPLOYEE & DEPARTMENT MANAGEMENT SYSTEM (.NET 9)                   ");
        Console.WriteLine("              Week 1 - Day 2: Object-Oriented Programming (OOP)                 ");
        Console.WriteLine("=================================================================================");
        Console.ResetColor();
        Console.WriteLine("Enterprise HR & Payroll domain demonstrating Inheritance, Polymorphism,");
        Console.WriteLine("Encapsulation, Interfaces, Records, Structs, and SOLID Principles.");
        Console.WriteLine();
    }

    private static void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("-------------------------------- MENU -------------------------------------------");
        Console.ResetColor();
        Console.WriteLine("  1. View All Employees (Polymorphic Roster)");
        Console.WriteLine("  2. Add New Employee (Full-Time or Contract - Encapsulation)");
        Console.WriteLine("  3. Search Employee by ID (O(1) Dictionary Lookup & Record Summary)");
        Console.WriteLine("  4. Search Employees by Department");
        Console.WriteLine("  5. View Payroll & Compensation Statistics (Polymorphic Calculation)");
        Console.WriteLine("  6. Run OOP & Day 2 Deep Dive Demo (Inheritance, Records, Structs, Orders)");
        Console.WriteLine("  7. Exit Application (or press 10)");
        Console.WriteLine("  8. Run Modern C# & LINQ Deep Dive (Week 1 - Day 3)");
        Console.WriteLine("  9. Run Async/Await & CancellationToken Deep Dive (Week 1 - Day 4)");
        Console.WriteLine(" 10. Exit Application");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("---------------------------------------------------------------------------------");
        Console.ResetColor();
    }

    private static void HandleViewAllEmployees()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== All Employee Records (Polymorphic View) ===");
        Console.ResetColor();

        var employees = _employeeService.GetAllEmployees();
        if (employees.Count == 0)
        {
            Console.WriteLine("No employee records found.");
            return;
        }

        Console.WriteLine(string.Format("{0,-5} | {1,-18} | {2,-16} | {3,-10} | {4,-12} | {5,-10}",
            "ID", "Full Name", "Department", "Type", "Gross Pay", "Net Pay"));
        Console.WriteLine(new string('-', 85));

        foreach (var emp in employees)
        {
            // Polymorphic method calls: GetEmployeeType(), CalculateGrossPay(), CalculateNetPay()
            Console.WriteLine(string.Format("{0,-5} | {1,-18} | {2,-16} | {3,-10} | {4,-12:C} | {5,-10:C}",
                emp.Id,
                emp.FullName.Length > 18 ? emp.FullName[..15] + "..." : emp.FullName,
                emp.Department.Name,
                emp.GetEmployeeType(),
                emp.CalculateGrossPay(),
                emp.CalculateNetPay()));
        }

        Console.WriteLine(new string('-', 85));
        Console.WriteLine($"Total Employees: {employees.Count}");
    }

    private static void HandleAddEmployee()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== Add New Employee ===");
        Console.ResetColor();

        Console.WriteLine("Select Employment Classification:");
        Console.WriteLine("  1. Full-Time Employee (Salaried + Annual Bonus)");
        Console.WriteLine("  2. Contract Employee (Hourly Rate * Hours Worked)");
        string empTypeChoice = ReadChoice("Select (1 or 2): ", new[] { "1", "2" });

        int id = ReadPositiveInt("Enter Employee ID (e.g., 106): ");
        if (_employeeService.GetEmployeeById(id) != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Validation Error] Employee ID {id} is already taken.");
            Console.ResetColor();
            return;
        }

        string firstName = ReadRequiredString("Enter First Name: ");
        Console.Write("Enter Middle Name (Optional, press Enter to skip): ");
        string? middleInput = Console.ReadLine();
        string? middleName = string.IsNullOrWhiteSpace(middleInput) ? null : middleInput.Trim();
        string lastName = ReadRequiredString("Enter Last Name: ");

        Console.WriteLine("\nAllowed Departments: " + string.Join(", ", Department.ValidDepartments));
        Department department = ReadDepartment();

        int age = ReadIntInRange("Enter Age (18 - 75): ", 18, 75);
        int maxExp = Math.Max(0, age - 16);
        int experience = ReadIntInRange($"Enter Years of Experience (0 - {maxExp}): ", 0, maxExp);
        DateTime joiningDate = ReadPastOrTodayDate("Enter Joining Date (yyyy-MM-dd, e.g., 2023-04-15): ");

        Employee newEmployee;

        if (empTypeChoice == "1")
        {
            decimal baseSalary = ReadPositiveDecimal("Enter Annual Base Salary in USD (e.g., 85000): ");
            decimal? bonus = ReadOptionalDecimal("Enter Annual Bonus (Optional, press Enter for None): ");
            Console.Write("Enter Health Insurance Plan (Optional, press Enter for Standard): ");
            string? plan = Console.ReadLine();
            string insurance = string.IsNullOrWhiteSpace(plan) ? "Standard Corporate Healthcare" : plan.Trim();

            newEmployee = new FullTimeEmployee(
                id, firstName, lastName, middleName, department,
                baseSalary, bonus, age, experience, joiningDate, insurance);
        }
        else
        {
            decimal hourlyRate = ReadPositiveDecimal("Enter Hourly Rate in USD (e.g., 65.00): ");
            int hours = ReadIntInRange("Enter Monthly Hours Worked (1 - 300): ", 1, 300);
            DateTime contractEnd = ReadFutureDate("Enter Contract End Date (yyyy-MM-dd, e.g., 2025-12-31): ");
            Console.Write("Enter Staffing Agency Name (Optional, press Enter for Independent): ");
            string? agency = Console.ReadLine();
            string agencyName = string.IsNullOrWhiteSpace(agency) ? "Independent Contractor" : agency.Trim();

            newEmployee = new ContractEmployee(
                id, firstName, lastName, middleName, department,
                hourlyRate, hours, contractEnd, age, experience, joiningDate, agencyName);
        }

        if (_employeeService.AddEmployee(newEmployee, out string error))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[Success] {newEmployee.GetEmployeeType()} Employee '{newEmployee.FullName}' (ID: {newEmployee.Id}) was successfully added!");
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
            Console.WriteLine("\n--- Employee Record Details ---");
            Console.ResetColor();
            Console.WriteLine($"  ID                  : {emp.Id}");
            Console.WriteLine($"  Full Name           : {emp.FullName}");
            Console.WriteLine($"  Classification      : {emp.GetEmployeeType()}");
            Console.WriteLine($"  Department          : {emp.Department.Name} (Code: {emp.Department.Code})");
            Console.WriteLine($"  Age                 : {emp.Age} years");
            Console.WriteLine($"  Experience          : {emp.YearsOfExperience} years");
            Console.WriteLine($"  Joining Date        : {emp.JoiningDate:yyyy-MM-dd}");
            Console.WriteLine($"  Gross Compensation  : {emp.CalculateGrossPay():C}");
            Console.WriteLine($"  Estimated Tax       : {emp.CalculateTaxDeduction():C}");
            Console.WriteLine($"  Net Take-Home Pay   : {emp.CalculateNetPay():C}");

            if (emp is FullTimeEmployee ft)
            {
                Console.WriteLine($"  Base Salary         : {ft.BaseSalary:C}");
                Console.WriteLine($"  Annual Bonus        : {(ft.AnnualBonus.HasValue ? ft.AnnualBonus.Value.ToString("C") : "None")}");
                Console.WriteLine($"  Health Plan         : {ft.HealthInsurancePlan}");
            }
            else if (emp is ContractEmployee ct)
            {
                Console.WriteLine($"  Hourly Rate         : {ct.HourlyRate:C}/hr");
                Console.WriteLine($"  Hours Worked        : {ct.HoursWorked} hrs");
                Console.WriteLine($"  Contract End Date   : {ct.ContractEndDate:yyyy-MM-dd}");
                Console.WriteLine($"  Staffing Agency     : {ct.AgencyName}");
            }

            // Demonstrating EmployeeSummary record snapshot
            EmployeeSummary summary = emp.ToSummary();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  [Record Snapshot] {summary}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[Not Found] No employee found with ID {id}.");
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
                Console.WriteLine($"  • [{emp.GetEmployeeType()}] ID: {emp.Id:D3} | {emp.FullName} | Net Pay: {emp.CalculateNetPay():C}");
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
    /// Live deep-dive interactive demonstration of Day 2 OOP Concepts:
    /// 1. Polymorphism and Method Overriding
    /// 2. Encapsulation & Validation Protection
    /// 3. Class vs Record vs Struct Equality
    /// 4. Composition (Employee has-a Department)
    /// </summary>
    private static void HandleOopDemonstration()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================================================");
        Console.WriteLine("               DEEP DIVE: C# OBJECT-ORIENTED PROGRAMMING (OOP)                   ");
        Console.WriteLine("=================================================================================");
        Console.ResetColor();

        // --- PART 1: POLYMORPHISM ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[1] Runtime Polymorphism (Base Class Reference -> Derived Objects):");
        Console.ResetColor();
        Console.WriteLine("Both FullTimeEmployee and ContractEmployee inherit from Employee.");
        Console.WriteLine("We treat both polymorphically through the Employee base reference:");

        Employee ft = new FullTimeEmployee(201, "Alice", "Wonder", null, Department.FromName("Engineering"), 90000m, 5000m, 30, 7, DateTime.Today);
        Employee ct = new ContractEmployee(202, "Bob", "Builder", null, Department.FromName("Engineering"), 75.00m, 160, DateTime.Today.AddMonths(6), 35, 10, DateTime.Today);

        Employee[] staff = { ft, ct };
        foreach (Employee emp in staff)
        {
            Console.WriteLine($"  -> emp.CalculateGrossPay() on {emp.GetEmployeeType()} Employee ({emp.FullName}) = {emp.CalculateGrossPay():C}");
        }
        Console.WriteLine("Notice: The CLR dynamically executes the correct overridden method at runtime!");

        // --- PART 2: ENCAPSULATION ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[2] Encapsulation & Controlled Data Invariants:");
        Console.ResetColor();
        Console.WriteLine("Properties protect private state against illegal modifications:");
        try
        {
            Console.WriteLine("Attempting: ft.BaseSalary = -50000;");
            ((FullTimeEmployee)ft).BaseSalary = -50000m;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [Blocked by Encapsulation] Exception caught: {ex.ParamName} - {ex.Message.Split('\n')[0]}");
            Console.ResetColor();
        }

        // --- PART 3: RECORD VS CLASS VS STRUCT EQUALITY ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[3] Class vs Record vs Struct (Equality Comparison):");
        Console.ResetColor();

        // Records (Value Equality)
        EmployeeSummary rec1 = new(101, "Aarav Sharma", "Engineering", "Full-Time", 90000m, 70200m);
        EmployeeSummary rec2 = new(101, "Aarav Sharma", "Engineering", "Full-Time", 90000m, 70200m);
        Console.WriteLine($"  Record 1: {rec1}");
        Console.WriteLine($"  Record 2: {rec2}");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"  rec1 == rec2 ? {rec1 == rec2} (Records compare by VALUE of properties!)");
        Console.ResetColor();

        // Classes (Reference Equality by default)
        Department dept1 = new("ENG", "Engineering");
        Department dept2 = new("ENG", "Engineering");
        Console.WriteLine($"  ReferenceEquals(dept1, dept2) ? {ReferenceEquals(dept1, dept2)} (Separate instances on Heap!)");

        // Struct (Stack-allocated Value Object)
        WorkHours hours1 = new(160, 10);
        WorkHours hours2 = new(160, 10);
        Console.WriteLine($"  WorkHours struct: {hours1} | Total: {hours1.TotalHours} hrs");
        Console.WriteLine($"  hours1.Equals(hours2) ? {hours1.Equals(hours2)} (Structs compare by value!)");

        // --- PART 4: COMPOSITION ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[4] Composition: Employee *has-a* Department:");
        Console.ResetColor();
        Console.WriteLine($"  Employee: {ft.FullName}");
        Console.WriteLine($"  Department object composed inside: {ft.Department}");

        // --- PART 5: ORDER MANAGEMENT DOMAIN & INTERFACE-BASED DESIGN ---
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[5] Order Management Domain & Interface-Based Architecture:");
        Console.ResetColor();
        Console.WriteLine("Flow: Customer -> Order -> OrderItem -> Product via IOrderService & IOrderRepository");

        // 1. Domain Entities Construction & Encapsulation
        var customer = new Customer(1, "Alice Smith", "alice.smith@example.com");
        var laptop = new Product(101, "Dell XPS 15 Laptop", 1899.99m, 10);
        var mouse = new Product(102, "Logitech MX Master 3S", 99.99m, 25);

        Console.WriteLine($"  Customer  : {customer}");
        Console.WriteLine($"  Product 1 : {laptop}");
        Console.WriteLine($"  Product 2 : {mouse}");

        // 2. Interface-Based Service and Repository Wiring (DIP)
        IOrderRepository orderRepo = new InMemoryOrderRepository();
        IOrderService orderService = new OrderService(orderRepo);

        // 3. Creating Order through Interface Contract
        var items = new List<(Product Product, int Quantity)>
        {
            (laptop, 1),
            (mouse, 2)
        };

        var order = orderService.CreateOrder(customer, items);
        Console.WriteLine($"\n  Order Created: [Order #{order.OrderId}] Date: {order.OrderDate:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"  Customer on Order: {order.Customer.Name}");
        Console.WriteLine("  Order Items:");
        foreach (var item in order.OrderItems)
        {
            Console.WriteLine($"    • {item.Product.Name} x {item.Quantity} @ {item.UnitPrice:C} = {item.CalculateSubtotal():C}");
        }
        Console.WriteLine($"  Calculated Total: {order.CalculateTotal():C}");
        Console.WriteLine($"  Remaining Stock -> Laptop: {laptop.AvailableStock}, Mouse: {mouse.AvailableStock}");

        // 4. Retrieving from Repository via Interface
        var retrieved = orderService.GetOrderById(order.OrderId);
        Console.WriteLine($"  Repository Retrieval: Found Order #{retrieved?.OrderId} with Total {retrieved?.CalculateTotal():C}");

        // 5. Encapsulation & Validation Boundaries
        try
        {
            Console.WriteLine("\n  Attempting invalid order item (quantity: -3):");
            new OrderItem(99, laptop, -3);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [Blocked by Invariant] {ex.ParamName}: {ex.Message.Split('\n')[0]}");
            Console.ResetColor();
        }
        Console.WriteLine("=================================================================================");
    }

    #region Input Validation Helpers

    private static string CleanInput(string? str) => str?.Trim().Trim('\uFEFF') ?? string.Empty;

    private static string ReadChoice(string prompt, string[] validChoices)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());
            if (Array.Exists(validChoices, c => string.Equals(c, input, StringComparison.OrdinalIgnoreCase)))
            {
                return input;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Invalid Choice] Allowed choices: {string.Join(", ", validChoices)}");
            Console.ResetColor();
        }
    }

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

    private static Department ReadDepartment()
    {
        while (true)
        {
            Console.Write("Enter Department: ");
            string input = CleanInput(Console.ReadLine());
            if (Department.IsValid(input))
            {
                return Department.FromName(input);
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
                Console.WriteLine("[Invalid Date] Date cannot be in the future.");
                Console.ResetColor();
                continue;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Invalid Date] Please enter a valid date in yyyy-MM-dd format (e.g. 2023-01-15).");
            Console.ResetColor();
        }
    }

    private static DateTime ReadFutureDate(string prompt)
    {
        string[] formats = { "yyyy-MM-dd", "yyyy/MM/dd", "dd-MM-yyyy", "dd/MM/yyyy" };
        while (true)
        {
            Console.Write(prompt);
            string input = CleanInput(Console.ReadLine());
            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                if (parsedDate.Date >= DateTime.Today)
                {
                    return parsedDate.Date;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Invalid Date] Contract end date must be today or in the future.");
                Console.ResetColor();
                continue;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Invalid Date] Please enter a valid date in yyyy-MM-dd format (e.g. 2025-12-31).");
            Console.ResetColor();
        }
    }

    #endregion
}
