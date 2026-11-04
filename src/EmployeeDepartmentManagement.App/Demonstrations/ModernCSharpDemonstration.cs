using EmployeeDepartmentManagement.Core.Common;
using EmployeeDepartmentManagement.Core.Events;
using EmployeeDepartmentManagement.Core.Extensions;
using EmployeeDepartmentManagement.Core.Interfaces;
using EmployeeDepartmentManagement.Core.Models;
using EmployeeDepartmentManagement.Core.Services;

namespace EmployeeDepartmentManagement.App.Demonstrations;

/// <summary>
/// Modular demonstration runner for Week 1 – Day 3: Modern C# and LINQ.
/// Separates demonstration orchestration from Program.cs to maintain clean architecture.
/// </summary>
public static class ModernCSharpDemonstration
{
    // Delegate declaration: Custom delegate for compensation performance adjustment
    public delegate decimal CompensationAdjustmentDelegate(decimal baseAmount, double performanceRating);

    public static void Run(IEmployeeService employeeService, IEmployeeQueryService queryService)
    {
        ArgumentNullException.ThrowIfNull(employeeService);
        ArgumentNullException.ThrowIfNull(queryService);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================================================");
        Console.WriteLine("             WEEK 1 - DAY 3: MODERN C# & LINQ DEEP DIVE DEMO                     ");
        Console.WriteLine("=================================================================================");
        Console.ResetColor();

        var employees = employeeService.GetAllEmployees();

        // -------------------------------------------------------------------------
        // SECTION 1: EIGHT LINQ QUERY OPERATIONS
        // -------------------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[1] LINQ Operation 1: Filter Employees by Salary (> $75,000)");
        Console.ResetColor();
        var highEarners = queryService.FilterByMinimumSalary(employees, 75000m);
        foreach (var emp in highEarners)
        {
            Console.WriteLine($"  • {emp.FullName,-20} | Dept: {emp.Department.Name,-16} | Gross: {emp.CalculateGrossPay():C}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[2] LINQ Operation 2: Filter Employees by Department ('Engineering')");
        Console.ResetColor();
        var engineeringStaff = queryService.FilterByDepartment(employees, "Engineering");
        foreach (var emp in engineeringStaff)
        {
            Console.WriteLine($"  • {emp.FullName,-20} | Role: {emp.GetEmployeeType(),-10} | Gross: {emp.CalculateGrossPay():C}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[3] LINQ Operation 3: Sort Employees by Salary (Descending)");
        Console.ResetColor();
        var sortedBySalary = queryService.SortBySalary(employees, ascending: false);
        foreach (var emp in sortedBySalary)
        {
            Console.WriteLine($"  • {emp.CalculateGrossPay(),12:C} | {emp.FullName,-20} ({emp.GetEmployeeType()})");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[4] LINQ Operation 4: Sort Employees Alphabetically by Full Name");
        Console.ResetColor();
        var sortedByName = queryService.SortByName(employees);
        foreach (var emp in sortedByName)
        {
            Console.WriteLine($"  • {emp.FullName,-22} | Dept: {emp.Department.Name}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[5] LINQ Operation 5: Group Employees by Department (GroupBy)");
        Console.ResetColor();
        var groups = queryService.GroupByDepartment(employees);
        foreach (var group in groups)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  📁 Department: {group.DepartmentName} (Count: {group.EmployeeCount})");
            Console.ResetColor();
            foreach (var emp in group.Employees)
            {
                Console.WriteLine($"     - [{emp.GetEmployeeType()}] {emp.FullName} (Net Pay: {emp.CalculateNetPay():C})");
            }
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[6] LINQ Operation 6: Average & Total Salary by Department (Aggregate)");
        Console.ResetColor();
        var deptAverages = queryService.GetAverageSalaryByDepartment(employees);
        Console.WriteLine(string.Format("  {0,-18} | {1,-6} | {2,-16} | {3,-16}", "Department", "Staff", "Average Salary", "Total Payroll"));
        Console.WriteLine("  " + new string('-', 64));
        foreach (var stat in deptAverages)
        {
            Console.WriteLine(string.Format("  {0,-18} | {1,-6} | {2,-16:C} | {3,-16:C}",
                stat.DepartmentName, stat.EmployeeCount, stat.AverageSalary, stat.TotalPayroll));
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[7] LINQ Operation 7: Find Highest-Paid Employee (MaxBy)");
        Console.ResetColor();
        var topEarner = queryService.GetHighestPaidEmployee(employees);
        if (topEarner != null)
        {
            Console.WriteLine($"  Top Earner: {topEarner.FullName} in {topEarner.Department.Name}");
            Console.WriteLine($"  Gross Compensation: {topEarner.CalculateGrossPay():C} | Classification: {topEarner.GetEmployeeType()}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[8] LINQ Operation 8: Dynamic Multiple-Condition Employee Search");
        Console.ResetColor();
        Console.WriteLine("  Search Criteria: Dept = 'Engineering', MinSalary = $80,000, Type = 'Full-Time'");
        var multiSearchResults = queryService.SearchEmployees(
            employees,
            department: "Engineering",
            minSalary: 80000m,
            employeeType: "Full-Time");
        foreach (var emp in multiSearchResults)
        {
            Console.WriteLine($"  -> Match: [{emp.Id:D3}] {emp.FullName} | Gross: {emp.CalculateGrossPay():C}");
        }

        // -------------------------------------------------------------------------
        // SECTION 2: GENERICS
        // -------------------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[9] Generics: Type-Safe Reusable Components (GenericFilter<T> & OperationResult<T>)");
        Console.ResetColor();
        var genericFilter = new GenericFilter<Employee>();
        var fullTimeStaff = genericFilter.Filter(employees, emp => emp is FullTimeEmployee).ToList();
        Console.WriteLine($"  GenericFilter<Employee>.Filter(emp => emp is FullTimeEmployee): Count = {fullTimeStaff.Count}");

        var result = OperationResult<Employee>.Success(
            topEarner!,
            "Successfully fetched top corporate earner via generic wrapper.");
        Console.WriteLine($"  OperationResult<T>: {result}");

        // -------------------------------------------------------------------------
        // SECTION 3: LAMBDAS & DELEGATES
        // -------------------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[10] Lambda Expressions & Delegates (Func<T>, Action<T>, Predicate<T>)");
        Console.ResetColor();

        // Func<T, TResult>: Takes an employee, returns calculated gross salary
        Func<Employee, decimal> salarySelector = emp => emp.CalculateGrossPay();
        Console.WriteLine($"  Func<Employee, decimal> on ID 101 ({employees[0].FullName}): {salarySelector(employees[0]):C}");

        // Predicate<T>: Returns true if employee is contract
        Predicate<Employee> isContractPredicate = emp => emp is ContractEmployee;
        int contractorCount = employees.Count(e => isContractPredicate(e));
        Console.WriteLine($"  Predicate<Employee> (is ContractEmployee): Matching count = {contractorCount}");

        // Custom Delegate: Adjust compensation by rating
        CompensationAdjustmentDelegate adjuster = (basePay, rating) => basePay * (decimal)(1.0 + (rating * 0.05));
        decimal adjustedSample = adjuster(85000m, 2.0); // 10% raise
        Console.WriteLine($"  Custom Delegate (Base: $85,000, Rating: 2.0 -> +10%): {adjustedSample:C}");

        // -------------------------------------------------------------------------
        // SECTION 4: EVENTS & PUBLISHER / SUBSCRIBER
        // -------------------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[11] Events (Publisher/Subscriber Pattern)");
        Console.ResetColor();
        Console.WriteLine("  Subscribing to EmployeeAdded and SalaryChanged events on EmployeeService...");

        // Local event handler methods (Subscribers)
        void OnEmployeeAddedHandler(object? sender, EmployeeEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [EVENT NOTIFICATION] Employee Enrolled -> {e.Employee.FullName} ({e.Employee.Department.Name}) at {e.EventTimestamp:HH:mm:ss} UTC");
            Console.ResetColor();
        }

        void OnSalaryChangedHandler(object? sender, SalaryChangedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"  [EVENT NOTIFICATION] Salary Adjusted -> {e.Employee.FullName} from {e.OldSalary:C} to {e.NewSalary:C}");
            Console.ResetColor();
        }

        // Subscribe
        employeeService.EmployeeAdded += OnEmployeeAddedHandler;
        employeeService.SalaryChanged += OnSalaryChangedHandler;

        // Trigger an event by adding a temporary demonstration employee
        var demoEmployee = new FullTimeEmployee(
            999, "Vikrant", "Gupta", null, Department.FromName("Engineering"),
            92000m, 6000m, 32, 8, DateTime.Today);
        employeeService.AddEmployee(demoEmployee, out _);

        // Trigger second event by updating salary
        employeeService.UpdateEmployeeSalary(999, 98000m, out _);

        // Unsubscribe to prevent memory leaks in production
        employeeService.EmployeeAdded -= OnEmployeeAddedHandler;
        employeeService.SalaryChanged -= OnSalaryChangedHandler;

        // -------------------------------------------------------------------------
        // SECTION 5: DEFERRED EXECUTION
        // -------------------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[12] Deferred Execution vs. Immediate Execution (ToList)");
        Console.ResetColor();
        var testList = new List<Employee>
        {
            new FullTimeEmployee(801, "Test User 1", "Alpha", null, Department.FromName("Finance"), 60000m, 0m, 25, 2, DateTime.Today),
            new FullTimeEmployee(802, "Test User 2", "Beta", null, Department.FromName("Finance"), 70000m, 0m, 27, 4, DateTime.Today)
        };

        // Step 1: Define query (Deferred - NOT executed yet!)
        var deferredQuery = testList.Where(e => e.CalculateGrossPay() >= 65000m);
        // Step 2: Immediate materialization via ToList
        var materializedList = testList.Where(e => e.CalculateGrossPay() >= 65000m).ToList();

        Console.WriteLine($"  Initial state -> Deferred count: {deferredQuery.Count()} | Materialized list count: {materializedList.Count}");

        // Step 3: Add new element AFTER query was defined
        testList.Add(new FullTimeEmployee(803, "Test User 3 (Added Later)", "Gamma", null, Department.FromName("Finance"), 85000m, 0m, 30, 6, DateTime.Today));
        Console.WriteLine("  Added 'Test User 3' (Gross: $85,000) to underlying collection.");

        // Step 4: Re-evaluate both
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  After Addition -> Deferred query count : {deferredQuery.Count()} (Dynamically re-evaluated!)");
        Console.WriteLine($"  After Addition -> Materialized list count: {materializedList.Count} (Remained static snapshot!)");
        Console.ResetColor();

        // -------------------------------------------------------------------------
        // SECTION 6: EXTENSION METHODS
        // -------------------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[13] Extension Methods (EmployeeExtensions)");
        Console.ResetColor();
        var sampleEmp = employees[0];
        Console.WriteLine($"  employee.IsHighEarner(80000): {sampleEmp.IsHighEarner(80000m)}");
        Console.WriteLine($"  employee.ToFormattedString(): {sampleEmp.ToFormattedString()}");
        Console.WriteLine($"  employees.TotalGrossPayroll(): {employees.TotalGrossPayroll():C}");
        Console.WriteLine($"  employees.AverageGrossSalary(): {employees.AverageGrossSalary():C}");

        // -------------------------------------------------------------------------
        // SECTION 7: PATTERN MATCHING
        // -------------------------------------------------------------------------
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[14] Modern C# Pattern Matching (Property, Type & Switch Patterns)");
        Console.ResetColor();
        foreach (var emp in employees.Take(4))
        {
            string tier = EmployeePatternMatcher.ClassifyEmployeeTier(emp);
            var (category, budgetCost) = EmployeePatternMatcher.EvaluateBudgetImpact(emp);
            Console.WriteLine($"  • {emp.FullName}:");
            Console.WriteLine($"    Tier   : {tier}");
            Console.WriteLine($"    Budget : {category} (Projected Annual Cost: {budgetCost:C})");
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================================================");
        Console.ResetColor();
    }
}
