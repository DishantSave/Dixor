# Design Decisions

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [Internals](https://github.com/DishantSave/Dixor/blob/main/docs/internals/Uuid7Specification.md)

---

This document explains the architectural decisions behind Dixor.Identity.

Understanding these decisions helps contributors and advanced users understand why the library behaves as it does.

---

# Goals

Dixor.Identity was designed with the following primary goals:

* RFC 9562 compliance.
* High performance.
* Simplicity.
* Predictable behavior.
* Database friendliness.
* Minimal allocations.
* Developer productivity.

---

# Why UUID Version 7?

Earlier UUID versions have several limitations.

## UUIDv4

Advantages:

* Simple.
* Widely supported.

Disadvantages:

* Completely random.
* Poor database index locality.
* Frequent index fragmentation.

UUIDv7 solves these problems by embedding timestamps.

---

# Why RFC 9562?

RFC 9562 is the official IETF specification for UUIDs.

Following the RFC guarantees:

* Interoperability.
* Future compatibility.
* Standards compliance.
* Cross-platform consistency.

---

# Why Use Cryptographically Secure Randomness?

The library uses:

```csharp
RandomNumberGenerator.Fill(...)
```

instead of:

```csharp
Random.Shared
```

or

```csharp
System.Random
```

Reasons:

* Better uniqueness.
* Strong collision resistance.
* Unpredictable identifiers.
* Industry-standard security.

---

# Why Use Internal Implementations?

Most implementation classes are marked:

```csharp
internal
```

Examples:

* Uuid7Generator
* Uuid7Formatter
* Uuid7Parser

Reasons:

* Preserve API stability.
* Allow internal refactoring.
* Reduce public surface area.
* Simplify versioning.

Public APIs delegate to these internal implementations.

---

# Why Use stackalloc?

Example:

```csharp
Span<byte> bytes =
    stackalloc byte[16];
```

Reasons:

* Eliminates heap allocations.
* Reduces garbage collection pressure.
* Improves throughput.
* Increases performance.

---

# Why Lazy Batch Generation?

Batch generation returns:

```csharp
IEnumerable<Guid>
```

instead of:

```csharp
List<Guid>
```

Reasons:

* Lower memory usage.
* Deferred execution.
* Efficient large-scale processing.

---

# Why Provide Try APIs?

Examples:

```csharp
TryParse(...)
TryExtract(...)
```

Reasons:

* Avoid exceptions.
* Improve performance.
* Support defensive programming.

---

# Why Include Roslyn Analyzers?

Analyzers detect incorrect usage during compilation.

Benefits:

* Earlier feedback.
* Reduced runtime failures.
* Better developer experience.

---

# Why Support Monotonic Generation?

Standard UUIDv7 ordering may not be guaranteed within the same millisecond.

Monotonic generation guarantees ordering.

Benefits:

* Better database locality.
* Stable ordering.
* Reduced index fragmentation.

---

# Why Restrict Separators?

Formatting APIs support only specific separators.

Reasons:

* Predictable parsing.
* Consistent formatting.
* Better interoperability.

---

# Thread Safety

Generators intended for concurrent use are explicitly synchronized.

Example:

```csharp
lock (Lock)
{
}
```

This ensures correctness under heavy parallel workloads.

---

# Why Use Exception-Based Validation?

Public APIs fail fast.

Benefits:

* Immediate feedback.
* Easier debugging.
* Predictable behavior.

---

# Analyzer Philosophy

Analyzers follow these principles:

* Fast.
* Accurate.
* Low false positives.
* Non-intrusive.
* Helpful.

---

# Future Design Principles

Future features should:

* Preserve RFC compliance.
* Remain allocation-conscious.
* Maintain API simplicity.
* Avoid breaking changes whenever possible.

---

# Related Documentation

* [UUIDv7 Specification](https://github.com/DishantSave/Dixor/blob/main/docs/internals/Uuid7Specification.md)
* [Byte Layout](https://github.com/DishantSave/Dixor/blob/main/docs/internals/ByteLayout.md)
* [UUIDv7 Best Practices](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BestPractices.md)
* [Architecture](../../Architecture.md)

---

## Navigation

⬅ Previous: [Byte Layout](https://github.com/DishantSave/Dixor/blob/main/docs/internals/ByteLayout.md)

➡ Next: [README](../../README.md)