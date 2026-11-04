using EmployeeDepartmentManagement.Core.OrderManagement.Interfaces;
using EmployeeDepartmentManagement.Core.OrderManagement.Models;

namespace EmployeeDepartmentManagement.Core.OrderManagement.Services;

/// <summary>
/// Business service orchestrating order lifecycle, inventory stock checks, and persistence.
/// Demonstrates:
/// - Dependency Inversion Principle (DIP): Depends on IOrderRepository abstraction
/// - Single Responsibility Principle (SRP): Focuses purely on order domain orchestration
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private int _nextOrderId = 1001;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    /// <summary>
    /// Creates and persists an order, enforcing customer validation, item validation, and stock deduction.
    /// </summary>
    public Order CreateOrder(Customer customer, IEnumerable<(Product Product, int Quantity)> items)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(items);

        var itemList = items.ToList();
        if (itemList.Count == 0)
            throw new ArgumentException("An order must contain at least one product item.", nameof(items));

        // Step 1: Pre-validate stock availability for all items before making any modifications
        foreach (var (product, quantity) in itemList)
        {
            ArgumentNullException.ThrowIfNull(product);
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(items), $"Order quantity for '{product.Name}' must be greater than zero.");
            if (quantity > product.AvailableStock)
                throw new InvalidOperationException($"Insufficient inventory for '{product.Name}'. Requested: {quantity}, In Stock: {product.AvailableStock}.");
        }

        // Step 2: Deduct inventory stock
        foreach (var (product, quantity) in itemList)
        {
            product.DeductStock(quantity);
        }

        // Step 3: Construct the Order and add line items
        var order = new Order(_nextOrderId++, customer, DateTime.UtcNow);
        foreach (var (product, quantity) in itemList)
        {
            order.AddItem(product, quantity);
        }

        // Step 4: Persist through repository
        _orderRepository.Add(order);

        return order;
    }

    /// <summary>
    /// Retrieves an order by its unique ID.
    /// </summary>
    public Order? GetOrderById(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentOutOfRangeException(nameof(orderId), "Order ID must be positive.");
        return _orderRepository.GetById(orderId);
    }

    /// <summary>
    /// Returns all registered orders.
    /// </summary>
    public IReadOnlyList<Order> GetAllOrders()
    {
        return _orderRepository.GetAll();
    }

    /// <summary>
    /// Calculates the total for the specified order.
    /// </summary>
    public decimal CalculateOrderTotal(int orderId)
    {
        var order = GetOrderById(orderId) ?? throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
        return order.CalculateTotal();
    }
}
