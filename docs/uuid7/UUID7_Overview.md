# UUID Version 7 Overview

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)

---

UUID version 7 (UUIDv7) is a modern universally unique identifier format standardized by RFC 9562.

Unlike traditional random UUIDs, UUIDv7 combines:

* A Unix timestamp.
* Cryptographically secure randomness.
* Natural chronological ordering.

This combination makes UUIDv7 particularly suitable for modern distributed applications and databases.

Dixor.Identity provides a complete, RFC 9562 compliant implementation of UUID version 7 for .NET applications.

---

# What Is A UUID?

A UUID (Universally Unique Identifier) is a 128-bit value designed to uniquely identify information across systems without requiring centralized coordination.

Example:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

UUIDs are commonly used as:

* Database primary keys.
* Correlation identifiers.
* Message identifiers.
* Distributed system identifiers.
* API resource identifiers.

---

# Why UUIDv7?

Traditional UUID versions suffer from several limitations.

For example, UUIDv4 is entirely random.

Example:

```text
6f7f17fd-b10b-4f1d-a6c3-41d0cb2f7a91
```

Although globally unique, UUIDv4 values:

* Cannot be sorted chronologically.
* Cause index fragmentation in databases.
* Do not contain creation timestamps.

UUIDv7 addresses these problems.

---

# UUIDv7 Structure

UUIDv7 consists of:

```text
+------------------------------------------------+
| Unix Timestamp (48 bits)                       |
+------------------------------------------------+
| Version (4 bits)                               |
+------------------------------------------------+
| Random Data (74 bits)                          |
+------------------------------------------------+
| Variant (2 bits)                               |
+------------------------------------------------+
```

The timestamp is stored as:

```text
Milliseconds since Unix Epoch
```

where:

```text
Unix Epoch = 1970-01-01T00:00:00Z
```

---

# Key Features

## Time Ordered

UUIDv7 values sort naturally by creation time.

Example:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
0197d8ef-f4d0-7b0a-80b4-6efdb239f8aa
0197d8ef-f4dd-7ef8-91b2-a81bfb79f371
```

The identifiers above were generated chronologically.

Sorting them lexicographically preserves creation order.

---

## Globally Unique

UUIDv7 uses cryptographically secure randomness.

This ensures extremely low collision probability across:

* Machines.
* Processes.
* Threads.
* Datacenters.
* Geographic regions.

---

## Embedded Timestamp

Creation time can be extracted directly from the UUID.

Example:

```csharp
Guid id = Uuid7.NewGuid();

DateTimeOffset created =
    Uuid7.GetTimestamp(id);
```

No separate timestamp column is required.

---

## Database Friendly

Sequential ordering significantly reduces:

* Page splits.
* Index fragmentation.
* Random inserts.

This improves database performance.

---

# UUIDv4 vs UUIDv7

| Feature               | UUIDv4 | UUIDv7  |
| --------------------- | ------ | ------- |
| Random                | Yes    | Partial |
| Time Ordered          | No     | Yes     |
| Embedded Timestamp    | No     | Yes     |
| Database Friendly     | No     | Yes     |
| RFC 9562              | No     | Yes     |
| Chronological Sorting | No     | Yes     |

---

# Features Provided By Dixor.Identity

Dixor.Identity provides:

## Standard UUID Generation

```csharp
Guid id = Uuid7.NewGuid();
```

---

## Monotonic UUID Generation

```csharp
Guid id = Uuid7.NewMonotonicGuid();
```

Ensures ordering even within the same millisecond.

---

## Batch Generation

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(1000);
```

---

## Formatting

```csharp
string id =
    Uuid7.NewString(
        prefix: "ORD",
        suffix: "EU",
        separator: "-");
```

Output:

```text
ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10-EU
```

---

## Parsing

```csharp
Guid id =
    Uuid7.Parse(
        "ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10");
```

---

## Validation

```csharp
bool valid =
    Uuid7.IsValid(value);
```

---

## Timestamp Extraction

```csharp
DateTimeOffset timestamp =
    Uuid7.GetTimestamp(id);
```

---

# Typical Use Cases

UUIDv7 is ideal for:

* Microservices.
* Distributed systems.
* Event sourcing.
* CQRS.
* Messaging systems.
* APIs.
* Database primary keys.
* Correlation IDs.
* Audit systems.

---

# RFC 9562 Compliance

Dixor.Identity fully complies with RFC 9562 requirements.

This includes:

* Version bits.
* Variant bits.
* Timestamp encoding.
* Big-endian ordering.
* UUIDv7 structure.

---

# Advantages Of UUIDv7

## Improved Database Performance

Sequential identifiers reduce fragmentation.

---

## Better Observability

Creation timestamps are embedded directly inside identifiers.

---

## Simplified Auditing

Creation times can be recovered without additional metadata.

---

## Natural Ordering

Records automatically sort chronologically.

---

## Excellent Scalability

No centralized ID generator is required.

---

# Example

```csharp
Guid id = Uuid7.NewGuid();

Console.WriteLine(id);

Console.WriteLine(
    Uuid7.GetTimestamp(id));

Console.WriteLine(
    Uuid7.IsUuid7(id));
```

Output:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
2026-06-28T10:30:15+00:00
True
```

---

# Summary

UUID version 7 represents the next generation of universally unique identifiers.

By combining:

* Time ordering.
* Cryptographic randomness.
* Embedded timestamps.
* RFC 9562 compliance.

UUIDv7 delivers superior performance and observability for modern applications.

Dixor.Identity provides a complete, high-performance and developer-friendly implementation of UUIDv7 for .NET.

---

# Related Documentation

* [UUID Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Generation.md)
* [Monotonic Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_MonotonicGeneration.md)
* [Batch Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BatchGeneration.md)
* [Formatting](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Formatting.md)
* [Validation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Validation.md)
* [Parsing](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Parsing.md)
* [Timestamp Extraction](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_TimestampExtraction.md)
* [Best Practices](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BestPractices.md)

---

## Navigation

⬅ Previous: [Migration Guide](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/MigrationGuide.md)

➡ Next: [UUID Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Generation.md)