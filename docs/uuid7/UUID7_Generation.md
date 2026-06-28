# UUIDv7 Generation

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)

---

> [!NOTE]
> **Documentation Path:**  
> README → UUIDv7 → Generation

---

## Navigation

- 🏠 [Home](../../README.md)
- ⬆️ [UUIDv7 Overview](UUID7_Overview.md)

---

Dixor.Identity provides a high-performance implementation for generating RFC 9562 compliant UUID version 7 identifiers.

UUIDv7 combines:

* A Unix timestamp with millisecond precision.
* Cryptographically secure random data.

The result is an identifier that is:

* Globally unique.
* Chronologically sortable.
* Database friendly.
* Suitable for distributed systems.

The public API exposes UUID generation through:

```csharp
Guid Uuid7.NewGuid()
```

and

```csharp
Guid Uuid7.NewGuid(DateTimeOffset timestamp)
```

Internally, generation is implemented by:

```csharp
Uuid7Generator
```

---

# What Is UUIDv7?

UUID version 7 is a modern UUID format standardized by RFC 9562.

Unlike older UUID versions, UUIDv7 was specifically designed to provide:

* Global uniqueness.
* Natural chronological ordering.
* Excellent database performance.
* Simplicity.
* Compatibility with existing UUID infrastructure.

Example UUIDv7:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

# Why UUIDv7 Was Introduced

Traditional UUIDv4 identifiers are completely random.

Example:

```text
8dcb8a83-67f5-42df-a67e-f76cc76e8b11
2af15ef2-3d77-4a0d-96cb-bdc0b5fd1579
6c8e74f0-6f14-4b14-b2c4-29e2f6e65b77
```

These values:

* Cannot be sorted by creation time.
* Cause index fragmentation.
* Reduce insert performance in databases.

UUIDv7 solves these problems by embedding a timestamp directly inside the UUID.

Example:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
0197d8ef-f4c7-72af-bf71-6f3d9ef3f201
0197d8ef-f4c8-7b0d-925e-4dc10eaf92b2
```

Notice how identifiers naturally increase over time.

---

# Advantages of UUIDv7

## Chronological Ordering

UUIDv7 values sort according to creation time.

Example:

```csharp
var ids = new[]
{
    Uuid7.NewGuid(),
    Uuid7.NewGuid(),
    Uuid7.NewGuid()
};

Array.Sort(ids);
```

Sorted order approximately matches generation order.

---

## Better Database Performance

Databases perform significantly better when inserted keys are sequential.

Benefits include:

* Reduced page splits.
* Reduced index fragmentation.
* Better cache locality.
* Faster inserts.
* Improved query performance.

This is especially beneficial for:

* SQL Server.
* PostgreSQL.
* MySQL.
* Oracle.

---

## Distributed System Friendly

Multiple machines can independently generate UUIDs without coordination.

No central server is required.

This makes UUIDv7 ideal for:

* Microservices.
* Event sourcing.
* Cloud applications.
* Distributed systems.

---

## Global Uniqueness

UUIDv7 combines:

* Timestamp bits.
* Cryptographically secure randomness.

This makes collisions practically impossible.

---

# UUIDv7 Structure

A UUIDv7 consists of 128 bits.

Layout:

```text
0                   47 48    51 52                              127
+---------------------+--------+----------------------------------+
| Unix Timestamp      | Ver=7 | Random Data                     |
| (48 bits)           |        | + Variant Bits                 |
+---------------------+--------+----------------------------------+
```

---

# Timestamp Section

The first 48 bits contain:

```text
Unix timestamp in milliseconds
```

This timestamp represents:

```text
Milliseconds since:
1970-01-01T00:00:00Z
```

also known as:

```text
Unix Epoch
```

Example:

```csharp
DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
```

returns:

```text
1751132765000
```

This value is embedded into the UUID.

---

# Random Section

The remaining bits are filled using:

```csharp
RandomNumberGenerator.Fill(...)
```

Dixor.Identity uses cryptographically secure randomness provided by .NET.

Benefits:

* Extremely low collision probability.
* Security.
* High entropy.
* Thread safety.

---

# Version Bits

Every UUID contains version information.

UUIDv7 must contain:

```text
0111
```

which represents:

```text
Version 7
```

Implementation:

```csharp
bytes[6] &= 0x0F;
bytes[6] |= 0x70;
```

Explanation:

First:

```text
xxxx xxxx
```

becomes:

```text
0000 xxxx
```

Then:

```text
0111 xxxx
```

The final four high bits always equal:

```text
0111
```

which indicates:

```text
UUID Version 7
```

---

# Variant Bits

RFC 9562 also requires a variant field.

The variant must begin with:

```text
10xxxxxx
```

Implementation:

```csharp
bytes[8] &= 0x3F;
bytes[8] |= 0x80;
```

Transformation:

```text
xxxxxxxx
```

becomes:

```text
10xxxxxx
```

This ensures interoperability with existing UUID implementations.

---

# Internal Generation Workflow

The generation process follows these steps:

```text
Obtain Timestamp
        │
        ▼
