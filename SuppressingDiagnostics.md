# Suppressing Diagnostics

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Analyzers Overview](./Analyzers_Overview.md)

---

Dixor.Identity includes built-in Roslyn analyzers that help identify incorrect API usage during development.

Although it is strongly recommended to keep diagnostics enabled, there may be situations where suppressing a diagnostic is necessary.

This document explains the various ways in which Dixor.Identity diagnostics can be suppressed and when suppression should be considered.

---

# What Is Diagnostic Suppression?

Diagnostic suppression is the process of instructing the compiler or IDE to ignore a specific analyzer rule.

For example, consider the following code:

```csharp
Uuid7.NewGuids(0);
```

The compiler reports:

```text
error DIXOR001:
The count argument passed to 'Uuid7.NewGuids'
must be greater than zero.
```

Suppressing the diagnostic prevents the compiler or IDE from reporting this issue.

---

# Should Diagnostics Be Suppressed?

In general, diagnostics should only be suppressed when there is a valid technical reason.

Most diagnostics indicate genuine problems that should be fixed rather than ignored.

Before suppressing a diagnostic, ask yourself:

* Is the reported issue intentional?
* Can the code be rewritten to satisfy the analyzer?
* Is the diagnostic a false positive?
* Does the diagnostic conflict with application requirements?

If the answer to all of these questions is **No**, the code should usually be corrected rather than suppressed.

---

# Recommended Approach

The recommended order for handling diagnostics is:

1. Fix the underlying issue.
2. Use local suppression if suppression is required.
3. Use project-wide suppression only when absolutely necessary.

Local suppression keeps the intent visible and easier to maintain.

---

# Runtime Validation Still Applies

Suppressing a diagnostic only affects compile-time analysis.

Runtime validation remains active.

For example:

```csharp
#pragma warning disable DIXOR001

IEnumerable<Guid> ids =
    Uuid7.NewGuids(0);

#pragma warning restore DIXOR001
```

Even though the compiler no longer reports the diagnostic, the following exception will still occur when the code executes:

```text
ArgumentOutOfRangeException
```

Suppressing a diagnostic does not disable runtime safety checks.

---

# Suppressing Diagnostics Using #pragma

The simplest and most common suppression mechanism uses compiler pragmas.

Pragmas suppress diagnostics for a specific region of code.

Example:

```csharp
#pragma warning disable DIXOR002

string id =
    Uuid7.NewString(
        prefix:
            "ExtremelyLongPrefixValueThatIntentionallyExceedsTheLimit");

#pragma warning restore DIXOR002
```

The diagnostic is ignored only between:

```csharp
#pragma warning disable
```

and

```csharp
#pragma warning restore
```

statements.

---

# Suppressing Multiple Diagnostics

Multiple diagnostics can be suppressed simultaneously.

Example:

```csharp
#pragma warning disable DIXOR002, DIXOR003

string id =
    Uuid7.NewString(
        prefix: VeryLongPrefix,
        suffix: VeryLongSuffix);

#pragma warning restore DIXOR002, DIXOR003
```

---

# Suppressing Diagnostics Using Attributes

Roslyn diagnostics can also be suppressed using the `SuppressMessageAttribute`.

Example:

```csharp
using System.Diagnostics.CodeAnalysis;

[SuppressMessage(
    "Usage",
    "DIXOR002",
    Justification =
        "Business identifiers require longer prefixes.")]
public void GenerateIdentifiers()
{
    string id =
        Uuid7.NewString(
            prefix:
                "VeryLongBusinessSpecificPrefix");
}
```

This approach is useful when entire methods, classes or assemblies require suppression.

---

# Suppressing Diagnostics for a Method

Example:

```csharp
[SuppressMessage(
    "Usage",
    "DIXOR003")]
public static void Generate()
{
}
```

The suppression applies to the decorated member.

---

# Suppressing Diagnostics for a Class

Example:

```csharp
[SuppressMessage(
    "Usage",
    "DIXOR002")]
public sealed class IdentifierFactory
{
}
```

All matching diagnostics within the class may be suppressed.

---

# Global Suppression Using GlobalSuppressions.cs

Large applications often centralize suppressions inside a dedicated file.

Create a file named:

```text
GlobalSuppressions.cs
```

Example:

```csharp
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Usage",
    "DIXOR002",
    Justification =
        "Legacy compatibility requirements.")]
```

This suppression applies to the entire assembly.

---

# Suppressing Diagnostics Using .editorconfig

Roslyn analyzers fully support configuration through `.editorconfig`.

Example:

```ini
dotnet_diagnostic.DIXOR002.severity = none
```

This completely disables the diagnostic.

---

# Changing Diagnostic Severity

