# UUID Parsing

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [UUIDv7 Overview](./UUID7_Overview.md)

---

Dixor.Identity provides functionality for extracting and parsing UUID values from arbitrary text.

Unlike the standard .NET `Guid.Parse()` API, which expects the input string to contain only a UUID, the Dixor.Identity parser can locate and extract UUIDs embedded inside larger strings.

The public API exposes this functionality through:

```csharp
Guid Uuid7.Parse(string input)

bool Uuid7.TryParse(
    string input,
    out Guid guid)
```

Internally, parsing is implemented by:

```csharp
Uuid7Parser
```

---

# Why Does This Feature Exist?

In real-world applications, UUID values are rarely stored as plain strings.

Developers often decorate identifiers with additional information.

Examples:

```text
ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

```text
Customer_0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10_US
```

```text
Invoice generated successfully.
Invoice Id:
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

```text
[INFO]
Request completed.
CorrelationId=0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

The standard .NET parser:

```csharp
Guid.Parse(...)
```

cannot parse these values directly because the input contains additional text.

Dixor.Identity solves this problem by automatically searching for UUID values inside the supplied text.

---

# Supported UUID Format

The parser currently supports UUIDs in the canonical textual representation:

```text
xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```

Example:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

This format contains:

| Section | Length        |
| ------- | ------------- |
| Part 1  | 8 characters  |
| Part 2  | 4 characters  |
| Part 3  | 4 characters  |
| Part 4  | 4 characters  |
| Part 5  | 12 characters |

Total:

```text
36 characters
```

including hyphens.

---

# Parsing Workflow

The parser follows the workflow below:

```text
Receive Input String
          │
          ▼
Validate Input Is Not Null
          │
          ▼
Search Input Using Regular Expression
          │
          ▼
UUID Found?
      ┌───┴────┐
      │        │
     Yes       No
      │        │
      ▼        ▼
Extract     Return False
Text        Or Throw Exception
      │
      ▼
Convert To Guid
      │
      ▼
Return Parsed UUID
```

---

# Parse Method

The `Parse` method extracts and returns the first UUID found in the input.

Signature:

```csharp
Guid Parse(string input)
```

Example:

```csharp
string text =
    "Order Id: 0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10";

Guid id =
    Uuid7.Parse(text);
```

Result:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

# Parse Behavior

The method:

1. Searches the input.
2. Locates the first UUID.
3. Converts it into a `Guid`.
4. Returns the parsed value.

If no UUID is found:

```csharp
Uuid7.Parse("Hello World");
```

the method throws:

```text
FormatException
```

---

# Parse Example

```csharp
string input =
    "Customer Id: CUST-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10";

Guid customerId =
    Uuid7.Parse(input);

Console.WriteLine(customerId);
```

Output:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

# TryParse Method

The `TryParse` method attempts to extract a UUID without throwing exceptions.

Signature:

```csharp
bool TryParse(
    string input,
    out Guid guid)
```

Example:

```csharp
string text =
    "Request Id: 0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10";

if (Uuid7.TryParse(text, out Guid id))
{
    Console.WriteLine(id);
}
```

---

# Why Use TryParse?

Exceptions are relatively expensive.

In scenarios where parsing failures are expected, using `TryParse` is preferred.

Examples:

* User input.
* API requests.
* Log processing.
* File parsing.
* Message queues.

Benefits:

* Better performance.
* Cleaner control flow.
* No exception handling required.

---

# Parse vs TryParse

| Feature                    | Parse     | TryParse            |
| -------------------------- | --------- | ------------------- |
| Returns Guid               | Yes       | Via `out` parameter |
| Throws Exceptions          | Yes       | No                  |
| Suitable For User Input    | No        | Yes                 |
| Suitable For Internal Data | Yes       | Yes                 |
| Failure Handling           | Exception | Boolean Result      |

---

# Extracting UUIDs From Logs

One common scenario is log analysis.

Example:

```csharp
string log =
    """
    [INFO]
    Request completed successfully.
    CorrelationId=0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
    """;

Guid correlationId =
    Uuid7.Parse(log);
```

---

# Extracting UUIDs From Business Identifiers

Example:

```csharp
string invoiceId =
    "INV-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10";

Guid id =
    Uuid7.Parse(invoiceId);
```

---

# Extracting UUIDs From API Responses

Example:

```csharp
string response =
    """
    {
        "id":
        "0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10"
    }
    """;

Guid id =
    Uuid7.Parse(response);
```

---

# Internal Implementation

Internally, the parser uses:

```csharp
Regex
```

to locate UUID values.

Implementation:

```csharp
private static readonly Regex GuidRegex =
    new(
        @"[0-9a-fA-F]{8}-" +
        @"[0-9a-fA-F]{4}-" +
        @"[0-9a-fA-F]{4}-" +
        @"[0-9a-fA-F]{4}-" +
        @"[0-9a-fA-F]{12}",
        RegexOptions.Compiled);
