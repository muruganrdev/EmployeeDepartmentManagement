using System.Text.RegularExpressions;

namespace EmployeeDepartmentManagement.Core.OrderManagement.Models;

/// <summary>
/// Domain model representing a Customer placing orders in the system.
/// Demonstrates Encapsulation with property validation for name and email invariants.
/// </summary>
public class Customer
{
    private int _customerId;
    private string _name = string.Empty;
    private string _email = string.Empty;

    /// <summary>
    /// Unique identifier for the customer.
    /// Uses 'init' so it cannot be altered once instantiated.
    /// </summary>
    public int CustomerId
    {
        get => _customerId;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "CustomerId must be a positive integer greater than zero.");
            _customerId = value;
        }
    }

    /// <summary>
    /// Full customer name. Guarded against empty or whitespace-only strings.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Customer Name cannot be empty or whitespace.", nameof(value));
            if (value.Trim().Length < 2)
                throw new ArgumentException("Customer Name must be at least 2 characters.", nameof(value));
            _name = value.Trim();
        }
    }

    /// <summary>
    /// Primary contact email for billing and receipts. Validates basic email format.
    /// </summary>
    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Customer Email cannot be empty.", nameof(value));
            
            string trimmed = value.Trim();
            if (!trimmed.Contains('@') || !trimmed.Contains('.') || trimmed.StartsWith('@') || trimmed.EndsWith('@'))
                throw new ArgumentException($"'{value}' is not a valid email address.", nameof(value));

            _email = trimmed.ToLowerInvariant();
        }
    }

    /// <summary>
    /// Parameterless constructor for serialization or deferred initialization.
    /// </summary>
    public Customer()
    {
    }

    /// <summary>
    /// Parameterized constructor enforcing domain validation at construction time.
    /// </summary>
    public Customer(int customerId, string name, string email)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
    }

    public override string ToString() => $"[Customer #{CustomerId}] {Name} ({Email})";

    public override bool Equals(object? obj)
    {
        if (obj is Customer other)
            return CustomerId == other.CustomerId;
        return false;
    }

    public override int GetHashCode() => CustomerId.GetHashCode();
}
