# Installation

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Getting Started](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/QuickStart.md)

---

# Installing Dixor.Identity

This guide explains how to install, verify, and configure **Dixor.Identity** in your .NET applications.

By the end of this guide, you will have:

* Installed the package.
* Generated your first UUID version 7 identifier.
* Verified that Roslyn analyzers are working.
* Confirmed that your environment is correctly configured.

---

# Prerequisites

Before installing Dixor.Identity, ensure that your development environment meets the following requirements.

## Supported Frameworks

Dixor.Identity currently supports:

* .NET 10 or later

Older versions of .NET are not supported.

---

## Verify Your .NET SDK Version

Open a terminal or command prompt and execute:

```bash
dotnet --version
```

Example output:

```text
10.0.100
```

If the installed version is lower than .NET 10, download and install the latest SDK from:

https://dotnet.microsoft.com/download

---

# Installing Dixor.Identity

Dixor.Identity can be installed using any standard NuGet package management workflow.

Choose the approach that best matches your development environment.

---

# Install Using .NET CLI

Navigate to your project directory and execute:

```bash
dotnet add package Dixor.Identity
```

Example:

```bash
cd MyApplication

dotnet add package Dixor.Identity
```

The .NET CLI will automatically:

1. Download the package.
2. Restore dependencies.
3. Update the project file.

---

# Install Using Visual Studio

If you are using Visual Studio:

1. Open your solution.
2. In **Solution Explorer**, right-click the project.
3. Select **Manage NuGet Packages**.
4. Open the **Browse** tab.
5. Search for:

```text
Dixor.Identity
```

6. Select the package.
7. Click **Install**.

Visual Studio will automatically restore the package.

---

# Install Using Package Manager Console

Open:

```text
Tools
→ NuGet Package Manager
→ Package Manager Console
```

Execute:

```powershell
Install-Package Dixor.Identity
```

---

# What Gets Installed?

Installing Dixor.Identity automatically provides:

* RFC 9562 UUID version 7 generation.
* Monotonic UUID generation.
* Batch UUID generation.
* UUID parsing utilities.
* UUID validation utilities.
* Timestamp extraction APIs.
* Identifier formatting APIs.
* Built-in Roslyn analyzers.

No additional packages are required.

---

# Namespaces

After installation, add the following namespace:

```csharp
using Dixor.Identity.UUID7;
```

This namespace exposes the complete public API surface.

---

# Verify The Installation

Create a simple console application:

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.NewGuid();

Console.WriteLine(id);
```

Example output:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

If the application builds and executes successfully, the package has been installed correctly.

---

# Your First UUIDv7

Generate your first UUID version 7 identifier:

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.NewGuid();

Console.WriteLine(id);
```

Sample output:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

Notice that the identifier:

* Is globally unique.
* Is chronologically sortable.
* Contains an embedded timestamp.

---

# Verifying Timestamp Extraction

UUIDv7 values contain embedded timestamps.

Example:

```csharp
Guid id = Uuid7.NewGuid();

DateTimeOffset timestamp =
    Uuid7.GetTimestamp(id);

Console.WriteLine(timestamp);
```

Example output:

```text
2026-06-28T12:15:30+00:00
```

---

# Built-In Roslyn Analyzer Support

Dixor.Identity ships with integrated Roslyn analyzers.

No additional configuration is required.

Supported IDEs include:

* Visual Studio
* JetBrains Rider
* Visual Studio Code with C# extensions

Once the package is installed, diagnostics automatically appear while writing code.

---

## Example Analyzer Diagnostic

Example:

```csharp
IEnumerable<Guid> ids =
    Uuid7.NewGuids(0);
```

The IDE will display a diagnostic because:

```text
Batch size must be greater than zero.
```

The analyzer helps identify problems during development instead of at runtime.

---

# Analyzer Benefits

The built-in analyzers help developers by:

* Detecting invalid API usage.
* Preventing common mistakes.
* Improving code quality.
* Providing immediate IDE feedback.
* Reducing runtime exceptions.

---

# Updating Dixor.Identity

To update to the latest version, execute:

```bash
dotnet add package Dixor.Identity
```

Alternatively, use your preferred NuGet management tool.

Visual Studio users may update packages directly from the NuGet Package Manager UI.

---

# Uninstalling

To remove Dixor.Identity from a project:

```bash
dotnet remove package Dixor.Identity
```

---

# Troubleshooting

## Package Not Found

Ensure that NuGet package sources are correctly configured.

Verify package sources:

```bash
dotnet nuget list source
```

You should see:

```text
nuget.org
```

If NuGet.org is missing, add it:

```bash
dotnet nuget add source https://api.nuget.org/v3/index.json
```

---

## Package Restore Failed

Try restoring packages manually:

```bash
dotnet restore
```

---

## Build Errors After Installation

Clean and rebuild the project:

```bash
dotnet clean

dotnet build
```

---

## Namespace Not Found

Ensure the following namespace is imported:

```csharp
using Dixor.Identity.UUID7;
```

Also verify that package restore completed successfully.

---

## Roslyn Analyzers Are Not Running

Ensure that:

* The project uses SDK-style project files.
* Package restore completed successfully.
* Your IDE supports Roslyn analyzers.
* Analyzer execution has not been disabled.

Example SDK-style project:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

</Project>
```

---

# Next Steps

After successfully installing Dixor.Identity, continue with the following guides:

1. **QuickStart.md**
2. **UUID7_Overview.md**
3. **UUID7_BestPractices.md**
4. **Generation.md**
5. **MonotonicGeneration.md**

These guides introduce the complete UUIDv7 feature set provided by Dixor.Identity.

---

# Need Help?

If you encounter issues or wish to contribute:

* Create an issue in the GitHub repository.
* Review the documentation.
* Check existing discussions and issues.

Dixor.Identity is designed to provide a simple, modern and RFC 9562 compliant UUIDv7 experience for .NET developers.

---

# Next Steps

After installing Dixor.Identity, continue with:

* [Quick Start](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/QuickStart.md)
* [Generate Your First UUID](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/FirstUuid.md)
* [Migration Guide](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/MigrationGuide.md)
* [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)

---

## Navigation

⬅ Previous: [README](../../README.md)

➡ Next: [Quick Start](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/QuickStart.md)