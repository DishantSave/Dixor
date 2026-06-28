# Architecture

## Overview

**Dixor.Identity** is a modern identity generation library for .NET designed to provide high-performance, allocation-conscious, and standards-compliant identifier generation APIs.

The library focuses on providing modern identifier formats suitable for:

* Distributed systems
* Cloud-native applications
* Database-driven applications
* Event-driven architectures
* High-throughput systems

The project follows a modular architecture to ensure maintainability, extensibility, and long-term stability.

---

## Design Goals

Dixor.Identity is built around the following principles:

### Standards Compliance

Whenever applicable, implementations follow official specifications and RFCs.

Examples:

* RFC 9562 UUIDv7

---

### Performance First

The library aims to minimize:

* Heap allocations
* Memory copying
* Synchronization overhead

Performance-sensitive APIs make extensive use of:

* `Span<T>`
* `stackalloc`
* Allocation-conscious algorithms

---

### Developer Experience

The library provides:

* Comprehensive XML documentation
* IntelliSense support
* Roslyn analyzers
* Consistent API design

---

### Thread Safety

Public APIs are designed to be thread-safe whenever possible.

This allows identifiers to be safely generated in multi-threaded and highly concurrent environments.

---

## Solution Structure

```text
Dixor/
├── Dixor.Identity.Analyzers
├── docs
├── src
│   └── Dixor.Identity
└── tests
    └── Dixor.Identity.Tests
```

---

## Project Responsibilities

### `src/Dixor.Identity`

Contains the core library implementation.

Responsibilities include:

* Identifier generation.
* Validation.
* Parsing.
* Formatting.
* Timestamp extraction.
* Internal infrastructure.

---

### `Dixor.Identity.Analyzers`

Contains Roslyn analyzers used to improve developer experience.

Responsibilities include:

* Detecting invalid API usage.
* Providing compile-time diagnostics.
* Enforcing library best practices.

---

### `Dixor.Identity.Tests`

Contains automated unit tests.

Responsibilities include:

* Functional testing.
* Regression testing.
* Validation of public APIs.
* Ensuring standards compliance.

---

### `docs`

Contains all user and contributor documentation.

Responsibilities include:

* Installation guides.
* API documentation.
* Architecture documentation.
* Analyzer documentation.
* Roadmaps and changelogs.

---

## Current Feature Modules

### UUIDv7

The current release focuses primarily on UUID version 7 support.

Implemented capabilities include:

* RFC 9562 compliant UUIDv7 generation.
* Monotonic UUIDv7 generation.
* Batch UUID generation.
* UUID validation.
* UUID parsing.
* Timestamp extraction.
* UUID formatting.

---

## Internal Components

Current UUIDv7 functionality is composed of multiple focused components.

### Generators

Responsible for creating identifiers.

Examples:

* `Uuid7Generator`
* `Uuid7MonotonicGenerator`

---

### Validators

Responsible for verifying identifier correctness.

Examples:

* `Uuid7Validator`

---

### Parsers

Responsible for extracting identifiers from textual input.

Examples:

* `Uuid7Parser`

---

### Extractors

Responsible for retrieving metadata embedded within identifiers.

Examples:

* `Uuid7TimestampExtractor`

---

### Internal Infrastructure

Provides reusable low-level utilities.

Examples:

* Byte conversion utilities.
* Endianness handling.
* Shared helper methods.

---

## Dependency Flow

```text
Public API
    ↓
Feature Components
    ↓
Internal Infrastructure
```

The internal infrastructure layer is intentionally hidden from consumers to preserve API stability.

---

## Future Architecture

Future releases are expected to introduce additional feature modules, including:

* Sequential GUIDs
* ULIDs
* COMB GUIDs
* Snowflake identifiers

Each feature will follow the same layered architecture used by UUIDv7.

---

## Versioning Strategy

Dixor.Identity follows Semantic Versioning (SemVer).

```text
MAJOR.MINOR.PATCH
```

Examples:

* `0.1.1`
* `0.2.0`
* `1.0.0`

Breaking changes will only occur during major version releases.

---

## Extensibility

The architecture is intentionally designed to support future expansion without breaking existing APIs.

New identifier formats can be added as independent modules while reusing common infrastructure and analyzer support.

---

## Conclusion

Dixor.Identity aims to provide a robust foundation for modern identifier generation in .NET while maintaining high performance, excellent developer experience, and long-term maintainability.