Instead of disabling diagnostics entirely, you may change their severity.

Example:

```ini
dotnet_diagnostic.DIXOR001.severity = warning
dotnet_diagnostic.DIXOR002.severity = error
dotnet_diagnostic.DIXOR003.severity = suggestion
dotnet_diagnostic.DIXOR010.severity = silent
```

Supported severity values include:

| Severity   | Description                                                |
| ---------- | ---------------------------------------------------------- |
| none       | Diagnostic is disabled.                                    |
| silent     | Hidden from the Error List but available for IDE features. |
| suggestion | Appears as a suggestion.                                   |
| warning    | Appears as a warning.                                      |
| error      | Appears as a compilation error.                            |

---

# Example .editorconfig File

```ini
# Disable DIXOR002
dotnet_diagnostic.DIXOR002.severity = none

# Treat DIXOR003 as an error
dotnet_diagnostic.DIXOR003.severity = error

# Downgrade DIXOR010 to warning
dotnet_diagnostic.DIXOR010.severity = warning
```

---

# Suppressing Diagnostics Using Project Files

Diagnostics may also be suppressed directly inside the project file.

Example:

```xml
<PropertyGroup>
    <NoWarn>
        DIXOR002;
        DIXOR003
    </NoWarn>
</PropertyGroup>
```

This disables the specified diagnostics for the entire project.

---

# Local Suppression vs Global Suppression

## Local Suppression

Local suppression affects only a small section of code.

Example:

```csharp
#pragma warning disable DIXOR002

Generate();

#pragma warning restore DIXOR002
```

### Advantages

* Easier to understand.
* Easier to maintain.
* Documents intent clearly.
* Limits suppression scope.

---

## Global Suppression

Global suppression affects the entire project or assembly.

Example:

```ini
dotnet_diagnostic.DIXOR002.severity = none
```

### Advantages

* Centralized configuration.
* Useful for organization-wide standards.

### Disadvantages

* Can hide legitimate issues.
* May reduce code quality.
* Makes problems harder to discover.

---

# Best Practices

The following practices are recommended when suppressing diagnostics.

## Prefer Fixing The Code

Correcting the code is usually preferable to suppression.

Example:

Instead of:

```csharp
#pragma warning disable DIXOR001

Uuid7.NewGuids(0);

#pragma warning restore DIXOR001
```

prefer:

```csharp
Uuid7.NewGuids(10);
```

---

## Prefer Local Suppression

Local suppression clearly communicates intent.

Prefer:

```csharp
#pragma warning disable DIXOR002
```

over:

```ini
dotnet_diagnostic.DIXOR002.severity = none
```

whenever possible.

---

## Always Provide Justification

When using attributes, always provide a justification.

Example:

```csharp
[SuppressMessage(
    "Usage",
    "DIXOR002",
    Justification =
        "Legacy external system requires this format.")]
```

---

## Review Suppressions Periodically

Suppression requirements often change over time.

Regularly review:

* `GlobalSuppressions.cs`
* `.editorconfig`
* Project files

to remove obsolete suppressions.

---

# Common Scenarios

## Legacy System Compatibility

External systems may require long prefixes or suffixes.

Suppression may be appropriate.

---

## Temporary Migration Work

During migration from existing identifier systems, temporary suppressions may be necessary.

---

## False Positives

Future analyzer versions could occasionally report false positives.

Suppressions may be used until a fix becomes available.

---

# Current Dixor.Identity Diagnostics

The following diagnostics currently support suppression.

| Diagnostic | Description                   |
| ---------- | ----------------------------- |
| DIXOR001   | Invalid UUID batch size       |
| DIXOR002   | Prefix exceeds maximum length |
| DIXOR003   | Suffix exceeds maximum length |
| DIXOR010   | Invalid separator             |

---

# Summary

Dixor.Identity diagnostics can be suppressed using standard Roslyn mechanisms including:

* `#pragma warning`
* `SuppressMessageAttribute`
* `GlobalSuppressions.cs`
* `.editorconfig`
* Project file configuration

Suppress diagnostics carefully and only when there is a valid reason.

Runtime validation remains active even when diagnostics are suppressed.

---

# Related Documentation

* [Analyzers Overview](./Analyzers_Overview.md)
* [DIXOR001 - Invalid Batch Size](./DIXOR001.md)
* [DIXOR002 - Prefix Exceeds Maximum Length](./DIXOR002.md)
* [DIXOR003 - Suffix Exceeds Maximum Length](./DIXOR003.md)
* [DIXOR010 - Invalid Separator](./DIXOR010.md)

---

## Navigation

⬅ Previous: [DIXOR010 - Invalid Separator](./DIXOR010.md)

➡ Next: [README](../../README.md)