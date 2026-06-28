# Basic Usage Examples

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BasicUsageExamples.md)

---

This document demonstrates the most common usage scenarios for **Dixor.Identity**.

These examples are intended to help developers quickly get started with UUID version 7 generation and related functionality.

---

# Generating a Single UUID

The simplest way to generate a UUID version 7 is by calling `Uuid7.New()`.

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.New();

Console.WriteLine(id);
```

Example output:

```text
01985b2d-c6b4-7893-a8dc-95cfd3dbdd7d
```

---

# Generating Multiple UUIDs

Generate multiple UUIDs simultaneously.

```csharp
IReadOnlyList<Guid> ids =
[
    ..Uuid7.NewGuids(5)
];

foreach (Guid id in ids)
{
    Console.WriteLine(id);
}
```

Example output:

```text
01985b2d-c6b4-7893-a8dc-95cfd3dbdd7d
01985b2d-c6b5-7e11-89f2-92b4ec5e5ad1
01985b2d-c6b6-7c54-bf84-7f6af7dbaf83
01985b2d-c6b7-72ef-8d90-c60f20a28753
01985b2d-c6b8-7f45-9f6d-c874bc44d7c4
```

---

# Generating a UUID With a Specific Timestamp

Sometimes applications need deterministic timestamps.

```csharp
DateTimeOffset timestamp =
    new(2026, 06, 28, 12, 00, 00, TimeSpan.Zero);

Guid id = Uuid7.New(timestamp);

Console.WriteLine(id);
```

This embeds the specified timestamp directly into the generated UUID.

---

# Generating a Monotonic UUID

Monotonic UUIDs guarantee ordering when multiple identifiers are generated within the same millisecond.

```csharp
Guid id = Uuid7.NewMonotonic();

Console.WriteLine(id);
```

Monotonic identifiers are recommended for high-throughput systems and database primary keys.

---

# Creating a Formatted UUID String

Business systems often require additional context around identifiers.

```csharp
Guid id = Uuid7.New();

string formatted =
    Uuid7.NewString(
        prefix: "ORD",
        suffix: "EU",
        separator: "-");

Console.WriteLine(formatted);
```

Example output:

```text
ORD-01985b2d-c6b4-7893-a8dc-95cfd3dbdd7d-EU
```

---

# Formatting an Existing UUID

You can also format an already existing UUID.

```csharp
Guid id = Uuid7.New();

string value =
    Uuid7.Format(
        id,
        prefix: "INV",
        suffix: "2026",
        separator: "_");

Console.WriteLine(value);
```

Example output:

```text
INV_01985b2d-c6b4-7893-a8dc-95cfd3dbdd7d_2026
```

---

# Validating a UUIDv7

Check whether a value is a valid UUID version 7.

```csharp
Guid id = Uuid7.New();

bool isValid =
    Uuid7.IsValid(id.ToString());

Console.WriteLine(isValid);
```

Output:

```text
True
```

---

# Parsing UUIDs From Strings

UUIDs can be extracted from larger strings.

```csharp
string value =
    "Order created successfully: ORD-01985b2d-c6b4-7893-a8dc-95cfd3dbdd7d";

Guid id = Uuid7.Parse(value);

Console.WriteLine(id);
```

Output:

```text
01985b2d-c6b4-7893-a8dc-95cfd3dbdd7d
```

---

# Safely Parsing UUIDs

Use `TryParse` when invalid input is expected.

```csharp
string input = "Invalid Value";

if (Uuid7.TryParse(input, out Guid id))
{
    Console.WriteLine(id);
}
else
{
    Console.WriteLine("No UUID found.");
}
```

---

# Extracting Timestamps

Every UUIDv7 contains an embedded timestamp.

```csharp
Guid id = Uuid7.New();

DateTimeOffset timestamp =
    Uuid7.ExtractTimestamp(id);

Console.WriteLine(timestamp);
```

Example output:

```text
2026-06-28T12:15:30+00:00
```

---

# Complete Example

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.New();

Console.WriteLine($"UUID: {id}");

bool valid = Uuid7.IsValid(id.ToString());

Console.WriteLine($"Valid UUIDv7: {valid}");

DateTimeOffset timestamp =
    Uuid7.ExtractTimestamp(id);

Console.WriteLine($"Created At: {timestamp}");
```

---

# When Should I Use Each API?

| Scenario                  | Recommended API            |
| ------------------------- | -------------------------- |
| Single UUID generation    | `Uuid7.New()`              |
| Multiple UUID generation  | `Uuid7.NewGuids()`         |
| High-throughput systems   | `Uuid7.NewMonotonic()`     |
| Custom identifier strings | `Uuid7.NewString()`        |
| Validation                | `Uuid7.IsValid()`          |
| Parsing                   | `Uuid7.Parse()`            |
| Safe parsing              | `Uuid7.TryParse()`         |
| Timestamp extraction      | `Uuid7.ExtractTimestamp()` |

---

# Related Documentation

* [Quick Start](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/QuickStart.md)
* [UUID Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Generation.md)
* [Batch Generation Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BatchGenerationExamples.md)
* [Sequential Identifier Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/SequentialIdentifiersExamples.md)

---

## Navigation

⬅ Previous: [UUIDv7 Best Practices](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BestPractices.md)

➡ Next: [Batch Generation Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BatchGenerationExamples.md)