Convert Timestamp To Unix Milliseconds
        │
        ▼
Allocate 16 Bytes
        │
        ▼
Write Timestamp Into First 48 Bits
        │
        ▼
Generate Cryptographically Secure Random Bits
        │
        ▼
Set Version Bits To 7
        │
        ▼
Set RFC Variant Bits
        │
        ▼
Convert Byte Array To Guid
        │
        ▼
Return UUIDv7
```

---

# Default Generation

Generate a UUID using the current UTC time:

```csharp
Guid id = Uuid7.NewGuid();
```

Example output:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

Internally:

```csharp
public static Guid Generate()
    => Generate(DateTimeOffset.UtcNow);
```

This is the recommended API for most applications.

---

# Custom Timestamp Generation

Developers may generate UUIDs for a specific timestamp.

Example:

```csharp
DateTimeOffset timestamp =
    new DateTimeOffset(
        2026,
        6,
        28,
        10,
        30,
        0,
        TimeSpan.Zero);

Guid id = Uuid7.NewGuid(timestamp);
```

This embeds the supplied timestamp inside the UUID.

Common scenarios:

* Testing.
* Historical data imports.
* Event replay.
* Data migration.

---

# Why Use UTC?

Dixor.Identity uses:

```csharp
DateTimeOffset.UtcNow
```

instead of:

```csharp
DateTime.Now
```

because UTC:

* Avoids time zone ambiguity.
* Avoids daylight saving issues.
* Provides globally consistent timestamps.

Distributed systems should always prefer UTC.

---

# Unix Epoch Validation

UUIDv7 timestamps cannot occur before:

```text
1970-01-01T00:00:00Z
```

Example:

```csharp
DateTimeOffset invalid =
    new DateTimeOffset(
        1960,
        1,
        1,
        0,
        0,
        0,
        TimeSpan.Zero);

