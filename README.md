# Dixor.Identity

![NuGet Version](https://img.shields.io/nuget/v/Dixor.Identity)
![NuGet Downloads](https://img.shields.io/nuget/dt/Dixor.Identity)
![License](https://img.shields.io/github/license/Dixor/Dixor.Identity)
![Framework](https://img.shields.io/badge/.NET-10+-blue)

</p>

---

# Table of Contents

* [Introduction](#introduction)
* [What is RFC 9562?](#what-is-rfc-9562)
* [What is UUID Version 7?](#what-is-uuid-version-7)
* [Why UUIDv7?](#why-uuidv7)
* [UUID Version Comparison](#uuid-version-comparison)
* [Why Dixor.Identity?](#why-dixoridentity)
* [Features](#features)
* [Installation](#installation)
* [Quick Start](#quick-start)
* [Documentation](#documentation)
* [Supported Frameworks](#supported-frameworks)
* [Roadmap](#roadmap)
* [License](#license)

---

# Introduction

**Dixor.Identity** is a modern, high-performance, allocation-conscious identity generation library for .NET.

The library focuses on generating **RFC 9562 compliant UUID version 7 identifiers** that are:

* Globally unique
* Naturally sortable
* Database friendly
* Distributed-system ready
* Chronologically traceable

Dixor.Identity additionally provides:

* Monotonic UUID generation
* Batch generation APIs
* Parsing utilities
* Validation APIs
* Timestamp extraction
* Business-friendly formatting APIs
* Built-in Roslyn analyzers
* Extensive documentation

---

# What is RFC 9562?

**RFC 9562** is the current IETF specification that defines Universally Unique Identifiers (UUIDs).

RFC 9562 supersedes the older **RFC 4122** specification.

The RFC introduces several modern UUID versions including:

| Version | Description                 |
| ------- | --------------------------- |
| UUIDv1  | Timestamp + MAC address     |
| UUIDv3  | Namespace-based (MD5)       |
| UUIDv4  | Random                      |
| UUIDv5  | Namespace-based (SHA-1)     |
| UUIDv6  | Reordered timestamp UUID    |
| UUIDv7  | Unix timestamp + randomness |
| UUIDv8  | Custom implementation       |

RFC 9562 specifically introduces **UUID version 7** as the recommended general-purpose UUID format for modern applications.

Official RFC:

https://www.rfc-editor.org/rfc/rfc9562

---

# What is UUID Version 7?

UUID version 7 is a time-ordered identifier format defined by RFC 9562.

A UUIDv7 consists of:

* A **48-bit Unix timestamp** (milliseconds since Unix Epoch)
* Version bits
* Variant bits
* Cryptographically secure random data

Example:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

The timestamp is embedded directly into the UUID.

This means identifiers generated later naturally sort after identifiers generated earlier.

---

# Why UUIDv7?

Traditional GUID generation in .NET:

```csharp
Guid id = Guid.NewGuid();
```

generates **UUID version 4**.

UUIDv4 is entirely random.

Although uniqueness is excellent, random identifiers cause several issues:

* Database index fragmentation
* Frequent page splits
* Poor clustered index performance
* No creation timestamp
* Difficult ordering
* Reduced cache locality

UUIDv7 addresses these problems.

---

# UUID Version Comparison

| Feature                  | UUIDv1  | UUIDv4 | UUIDv7 |
| ------------------------ | ------- | ------ | ------ |
| Timestamp Based          | ✅       | ❌      | ✅      |
| Chronologically Sortable | Partial | ❌      | ✅      |
| Cryptographically Random | ❌       | ✅      | ✅      |
| Database Friendly        | Partial | ❌      | ✅      |
| Embedded Timestamp       | ✅       | ❌      | ✅      |
| Privacy Friendly         | ❌       | ✅      | ✅      |
| Modern Standard          | ❌       | ❌      | ✅      |
| RFC 9562 Recommended     | ❌       | ❌      | ✅      |

---

# Why Dixor.Identity?

Dixor.Identity provides a production-ready UUIDv7 implementation specifically optimized for .NET applications.

Advantages include:

* RFC 9562 compliance
* Allocation-conscious implementation
* Optimized byte layout
* Chronological ordering
* Monotonic UUID support
* Improved database performance
* Rich developer tooling
* Compile-time diagnostics

---

# Features

## 🚀 RFC 9562 UUIDv7 Generation

Generate modern time-ordered UUIDs.

```csharp
Guid id = Uuid7.New();
```

---

## ⚡ Monotonic UUID Generation

Guarantees ordering for UUIDs generated during the same millisecond.

```csharp
Guid id = Uuid7.NewMonotonic();
```

Ideal for:

* High-throughput systems
* Event sourcing
* Database primary keys

---

## 📦 Batch Generation

Generate multiple UUIDs efficiently.

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(100);
```

---

## 🔍 Validation

Validate UUIDv7 values.

```csharp
bool isValid =
    Uuid7.IsValid(id);
```

---

## 📝 Parsing

Extract UUIDs from arbitrary strings.

```csharp
Guid id =
    Uuid7.Parse(
        "Order-019853a6-35c4-7e58-bf91-cb7d0b1b11d2");
```

---

## ⏱ Timestamp Extraction

Retrieve embedded creation timestamps.

```csharp
DateTimeOffset timestamp =
    Uuid7.GetTimestamp(id);
```

---

## 🎨 Formatting

Create business-friendly identifiers.

```csharp
string value =
    Uuid7.NewString(
        prefix: "ORD",
        suffix: "US");
```

Example:

```text
ORD-019853a6-35c4-7e58-bf91-cb7d0b1b11d2-US
```

---

## 🧠 Roslyn Analyzers

Built-in analyzers automatically detect invalid API usage.

Example:

```csharp
Uuid7.NewGuids(0);
```

Produces:

```text
DIXOR001
Invalid UUID batch size
```

No additional installation is required.

---

# Installation

Install using the .NET CLI:

```bash
dotnet add package Dixor.Identity
```

Package Manager:

```powershell
Install-Package Dixor.Identity
```

---

# Quick Start

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.New();

Console.WriteLine(id);
```

Example output:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

Generate multiple identifiers:

```csharp
foreach (Guid id in Uuid7.NewGuids(5))
{
    Console.WriteLine(id);
}
```

---

# Documentation

The documentation is organized hierarchically.

## Getting Started

* [Installation](docs/getting-started/Installation.md)
* [Quick Start](docs/getting-started/QuickStart.md)
* [Generate Your First UUID](docs/getting-started/FirstUuid.md)
* [Migration Guide](docs/getting-started/MigrationGuide.md)

---

## UUIDv7 Documentation

* [UUIDv7 Overview](docs/uuid7/UUID7_Overview.md)
* [Generation](docs/uuid7/UUID7_Generation.md)
* [Monotonic Generation](docs/uuid7/UUID7_MonotonicGeneration.md)
* [Batch Generation](docs/uuid7/UUID7_BatchGeneration.md)
* [Formatting](docs/uuid7/UUID7_Formatting.md)
* [Parsing](docs/uuid7/UUID7_Parsing.md)
* [Validation](docs/uuid7/UUID7_Validation.md)
* [Timestamp Extraction](docs/uuid7/UUID7_TimestampExtraction.md)
* [Best Practices](docs/uuid7/UUID7_BestPractices.md)

---

## Analyzer Documentation

* [Analyzer Overview](docs/analyzers/Analyzers_Overview.md)
* [Suppressing Diagnostics](docs/analyzers/SuppressingDiagnostics.md)
* [DIXOR001](docs/analyzers/DIXOR001.md)
* [DIXOR002](docs/analyzers/DIXOR002.md)
* [DIXOR003](docs/analyzers/DIXOR003.md)
* [DIXOR010](docs/analyzers/DIXOR010.md)

---

## Internal Design

* [UUIDv7 Specification](docs/internals/Uuid7Specification.md)
* [Byte Layout](docs/internals/ByteLayout.md)
* [Design Decisions](docs/internals/DesignDecisions.md)

---

## Examples

* [Basic Usage Examples](docs/examples/BasicUsageExamples.md)
* [Batch Generation Examples](docs/examples/BatchGenerationExamples.md)
* [Sequential Identifier Examples](docs/examples/SequentialIdentifiersExamples.md)

---

## Additional Documentation

* [Architecture](docs/Architecture.md)
* [Roadmap](docs/Roadmap.md)
* [Changelog](docs/Changelog.md)

---

# Supported Frameworks

| Framework |
| --------- |
| .NET 10+  |

---

# Roadmap

Current version:

```text
v0.1.3
```

Planned future additions:

* ULID support
* Sequential GUID support
* Snowflake identifiers
* Additional analyzers
* Source generators
* Performance benchmarks

See:

[Roadmap](docs/Roadmap.md)

---

# Status

🚧 Under active development.

Contributions, issues and suggestions are welcome.

---

# License

Dixor.Identity is licensed under the MIT License.

See:

[LICENSE](LICENSE)

---

# Contributing

Contributions are welcome.

Please open an issue or submit a pull request.

---

<p align="center">
Built with ❤️ for modern .NET applications.
</p>

---

## Documentation Navigation

## Getting Started

* [Installation](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/Installation.md)
* [First UUID](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/FirstUuid.md)
* [Quick Start](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/QuickStart.md)
* [Migration Guide](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/MigrationGuide.md)

---

### UUIDv7

* [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)
* [Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Generation.md)
* [Monotonic Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_MonotonicGeneration.md)
* [Batch Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BatchGeneration.md)
* [Formatting](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Formatting.md)
* [Parsing](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Parsing.md)
* [Validation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Validation.md)
* [Timestamp Extraction](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_TimestampExtraction.md)
* [Best Practices](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_BestPractices.md)

---

### Analyzers

* [Analyzers Overview](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/Analyzers_Overview.md)
* [DIXOR001](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR001.md)
* [DIXOR002](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR002.md)
* [DIXOR003](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR003.md)
* [DIXOR010](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/DIXOR010.md)
* [Suppressing Diagnostics](https://github.com/DishantSave/Dixor/blob/main/docs/analyzers/SuppressingDiagnostics.md)

---

### Internals

* [UUIDv7 Specification](https://github.com/DishantSave/Dixor/blob/main/docs/internals/Uuid7Specification.md)
* [Byte Layout](https://github.com/DishantSave/Dixor/blob/main/docs/internals/ByteLayout.md)
* [Design Decisions](https://github.com/DishantSave/Dixor/blob/main/docs/internals/DesignDecisions.md)

---

### Examples

* [Basic Usage](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BasicUsageExamples.md)
* [Batch Generation Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/BatchGenerationExamples.md)
* [Sequential Identifier Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/SequentialIdentifiersExamples.md)

---

## Additional Information

* [Architecture](https://github.com/DishantSave/Dixor/blob/main/docs/Architecture.md)
* [Roadmap](https://github.com/DishantSave/Dixor/blob/main/docs/Roadmap.md)
* [Changelog](https://github.com/DishantSave/Dixor/blob/main/docs/Changelog.md)
