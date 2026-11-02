namespace EmployeeDepartmentManagement.Core.ValueObjects;

/// <summary>
/// Value object representing employee work hours.
/// Demonstrates C# readonly struct, value-type semantics,
/// and encapsulation with self-validation.
/// </summary>
public readonly struct WorkHours
{
    public int RegularHours { get; }
    public int OvertimeHours { get; }

    public int TotalHours => RegularHours + OvertimeHours;

    public WorkHours(int regularHours, int overtimeHours = 0)
    {
        if (regularHours < 0 || regularHours > 240)
            throw new ArgumentOutOfRangeException(nameof(regularHours), "Regular hours must be between 0 and 240 hours per month.");
        if (overtimeHours < 0 || overtimeHours > 100)
            throw new ArgumentOutOfRangeException(nameof(overtimeHours), "Overtime hours must be between 0 and 100 hours per month.");

        RegularHours = regularHours;
        OvertimeHours = overtimeHours;
    }

    public override string ToString() => $"{TotalHours} hrs (Regular: {RegularHours}, Overtime: {OvertimeHours})";
}
