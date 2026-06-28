# Timestamp Extraction

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [UUIDv7 Overview](./UUID7_Overview.md)

---

One of the most powerful features of UUID version 7 is the ability to determine when an identifier was created.

Unlike traditional random UUIDs, UUIDv7 embeds a timestamp directly inside the identifier itself.

Dixor.Identity exposes APIs that allow developers to extract this timestamp.

The public API exposes this functionality through:

```csharp
DateTimeOffset Uuid7.GetTimestamp(Guid guid)

bool Uuid7.TryGetTimestamp(
    Guid guid,
    out DateTimeOffset timestamp)
```

Internally, timestamp extraction is implemented by:

```csharp
Uuid7TimestampExtractor
```

---

# Why Timestamp Extraction Exists

Traditional UUID versions such as UUIDv4 are completely random.

Example:

```text
6f7f17fd-b10b-4f1d-a6c3-41d0cb2f7a91
```

From this identifier alone, it is impossible to determine:

* When it was created.
* Which identifier was created first.
* The approximate age of the identifier.

UUIDv7 solves this problem.

Example:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

The first 48 bits of this UUID contain:

```text
Unix Timestamp (milliseconds)
```

This allows applications to reconstruct the original creation time.

---

# Advantages Of Timestamp Extraction

## No Additional Timestamp Column Required

Applications often store:

```sql
CREATE TABLE Orders
(
    Id UNIQUEIDENTIFIER,
    CreatedOn DATETIME2
)
```

With UUIDv7, the creation time can be derived directly from the identifier.

This can reduce:

* Storage requirements.
* Data duplication.
* Index size.

---

## Simplifies Auditing

Developers can determine:

* When a record was created.
* Relative ordering between records.
* Approximate event times.

without storing additional metadata.

---

## Event Stream Analysis

Event-driven systems frequently require event ordering.

Example:

```text
Event A
Event B
Event C
```

UUIDv7 timestamps allow developers to reconstruct event chronology.

---

## Useful For Diagnostics

During troubleshooting, developers can determine:

* When a request was created.
* When a message was published.
* When an entity was generated.

simply by inspecting the UUID.

---

# UUIDv7 Timestamp Layout

RFC 9562 specifies that the first:

```text
48 bits
```

of a UUIDv7 contain the Unix timestamp.

Structure:

```text
0                   47
+---------------------+
| Unix Timestamp      |
| (48 bits)           |
+---------------------+
```

The timestamp represents:

```text
Milliseconds since
1970-01-01T00:00:00Z
```

also known as:

```text
Unix Epoch
```

---

# What Is Unix Time?

Unix time represents the number of elapsed seconds or milliseconds since:

```text
1970-01-01T00:00:00Z
```

Example:

```csharp
long unixTime =
    DateTimeOffset.UtcNow
        .ToUnixTimeMilliseconds();
```

Result:

```text
1751132765000
```

This numeric value is embedded inside every UUIDv7.

---

# Extract Method

The `Extract` method extracts the timestamp from a UUIDv7.

Signature:

```csharp
DateTimeOffset Extract(Guid guid)
```

Example:

```csharp
Guid id = Uuid7.NewGuid();

DateTimeOffset timestamp =
    Uuid7.GetTimestamp(id);

Console.WriteLine(timestamp);
```

Output:

```text
2026-06-28 10:30:15 +00:00
```

---

# Extract Workflow

The extraction process follows these steps:

```text
Receive UUID
      │
      ▼
Validate UUID Is Version 7
      │
      ▼
Convert UUID To Bytes
      │
      ▼
Read First Six Bytes
      │
      ▼
Reconstruct Unix Timestamp
      │
      ▼
Convert To DateTimeOffset
      │
      ▼
Return Timestamp
```

---

# Validation Step

Timestamp extraction only supports UUIDv7 values.

Implementation:

```csharp
if (!Uuid7Validator.IsUuid7(guid))
{
    throw new ArgumentException(
        "The supplied Guid is not UUIDv7.");
}
```

Example:

```csharp
Guid randomGuid = Guid.NewGuid();

Uuid7.GetTimestamp(randomGuid);
```

Produces:

```text
ArgumentException
```

Reason:

Random UUIDs do not contain embedded timestamps.

---

# Why Validation Is Necessary

Consider:

```text
550e8400-e29b-41d4-a716-446655440000
```

This is a UUIDv4.

The first six bytes contain random data.

Interpreting these bytes as a timestamp would produce meaningless results.

Therefore, Dixor.Identity validates the UUID version before extraction.

---

# Internal Byte Extraction

Internally, the UUID is first converted into bytes.

Implementation:

```csharp
Span<byte> bytes = stackalloc byte[16];

GuidByteConverter.ToBytes(
    guid,
    bytes);
```

Result:

```text
Byte 0
Byte 1
Byte 2
Byte 3
Byte 4
Byte 5
...
Byte 15
```

The timestamp is stored within:

```text
Bytes 0 through 5
```

---

# Reconstructing The Timestamp

Implementation:

```csharp
long unixTime =
      ((long)bytes[0] << 40)
    | ((long)bytes[1] << 32)
    | ((long)bytes[2] << 24)
    | ((long)bytes[3] << 16)
    | ((long)bytes[4] << 8)
    | bytes[5];
```

This combines six individual bytes into a single:

```text
48-bit integer
```

using bitwise operations.

---

# Understanding Bit Shifting

Example:

```csharp
(long)bytes[0] << 40
```

means:

> Move the first byte forty positions to the left.

Likewise:

```csharp
(long)bytes[1] << 32
```

means:

> Move the second byte thirty-two positions.

Each byte is placed back into its original position.

Finally:

```csharp
|
```

(bitwise OR)

combines the values.

