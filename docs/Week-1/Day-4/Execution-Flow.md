# Execution Flow – Day 4 Unit Testing and Async Operations

## 1. Multi-Project Reference Architecture

The solution uses a clean three-tier project topology separating domain contracts, console presentation, and automated testing:

```mermaid
graph TD
    subgraph Solution: EmployeeDepartmentManagement.sln
        Core["EmployeeDepartmentManagement.Core<br/>(Domain Models, Interfaces, ValueObjects, Services)"]
        App["EmployeeDepartmentManagement.App<br/>(Console Presentation, CLI Menus, Services)"]
        Tests["EmployeeDepartmentManagement.Tests<br/>(xUnit Test Suite, Moq, Assertions)"]
    end

    App -->|ProjectReference| Core
    Tests -->|ProjectReference| Core
    Tests -->|ProjectReference| App
```

- **`Core`**: Contains zero dependencies on UI or testing libraries. Pure domain logic, models, and interfaces (`IEmployeeService`, `IEmployeeAsyncService`, `EmployeeAsyncService`).
- **`App`**: References `Core`. Implements concrete console presentation, interactive demonstration handlers, and startup routines.
- **`Tests`**: References both `Core` and `App`. Discovers all public interfaces and classes for automated unit testing.

---

## 2. Unit Test Execution Lifecycle (xUnit Engine)

```mermaid
sequenceDiagram
    autonumber
    actor Developer as Developer / CI Pipeline
    participant Runner as xUnit Test Runner (vstest.console)
    participant TestClass as EmployeeServiceTests
    participant SUT as EmployeeService (System Under Test)
    participant AssertEngine as Xunit.Assert

    Developer->>Runner: Execute 'dotnet test'
    Runner->>Runner: Discover all [Fact] and [Theory] methods via reflection
    
    rect rgb(240, 248, 255)
    Note over Runner,TestClass: Test Method 1 Execution
    Runner->>TestClass: Instantiate new EmployeeServiceTests() (Isolation)
    TestClass->>SUT: new EmployeeService() (Seeds fresh test data)
    TestClass->>TestClass: ARRANGE: Instantiate new FullTimeEmployee(...)
    TestClass->>SUT: ACT: AddEmployee(emp, out errorMessage)
    SUT-->>TestClass: Returns true, errorMessage = ""
    TestClass->>AssertEngine: ASSERT: Assert.True(result), Assert.Empty(errorMessage)
    AssertEngine-->>TestClass: Passes successfully
    Runner->>TestClass: Dispose() if IDisposable implemented
    end

    rect rgb(255, 250, 240)
    Note over Runner,TestClass: Test Method 2 Execution (Clean Instance)
    Runner->>TestClass: Instantiate NEW EmployeeServiceTests() (Zero shared state!)
    TestClass->>SUT: new EmployeeService()
    TestClass->>SUT: ACT: UpdateEmployeeSalary(101, -5000m, out errorMessage)
    SUT-->>TestClass: Returns false, errorMessage = "must be greater than zero"
    TestClass->>AssertEngine: ASSERT: Assert.False(result), Assert.NotEmpty(errorMessage)
    AssertEngine-->>TestClass: Passes successfully
    end

    Runner-->>Developer: Test Run Summary: 60 Passed, 0 Failed, 0 Skipped
```

---

## 3. How Async/Await Flows from Test through Service to Stream

```mermaid
sequenceDiagram
    autonumber
    actor TestRunner as xUnit Async Test Runner
    participant AsyncTest as EmployeeAsyncServiceTests
    participant AsyncSvc as EmployeeAsyncService
    participant StreamWriter as System.IO.StreamWriter
    participant MemStream as System.IO.MemoryStream

    TestRunner->>AsyncTest: Invoke ExportPayrollReportAsync_WritableMemoryStream_WritesCsvAndReturnsBytes()
    AsyncTest->>MemStream: new MemoryStream()
    AsyncTest->>AsyncSvc: await ExportPayrollReportAsync(memoryStream, token)
    
    AsyncSvc->>AsyncSvc: token.ThrowIfCancellationRequested()
    AsyncSvc->>StreamWriter: new StreamWriter(memoryStream, leaveOpen: true)
    
    AsyncSvc->>StreamWriter: await writer.WriteLineAsync(header.AsMemory(), token)
    StreamWriter->>MemStream: Writes UTF-8 bytes to buffer
    
    loop For each employee record
        AsyncSvc->>AsyncSvc: token.ThrowIfCancellationRequested()
        AsyncSvc->>StreamWriter: await writer.WriteLineAsync(csvLine.AsMemory(), token)
        StreamWriter->>MemStream: Writes bytes
    end

    AsyncSvc->>StreamWriter: await writer.FlushAsync(token)
    StreamWriter->>MemStream: Flushes buffer to underlying stream
    
    AsyncSvc-->>AsyncTest: Returns total bytes written (e.g. 348 bytes)
    AsyncTest->>AsyncTest: Assert.True(bytesWritten > 0)
    AsyncTest-->>TestRunner: Test Completed Successfully
```
