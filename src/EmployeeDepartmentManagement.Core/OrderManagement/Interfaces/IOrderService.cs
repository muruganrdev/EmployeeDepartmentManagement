using EmployeeDepartmentManagement.Core.OrderManagement.Models;

namespace EmployeeDepartmentManagement.Core.OrderManagement.Interfaces;

/// <summary>
/// Service contract defining business operations for order orchestration.
/// Demonstrates Interface-Based Design and decoupling of callers from business implementations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Coordinates customer order creation, inventory stock deduction, and persistence.
    /// </summary>
    Order CreateOrder(Customer customer, IEnumerable<(Product Product, int Quantity)> items);

    /// <summary>
    /// Retrieves an order by its unique ID.
    /// </summary>
    Order? GetOrderById(int orderId);

    /// <summary>
    /// Retrieves all orders recorded in the system.
    /// </summary>
    IReadOnlyList<Order> GetAllOrders();

    /// <summary>
    /// Computes the total monetary value for a given order.
    /// </summary>
    decimal CalculateOrderTotal(int orderId);
}
