# UUIDv7 Formatting

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [UUIDv7 Overview](./UUID7_Overview.md)

---

Dixor.Identity provides built-in support for formatting UUID version 7 identifiers with optional prefixes, suffixes, and custom separators.

Formatting allows developers to transform raw UUIDv7 values into business-friendly identifiers that are easier to understand, categorize, and integrate into existing systems.

The public API exposes this functionality through:

```csharp
string Uuid7.NewString(
    string? prefix = null,
    string? suffix = null,
    string separator = "-");
```

Internally, this functionality is implemented by the `Uuid7Formatter` type.

---

# Why Formatting Matters

A raw UUID typically looks like this:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

Although globally unique, it provides little business context.

Many applications prefer identifiers such as:

```text
ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

or

```text
INV_0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10_US
```

These identifiers are easier for developers, administrators, and support teams to understand.

---

# Common Use Cases

Formatting is particularly useful in:

* Order management systems.
* Invoice systems.
* ERP applications.
* Warehouse management systems.
* Distributed microservices.
* Logging and diagnostics.
* Multi-tenant applications.
* Event sourcing systems.

Examples:

| Identifier        | Meaning                    |
| ----------------- | -------------------------- |
| `ORD-0197...`     | Order Identifier           |
| `INV-0197...`     | Invoice Identifier         |
| `USR-0197...`     | User Identifier            |
| `PAY-0197...`     | Payment Identifier         |
| `TENANT1-0197...` | Tenant-specific identifier |

---

# Basic Usage

Generate a formatted UUIDv7:

```csharp
string value =
    Uuid7.NewString(
        prefix: "ORD");
```

Output:

```text
ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

# Method Signature

```csharp
public static string NewString(
    string? prefix = null,
    string? suffix = null,
    string separator = "-")
```

Internally:

```csharp
internal static string Format(
    Guid guid,
    string? prefix = null,
    string? suffix = null,
    string separator = "-")
```

---

# Parameters

## guid

The UUIDv7 value to format.

Example:

```csharp
Guid id = Uuid7.NewGuid();
```

---

## prefix

An optional value prepended to the UUID.

If the prefix is:

* `null`
* Empty string (`""`)
* Whitespace (`" "`)

no prefix is added.

Example:

```csharp
Uuid7.NewString(prefix: "ORD");
```

Produces:

```text
ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

## suffix

An optional value appended after the UUID.

Example:

```csharp
Uuid7.NewString(
    suffix: "US");
```

Produces:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10-US
```

---

## separator

The separator inserted between the UUID and the prefix or suffix.

Default value:

```text
-
```

Example:

```csharp
Uuid7.NewString(
    prefix: "ORD",
    separator: "_");
```

Produces:

```text
ORD_0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

# Formatting Examples

## Prefix Only

```csharp
string id =
    Uuid7.NewString(
        prefix: "ORD");
```

Output:

```text
ORD-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

## Suffix Only

```csharp
string id =
    Uuid7.NewString(
        suffix: "US");
```

Output:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10-US
```

---

## Prefix and Suffix

```csharp
string id =
    Uuid7.NewString(
        prefix: "INV",
        suffix: "EU");
```

Output:

```text
INV-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10-EU
```

---

## Custom Separator

```csharp
string id =
    Uuid7.NewString(
        prefix: "ORD",
        suffix: "PSA",
        separator: "_");
