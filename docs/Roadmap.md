# Roadmap

This document outlines the planned direction and upcoming features for **Dixor.Identity**.

> The roadmap is subject to change as the project evolves and community feedback is incorporated.

---

## ✅ v0.1.0 - Initial Foundation (Released: 2026-06-03)

### Completed

* Initial solution structure.
* Repository setup.
* Test project infrastructure.
* Documentation structure.
* Base library architecture.

---

## ✅ v0.1.1 - UUIDv7 Foundation (Released: 2026-06-28)

### Completed

#### UUIDv7

* RFC 9562 compliant UUIDv7 generation.
* Monotonic UUIDv7 generation.
* Batch UUID generation.
* UUIDv7 parsing utilities.
* UUIDv7 validation utilities.
* UUIDv7 timestamp extraction.
* UUIDv7 formatting support.

#### Developer Experience

* Comprehensive XML documentation.
* Roslyn analyzer infrastructure.
* Initial analyzer diagnostics.
* NuGet package metadata and packaging.

#### Quality

* Unit test coverage for UUIDv7 functionality.
* Architecture and documentation improvements.

---

## 🚧 v0.2.0 - Sequential Identifier Enhancements

### Planned

* Sequential GUID implementation.
* SQL Server optimized identifiers.
* COMB GUID support.
* Sequential identifier validation APIs.
* Performance benchmarks.

---

## 🚧 v0.3.0 - ULID Support

### Planned

* ULID generation.
* Monotonic ULID generation.
* ULID parsing.
* ULID validation.
* ULID formatting.
* Timestamp extraction from ULIDs.

---

## 🚧 v0.4.0 - Analyzer and Performance Improvements

### Planned

* Additional Roslyn analyzers.
* Code fixes for supported diagnostics.
* Source generators where applicable.
* Allocation and throughput optimizations.
* BenchmarkDotNet performance suite.

---

## 🚧 v0.5.0 - Framework Integrations

### Planned

* Entity Framework Core integration helpers.
* ASP.NET Core integration helpers.
* Database provider guidance and samples.
* Logging and distributed tracing examples.

---

## 🎯 v1.0.0 - Stable Release

### Goals

* Stable and production-ready public API.
* Complete documentation coverage.
* Mature analyzer ecosystem.
* Comprehensive test coverage.
* Performance validation and benchmarking.
* Full semantic versioning support.
* Long-term maintenance commitment.

---

## Future Considerations

Potential features being evaluated for future releases:

* Snowflake ID generation.
* NanoID support.
* Base32/Base58 encoding utilities.
* Distributed ID generators.
* Custom identifier pipelines.
* Source-generated parsers and formatters.
* Additional database-specific optimizations.
