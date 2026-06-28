# UUID Validation

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [UUIDv7 Overview](./UUID7_Overview.md)

---

Dixor.Identity provides functionality for validating whether a value represents a valid UUID version 7 identifier.

The public API exposes this functionality through:

```csharp
bool Uuid7.IsUuid7(Guid guid)

bool Uuid7.IsValid(string input)
```

Internally, validation is implemented by:

```csharp
Uuid7Validator
```

---

# Why Validation Is Necessary

Many developers assume that all `Guid` values are identical.

This is not true.

A GUID can belong to different UUID versions.

Examples include:

| UUID Version | Purpose                 |
| ------------ | ----------------------- |
| UUIDv1       | Time-based              |
| UUIDv3       | Name-based (MD5)        |
| UUIDv4       | Random                  |
| UUIDv5       | Name-based (SHA-1)      |
| UUIDv6       | Reordered time-based    |
| UUIDv7       | Unix timestamp + random |
| UUIDv8       | Custom implementation   |

Example:

```csharp
Guid id = Guid.NewGuid();
```

The generated value is usually:

```text
UUIDv4
```

and not:

```text
UUIDv7
```

Many Dixor.Identity features require genuine UUIDv7 values.

Examples:

* Timestamp extraction.
* Chronological ordering.
* Monotonic generation.
* RFC 9562 UUIDv7 processing.

Therefore, validation becomes essential.

---

# What Does Validation Mean?

Validation determines whether a supplied value:

1. Contains a valid GUID structure.
2. Represents UUID version 7.

Examples:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

Result:

```text
true
```

Example:

```text
550e8400-e29b-41d4-a716-446655440000
```

Result:

```text
false
```

because this UUID is version 4.

---

# Validation APIs

Dixor.Identity provides two validation methods.

## Guid Validation

```csharp
bool IsUuid7(Guid guid)
```

Used when a `Guid` instance already exists.

---

## String Validation

```csharp
bool IsValid(string input)
```

Used when working with textual input.

---

# IsUuid7(Guid)

The `IsUuid7` method determines whether a supplied `Guid` is a UUIDv7.

Signature:

```csharp
bool IsUuid7(Guid guid)
```

Example:

```csharp
Guid id = Uuid7.NewGuid();

bool result =
    Uuid7.IsUuid7(id);

Console.WriteLine(result);
```

Output:

```text
true
```

---

# Example With Standard GUID

```csharp
Guid id = Guid.NewGuid();

bool result =
    Uuid7.IsUuid7(id);

Console.WriteLine(result);
```

Output:

```text
false
```

---

# How UUID Versions Work

RFC 9562 specifies that the UUID version number is stored inside:

```text
Byte 6
```

More specifically:

```text
Most Significant 4 Bits
```

also called:

```text
High Nibble
```

Structure:

```text
Byte 6

+---+---+---+---+---+---+---+---+
| V | V | V | V | x | x | x | x |
+---+---+---+---+---+---+---+---+
```

Where:

```text
V = Version Bits
```

---

# UUIDv7 Version Bits

For UUIDv7:

```text
0111
```

Binary:

```text
0x7
```

Therefore, a UUID is considered UUIDv7 when:

```text
High Nibble == 0x7
```

---

# Internal Validation Workflow

The validator follows the workflow below:

```text
Receive Guid
      │
      ▼
Convert Guid To Bytes
      │
      ▼
Read Byte 6
      │
      ▼
Extract Upper Four Bits
      │
      ▼
Version == 7 ?
   ┌────┴─────┐
   │          │
  Yes         No
   │          │
   ▼          ▼
 Return      Return
  True        False
```

---

# Internal Implementation

Implementation:

```csharp
Span<byte> bytes =
    stackalloc byte[16];

GuidByteConverter.ToBytes(
    guid,
    bytes);

return (bytes[6] >> 4) == 0x7;
```

---

# Understanding The Implementation

## Step 1: Allocate Buffer

```csharp
Span<byte> bytes =
    stackalloc byte[16];
```

UUIDs always contain:

```text
16 bytes
```

A stack allocation avoids heap allocations and improves performance.

---

## Step 2: Convert Guid To Bytes

Implementation:

```csharp
GuidByteConverter.ToBytes(
    guid,
    bytes);
```

Example UUID:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

becomes:

```text
01 97 D8 EF F4 C6 7A 35 99 E3 1D 4A 7E 3C 8E 10
```

---

## Step 3: Extract Version Bits

Implementation:

```csharp
bytes[6] >> 4
```

Suppose:

```text
bytes[6] = 0x7A
```

Binary:

```text
01111010
```

Right shifting by four positions:

```text
01111010 >> 4
```

Result:

```text
00000111
```

which equals:

```text
0x7
```

Therefore:

```text
UUID Version = 7
```

---

# What Is A Nibble?

A nibble represents:

```text
4 bits
```

One byte contains:

```text
8 bits
```

Structure:

```text
+--------+--------+
| Nibble | Nibble |
+--------+--------+
```

Example:

```text
01111010
```

contains:

```text
0111 -> High Nibble
1010 -> Low Nibble
```

Only the high nibble stores the UUID version.

---

# IsValid(string)

This method validates textual input.

Signature:

```csharp
bool IsValid(string input)
```

Unlike `IsUuid7`, this method accepts:

