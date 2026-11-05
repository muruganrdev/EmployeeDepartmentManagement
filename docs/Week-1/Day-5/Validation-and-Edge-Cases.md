# Validation and Edge Cases Guide

## 1. Overview of Domain Invariants and Validation Rules

In production software, the system must remain reliable under all conditions. It must never crash, corrupt data, or throw uncaught exceptions when provided with invalid, unexpected, or boundary inputs.

---

## 2. Matrix of Tested Inputs and Edge Cases

| Input Category | Scenario Tested | Input Value | Expected System Response | Test Verification |
| :--- | :--- | :--- | :--- | :--- |
| **Null Reference** | Null Employee in Validation | `emp = null` | Returns `(false, "Employee instance cannot be null.")` | `ValidateEmployee_NullEmployee_...` |
| **Null Reference** | Null Employee in Enrollment | `emp = null` | Returns `false`, `error = "Cannot enroll a null employee."` | `AddEmployee_NullEmployee_...` |
| **Duplicate Record** | Duplicate ID Collision | `emp.Id = 101` (Aarav exists) | Returns `false`, `error = "An employee with ID 101 already exists."` | `AddEmployee_DuplicateId_...` |
| **Non-Existing ID** | Query for unassigned ID | `id = 99999` | Returns `null` safely without `KeyNotFoundException` | `GetEmployeeById_NonExistentId_...` |
| **Negative Number** | Negative Compensation Update | `newSalary = -15000m` | Returns `false`, keeps existing salary intact | `UpdateEmployeeSalary_Negative...` |
| **Zero Value** | Zero Compensation Update | `newSalary = 0m` | Returns `false`, `error = "greater than zero"` | `UpdateEmployeeSalary_Zero...` |
| **Boundary Value** | Minimum Allowed Working Age | `age = 18` | Allowed; employee successfully created | `Employee_AgeEncapsulation_...` |
| **Boundary Value** | Maximum Allowed Working Age | `age = 75` | Allowed; employee successfully created | `Employee_AgeEncapsulation_...` |
| **Out-of-Bounds** | Underage Candidate | `age = 17` | Encapsulation throws `ArgumentOutOfRangeException` | `Employee_AgeEncapsulation_...` |
| **Out-of-Bounds** | Overage Candidate | `age = 76` | Encapsulation throws `ArgumentOutOfRangeException` | `Employee_AgeEncapsulation_...` |
| **Temporal Edge** | Future Joining Date | `joiningDate = Today + 7` | Returns `false`, `error = "Date cannot be in the future"` | `AddEmployee_FutureJoiningDate_...` |
| **Relational Boundary**| Career start at 16 boundary | `age = 18, exp = 2` | Allowed ($18 - 16 = 2$, valid boundary) | `ValidateEmployee_EighteenYearOld...` |
| **String Whitespace** | Name search with surrounding spaces | `"   aarav   "` | Automatically trimmed; matches `Aarav Sharma` | `SearchEmployeesByName_QueryWith...` |
| **Empty String** | Empty / whitespace search query | `""` or `"   "` | Returns empty `List<Employee>` without crashing | `SearchEmployees_NullOrWhitespace...` |
| **Empty Collection** | Empty input to LINQ Query | `new List<Employee>()` | Returns `null` for `MaxBy`, 0 for counts | `GetHighestPaidEmployee_Empty...` |

---

## 3. How the UI Layer Protects Against Invalid Console Input

In [Program.cs](file:///C:/Local%20Disk%20D/Murugan%20Project/Projects/test/EmployeeDepartmentManagement/src/EmployeeDepartmentManagement.App/Program.cs), defensive helper methods prevent format exceptions before business services are ever called:

1. `ReadPositiveInt(prompt)`: Loops until the user enters an integer $> 0$.
2. `ReadPositiveDecimal(prompt)`: Loops until the user enters a decimal number $> 0$.
3. `ReadDepartment()`: Validates that the entered department name is strictly contained within `Department.ValidDepartments`.
4. `ReadPastOrTodayDate(prompt)`: Enforces that the date is formatted properly (`yyyy-MM-dd`) and not in the future.
