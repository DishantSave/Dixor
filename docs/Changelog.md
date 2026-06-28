# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

---

## [0.1.2] - 2026-06-28

### Added

#### Framework Support

* Added support for **.NET 8**.
* Added support for **.NET 9**.
* Expanded multi-targeting support to improve compatibility across modern .NET applications.

#### Documentation

* Added comprehensive API documentation for the `Uuid7` public facade.
* Added complete UUIDv7 conceptual documentation.
* Added comprehensive Getting Started documentation.
* Added extensive Examples documentation.
* Added detailed Internals documentation.
* Added complete Roslyn Analyzer documentation.
* Added navigation links across all documentation sections.
* Added parent-child navigation hierarchy throughout the documentation.
* Added cross-referenced documentation structure for improved discoverability.
* Added detailed RFC 9562 overview and UUID comparison documentation to the package README.

### Changed

#### Documentation

* Reorganized the entire documentation structure.
* Improved documentation discoverability and navigation.
* Updated README to serve as the central documentation hub.
* Enhanced README with detailed UUIDv7 explanations and RFC 9562 references.
* Improved documentation consistency across all sections.
* Improved documentation accessibility for both GitHub and NuGet consumers.

#### Package

* Improved NuGet package documentation experience.
* Improved package metadata and documentation integration.
* Enhanced overall developer onboarding experience.

### Fixed

#### Documentation

* Fixed broken documentation navigation links.
* Fixed cross-document navigation issues in GitHub.
* Fixed NuGet documentation link resolution issues.
* Fixed README navigation inconsistencies.
* Fixed documentation hierarchy and parent navigation structure.

#### Compatibility

* Fixed package compatibility limitations by extending support beyond .NET 10.
* Resolved framework compatibility issues when consuming the package from .NET 8 and .NET 9 applications.

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