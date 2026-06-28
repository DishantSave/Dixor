# Quick Start

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Getting Started](./QuickStart.md)

---

This guide provides a quick introduction to the most commonly used Dixor.Identity APIs.

If you want to start using UUID version 7 immediately, this guide is for you.

---

# Generate A UUIDv7

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.NewGuid();
```

Example output:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

---

# Generate A Monotonic UUID

```csharp
Guid id =
    Uuid7.NewMonotonicGuid();
```

Use monotonic generation for:

* High throughput systems.
* Database primary keys.
* Event stores.

---

# Generate Multiple UUIDs

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(100);
```

---

# Create A Business Identifier

```csharp
string id =
    Uuid7.NewString(
        prefix: "ORD",
        suffix: "EU");
```

Output:

```text
ORD-019853a6-35c4-7e58-bf91-cb7d0b1b11d2-EU
```

---

# Parse UUIDs

```csharp
Guid id =
    Uuid7.Parse(
        "ORD-019853a6-35c4-7e58-bf91-cb7d0b1b11d2");
```

---

# Safe Parsing

```csharp
if (Uuid7.TryParse(
        input,
        out Guid id))
{
    Console.WriteLine(id);
}
```

---

# Validate UUIDv7

```csharp
bool valid =
    Uuid7.IsValid(input);
```

---

# Validate Existing Guid

```csharp
bool valid =
    Uuid7.IsUuid7(guid);
```

---

# Extract Timestamp

```csharp
DateTimeOffset created =
    Uuid7.GetTimestamp(id);
```

---

# Batch Example

```csharp
foreach (Guid id in Uuid7.NewGuids(10))
{
    Console.WriteLine(id);
}
```

---

# Typical ASP.NET Core Example

```csharp
app.MapPost(
    "/orders",
    (CreateOrderRequest request) =>
{
    Order order = new()
    {
        Id = Uuid7.NewGuid()
    };

    return Results.Ok(order);
});
```

---

# Recommended Database Type

| Database   | Recommended Type |
| ---------- | ---------------- |
| SQL Server | UNIQUEIDENTIFIER |
| PostgreSQL | UUID             |
| MySQL      | BINARY(16)       |
| SQLite     | BLOB             |

---

# Most Common APIs

| API                      | Purpose                      |
| ------------------------ | ---------------------------- |
| Uuid7.NewGuid()          | Generate UUIDv7              |
| Uuid7.NewMonotonicGuid() | Generate ordered UUID        |
| Uuid7.NewGuids()         | Batch generation             |
| Uuid7.NewString()        | Create formatted identifiers |
| Uuid7.Parse()            | Parse UUID                   |
| Uuid7.TryParse()         | Safe parsing                 |
| Uuid7.IsUuid7()          | Validate Guid                |
| Uuid7.IsValid()          | Validate string              |
| Uuid7.GetTimestamp()     | Extract timestamp            |

---

# Next Steps

Explore the detailed documentation:

* UUID7_Overview.md
* UUID7_BestPractices.md
* Generation.md
* MonotonicGeneration.md
* Parsing.md
* Validation.md
* TimestampExtraction.md

---

# Summary

You now know how to:

* Generate UUIDv7 values.
* Generate monotonic UUIDs.
* Format identifiers.
* Parse identifiers.
* Validate identifiers.
* Extract timestamps.

You are ready to start building applications with Dixor.Identity.

---

# Related Documentation

* [Installation](./Installation.md)
* [Generate Your First UUID](./FirstUuid.md)
* [Migration Guide](./MigrationGuide.md)
* [UUIDv7 Overview](../uuid7/UUID7_Overview.md)

---

## Navigation

⬅ Previous: [Installation](./Installation.md)

➡ Next: [Generate Your First UUID](./FirstUuid.md)