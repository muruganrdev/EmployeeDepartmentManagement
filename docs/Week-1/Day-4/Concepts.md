# Week 1 – Day 4: Unit Testing, Async/Await and CancellationToken – Core Concepts

## 1. Topic A: Unit Testing Fundamentals

### 1.1 What is Software Testing?
Software testing is the systematic process of executing software components to verify whether they behave as expected and satisfy functional, business, and non-functional requirements.

In software development, defects discovered in production are orders of magnitude more expensive to fix than defects caught during local development. Automated testing acts as a safety harness, preventing regressions when adding new features or refactoring legacy code.

### 1.2 What is Unit Testing?
A **Unit Test** is an automated piece of code that invokes an isolated "unit" of production code (typically a single method, class, or algorithm) and checks a specific assumption about its behavior.

- **Scope:** Isolated, small, fast.
- **Speed:** Executes in milliseconds (e.g., in memory without network or disk lag).
- **Independence:** Does not depend on external databases, web APIs, or file systems.
- **Repeatable:** Produces the exact same result every time it runs.

### 1.3 Unit Testing vs. Integration Testing
| Feature | Unit Testing | Integration Testing |
| :--- | :--- | :--- |
| **Scope** | Single method or class in total isolation. | Multiple classes, database, file system, or network working together. |
| **Execution Time** | Microseconds to milliseconds. | Hundreds of milliseconds to minutes. |
| **Failure Diagnosis** | Pinpoints the exact line of code that failed. | Pinpoints that an interaction failed, but root cause requires debugging. |
| **Dependencies** | External dependencies are mocked or isolated. | Uses real databases, web servers, or message brokers. |
| **Analogy** | Testing a car spark plug on a test bench. | Starting the assembled car engine and testing transmission. |

### 1.4 What is xUnit?
**xUnit.net** is the modern, open-source unit testing framework for .NET applications. It is the testing framework used by the official .NET runtime and ASP.NET Core teams at Microsoft.

Key xUnit design philosophies:
1. **Isolation by default:** xUnit creates a *new instance* of the test class for *every single test method* execution. This guarantees zero state leakage between tests.
2. **Clean attributes:** Replaces legacy `[Test]` with `[Fact]` (for parameterless tests) and `[Theory]` (for parameterized tests).
3. **No `[SetUp]` and `[TearDown]`:** Standard C# constructors handle test setup, and `IDisposable.Dispose()` handles teardown.

### 1.5 What is a Test Case?
A test case is a specific scenario with:
- Known inputs and preconditions.
- An action under test.
- An expected outcome or assertion.

Example in Employee Management:
> *Given an existing employee with ID 101 earning $85,000, when `UpdateEmployeeSalary(101, -5000)` is called, then the update should return `false` and the employee's salary should remain $85,000.*

### 1.6 What are Assertions?
An assertion is a boolean check that verifies whether the actual outcome of code execution matches the expected outcome. If an assertion evaluates to false, the testing engine throws an assertion exception, and the test is marked as **FAILED**.

### 1.7 What is the Arrange-Act-Assert (AAA) Pattern?
AAA is the universal standard for structuring unit tests into three visually distinct blocks:
1. **Arrange:** Set up all prerequisites, test objects, mock expectations, and input parameters.
2. **Act:** Execute the specific method being tested (typically a single line of code).
3. **Assert:** Verify that the output, state change, or exceptions match what was expected.

### 1.8 What is Test Isolation?
Test isolation ensures that:
- Tests can run in any order.
- Tests can run concurrently in parallel threads without interfering with each other.
- Test A modifying a list or record does not affect Test B.

In `EmployeeServiceTests`, `_sut = new EmployeeService();` runs inside the constructor, so every test receives its own fresh in-memory collection.

### 1.9 What is Mocking, and When Should We Use It?
**Mocking** is the practice of replacing a real external dependency (like a SQL database, remote REST API, or email server) with a simulated object (a **test double**) that mimics the behavior of the real dependency.

**Why use Mocking?**
1. **Speed:** Database connections take 50ms; a mock takes 0.001ms.
2. **Determinism:** Remote web servers can go offline, causing flaky tests.
3. **Fault Simulation:** Easily test rare edge cases (e.g., simulate a network timeout or HTTP 500 error).

**Employee Management Example:**
```csharp
var mockRepo = new Mock<IEmployeeService>();
mockRepo.Setup(r => r.GetEmployeeById(501)).Returns(new FullTimeEmployee(...));
var service = new EmployeeAsyncService(mockRepo.Object);
var emp = await service.GetEmployeeByIdAsync(501);
Assert.NotNull(emp);
mockRepo.Verify(r => r.GetEmployeeById(501), Times.Once);
```

---

## 2. Topic B: xUnit Attributes and Assertions

### Attributes
- `[Fact]`: Marks a normal test method that requires no parameters and runs once.
- `[Theory]`: Marks a data-driven test method that runs multiple times with different datasets.
- `[InlineData(arg1, arg2)]`: Supplies literal values to a `[Theory]` method parameters.