---

# Example Reconstruction

Suppose the UUID contains:

```text
Byte0 = 0x01
Byte1 = 0x97
Byte2 = 0xD8
Byte3 = 0xEF
Byte4 = 0xF4
Byte5 = 0xC6
```

These bytes are combined to reconstruct:

```text
1751132765000
```

which represents:

```text
2026-06-28T10:30:15Z
```

---

# Converting To DateTimeOffset

Once the Unix timestamp is reconstructed, it is converted back into a .NET date.

Implementation:

```csharp
DateTimeOffset.FromUnixTimeMilliseconds(
    unixTime);
```

Example:

```csharp
DateTimeOffset timestamp =
    DateTimeOffset
        .FromUnixTimeMilliseconds(
            1751132765000);
```

Output:

```text
2026-06-28T10:30:15+00:00
```

---

# TryExtract Method

`TryExtract` provides a non-throwing alternative.

Signature:

```csharp
bool TryExtract(
    Guid guid,
    out DateTimeOffset timestamp)
```

Example:

```csharp
if (Uuid7.TryGetTimestamp(
        guid,
        out DateTimeOffset timestamp))
{
    Console.WriteLine(timestamp);
}
```

---

# Why Use TryExtract?

Exceptions are relatively expensive.

When failures are expected, `TryExtract` is preferred.

Examples:

* User supplied UUIDs.
* API requests.
* Deserialized data.
* External integrations.

Benefits:

* Better performance.
* Cleaner code.
* No exception handling required.

---

# Extract vs TryExtract

| Feature                 | Extract   | TryExtract          |
| ----------------------- | --------- | ------------------- |
| Returns Timestamp       | Yes       | Via `out` parameter |
| Throws Exceptions       | Yes       | No                  |
| Suitable For User Input | No        | Yes                 |
| Failure Handling        | Exception | Boolean Result      |

---

# Exception Handling

## Invalid UUID Version

Example:

```csharp
Guid id = Guid.NewGuid();

Uuid7.GetTimestamp(id);
```

Produces:

```text
ArgumentException
```

because the UUID is not version 7.

---

# Failure Scenarios

## Non UUIDv7 Value

```csharp
Uuid7.GetTimestamp(Guid.NewGuid());
```

Result:

```text
ArgumentException
```

---

## Using TryExtract

```csharp
bool success =
    Uuid7.TryGetTimestamp(
        Guid.NewGuid(),
        out DateTimeOffset timestamp);
```

Result:

```text
false
```

and:

```text
timestamp == default
```

---

# Why `stackalloc` Is Used

Implementation:

```csharp
Span<byte> bytes =
    stackalloc byte[16];
```

Benefits:

* No heap allocation.
* Reduced garbage collection.
* Better throughput.
* Lower memory pressure.

Since UUIDs always contain sixteen bytes, stack allocation is highly efficient.

---

# Real-World Use Cases

## Audit Systems

```csharp
DateTimeOffset createdOn =
    Uuid7.GetTimestamp(order.Id);
```

---

## Distributed Tracing

```csharp
DateTimeOffset requestStarted =
    Uuid7.GetTimestamp(
        correlationId);
```

---

## Event Sourcing

```csharp
events.OrderBy(
    e => Uuid7.GetTimestamp(e.Id));
```

---

## Log Analysis

```csharp
DateTimeOffset created =
    Uuid7.GetTimestamp(logId);
```

---

# UUIDv4 vs UUIDv7

| Feature              | UUIDv4 | UUIDv7 |
| -------------------- | ------ | ------ |
| Contains Timestamp   | No     | Yes    |
| Sortable             | No     | Yes    |
| Timestamp Extraction | No     | Yes    |
| Event Ordering       | No     | Yes    |
| Audit Friendly       | No     | Yes    |

---

# Roslyn Analyzer Support

Dixor.Identity ships with:

```text
Uuid7TimestampExtractorAnalyzer
```

The analyzer currently performs:

```text
No compile-time analysis
```

and acts as a placeholder for future enhancements.

Implementation:

```csharp
SupportedDiagnostics => [];
```

---

# Why Does The Analyzer Exist?

Future versions may introduce diagnostics for:

* Calling extraction APIs with constant non-UUIDv7 values.
* Suggesting `TryExtract` for user input.
* Detecting unnecessary exception handling.
* Identifying misuse of timestamp APIs.

Maintaining a dedicated analyzer ensures architectural consistency throughout the library.

---

# Best Practices

## Use `TryExtract` For External Input

Recommended:

```csharp
if (Uuid7.TryGetTimestamp(
        guid,
        out DateTimeOffset timestamp))
{
    // Use timestamp
}
```

---

## Use `Extract` For Trusted Data

Example:

```csharp
DateTimeOffset timestamp =
    Uuid7.GetTimestamp(order.Id);
```

---

## Always Use UTC

The extracted timestamp is returned as:

```text
UTC
```

Applications should preserve UTC whenever possible.

---

# Summary

Timestamp extraction is one of the defining capabilities of UUIDv7.

Dixor.Identity enables developers to recover creation times directly from UUIDs without requiring additional storage.

Benefits include:

* Embedded creation timestamps.
* Simplified auditing.
* Event ordering.
* Reduced storage requirements.
* High performance.
* Easy diagnostics.
* Improved observability.

This functionality makes UUIDv7 significantly more powerful than traditional random UUID implementations.

---

# Related Documentation

* [Parsing](./UUID7_Parsing.md)
* [Validation](./UUID7_Validation.md)
* [Best Practices](./UUID7_BestPractices.md)

---

## Navigation

⬅ Previous: [Parsing](./UUID7_Parsing.md)

➡ Next: [Best Practices](./UUID7_BestPractices.md)