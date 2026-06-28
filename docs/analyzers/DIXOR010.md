# DIXOR010 - Invalid Separator

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Analyzers Overview](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/Analyzers_Overview.md)

---

## Diagnostic Information

| Property      | Value               |
| ------------- | ------------------- |
| Diagnostic ID | DIXOR010            |
| Category      | Usage               |
| Severity      | Error               |
| Default State | Enabled             |
| Analyzer      | Uuid7FormatAnalyzer |

---

# Summary

`DIXOR010` is reported when an unsupported separator is supplied to:

```csharp
Uuid7.Format(...)
Uuid7.NewString(...)
```

---

# Why This Diagnostic Exists

Dixor.Identity intentionally restricts separator characters to a predefined set.

This ensures:

* Consistent formatting.
* Predictable parsing.
* Better interoperability.
* Safer identifier generation.

---

# Supported Separators

The following separators are supported:

```text
-
:
~
|
{
}
[
]
<
>
/
\
_
```

---

# Rule Description

The analyzer reports this diagnostic when:

* The separator is known at compile time.
* The separator contains more than one character.

or

* The separator is not included in the supported separator list.

---

# Example

## Invalid

```csharp
string id =
    Uuid7.NewString(
        separator: "**");
```

Compiler output:

```text
error DIXOR010:
Separator '**' is not supported.
```

---

## Also Invalid

```csharp
Uuid7.NewString(separator: "@");
```

---

## Valid

```csharp
Uuid7.NewString(separator: "-");
```

```csharp
Uuid7.NewString(separator: "_");
```

```csharp
Uuid7.NewString(separator: ":");
```

---

# Separator Rules

A separator must:

1. Contain exactly one character.
2. Be part of the supported separator list.

Examples:

| Separator | Valid |
| --------- | ----- |
| `-`       | ✔     |
| `_`       | ✔     |
| `:`       | ✔     |
| `@`       | ✘     |
| `**`      | ✘     |
| `##`      | ✘     |

---

# Runtime Behavior

Without analyzer support:

```csharp
Uuid7.NewString(separator: "@");
```

throws:

```text
ArgumentException
```

---

# Non-Constant Values

Example:

```csharp
string separator =
    Configuration["Separator"];

Uuid7.NewString(separator: separator);
```

No diagnostic is reported because the value is not known during compilation.

Runtime validation still occurs.

---

# How To Fix

Replace unsupported separators with supported values.

Example:

```csharp
Uuid7.NewString(separator: "-");
```

or

```csharp
Uuid7.NewString(separator: "_");
```

---

# When To Suppress

Suppression is generally not recommended.

Unsupported separators are rejected at runtime and therefore suppressing the diagnostic rarely provides value.

---

# Related Documentation

* [UUID7 Formatting](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Formatting.md)
* [DIXOR003 - Suffix Exceeds Maximum Length](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR003.md)
* [Suppressing Diagnostics](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/SuppressingDiagnostics.md)
* [Analyzers Overview](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/Analyzers_Overview.md)

---

## Navigation

⬅ Previous: [DIXOR003 - Suffix Exceeds Maximum Length](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR003.md)

➡ Next: [Suppressing Diagnostics](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/SuppressingDiagnostics.md)