```text
Strings
```

instead of:

```text
Guid objects
```

---

# Validation Workflow For Strings

```text
Receive String
      │
      ▼
Extract UUID From Text
      │
      ▼
UUID Found?
   ┌────┴────┐
   │         │
  No        Yes
   │         │
   ▼         ▼
Return    Validate
False     UUID Version
              │
              ▼
         Version == 7 ?
           ┌───┴───┐
           │       │
          Yes      No
           │       │
           ▼       ▼
        True      False
```

---

# Internal Implementation

```csharp
return Uuid7Parser.TryParse(
    input,
    out Guid guid)
    && IsUuid7(guid);
```

This implementation performs two operations:

1. Parse the input.
2. Validate the UUID version.

---

# Example: Embedded UUID

```csharp
string value =
    "ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10";

bool result =
    Uuid7.IsValid(value);
```

Output:

```text
true
```

---

# Example: Invalid Input

```csharp
bool result =
    Uuid7.IsValid("Hello World");
```

Output:

```text
false
```

---

# Example: UUIDv4 Input

```csharp
bool result =
    Uuid7.IsValid(
        "550e8400-e29b-41d4-a716-446655440000");
```

Output:

```text
false
```

Reason:

```text
Version = 4
```

---

# Null Handling

The validator relies on:

```csharp
Uuid7Parser.TryParse(...)
```

The parser throws:

```text
ArgumentNullException
```

when:

```csharp
input == null
```

Example:

```csharp
Uuid7.IsValid(null!);
```

Produces:

```text
ArgumentNullException
```

---

# Real-World Use Cases

## API Validation

```csharp
if (!Uuid7.IsValid(request.Id))
{
    return BadRequest();
}
```

---

## Route Parameter Validation

```csharp
app.MapGet("/orders/{id}",
(Guid id) =>
{
    if (!Uuid7.IsUuid7(id))
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});
```

---

## Event Validation

```csharp
if (Uuid7.IsValid(message.CorrelationId))
{
    Process(message);
}
```

---

## Import Validation

```csharp
foreach (string row in csvRows)
{
    if (!Uuid7.IsValid(row))
    {
        continue;
    }
}
```

---

# Why Only Version Validation?

The validator intentionally checks only:

```text
UUID Version
```

It assumes that a supplied `Guid` already represents a structurally valid UUID.

Reason:

A .NET `Guid` object cannot exist in an invalid binary format.

Therefore, only version verification is required.

---

# Performance Characteristics

The validator is extremely fast because it:

* Allocates only 16 bytes on the stack.
* Performs no heap allocations.
* Uses simple bit operations.
* Executes in constant time `O(1)`.

Complexity:

```text
Time Complexity: O(1)
Space Complexity: O(1)
```

---

# Advantages Of UUID Validation

## Prevents Invalid Processing

Avoids calling UUIDv7-specific APIs on non-UUIDv7 values.

---

## Improves Reliability

Ensures only supported identifiers enter the system.

---

## Protects Timestamp Extraction

Timestamp extraction requires genuine UUIDv7 values.

Validation prevents incorrect results.

---

## Simplifies Input Validation

Applications can easily validate external input.

---

## Improves API Safety

Invalid identifiers can be rejected immediately.

---

# Roslyn Analyzer Support

Dixor.Identity ships with:

```text
Uuid7ValidatorAnalyzer
```

---

# Current Analyzer Behavior

The analyzer currently performs:

```text
No compile-time analysis
```

Implementation:

```csharp
SupportedDiagnostics => [];
```

No diagnostics are currently reported.

---

# Why Does The Analyzer Exist?

The analyzer acts as a future extension point.

Possible future diagnostics include:

| Scenario                   | Diagnostic    |
| -------------------------- | ------------- |
| Constant non-UUIDv7 values | Warning       |
| Invalid UUID literals      | Error         |
| Redundant validation       | Suggestion    |
| Misuse of validation APIs  | Informational |

Maintaining a dedicated analyzer ensures consistency across the Dixor.Identity analyzer ecosystem.

---

# Best Practices

## Validate External Input

Always validate user-supplied identifiers.

Recommended:

```csharp
if (!Uuid7.IsValid(input))
{
    return;
}
```

---

## Use `IsUuid7` For Existing Guid Values

```csharp
bool valid =
    Uuid7.IsUuid7(guid);
```

---

## Validate Before Extracting Timestamps

```csharp
if (Uuid7.IsUuid7(id))
{
    DateTimeOffset timestamp =
        Uuid7.GetTimestamp(id);
}
```

---

# Summary

The Dixor.Identity validation APIs allow developers to determine whether values represent genuine RFC 9562 UUID version 7 identifiers.

Key benefits include:

* Fast validation.
* RFC 9562 compliance.
* Improved reliability.
* Safer APIs.
* Protection against invalid input.
* Efficient bit-level verification.

Validation is a fundamental building block for safely using advanced UUIDv7 capabilities throughout an application.

---

# Related Documentation

* [Formatting](./UUID7_Formatting.md)
* [Parsing](./UUID7_Parsing.md)
* [Timestamp Extraction](./UUID7_TimestampExtraction.md)

---

## Navigation

⬅ Previous: [Formatting](./UUID7_Formatting.md)

➡ Next: [Parsing](./UUID7_Parsing.md)