```

Output:

```text
ORD_0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10_PSA
```

---

# Supported Separators

Dixor.Identity intentionally restricts separators to a known safe set.

Supported separators:

| Character |   |
| --------- | - |
| `-`       |   |
| `_`       |   |
| `:`       |   |
| `~`       |   |
| `         | ` |
| `{`       |   |
| `}`       |   |
| `[`       |   |
| `]`       |   |
| `<`       |   |
| `>`       |   |
| `/`       |   |
| `\`       |   |

Examples:

```csharp
Uuid7.NewString("ORD", separator: "-");
Uuid7.NewString("ORD", separator: "_");
Uuid7.NewString("ORD", separator: ":");
Uuid7.NewString("ORD", separator: "~");
```

---

# Why Are Separators Restricted?

A beginner might ask:

> Why not allow every possible character?

Certain characters can create problems in:

* Databases.
* URLs.
* File systems.
* APIs.
* Logging systems.
* Message queues.
* Shell scripts.

For example:

```text
?
*
&
#
%
$
```

may require escaping or may be interpreted differently across systems.

Restricting separators ensures:

* Consistent formatting.
* Better interoperability.
* Reduced bugs.
* Safer identifiers.

---

# Prefix Length Limit

Maximum supported prefix length:

```text
50 characters
```

Valid:

```csharp
Uuid7.NewString(
    prefix: "CUSTOMER");
```

Invalid:

```csharp
string prefix = new string('A', 51);

Uuid7.NewString(prefix: prefix);
```

Throws:

```text
ArgumentOutOfRangeException
```

---

# Why Is Prefix Length Limited?

Extremely large prefixes can:

* Increase memory consumption.
* Produce unreadable identifiers.
* Exceed database column limits.
* Reduce usability.

Example:

```text
CUSTOMER-SALES-DEPARTMENT-EUROPE-DIVISION-
0197...
```

Such identifiers become difficult to manage.

A 50-character limit provides flexibility while preventing abuse.

---

# Suffix Length Limit

Maximum supported suffix length:

```text
50 characters
```

Example:

```csharp
Uuid7.NewString(
    suffix: "PRODUCTION");
```

Anything exceeding fifty characters produces:

```text
ArgumentOutOfRangeException
```

---

# Null and Whitespace Handling

The formatter gracefully ignores:

```csharp
null
""
"   "
```

Examples:

```csharp
Uuid7.NewString(prefix: null);
```

```csharp
Uuid7.NewString(prefix: "");
```

```csharp
Uuid7.NewString(prefix: "   ");
```

All produce:

```text
0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
```

---

# Internal Implementation

Internally the formatter follows this workflow:

```text
Validate Separator
        │
        ▼
Validate Prefix Length
        │
        ▼
Validate Suffix Length
        │
        ▼
Convert Guid to String
        │
        ▼
Append Prefix
        │
        ▼
Append Suffix
        │
        ▼
Return Result
```

Simplified implementation:

```csharp
string value = guid.ToString();

if (!string.IsNullOrWhiteSpace(prefix))
{
    value = string.Concat(
        prefix,
        separator,
        value);
}

if (!string.IsNullOrWhiteSpace(suffix))
{
    value = string.Concat(
        value,
        separator,
        suffix);
}

return value;
```

---

# Why `string.Concat` Is Used

The implementation uses:

```csharp
string.Concat(...)
```

instead of:

```csharp
prefix + separator + value
```

because:

* It avoids unnecessary allocations.
* It improves performance.
* It reduces garbage collection pressure.

This aligns with the performance goals of Dixor.Identity.

---

# Exception Handling

## ArgumentOutOfRangeException

Thrown when:

* Prefix length exceeds 50 characters.
* Suffix length exceeds 50 characters.

Example:

```csharp
Uuid7.NewString(
    prefix: new string('A', 100));