```

---

# Understanding The Regular Expression

The expression:

```text
[0-9a-fA-F]{8}
```

means:

> Match exactly eight hexadecimal characters.

Hexadecimal characters include:

```text
0 1 2 3 4 5 6 7 8 9
a b c d e f
A B C D E F
```

---

The complete expression:

```text
[0-9a-fA-F]{8}-
[0-9a-fA-F]{4}-
[0-9a-fA-F]{4}-
[0-9a-fA-F]{4}-
[0-9a-fA-F]{12}
```

matches:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

but does not match:

```text
0197d8ef
```

or:

```text
Hello World
```

---

# Why Is `RegexOptions.Compiled` Used?

Implementation:

```csharp
RegexOptions.Compiled
```

This instructs .NET to compile the regular expression into executable code.

Benefits:

* Faster repeated execution.
* Better throughput.
* Improved performance for bulk parsing.

This is especially beneficial when processing:

* Large log files.
* Millions of records.
* Batch imports.
* Streaming data.

---

# First Match Behavior

The parser returns only the first UUID found.

Example:

```csharp
string input =
    """
    First:
    0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10

    Second:
    0197d8ef-f4c7-72af-bf71-6f3d9ef3f201
    """;

Guid id =
    Uuid7.Parse(input);
```

Result:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

Only the first match is returned.

---

# Null Validation

Both APIs validate input.

Example:

```csharp
Uuid7.Parse(null!);
```

Produces:

```text
ArgumentNullException
```

Implementation:

```csharp
ArgumentNullException.ThrowIfNull(input);
```

Benefits:

* Early validation.
* Clear error messages.
* Improved reliability.

---

# Failure Scenarios

## No UUID Found

Input:

```csharp
Uuid7.Parse("Hello World");
```

Result:

```text
FormatException
```

---

## Invalid UUID

Input:

```text
123-invalid-guid
```

Result:

```text
FormatException
```

---

## Null Input

Input:

```csharp
Uuid7.Parse(null!);
```

Result:

```text
ArgumentNullException
```

---

# Advantages Of Dixor.Identity Parsing

## Supports Embedded UUIDs

Example:

```text
ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

can be parsed directly.

---

## Simplifies Log Processing

No manual string manipulation is required.

---

## Reduces Boilerplate Code

Without Dixor.Identity:

```csharp
string[] parts = value.Split('-');
// Additional processing...
```

With Dixor.Identity:

```csharp
Guid id = Uuid7.Parse(value);
```

---

## High Performance

Compiled regular expressions provide efficient repeated parsing.

---

## Developer Friendly

Works naturally with:

* Logs
* Messages
* Business identifiers
* APIs
* Text files

---

# Roslyn Analyzer Support

Dixor.Identity ships with:

```text
Uuid7ParserAnalyzer
```

---

# Current Analyzer Behavior

The analyzer currently reports:

```text
No diagnostics
```

Implementation:

```csharp
SupportedDiagnostics =>
    ImmutableArray<DiagnosticDescriptor>.Empty;
```

No compile-time analysis is currently performed.

---

# Why Does The Analyzer Exist?

Parsing operations typically occur on runtime data.

Because runtime values are not known during compilation, meaningful analysis opportunities are currently limited.

However, maintaining a dedicated analyzer provides:

* Architectural consistency.
* Future extensibility.
* Stable analyzer infrastructure.

---

# Potential Future Analyzer Features

Future versions may introduce diagnostics for:

| Scenario                         | Possible Diagnostic |
| -------------------------------- | ------------------- |
| Invalid constant UUID string     | Warning             |
| Unhandled `Parse` exceptions     | Suggestion          |
| Prefer `TryParse` for user input | Informational       |
| Invalid UUID literals            | Error               |

---

# Best Practices

## Use `TryParse` For External Input

Recommended:

```csharp
if (Uuid7.TryParse(input, out Guid id))
{
    // Use id
}
```

---

## Use `Parse` When Failure Is Exceptional

Example:

```csharp
Guid id =
    Uuid7.Parse(databaseValue);
```

---

## Validate User Input

Always assume external data may be invalid.

---

## Avoid Catching Exceptions Repeatedly

Prefer:

```csharp
TryParse()
```

inside loops.

---

# Summary

The Dixor.Identity parser enables extraction of UUID values from arbitrary text.

Key benefits include:

* Embedded UUID support.
* Simplified parsing.
* High performance.
* Reduced boilerplate.
* Improved developer productivity.
* Efficient log and message processing.

The parser is designed to make UUID extraction simple, fast, and reliable across a wide variety of real-world scenarios.

---

# Related Documentation

* [Validation](./UUID7_Validation.md)
* [Timestamp Extraction](./UUID7_TimestampExtraction.md)
* [Generation](./UUID7_Generation.md)

---

## Navigation

⬅ Previous: [Validation](./UUID7_Validation.md)

➡ Next: [Timestamp Extraction](./UUID7_TimestampExtraction.md)