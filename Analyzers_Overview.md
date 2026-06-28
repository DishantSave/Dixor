# UUIDv7 Roslyn Analyzers Overview

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Analyzers](./Analyzers_Overview.md)

---

Dixor.Identity includes a collection of built-in Roslyn analyzers designed to improve developer productivity and help detect incorrect API usage during development.

These analyzers execute during compilation and inside supported IDEs, allowing potential issues to be identified before the application is executed.

No additional installation or configuration is required.

The analyzers are automatically installed and enabled when the Dixor.Identity NuGet package is added to a project.

---

# What Are Roslyn Analyzers?

Roslyn analyzers are compile-time tools built on top of the .NET Compiler Platform (Roslyn).

They inspect source code as it is being written and can provide:

* Errors
* Warnings
* Suggestions
* Best practice recommendations

Unlike runtime validation, analyzers provide immediate feedback directly inside the editor.

For example:

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(0);
```

Since a batch size of `0` is invalid, the analyzer immediately reports an error before the application is run.

This helps developers discover issues much earlier in the development lifecycle.

---

# Why Does Dixor.Identity Include Analyzers?

Dixor.Identity performs extensive runtime validation.

However, certain problems can be detected even earlier.

Consider the following code:

```csharp
Uuid7.NewGuids(0);
```

The application would eventually throw:

```text
ArgumentOutOfRangeException
```

at runtime.

Instead of waiting until execution, the analyzer detects this issue during compilation and informs the developer immediately.

This approach provides several advantages:

* Faster feedback.
* Reduced debugging time.
* Fewer runtime exceptions.
* Improved code quality.
* Safer API usage.
* Better developer experience.

---

# How The Analyzers Work

The analyzers inspect source code during compilation.

When an API invocation is encountered, the analyzer examines:

* The invoked method.
* Supplied arguments.
* Compile-time constant values.
* API usage patterns.

Example:

```csharp
Uuid7.NewString(
    prefix: "ORD",
    separator: "**");
```

The analyzer determines that:

1. The invoked API is `Uuid7.NewString(...)`.
2. The separator argument is a compile-time constant.
3. The separator contains more than one character.
4. The separator is invalid.

A diagnostic is then reported.

---

# Compile-Time Validation vs Runtime Validation

Dixor.Identity validates input at two different stages.

## Compile-Time Validation

Compile-time validation occurs while code is being written or compiled.

Example:

```csharp
Uuid7.NewGuids(0);
```

Result:

```text
DIXOR001: Invalid UUID batch size.
```

The developer can fix the issue immediately.

---

## Runtime Validation

Some values cannot be known during compilation.

Example:

```csharp
int count = GetCount();

Uuid7.NewGuids(count);
```

Since the compiler cannot determine the value returned by `GetCount()`, no analyzer diagnostic is produced.

Instead, validation occurs when the application executes.

Runtime validation always remains active, regardless of analyzer behavior.

---

# Supported Development Environments

Dixor.Identity analyzers work in any environment that supports Roslyn analyzers.

Supported environments include:

* Visual Studio
* JetBrains Rider
* Visual Studio Code with C# extensions
* `dotnet build`
* `dotnet test`
* Continuous Integration pipelines
* Continuous Delivery pipelines

Examples of supported CI systems include:

* GitHub Actions
* Azure DevOps
* GitLab CI/CD
* Jenkins
* TeamCity

Because analyzers execute during builds, code quality rules remain consistently enforced across all environments.

---

# Automatic Installation

No separate analyzer package needs to be installed.

Installing the main package automatically enables analyzer support.

Example:

```bash
dotnet add package Dixor.Identity
```

Once installed, diagnostics begin appearing automatically inside supported IDEs.

---

# Available Diagnostics

The following diagnostics are currently provided.

| Diagnostic ID | Severity | Category | Description                                           |
| ------------- | -------- | -------- | ----------------------------------------------------- |
| DIXOR001      | Error    | Usage    | Invalid batch size supplied to `Uuid7.NewGuids(...)`. |
| DIXOR002      | Warning  | Usage    | Prefix exceeds the maximum supported length.          |
| DIXOR003      | Warning  | Usage    | Suffix exceeds the maximum supported length.          |
| DIXOR010      | Error    | Usage    | Invalid separator supplied to formatting APIs.        |

Detailed documentation for each diagnostic is available separately.

---

# Diagnostic Severity Levels

Every diagnostic has an associated severity level.

## Error

Errors indicate code that is invalid and will likely fail during execution.

Compilation typically fails until the issue is fixed.

Example:

```csharp
Uuid7.NewGuids(0);
```

Produces:

```text
error DIXOR001
```

---

## Warning

Warnings indicate potentially problematic code.

Compilation succeeds, but developers should review the code.

Example:

```csharp
Uuid7.NewString(
    prefix:
        "ThisPrefixExceedsTheMaximumSupportedLength...");
