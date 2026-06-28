# UUID Version 7 Best Practices

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [UUIDv7 Overview](./UUID7_Overview.md)

---

This document describes recommended practices for using UUID version 7 in production applications.

Following these recommendations will help maximize performance, reliability and maintainability.

---

# Use UUIDv7 As Database Primary Keys

Recommended:

```sql
CREATE TABLE Orders
(
    Id UNIQUEIDENTIFIER PRIMARY KEY
)
```

Generate identifiers using:

```csharp
Guid id = Uuid7.NewGuid();
```

UUIDv7 values insert sequentially and significantly reduce index fragmentation.

---

# Prefer Monotonic UUIDs For High Throughput Systems

Recommended:

```csharp
Guid id =
    Uuid7.NewMonotonicGuid();
```

Use monotonic generation when:

* Thousands of IDs are generated per second.
* High write throughput exists.
* Database locality is important.

Examples:

* Event stores.
* Messaging systems.
* CQRS applications.

---

# Use Batch Generation For Bulk Operations

Recommended:

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(10000);
```

Ideal for:

* Data seeding.
* Bulk inserts.
* Import operations.
* Test data generation.

Avoid:

```csharp
for (int i = 0; i < 10000; i++)
{
    Uuid7.NewGuid();
}
```

---

# Always Store UUIDs As Native GUID Types

Recommended:

```sql
UNIQUEIDENTIFIER
```

Avoid:

```sql
VARCHAR(36)
NVARCHAR(36)
```

Benefits:

* Reduced storage.
* Faster indexing.
* Better query performance.

---

# Preserve UTC

UUIDv7 timestamps are stored in UTC.

Always work with:

```csharp
DateTimeOffset
```

Avoid converting timestamps to local time unless required for display purposes.

---

# Validate External Input

Always validate identifiers supplied by:

* Users.
* APIs.
* Files.
* External systems.

Recommended:

```csharp
if (!Uuid7.IsValid(input))
{
    return BadRequest();
}
```

---

# Prefer Try APIs For External Data

Recommended:

```csharp
if (Uuid7.TryParse(
        input,
        out Guid id))
{
    Process(id);
}
```

Avoid:

```csharp
Guid id =
    Uuid7.Parse(input);
```

for user input.

---

# Avoid Exception Driven Logic

Bad:

```csharp
try
{
    Guid id = Uuid7.Parse(input);
}
catch
{
}
```

Recommended:

```csharp
if (Uuid7.TryParse(
        input,
        out Guid id))
{
}
```

---

# Do Not Modify UUID Values

Avoid:

```csharp
string value =
    id.ToString().Replace("-", "");
```

Changing UUID representations may:

* Break parsing.
* Break interoperability.
* Reduce readability.

---

# Use Formatting APIs For Business Identifiers

Recommended:

```csharp
string orderId =
    Uuid7.NewString(
        prefix: "ORD",
        suffix: "EU");
```

Avoid manual concatenation.

---

# Do Not Rely On UUID Timestamps For Security

UUID timestamps expose creation time.

Never use timestamps for:

* Authentication.
* Authorization.
* Security decisions.

UUIDv7 is designed for identification, not security.

---

# Avoid Sequential Assumptions Across Machines

Although UUIDv7 is time ordered, distributed systems may still generate identifiers concurrently.

Never assume:

```text
ID A + 1 = ID B
```

UUIDs are identifiers, not counters.

---

# Use Timestamp Extraction For Auditing

Recommended:

```csharp
DateTimeOffset created =
    Uuid7.GetTimestamp(id);
```

Use for:

* Diagnostics.
* Event ordering.
* Audit logs.

Avoid using extracted timestamps as the sole source of business truth.

---

# Prefer Dependency Injection

Abstract identifier generation:

```csharp
public interface IIdGenerator
{
    Guid NewId();
}
```

Implementation:

```csharp
public sealed class UuidGenerator :
    IIdGenerator
{
    public Guid NewId()
        => Uuid7.NewGuid();
}
```

Benefits:

* Easier testing.
* Better maintainability.
* Improved decoupling.

---

# Test UUID Generation

Recommended:

```csharp
Assert.True(
    Uuid7.IsUuid7(id));
```

Validate:

* Correct version.
* Ordering.
* Timestamp extraction.

---

# Monitor Database Fragmentation

Even with UUIDv7, periodically monitor:

* Index fragmentation.
* Page splits.
* Fill factors.

Database tuning remains important.

---

# Use Analyzer Recommendations

Dixor.Identity ships with Roslyn analyzers.

Always address diagnostics produced by the analyzers.

Analyzer guidance helps:

* Prevent mistakes.
* Improve performance.
* Enforce best practices.

---

# Common Mistakes

| Mistake                    | Recommendation          |
| -------------------------- | ----------------------- |
| Storing UUID as string     | Store as Guid           |
| Using Parse for user input | Use TryParse            |
| Ignoring validation        | Validate external input |
| Manual formatting          | Use formatter APIs      |
| Using UUID as counter      | Avoid                   |
| Ignoring UTC               | Preserve UTC            |

---

# Production Checklist

Before deploying:

* [ ] Store UUIDs as native GUID types.
* [ ] Validate all external identifiers.
* [ ] Use Try APIs for untrusted input.
* [ ] Prefer monotonic generation for high throughput systems.
* [ ] Preserve UTC timestamps.
* [ ] Monitor database indexes.
* [ ] Use analyzers.
* [ ] Write integration tests.

---

# Summary

UUIDv7 offers significant advantages over traditional UUID implementations.

To maximize these benefits:

* Use native GUID storage.
* Prefer Try APIs.
* Validate external data.
* Use monotonic generation where appropriate.
* Preserve UTC.
* Follow analyzer recommendations.

Adhering to these practices ensures robust, scalable and high-performance applications.

---

# Related Documentation

* [UUIDv7 Overview](./UUID7_Overview.md)
* [Generation](./UUID7_Generation.md)
* [Monotonic Generation](./UUID7_MonotonicGeneration.md)
* [Timestamp Extraction](./UUID7_TimestampExtraction.md)
* [Examples](../examples/BasicUsageExamples.md)

---

## Navigation

⬅ Previous: [Timestamp Extraction](./UUID7_TimestampExtraction.md)

➡ Next: [Basic Usage Examples](../examples/BasicUsageExamples.md)