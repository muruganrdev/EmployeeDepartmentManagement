namespace EmployeeDepartmentManagement.Core.Common;

/// <summary>
/// Demonstrates Generic Programming in C#.
/// Provides reusable collection filtering and search algorithms that work across ANY entity type.
/// </summary>
/// <typeparam name="T">The entity type operated on (e.g., Employee, Product, Customer).</typeparam>
public class GenericFilter<T> where T : class
{
    /// <summary>
    /// Filters a source collection using a generic predicate delegate Func&lt;T, bool&gt;.
    /// </summary>
    public IEnumerable<T> Filter(IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (T item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Finds the first element satisfying the predicate, or null if none match.
    /// </summary>
    public T? FindFirst(IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (T item in source)
        {
            if (predicate(item))
            {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Counts how many elements satisfy the predicate delegate.
    /// </summary>
    public int CountMatching(IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        int count = 0;
        foreach (T item in source)
        {
            if (predicate(item))
            {
                count++;
            }
        }

        return count;
    }
}

/// <summary>
/// Generic result wrapper demonstrating type-safe outcomes without boxing.
/// </summary>
/// <typeparam name="T">Payload data type.</typeparam>
public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string Message { get; }
    public string? ErrorCode { get; }

    private OperationResult(bool isSuccess, T? data, string message, string? errorCode = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        ErrorCode = errorCode;
    }

    public static OperationResult<T> Success(T data, string message = "Operation completed successfully.") =>
        new(true, data, message);

    public static OperationResult<T> Failure(string message, string? errorCode = null) =>
        new(false, default, message, errorCode);

    public override string ToString() =>
        IsSuccess ? $"[Success] {Message} (Data: {Data})" : $"[Failure] {Message} (Code: {ErrorCode})";
}
