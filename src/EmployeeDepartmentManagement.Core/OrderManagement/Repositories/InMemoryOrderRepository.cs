using EmployeeDepartmentManagement.Core.OrderManagement.Interfaces;
using EmployeeDepartmentManagement.Core.OrderManagement.Models;

namespace EmployeeDepartmentManagement.Core.OrderManagement.Repositories;

/// <summary>
/// In-memory implementation of IOrderRepository using a Dictionary for O(1) lookups.
/// Demonstrates the Repository Pattern and satisfies the IOrderRepository contract.
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _orders = new();

    public void Add(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (_orders.ContainsKey(order.OrderId))
            throw new InvalidOperationException($"An order with ID {order.OrderId} already exists.");

        _orders.Add(order.OrderId, order);
    }

    public Order? GetById(int orderId)
    {
        if (_orders.TryGetValue(orderId, out Order? order))
            return order;
        return null;
    }

    public IReadOnlyList<Order> GetAll() => _orders.Values.ToList().AsReadOnly();

    public bool Exists(int orderId) => _orders.ContainsKey(orderId);
}