Uuid7.NewGuid(invalid);
```

Produces:

```text
ArgumentOutOfRangeException
```

Reason:

Unix timestamps are defined relative to the Unix epoch.

---

# Why `stackalloc` Is Used

Implementation:

```csharp
Span<byte> bytes = stackalloc byte[16];
```

Instead of:

```csharp
byte[] bytes = new byte[16];
```

Benefits:

* No heap allocation.
* Reduced garbage collection.
* Improved performance.
* Lower memory pressure.

Since UUIDs always contain exactly 16 bytes, stack allocation is extremely efficient.

---

# Why Big-Endian Is Used

RFC 9562 specifies:

```text
Network Byte Order (Big Endian)
```

Implementation:

```csharp
bytes[0] = (byte)(unixTime >> 40);
bytes[1] = (byte)(unixTime >> 32);
bytes[2] = (byte)(unixTime >> 24);
bytes[3] = (byte)(unixTime >> 16);
bytes[4] = (byte)(unixTime >> 8);
bytes[5] = (byte)unixTime;
```

Big-endian ordering ensures:

* RFC compliance.
* Lexicographical sorting.
* Cross-platform compatibility.

---

# Example Byte Layout

Suppose:

```text
Unix Time = 1751132765000
```

Bytes:

```text
Byte 0 -> Most significant timestamp bits
Byte 1
Byte 2
Byte 3
Byte 4
Byte 5 -> Least significant timestamp bits
Byte 6 -> Version + Random Bits
Byte 7 -> Random Bits
Byte 8 -> Variant + Random Bits
Byte 9-15 -> Random Bits
```

---

# Exception Handling

## ArgumentOutOfRangeException

Thrown when:

```csharp
timestamp < UnixEpoch
```

Example:

```csharp
Uuid7.NewGuid(
    new DateTimeOffset(
        1965,
        1,
        1,
        0,
        0,
        0,
        TimeSpan.Zero));
```

---

# Roslyn Analyzer Support

Dixor.Identity ships with:

```text
Uuid7GeneratorAnalyzer
```

This analyzer currently does not produce diagnostics.

---

# Why Does The Analyzer Exist?

Although no diagnostics are reported today, maintaining a dedicated analyzer provides:

* Future extensibility.
* Clear separation of concerns.
* A stable analyzer architecture.

Future versions may analyze:

* Invalid timestamp usage.
* Performance anti-patterns.
* Misuse of generation APIs.
* Best-practice violations.

---

# Current Analyzer Behavior

Current implementation:

```csharp
SupportedDiagnostics => [];
```

This means:

```text
No diagnostics are produced.
```

The analyzer simply registers a placeholder callback:

```csharp
context.RegisterOperationAction(
    NopAnalyze,
    OperationKind.Invocation);
```

where:

```csharp
private static void NopAnalyze(
    OperationAnalysisContext context)
{
    // Intentionally left blank.
}
```

---

# Potential Future Analyzer Features

Future versions may report diagnostics such as:

| Scenario                               | Potential Diagnostic  |
| -------------------------------------- | --------------------- |
| Timestamp before Unix epoch            | Warning               |
| Repeated generation inside tight loops | Performance hint      |
| Excessive allocations                  | Suggestion            |
| Non-UTC timestamps                     | Best practice warning |

---

# UUIDv7 vs UUIDv4

| Feature            | UUIDv4   | UUIDv7    |
| ------------------ | -------- | --------- |
| Random             | Yes      | Yes       |
| Timestamp          | No       | Yes       |
| Sortable           | No       | Yes       |
| Database Friendly  | No       | Yes       |
| RFC Standard       | RFC 4122 | RFC 9562  |
| Sequential Inserts | Poor     | Excellent |

---

# Best Practices

## Prefer Default Generation

Use:

```csharp
Uuid7.NewGuid();
```

for most scenarios.

---

## Use Custom Timestamps Only When Necessary

Typical applications should avoid manually supplying timestamps.

---

## Store UUIDs Using Native Database Types

Prefer:

```sql
uniqueidentifier
uuid
binary(16)
```

instead of string columns.

---

## Avoid Parsing Timestamps Manually

Use:

```csharp
Uuid7.GetTimestamp(id);
```

instead.

---

# Summary

UUIDv7 generation in Dixor.Identity provides:

* RFC 9562 compliance.
* Chronological ordering.
* Cryptographically secure randomness.
* Database-friendly identifiers.
* High performance.
* Global uniqueness.
* Distributed system compatibility.

Dixor.Identity is designed to provide a modern, efficient, and standards-compliant UUID generation experience for .NET applications.

---

# Related Documentation

* [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)
* [Monotonic Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_MonotonicGeneration.md)
* [Batch Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BatchGeneration.md)
* [Formatting](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Formatting.md)

---

## Navigation

⬅ Previous: [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)

➡ Next: [Monotonic Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_MonotonicGeneration.md)