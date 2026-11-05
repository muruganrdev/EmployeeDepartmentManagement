# Practical Debugging Guide – Step-by-Step Defect Resolution

## 1. Defect Report & Scenario

### The User Complaint (Bug Ticket HR-1042):
> **Title:** Valid young professional (Age: 18, Experience: 2 years) rejected during onboarding registration.  
> **Severity:** Medium (Blocks valid new-hire enrollment).  
> **Reported Behavior:** HR attempted to register an 18-year-old developer who started technical training/internships at age 16 (2 years of experience). The system unexpectedly rejected the candidate with the error:  
> *"Years of Experience must be realistic for the given age."*

---

## 2. Step-by-Step Debugging Investigation

### Step 1: Understand the Reported Issue
The business rule in the requirements specification states:
> *"Years of experience must be realistic for the candidate's age, under the reasonable assumption that formal technical career experience cannot begin before age 16."*

Mathematically:
- Maximum possible experience for age $A$ is: $MaxExp = A - 16$.
- If $A = 18$, then $MaxExp = 18 - 16 = 2$.
- The candidate has exactly 2 years of experience.
- Since $2 \le 2$, the candidate's experience is realistic and should be accepted!

### Step 2: Reproduce the Issue with a Test
Before touching any production code, we wrote a test case in our test suite to reproduce the failure:

```csharp
[Fact]
public void ValidateEmployee_EighteenYearOldWithTwoYearsExperience_ReturnsTrue()
{
    var candidate = new FullTimeEmployee(
        id: 887, firstName: "Young", lastName: "Professional", middleName: null,
        department: Department.FromName("Engineering"), baseSalary: 60000m,
        annualBonus: null, age: 18, yearsOfExperience: 2, joiningDate: DateTime.Today
    );

    var (isValid, errorMessage) = _sut.ValidateEmployee(candidate);

    Assert.True(isValid); // FAILED here! It returned false!
}
```

### Step 3: Identify the Affected Method
By searching for the error message `"Years of Experience must be realistic for the given age."`, we located the method:
`EmployeeService.ValidateEmployee(Employee emp)` in [EmployeeService.cs](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Services/EmployeeService.cs).

### Step 4 & 5: Debugging with Breakpoints and Inspecting Variables
We set a breakpoint at the validation line:
```csharp
if (emp.YearsOfExperience >= emp.Age - 16) // Breakpoint placed here
```

When execution pauses at the breakpoint:
1. Open the **Locals** or **Watch** window in Visual Studio / VS Code.
2. Inspect the values:
   - `emp.Age` = `18`
   - `emp.YearsOfExperience` = `2`
   - Expression `emp.Age - 16` = `2`
   - Condition `emp.YearsOfExperience >= emp.Age - 16` -> evaluates to `2 >= 2` -> **`true`**!

### Step 6: Identify the Root Cause
The developer who originally authored this line used the **greater-than-or-equal-to** operator (`>=`) instead of the **greater-than** operator (`>`).
Because $2 \ge 2$ evaluated to true, the code treated a valid boundary case as an error!

### Step 7: Apply the Fix
Change `>=` to `>`:
```csharp
// Before:
if (emp.YearsOfExperience < 0 || emp.YearsOfExperience >= emp.Age - 16)

// After (Fixed):
if (emp.YearsOfExperience < 0 || emp.YearsOfExperience > emp.Age - 16)
```

### Step 8: Verify the Regression Test
Re-run the test:
```powershell
dotnet test --filter "FullyQualifiedName~ValidateEmployee_EighteenYearOldWithTwoYearsExperience_ReturnsTrue"
```
**Outcome:** The test turns **GREEN**!

### Step 9: Run Full Solution Test Suite
Execute all tests to confirm zero side effects on other validations:
```powershell
dotnet test --configuration Release
```
**Outcome:** All 83 tests pass!
