# Sequential Identifier Examples

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [Examples](./BasicUsageExamples.md)

---

This document demonstrates how to use UUID version 7 identifiers as sequential identifiers in real-world applications.

UUIDv7 provides natural chronological ordering while maintaining global uniqueness.

---

# Why Sequential Identifiers?

Traditional random UUIDs often cause:

* Database index fragmentation.
* Increased page splits.
* Reduced insert performance.

UUIDv7 significantly reduces these issues.

---

# Using UUIDv7 as Primary Keys

Entity Framework example:

```csharp
public sealed class Order
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;
}
```

Generating identifiers:

```csharp
Order order = new()
{
    Id = Uuid7.New(),
    CustomerName = "John Doe"
};

dbContext.Orders.Add(order);

await dbContext.SaveChangesAsync();
```

---

# High-Throughput Database Inserts

For extremely busy systems, monotonic UUIDs are recommended.

```csharp
Order order = new()
{
    Id = Uuid7.NewMonotonic()
};
```

Monotonic UUIDs ensure ordering even when multiple identifiers are generated during the same millisecond.

---

# Ordering Records By Identifier

Because UUIDv7 embeds timestamps, sorting by identifier generally reflects creation order.

```csharp
List<Guid> ids =
[
    Uuid7.New(),
    Uuid7.New(),
    Uuid7.New()
];

var ordered =
    ids.OrderBy(x => x);

foreach (Guid id in ordered)
{
    Console.WriteLine(id);
}
```

---

# SQL Server Example

```sql
CREATE TABLE Orders
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    CustomerName NVARCHAR(100)
);
```

Insertion:

```csharp
await connection.ExecuteAsync(
    """
    INSERT INTO Orders(Id, CustomerName)
    VALUES(@Id, @CustomerName)
    """,
    new
    {
        Id = Uuid7.New(),
        CustomerName = "John Doe"
    });
```

---

# Distributed Systems Example

Multiple services can safely generate identifiers independently.

```csharp
Guid orderId = Uuid7.New();
Guid paymentId = Uuid7.New();
Guid shipmentId = Uuid7.New();
```

No central coordination is required.

---

# Message Queue Example

```csharp
var message = new OrderCreatedMessage
{
    Id = Uuid7.New(),
    OrderNumber = "ORD-1001"
};
```

The embedded timestamp makes event tracing easier.

---

# Auditing Example

```csharp
Guid id = Uuid7.New();

DateTimeOffset createdAt =
    Uuid7.ExtractTimestamp(id);

Console.WriteLine(
    $"Created At: {createdAt}");
```

---

# Recommended Identifier Strategy

| Workload               | Recommendation         |
| ---------------------- | ---------------------- |
| General applications   | `Uuid7.New()`          |
| Database primary keys  | `Uuid7.New()`          |
| Heavy insert workloads | `Uuid7.NewMonotonic()` |
| Event sourcing         | `Uuid7.NewMonotonic()` |
| Distributed systems    | `Uuid7.New()`          |

---

# Benefits

Using UUIDv7 as sequential identifiers provides:

* Better database performance.
* Reduced index fragmentation.
* Natural ordering.
* Global uniqueness.
* Easier auditing.
* Improved scalability.

---

# Related Documentation

* [Basic Usage Examples](./BasicUsageExamples.md)
* [Batch Generation Examples](./BatchGenerationExamples.md)
* [Monotonic Generation](../uuid7/UUID7_MonotonicGeneration.md)
* [UUIDv7 Best Practices](../uuid7/UUID7_BestPractices.md)

---

## Navigation

⬅ Previous: [Batch Generation Examples](./BatchGenerationExamples.md)

➡ Next: [Byte Layout](../internals/ByteLayout.md)