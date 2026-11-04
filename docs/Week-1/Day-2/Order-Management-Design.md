# Week 1 – Day 2: Order Management Domain & Interface-Based Design

## 1. Module Overview

The **Order Management** module in `EmployeeDepartmentManagement.Core` was developed to fulfill the Week 1 – Day 2 domain modeling and interface-based design specifications. It coexists cleanly alongside the existing Employee & Department domain without modifying or breaking any Employee Management features.

### Architecture Structure

```text
src/EmployeeDepartmentManagement.Core/OrderManagement/
├── Models/
│   ├── Customer.cs           # Customer entity with ID, Name, Email validation
│   ├── Product.cs            # Product entity with inventory tracking & price validation
│   ├── OrderItem.cs          # Line item associating Product, Quantity, UnitPrice & Subtotal
│   └── Order.cs              # Aggregate Root managing OrderItems & Total calculation
├── Interfaces/
│   ├── IOrderRepository.cs   # Data access contract (Add, GetById, GetAll, Exists)
│   └── IOrderService.cs      # Domain service contract (CreateOrder, GetOrderById, CalculateTotal)
├── Repositories/
│   └── InMemoryOrderRepository.cs # O(1) in-memory storage satisfying IOrderRepository
└── Services/
    └── OrderService.cs       # Orchestrator satisfying IOrderService and depending on IOrderRepository
```

---

## 2. Domain Models & Object Relationships

### 1. `Customer`
- **Fields / Properties**: `CustomerId` (int, positive, init-only), `Name` (string, min 2 chars), `Email` (string, validated format).
- **Encapsulation**: Property setters guard against empty strings, whitespace, and malformed email addresses.

### 2. `Product`
- **Fields / Properties**: `ProductId` (int, positive, init-only), `Name` (string, required), `Price` (decimal, > 0), `AvailableStock` (int, non-negative, private setter).
- **Behavior**:
  - `DeductStock(int quantity)`: Validates stock availability and reduces inventory atomically.
  - `Restock(int quantity)`: Safely restores inventory.

### 3. `OrderItem`
- **Fields / Properties**: `OrderItemId` (int), `Product` (Product reference), `Quantity` (int, > 0), `UnitPrice` (decimal, locked-in unit price).
- **Price Freezing**: Captures `UnitPrice` at the time of order placement, protecting historical order subtotals against future product price modifications.
- **Behavior**:
  - `CalculateSubtotal()`: Evaluates `Quantity * UnitPrice`.

### 4. `Order` (Aggregate Root)
- **Fields / Properties**: `OrderId` (int), `Customer` (Customer reference), `OrderItems` (read-only list), `OrderDate` (DateTime).
- **Encapsulation**: Private `List<OrderItem> _orderItems` prevents direct external modification. Items are appended strictly through `AddOrderItem(OrderItem)` or `AddItem(Product, quantity)`.
- **Behavior**:
  - `CalculateTotal()`: Sums all item subtotals (`_orderItems.Sum(item => item.CalculateSubtotal())`).

---

## 3. Relationships Explained

```text
Customer (1) <────── (Many) Order (1) *────── (Many) OrderItem (Many) ──────> (1) Product
```

1. **Customer $\rightarrow$ Order (Association)**:
   - A Customer places one or more Orders.
   - An Order references exactly one Customer.
   - If an Order is cancelled or deleted, the Customer entity continues to exist independently.
2. **Order $\rightarrow$ OrderItem (Composition `*--`)**:
   - An Order *owns* its line items.
   - An `OrderItem` cannot exist independently without its parent `Order`.
   - If an Order is deleted, its constituent `OrderItem` instances cease to exist.
3. **OrderItem $\rightarrow$ Product (Association `-->`)**:
   - An `OrderItem` holds a reference to a `Product` and records the quantity purchased.
   - Products exist independently in the catalog regardless of whether they have been ordered.

---

## 4. Execution Flow

```text
Console / Caller
       │
       ▼ (depends on interface)
 [IOrderService]
       │
       ▼ (concrete implementation)
  OrderService
       │
       ├── Validates Customer & Stock
       ├── Deducts Stock from Product
       ├── Constructs Order & OrderItems
       │
       ▼ (persists via abstraction)
[IOrderRepository]
       │
       ▼ (concrete implementation)
InMemoryOrderRepository (Dictionary<int, Order>)
```

---

## 5. Interface-Based Design & SOLID Principles in Order Management

### 1. Why Use Interfaces?
An interface specifies **what** operations must be supported without dictating **how** they are implemented.
- Callers (like `Program.cs`) depend only on `IOrderService`, allowing the concrete implementation to be replaced (e.g., swapping `InMemoryOrderRepository` for `SqlOrderRepository` on Day 5) without changing a single line of business orchestration.

### 2. Single Responsibility Principle (SRP)
- `Customer`, `Product`, `OrderItem`, `Order`: Pure domain entities responsible only for holding state and validating their own domain invariants.
- `InMemoryOrderRepository`: Responsible *only* for storing and retrieving orders.
- `OrderService`: Responsible *only* for business orchestration (stock deduction + order placement).

### 3. Open/Closed Principle (OCP)
- If we need a `DiscountedOrderService` or `BulkOrderService`, we can implement `IOrderService` or extend `OrderService` without editing existing caller code.

### 4. Liskov Substitution Principle (LSP)
- Any class implementing `IOrderRepository` (whether in-memory, file-based, or database-driven) can substitute `InMemoryOrderRepository` without altering the correctness of `OrderService`.

### 5. Interface Segregation Principle (ISP)
- `IOrderRepository` only contains data access methods (`Add`, `GetById`, `GetAll`, `Exists`).
- `IOrderService` only contains business workflow methods (`CreateOrder`, `GetOrderById`, `GetAllOrders`, `CalculateOrderTotal`).
- Neither interface forces clients to implement unused methods.

### 6. Dependency Inversion Principle (DIP)
- `OrderService` does **not** instantiate `new InMemoryOrderRepository()`. It receives `IOrderRepository` via its constructor:
  ```csharp
  public OrderService(IOrderRepository orderRepository)
  {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
  }
  ```
- High-level business logic (`OrderService`) depends on the abstraction (`IOrderRepository`), satisfying DIP completely.