```

Produces:

```text
warning DIXOR002
```

---

# Example Analyzer Output

Consider the following code:

```csharp
string id =
    Uuid7.NewString(
        prefix:
            "ThisIsAnExtremelyLongPrefixThatExceedsTheMaximumSupportedLength",
        separator: "**");
```

The IDE may display:

```text
warning DIXOR002: The prefix exceeds the maximum supported length of 50 characters.

error DIXOR010: Separator '**' is not supported.
```

Multiple diagnostics can be reported simultaneously.

---

# Understanding Compile-Time Constants

Current analyzers primarily operate on compile-time constant values.

A compile-time constant is a value known by the compiler while compiling the application.

Examples:

```csharp
Uuid7.NewGuids(10);

Uuid7.NewString(
    separator: "-");
```

These values are compile-time constants.

---

The following example is not a compile-time constant:

```csharp
string separator =
    Configuration["Separator"];

Uuid7.NewString(
    separator: separator);
```

Because the value is determined at runtime, no diagnostic is reported.

Runtime validation will still occur.

---

# Performance Considerations

The analyzers are designed to have minimal impact on build performance.

Key design principles include:

* Concurrent execution is enabled.
* Generated code is ignored.
* Only relevant API invocations are analyzed.
* Analysis focuses primarily on compile-time constants.
* Expensive operations are avoided.

As a result, the analyzers add negligible overhead to typical builds.

---

# Design Philosophy

The Dixor.Identity analyzer suite follows several important principles.

## Fast

Analyzer execution should not noticeably impact build performance.

## Accurate

False positives should be minimized whenever possible.

## Helpful

Diagnostics should provide actionable guidance.

## Safe

Analyzers should never alter application behavior.

## Non-Intrusive

Developers remain in full control and may suppress diagnostics when necessary.

---

# Diagnostic Suppression

All diagnostics support standard Roslyn suppression mechanisms.

Examples include:

* `#pragma warning disable`
* `.editorconfig`
* `GlobalSuppressions.cs`
* `SuppressMessageAttribute`

Example:

```csharp
#pragma warning disable DIXOR002

string id =
    Uuid7.NewString(
        prefix: VeryLongPrefix);

#pragma warning restore DIXOR002
```

For additional information, see:

```text
SuppressingDiagnostics.md
```

---

# Current Limitations

Current analyzers intentionally focus on deterministic scenarios.

Diagnostics are generally reported only when values are known during compilation.

Example:

```csharp
int count = GetCount();

Uuid7.NewGuids(count);
```

No diagnostic is produced because the actual value is unknown.

This behavior helps avoid false positives.

---

# Future Enhancements

Future releases may introduce additional diagnostics, including:

* Invalid timestamp detection.
* Parsing recommendations.
* UUID generation best practices.
* Performance recommendations.
* Database usage guidance.
* Misuse detection for future APIs.

The analyzer infrastructure has been designed to support these enhancements without breaking existing functionality.

---

# Best Practices

To get the maximum benefit from Dixor.Identity analyzers:

* Keep analyzers enabled.
* Avoid globally suppressing diagnostics.
* Treat warnings seriously.
* Review diagnostics during code reviews.
* Integrate analyzer execution into CI/CD pipelines.
* Use `.editorconfig` to enforce organization-wide standards.

---

# Summary

Dixor.Identity analyzers provide compile-time validation for UUID version 7 APIs.

They help developers:

* Detect issues earlier.
* Reduce runtime exceptions.
* Improve code quality.
* Follow recommended API usage patterns.
* Increase development productivity.

Because the analyzers are included automatically with the package, developers benefit from these validations immediately after installation.

---

# Related Documentation

## Analyzer Diagnostics

* [DIXOR001 - Invalid Batch Size](./DIXOR001.md)
* [DIXOR002 - Prefix Exceeds Maximum Length](./DIXOR002.md)
* [DIXOR003 - Suffix Exceeds Maximum Length](./DIXOR003.md)
* [DIXOR010 - Invalid Separator](./DIXOR010.md)

## Additional Documentation

* [Suppressing Diagnostics](./SuppressingDiagnostics.md)
* [Installation Guide](../getting-started/Installation.md)
* [Quick Start](../getting-started/QuickStart.md)

---

## Navigation

⬅ Previous: [README](../../README.md)

➡ Next: [DIXOR001 - Invalid Batch Size](./DIXOR001.md)