### Core Assertions
| Assertion | Purpose | Example |
| :--- | :--- | :--- |
| `Assert.Equal(expected, actual)` | Verifies value equality. | `Assert.Equal(85000m, emp.BaseSalary);` |
| `Assert.NotEqual(notExpected, actual)` | Verifies values are different. | `Assert.NotEqual(0m, emp.CalculateGrossPay());` |
| `Assert.True(condition)` | Verifies boolean condition is true. | `Assert.True(service.AddEmployee(emp, out _));` |
| `Assert.False(condition)` | Verifies boolean condition is false. | `Assert.False(service.UpdateEmployeeSalary(101, -100m, out _));` |
| `Assert.Null(object)` | Verifies reference is null. | `Assert.Null(service.GetEmployeeById(99999));` |
| `Assert.NotNull(object)` | Verifies reference is not null. | `Assert.NotNull(service.GetEmployeeById(101));` |
| `Assert.Throws<TException>(action)` | Verifies that executing `action` throws `TException`. | `Assert.Throws<ArgumentOutOfRangeException>(() => emp.Age = 16);` |
| `Assert.Contains(substring, fullString)` | Verifies substring exists inside collection or string. | `Assert.Contains("Engineering", report);` |
| `Assert.Empty(collection)` | Verifies collection contains zero elements. | `Assert.Empty(service.SearchEmployees("Unknown"));` |

---

## 3. Topic C: Async/Await in .NET

### 3.1 Synchronous vs. Asynchronous Programming
- **Synchronous Execution:** Code executes sequentially. If a method reads from disk or calls a web API, the operating system thread is placed in a sleep/wait state, doing zero useful work while waiting for the hardware or network response.
- **Asynchronous Execution:** When an I/O operation begins, the calling thread is released back to the thread pool to process other user requests. When the I/O completes (via OS completion port hardware interrupt), a thread resumes execution of the remainder of the method.

### 3.2 Real-World Restaurant Analogy
- **Synchronous Waiter:** The waiter takes Order 1 to the kitchen and stands staring at the chef for 20 minutes until the meal is cooked before serving and taking Order 2. (Result: The restaurant freezes).
- **Asynchronous Waiter:** The waiter sends Order 1 to the kitchen, immediately attends to Tables 2, 3, and 4, and returns to Table 1 only when the kitchen bell dings (`await`).

### 3.3 What are `Task` and `Task<T>`?
- `Task`: Represents an ongoing asynchronous operation that does not return a value (like an asynchronous `void`).
- `Task<T>`: Represents an ongoing asynchronous operation that will eventually produce a value of type `T` (like an asynchronous `return T`).

### 3.4 What do `async` and `await` Mean?
- `async`: A method modifier that informs the C# compiler that the method contains asynchronous expressions. The compiler rewrites the method into an optimized **state machine** struct behind the scenes.
- `await`: An operator applied to an awaitable task. It pauses the current method's execution and releases the thread. When the awaited task completes, execution resumes at the next line.

### 3.5 I/O-Bound vs. CPU-Bound Work
- **I/O-Bound Work:** Waiting for external devices (disk files, SQL database queries, HTTP network calls).
  *Rule:* Use `async`/`await` with natural asynchronous APIs (`Stream.WriteAsync`, `HttpClient.GetAsync`). **Do NOT use `Task.Run()` for I/O!**
- **CPU-Bound Work:** Heavy computational algorithms (image processing, cryptographic hashing, matrix math).
  *Rule:* Use `Task.Run()` to offload intense calculations to background ThreadPool threads if needed to keep UI threads responsive.

### 3.6 Why Avoid `.Result` and `.Wait()`?
Calling `task.Result` or `task.Wait()` on an incomplete task forces the thread to block synchronously:
1. **Thread Pool Starvation:** Wastes a thread that could be serving other users.
2. **Deadlocks:** In ASP.NET or WPF/WinForms environments with a synchronization context, blocking synchronously while waiting for a continuation can lead to fatal unrecoverable deadlocks.

---

## 4. Topic D: CancellationToken and Cooperative Cancellation

### 4.1 What is a CancellationToken?
A `CancellationToken` is a lightweight struct passed into asynchronous methods to inform them whether the operation should be cancelled before completing.

### 4.2 Why is Cancellation "Cooperative"?
In modern operating systems and runtimes, killing a thread abruptly (like legacy `Thread.Abort()` in .NET Framework) is dangerous—it can leave open file handles, corrupt database transactions, or lock memory structures.

Instead, .NET uses **Cooperative Cancellation**:
1. The **caller** signals a request: *"Please stop when convenient."*
2. The **worker** periodically checks the token: *"Has cancellation been requested?"*
3. If yes, the worker cleanly aborts its work, releases resources (e.g., closes streams), and throws `OperationCanceledException`.

### 4.3 CancellationTokenSource (CTS)
- `CancellationTokenSource`: The **owner** and generator of the cancellation signal.
  - Can cancel manually: `cts.Cancel()`.
  - Can cancel on timeout: `cts.CancelAfter(TimeSpan.FromSeconds(5))`.
- `cts.Token`: The lightweight token distributed to child methods.

### 4.4 Real-World Web API Example
When a user searches for an employee report on a browser dashboard and closes the browser tab before the report finishes rendering:
- The web server detects the disconnected TCP socket.
- ASP.NET Core signals the `HttpContext.RequestAborted` CancellationToken.
- The controller and database query cancel immediately, saving server CPU, RAM, and database I/O!
