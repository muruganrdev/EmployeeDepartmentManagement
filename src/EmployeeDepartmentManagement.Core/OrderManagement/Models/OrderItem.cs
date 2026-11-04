namespace EmployeeDepartmentManagement.Core.OrderManagement.Models;

/// <summary>
/// Domain line item inside an Order.
/// Demonstrates:
/// - Association with Product (references Product entity)
/// - Historical snapshot pricing (UnitPrice captured independently of current Product.Price)
/// - Encapsulated Subtotal computation
/// </summary>
public class OrderItem
{
    private int _orderItemId;
    private Product _product = null!;
    private int _quantity;
    private decimal _unitPrice;

    /// <summary>
    /// Line item identifier.
    /// </summary>
    public int OrderItemId
    {
        get => _orderItemId;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "OrderItemId must be a positive integer greater than zero.");
            _orderItemId = value;
        }
    }

    /// <summary>
    /// Associated product being ordered.
    /// </summary>
    public Product Product
    {
        get => _product;
        private set => _product = value ?? throw new ArgumentNullException(nameof(value), "Product cannot be null.");
    }

    /// <summary>
    /// Quantity of the product ordered. Must be at least 1.
    /// </summary>
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Quantity must be greater than zero.");
            _quantity = value;
        }
    }

    /// <summary>
    /// Locked-in unit price at the time of order placement.
    /// </summary>
    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            if (value <= 0m)
                throw new ArgumentOutOfRangeException(nameof(value), "UnitPrice must be greater than zero.");
            _unitPrice = value;
        }
    }

    public OrderItem()
    {
    }

    public OrderItem(int orderItemId, Product product, int quantity, decimal? unitPrice = null)
    {
        OrderItemId = orderItemId;
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice ?? product.Price;
    }

    /// <summary>
    /// Computes the subtotal for this line item (Quantity * UnitPrice).
    /// </summary>
    public decimal CalculateSubtotal()
    {
        return Quantity * UnitPrice;
    }

    public override string ToString() =>
        $"Item #{OrderItemId}: {Product.Name} x {Quantity} @ {UnitPrice:C} = {CalculateSubtotal():C}";
}
