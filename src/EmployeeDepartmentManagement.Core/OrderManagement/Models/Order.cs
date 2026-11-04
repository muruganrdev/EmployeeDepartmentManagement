namespace EmployeeDepartmentManagement.Core.OrderManagement.Models;

/// <summary>
/// Domain Aggregate Root representing a customer purchase order.
/// Demonstrates:
/// - Composition: Order *has-many* OrderItems (Order owns OrderItem lifecycle)
/// - Association: Order *references* Customer
/// - Invariant validation & Total aggregation
/// </summary>
public class Order
{
    private int _orderId;
    private Customer _customer = null!;
    private readonly List<OrderItem> _orderItems = new();
    private DateTime _orderDate = DateTime.UtcNow;

    /// <summary>
    /// Unique identifier for the order.
    /// </summary>
    public int OrderId
    {
        get => _orderId;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "OrderId must be a positive integer greater than zero.");
            _orderId = value;
        }
    }

    /// <summary>
    /// The customer who placed this order.
    /// </summary>
    public Customer Customer
    {
        get => _customer;
        set => _customer = value ?? throw new ArgumentNullException(nameof(value), "Customer cannot be null.");
    }

    /// <summary>
    /// Timestamp when the order was registered.
    /// </summary>
    public DateTime OrderDate
    {
        get => _orderDate;
        set => _orderDate = value;
    }

    /// <summary>
    /// Read-only snapshot of line items belonging to this order.
    /// External callers cannot directly modify the internal list without using AddOrderItem.
    /// </summary>
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Order()
    {
    }

    public Order(int orderId, Customer customer, DateTime? orderDate = null)
    {
        OrderId = orderId;
        Customer = customer;
        OrderDate = orderDate ?? DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a line item to the order, encapsulating list mutation and duplicate item checks.
    /// </summary>
    public void AddOrderItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        // If product already in order, increment quantity
        var existing = _orderItems.Find(i => i.Product.ProductId == item.Product.ProductId);
        if (existing != null)
        {
            existing.Quantity += item.Quantity;
        }
        else
        {
            _orderItems.Add(item);
        }
    }

    /// <summary>
    /// Convenience helper to construct and add an OrderItem directly.
    /// </summary>
    public OrderItem AddItem(Product product, int quantity)
    {
        int nextItemId = _orderItems.Count + 1;
        var item = new OrderItem(nextItemId, product, quantity);
        AddOrderItem(item);
        return item;
    }

    /// <summary>
    /// Calculates the grand total for the order by summing all line item subtotals.
    /// </summary>
    public decimal CalculateTotal()
    {
        decimal total = 0m;
        for (int i = 0; i < _orderItems.Count; i++)
        {
            total += _orderItems[i].CalculateSubtotal();
        }
        return total;
    }

    public override string ToString() =>
        $"[Order #{OrderId}] Date: {OrderDate:yyyy-MM-dd HH:mm} | Customer: {Customer.Name} | Items: {_orderItems.Count} | Total: {CalculateTotal():C}";
}
