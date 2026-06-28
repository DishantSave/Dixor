# DIXOR001 - Invalid UUID Batch Size

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Analyzers Overview](./Analyzers_Overview.md)

---

## Diagnostic Information

| Property      | Value                   |
| ------------- | ----------------------- |
| Diagnostic ID | DIXOR001                |
| Category      | Usage                   |
| Severity      | Error                   |
| Default State | Enabled                 |
| Analyzer      | Uuid7BatchCountAnalyzer |

---

# Summary

`DIXOR001` is reported when an invalid batch size is supplied to:

```csharp
Uuid7.NewGuids(...)
```

The batch size must always be greater than zero.

Passing a value less than or equal to zero is not supported.

---

# Why This Diagnostic Exists

Batch generation APIs are designed to generate one or more UUIDs.

Generating zero or a negative number of UUIDs is invalid and would eventually cause an exception at runtime.

The analyzer detects this issue during compilation, preventing runtime failures.

---

# Rule Description

The analyzer reports `DIXOR001` when:

* The method being called is `Uuid7.NewGuids(...)`.
* The supplied count is a compile-time constant.
* The count is less than or equal to zero.

---

# Example

## Invalid

```csharp
var ids = Uuid7.NewGuids(0);
```

Compiler output:

```text
error DIXOR001:
The count argument passed to 'Uuid7.NewGuids'
must be greater than zero.
```

---

## Also Invalid

```csharp
var ids = Uuid7.NewGuids(-10);
```

---

## Valid

```csharp
var ids = Uuid7.NewGuids(100);
```

---

# Runtime Behavior

Without analyzer support:

```csharp
Uuid7.NewGuids(0);
```

throws:

```text
ArgumentOutOfRangeException
```

The analyzer prevents this issue from reaching runtime.

---

# Non-Constant Values

The analyzer only validates compile-time constants.

Example:

```csharp
int count = GetBatchSize();

Uuid7.NewGuids(count);
```

No diagnostic is reported because the compiler cannot determine the value.

Runtime validation still occurs.

---

# How To Fix

Use a value greater than zero.

```csharp
var ids = Uuid7.NewGuids(1);
```

or

```csharp
var ids = Uuid7.NewGuids(500);
```

---

# When To Suppress

Suppression is rarely appropriate.

If the batch size is intentionally dynamic:

```csharp
int count = GetCount();

Uuid7.NewGuids(count);
```

no suppression is required because no diagnostic will be reported.

---

# Related Documentation

* [Analyzers Overview](./Analyzers_Overview.md)
* [DIXOR002 - Prefix Exceeds Maximum Length](./DIXOR002.md)
* [Suppressing Diagnostics](./SuppressingDiagnostics.md)
* [Batch Generation](../uuid7/UUID7_BatchGeneration.md)

---

## Navigation

⬅ Previous: [Analyzers Overview](./Analyzers_Overview.md)

➡ Next: [DIXOR002 - Prefix Exceeds Maximum Length](./DIXOR002.md)