```

---

## ArgumentException

Thrown when:

* Separator is null.
* Separator is empty.
* Separator contains multiple characters.
* Separator is unsupported.

Examples:

```csharp
Uuid7.NewString(separator: "");
```

```csharp
Uuid7.NewString(separator: "##");
```

```csharp
Uuid7.NewString(separator: "*");
```

---

# Roslyn Analyzer Support

Dixor.Identity includes a built-in Roslyn analyzer named:

```text
Uuid7FormatAnalyzer
```

The analyzer validates formatting arguments during compilation.

Diagnostics are only reported when values are known at compile time.

This means many formatting mistakes can be detected before the application runs.

---

# Supported Diagnostics

| Diagnostic | Description                             |
| ---------- | --------------------------------------- |
| `DIXOR002` | Prefix exceeds maximum supported length |
| `DIXOR003` | Suffix exceeds maximum supported length |
| `DIXOR004` | Invalid separator supplied              |

---

# Why Compile-Time Validation Matters

Without analyzers:

```csharp
Uuid7.NewString(separator: "*");
```

The application compiles.

The exception only occurs at runtime.

With analyzers enabled:

```text
DIXOR004:
Separator '*' is not supported.
```

Developers receive immediate feedback inside the IDE.

---

# Invalid Examples

## Invalid Prefix

```csharp
Uuid7.NewString(
    prefix: "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
```

Produces:

```text
DIXOR002
```

---

## Invalid Suffix

```csharp
Uuid7.NewString(
    suffix: "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
```

Produces:

```text
DIXOR003
```

---

## Invalid Separator

```csharp
Uuid7.NewString(
    separator: "*");
```

Produces:

```text
DIXOR004
```

---

## Invalid Multi-Character Separator

```csharp
Uuid7.NewString(
    separator: "--");
```

Produces:

```text
DIXOR004
```

---

# Compile-Time Constant Analysis

The analyzer only validates constants.

This produces diagnostics:

```csharp
const string Separator = "*";

Uuid7.NewString(
    separator: Separator);
```

This does not:

```csharp
string separator =
    Console.ReadLine();

Uuid7.NewString(
    separator: separator);
```

Reason:

The value is unknown during compilation.

Runtime validation still guarantees correctness.

---

# Analyzer Workflow

```text
Method Invocation
        │
        ▼
Is method Format or NewString?
        │
       Yes
        │
        ▼
Belongs to Uuid7?
        │
       Yes
        │
        ▼
Inspect Arguments
        │
        ▼
Compile-time Constant?
        │
       Yes
        │
        ▼
Validate:
- Prefix Length
- Suffix Length
- Separator
        │
        ▼
Report Diagnostics
```

---

# Advantages of UUID Formatting

## Improved Readability

```text
ORD-0197...
```

is easier to understand than:

```text
0197...
```

---

## Business Context

Identifiers immediately communicate their purpose.

Examples:

```text
ORD-...
INV-...
PAY-...
USR-...
```

---

## Better Logging

Logs become easier to analyze:

```text
Order Created:
ORD-0197...
```

instead of:

```text
0197...
```

---

## Easier Troubleshooting

Support teams can quickly identify entity types.

---

## Multi-Tenant Support

Example:

```text
TENANT1-0197...
TENANT2-0197...
```

---

## Improved System Integration

External systems often require business-friendly identifiers.

Formatting makes integration easier.

---

# Best Practices

## Keep Prefixes Short

Good:

```text
ORD
INV
USR
PAY
```

Avoid:

```text
CUSTOMER_ORDER_PROCESSING_DEPARTMENT
```

---

## Use Consistent Naming

Choose one convention across the application.

Example:

```text
ORD-...
INV-...
USR-...
```

---

## Use Safe Separators

Recommended:

```text
-
_
:
```

---

## Avoid Business Logic in Suffixes

Good:

```text
US
EU
DEV
```

Avoid:

```text
CUSTOMER_HAS_PENDING_PAYMENT
```

---

# Summary

UUID formatting enables developers to create business-friendly identifiers while preserving all uniqueness guarantees of UUIDv7.

Features include:

* Prefix support.
* Suffix support.
* Custom separators.
* Compile-time validation.
* Runtime validation.
* Safe formatting rules.
* Performance-optimized implementation.

Formatting helps bridge the gap between technical identifiers and real-world business requirements.

---

# Related Documentation

* [Batch Generation](./UUID7_BatchGeneration.md)
* [Validation](./UUID7_Validation.md)
* [DIXOR002](../analyzers/DIXOR002.md)
* [DIXOR003](../analyzers/DIXOR003.md)
* [DIXOR010](../analyzers/DIXOR010.md)

---

## Navigation

⬅ Previous: [Batch Generation](./UUID7_BatchGeneration.md)

➡ Next: [Validation](./UUID7_Validation.md)