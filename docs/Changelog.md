# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

---

## [0.1.1] - 2026-06-28

### Added

#### UUIDv7

- Added RFC 9562 compliant UUIDv7 generation.
- Added monotonic UUIDv7 generation.
- Added efficient batch UUID generation support.
- Added UUIDv7 parsing functionality.
- Added UUIDv7 validation functionality.
- Added UUIDv7 timestamp extraction functionality.
- Added UUIDv7 formatting support.

#### Roslyn Analyzers

- Added `DIXOR001` diagnostic for invalid UUID batch sizes.
- Added analyzer infrastructure for future diagnostics.
- Added analyzer constants, descriptors, and diagnostic identifiers.
- Added placeholder analyzers for UUIDv7-related APIs.

#### Documentation

- Added comprehensive XML documentation for all public APIs.
- Added solution-level documentation structure.
- Added architecture documentation.
- Added roadmap documentation.
- Added analyzer documentation infrastructure.
- Added installation and getting started documentation.
- Added NuGet package README documentation.

#### Testing

- Added unit tests for UUIDv7 generation.
- Added unit tests for monotonic UUID generation.
- Added unit tests for UUID parsing.
- Added unit tests for UUID validation.
- Added unit tests for UUID timestamp extraction.
- Added unit tests for UUID formatting functionality.

#### Package

- Added NuGet package metadata.
- Added package icon support.
- Added package README support.
- Added repository metadata.
- Added MIT license configuration.

### Changed

- Improved project structure and organization.
- Improved API discoverability through IntelliSense documentation.
- Improved package metadata for NuGet distribution.
- Enhanced documentation coverage across the library.

---

## [0.1.0] - 2026-06-03

### Added

- Created the initial solution structure.
- Added the `Dixor.Identity` library project.
- Added the `Dixor.Identity.Tests` test project.
- Established the initial documentation structure.
- Added base project architecture and repository layout.

---

[0.1.1]: https://github.com/DishantSave/Dixor/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/DishantSave/Dixor/releases/tag/v0.1.0