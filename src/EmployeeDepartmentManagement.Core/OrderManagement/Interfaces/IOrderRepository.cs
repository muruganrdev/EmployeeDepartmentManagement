using EmployeeDepartmentManagement.Core.OrderManagement.Models;

namespace EmployeeDepartmentManagement.Core.OrderManagement.Interfaces;

/// <summary>
/// Data access contract for Order persistence operations.
/// Demonstrates the Repository Pattern and Dependency Inversion Principle (DIP).
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Persists a new order into the data store.
    /// </summary>
    void Add(Order order);

    /// <summary>
    /// Retrieves an order by its unique identifier, or null if not found.
    /// </summary>
    Order? GetById(int orderId);

    /// <summary>
    /// Retrieves all stored orders.
    /// </summary>
    IReadOnlyList<Order> GetAll();

    /// <summary>
    /// Checks whether an order with the given ID exists.
    /// </summary>
    bool Exists(int orderId);
}
