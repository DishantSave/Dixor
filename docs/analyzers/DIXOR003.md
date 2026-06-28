# DIXOR003 - Suffix Exceeds Maximum Length

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Analyzers Overview](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/Analyzers_Overview.md)

---

## Diagnostic Information

| Property      | Value               |
| ------------- | ------------------- |
| Diagnostic ID | DIXOR003            |
| Category      | Usage               |
| Severity      | Warning             |
| Default State | Enabled             |
| Analyzer      | Uuid7FormatAnalyzer |

---

# Summary

`DIXOR003` is reported when a suffix supplied to:

```csharp
Uuid7.Format(...)
Uuid7.NewString(...)
```

exceeds the maximum supported length of 50 characters.

---

# Why This Diagnostic Exists

Suffixes should remain concise.

Excessively long suffixes:

* Reduce readability.
* Increase storage size.
* Produce cumbersome identifiers.
* Negatively impact logging and debugging.

---

# Rule Description

The analyzer reports this diagnostic when:

* The argument is named `suffix`.
* The value is known at compile time.
* The length exceeds 50 characters.

---

# Example

## Invalid

```csharp
string id =
    Uuid7.NewString(
        suffix:
            "ThisSuffixExceedsTheMaximumLengthSupportedByTheLibrary");
```

Compiler output:

```text
warning DIXOR003:
The suffix exceeds the maximum supported length of 50 characters.
```

---

## Valid

```csharp
string id =
    Uuid7.NewString(
        suffix: "EU");
```

---

# Runtime Behavior

Without analyzer support:

```csharp
Uuid7.NewString(suffix: longSuffix);
```

throws:

```text
ArgumentOutOfRangeException
```

---

# Non-Constant Values

Example:

```csharp
string suffix =
    Configuration["Suffix"];

Uuid7.NewString(suffix: suffix);
```

No diagnostic is reported because the value is unknown during compilation.

---

# How To Fix

Use shorter suffix values.

Examples:

```text
EU
US
PROD
DEV
TEST
```

---

# When To Suppress

Suppression may be appropriate for compatibility with external systems.

---

# Related Documentation

* [UUID7 Formatting](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Formatting.md)
* [DIXOR002 - Prefix Exceeds Maximum Length](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR002.md)
* [DIXOR010 - Invalid Separator](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR010.md)
* [Suppressing Diagnostics](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/SuppressingDiagnostics.md)
* [Analyzers Overview](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/Analyzers_Overview.md)

---

## Navigation

⬅ Previous: [DIXOR002 - Prefix Exceeds Maximum Length](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR002.md)

➡ Next: [DIXOR010 - Invalid Separator](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR010.md)
