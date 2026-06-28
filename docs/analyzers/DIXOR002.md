# DIXOR002 - Prefix Exceeds Maximum Length

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Analyzers Overview](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/Analyzers_Overview.md)

---


## Diagnostic Information

| Property      | Value               |
| ------------- | ------------------- |
| Diagnostic ID | DIXOR002            |
| Category      | Usage               |
| Severity      | Warning             |
| Default State | Enabled             |
| Analyzer      | Uuid7FormatAnalyzer |

---

# Summary

`DIXOR002` is reported when a prefix supplied to:

```csharp
Uuid7.Format(...)
Uuid7.NewString(...)
```

exceeds the maximum supported length of 50 characters.

---

# Why This Diagnostic Exists

Prefixes are intended to provide concise business context.

Excessively long prefixes:

* Reduce readability.
* Increase storage requirements.
* Produce unnecessarily large identifiers.
* Make logs and diagnostics difficult to read.

---

# Rule Description

The analyzer reports this diagnostic when:

* The argument is named `prefix`.
* The value is known at compile time.
* The length exceeds 50 characters.

---

# Example

## Invalid

```csharp
string id =
    Uuid7.NewString(
        prefix:
            "ThisPrefixIsFarLongerThanTheMaximumAllowedLengthSupported");
```

Compiler output:

```text
warning DIXOR002:
The prefix exceeds the maximum supported length of 50 characters.
```

---

## Valid

```csharp
string id =
    Uuid7.NewString(
        prefix: "ORD");
```

---

# Maximum Supported Length

The maximum supported prefix length is:

```text
50 characters
```

---

# Runtime Behavior

If analyzer support is unavailable:

```csharp
Uuid7.NewString(prefix: veryLongPrefix);
```

throws:

```text
ArgumentOutOfRangeException
```

---

# Non-Constant Values

The analyzer only evaluates compile-time constants.

```csharp
string prefix = configuration["Prefix"];

Uuid7.NewString(prefix: prefix);
```

No diagnostic is reported.

Runtime validation still occurs.

---

# How To Fix

Use shorter prefixes.

Recommended examples:

```text
ORD
INV
USR
TXN
EMP
```

---

# Business Examples

```csharp
Uuid7.NewString(prefix: "ORDER");
Uuid7.NewString(prefix: "INVOICE");
Uuid7.NewString(prefix: "CUSTOMER");
```

---

# When To Suppress

Suppression may be appropriate when integrating with legacy systems that require long prefixes.

---

# Related Documentation

* [UUID7 Formatting](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Formatting.md)
* [DIXOR001 - Invalid Batch Size](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR001.md)
* [DIXOR003 - Suffix Exceeds Maximum Length](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR003.md)
* [Suppressing Diagnostics](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/SuppressingDiagnostics.md)
* [Analyzers Overview](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/Analyzers_Overview.md)

---

## Navigation

⬅ Previous: [DIXOR001 - Invalid Batch Size](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR001.md)

➡ Next: [DIXOR003 - Suffix Exceeds Maximum Length](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR003.md)
