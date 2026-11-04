namespace EmployeeDepartmentManagement.Core.OrderManagement.Models;

/// <summary>
/// Domain model representing a Product available for purchase.
/// Demonstrates Encapsulation with inventory management logic and price validation.
/// </summary>
public class Product
{
    private int _productId;
    private string _name = string.Empty;
    private decimal _price;
    private int _availableStock;

    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    public int ProductId
    {
        get => _productId;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "ProductId must be a positive integer greater than zero.");
            _productId = value;
        }
    }

    /// <summary>
    /// Descriptive name of the product.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product Name cannot be empty or whitespace.", nameof(value));
            _name = value.Trim();
        }
    }

    /// <summary>
    /// Unit sale price. Uses 128-bit decimal for currency precision.
    /// Must be strictly greater than zero.
    /// </summary>
    public decimal Price
    {
        get => _price;
        set
        {
            if (value <= 0m)
                throw new ArgumentOutOfRangeException(nameof(value), "Product Price must be greater than zero.");
            _price = value;
        }
    }

    /// <summary>
    /// Quantity of items physically available in inventory.
    /// Setter is private to prevent arbitrary modification outside domain methods.
    /// </summary>
    public int AvailableStock
    {
        get => _availableStock;
        private set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "AvailableStock cannot be negative.");
            _availableStock = value;
        }
    }

    public Product()
    {
    }

    public Product(int productId, string name, decimal price, int availableStock)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        AvailableStock = availableStock;
    }

    /// <summary>
    /// Safely reduces inventory when an order item is placed.
    /// </summary>
    public void DeductStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity to deduct must be greater than zero.");
        if (quantity > AvailableStock)
            throw new InvalidOperationException($"Insufficient stock for '{Name}'. Requested: {quantity}, Available: {AvailableStock}.");

        AvailableStock -= quantity;
    }

    /// <summary>
    /// Restocks inventory when goods arrive or orders are cancelled.
    /// </summary>
    public void Restock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Restock quantity must be greater than zero.");

        AvailableStock += quantity;
    }

    public override string ToString() => $"[Product #{ProductId}] {Name} - {Price:C} (Stock: {AvailableStock})";

    public override bool Equals(object? obj)
    {
        if (obj is Product other)
            return ProductId == other.ProductId;
        return false;
    }

    public override int GetHashCode() => ProductId.GetHashCode();
}
