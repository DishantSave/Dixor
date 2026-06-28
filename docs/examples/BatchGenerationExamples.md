# Batch Generation Examples

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BasicUsageExamples.md)

---

This document demonstrates various approaches for generating multiple UUID version 7 identifiers using **Dixor.Identity**.

Batch generation is useful when applications need to create large numbers of identifiers efficiently.

Common scenarios include:

* Bulk database inserts.
* Import operations.
* Data seeding.
* Load testing.
* Distributed message processing.

---

# Generating Multiple UUIDs

Generate a fixed number of UUIDs.

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(10);

foreach (Guid id in ids)
{
    Console.WriteLine(id);
}
```

---

# Materializing Results Into a List

If the collection must be reused multiple times, convert it to a list.

```csharp
List<Guid> ids =
[
    ..Uuid7.NewGuids(100)
];

Console.WriteLine(ids.Count);
```

Output:

```text
100
```

---

# Using Batch Generation For Database Inserts

```csharp
List<Order> orders = [];

foreach (Guid id in Uuid7.NewGuids(1000))
{
    orders.Add(
        new Order
        {
            Id = id
        });
}

await dbContext.Orders.AddRangeAsync(orders);

await dbContext.SaveChangesAsync();
```

---

# Seeding Test Data

```csharp
foreach (Guid id in Uuid7.NewGuids(50))
{
    Console.WriteLine(
        $"Seeded User: {id}");
}
```

---

# Generating Extremely Large Batches

The generator uses deferred execution.

```csharp
foreach (Guid id in Uuid7.NewGuids(1_000_000))
{
    Process(id);
}
```

Because UUIDs are generated lazily, memory consumption remains low.

---

# Deferred Execution Example

No UUIDs are generated until enumeration begins.

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(1000);

// Nothing generated yet.

foreach (Guid id in ids)
{
    Console.WriteLine(id);
}
```

---

# LINQ Integration

Batch generation integrates naturally with LINQ.

```csharp
var values =
    Uuid7.NewGuids(20)
          .Select(id => new
          {
              Id = id,
              CreatedAt =
                  Uuid7.ExtractTimestamp(id)
          });

foreach (var value in values)
{
    Console.WriteLine(
        $"{value.Id} - {value.CreatedAt}");
}
```

---

# Parallel Processing Example

```csharp
Parallel.ForEach(
    Uuid7.NewGuids(10_000),
    id =>
    {
        Process(id);
    });
```

---

# Invalid Batch Sizes

The following code throws an exception:

```csharp
Uuid7.NewGuids(0);
```

Exception:

```text
ArgumentOutOfRangeException
```

Additionally, the built-in analyzer reports:

```text
DIXOR001
```

at compile time.

---

# Best Practices

✔ Use deferred execution for large batches.

✔ Materialize only when necessary.

✔ Use monotonic UUIDs for database-heavy workloads.

✔ Avoid generating unnecessary identifiers.

✔ Prefer analyzers to catch issues during development.

---

# Related Documentation

* [Basic Usage Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BasicUsageExamples.md)
* [UUID Batch Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BatchGeneration.md)
* [Sequential Identifier Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/SequentialIdentifiersExamples.md)
* [DIXOR001](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR001.md)

---

## Navigation

⬅ Previous: [Basic Usage Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BasicUsageExamples.md)

➡ Next: [Sequential Identifier Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/SequentialIdentifiersExamples.md)