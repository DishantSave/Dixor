# Batch UUIDv7 Generation

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [UUIDv7 Overview](./UUID7_Overview.md)

---

Dixor.Identity provides support for generating multiple UUID version 7 identifiers in a single operation.

Batch generation is useful when your application needs to create many identifiers at once, such as when:

* Seeding databases.
* Inserting thousands of records.
* Generating test data.
* Processing import/export operations.
* Creating identifiers for distributed systems.

The library exposes this functionality through:

```csharp
IEnumerable<Guid> Uuid7.NewGuids(int count)
```

Internally, this functionality is implemented by the `Uuid7BatchGenerator` type.

---

# Why Batch Generation?

Many applications need more than a single identifier.

Consider the following example:

```csharp
var orderIds = new List<Guid>();

for (int i = 0; i < 10000; i++)
{
    orderIds.Add(Uuid7.NewGuid());
}
```

While this works, it requires the consumer to repeatedly invoke the generator and manually manage collections.

Batch generation simplifies this process:

```csharp
var orderIds = Uuid7.NewGuids(10000);
```

The library handles the iteration internally and returns a sequence of UUIDv7 values.

---

# Basic Usage

Generate five UUIDv7 values:

```csharp
foreach (var id in Uuid7.NewGuids(5))
{
    Console.WriteLine(id);
}
```

Example output:

```text
0197b9dd-5f4d-78ea-bcd8-2c847fda4c1a
0197b9dd-5f4d-78eb-a8b4-6bc89fdba1fe
0197b9dd-5f4d-78ec-a1b2-c34d9e7ab2f4
0197b9dd-5f4d-78ed-8b71-32a4ce61fa20
0197b9dd-5f4d-78ee-92b8-a5f3e6c18b42
```

---

# Method Signature

```csharp
public static IEnumerable<Guid> NewGuids(int count)
```

## Parameters

| Parameter | Description                                                          |
| --------- | -------------------------------------------------------------------- |
| `count`   | Number of UUIDv7 identifiers to generate. Must be greater than zero. |

## Returns

Returns a lazily evaluated sequence containing the requested number of UUIDv7 values.

---

# Understanding Lazy Evaluation

One of the most important concepts used by Dixor.Identity batch generation is **lazy evaluation**.

A beginner may ask:

> "What does lazy evaluation mean?"

Lazy evaluation means that UUIDs are generated only when they are actually needed.

Internally, the implementation uses C# `yield return`.

Simplified implementation:

```csharp
public static IEnumerable<Guid> Generate(int count)
{
    for (int i = 0; i < count; i++)
    {
        yield return Uuid7Generator.Generate();
    }
}
```

The `yield return` statement tells .NET:

> "Generate one UUID, return it to the caller, and pause execution until the caller requests the next UUID."

This differs from creating all UUIDs immediately.

---

# Eager vs Lazy Generation

## Eager Generation

```csharp
var list = new List<Guid>();

for (int i = 0; i < 1000000; i++)
{
    list.Add(Uuid7.NewGuid());
}
```

Memory usage:

```text
Generate everything immediately.
Store everything in memory.
```

---

## Lazy Generation

```csharp
var ids = Uuid7.NewGuids(1000000);

foreach (var id in ids)
{
    Process(id);
}
```

Memory usage:

```text
Generate one UUID.
Process it.
Discard it.
Generate next UUID.
```

Only one identifier needs to exist at a time.

---

# Advantages of Lazy Generation

## Reduced Memory Consumption

Large batches do not require allocating huge collections.

Example:

```csharp
var ids = Uuid7.NewGuids(10_000_000);
```

The above statement does **not** immediately allocate ten million UUIDs.

Instead, identifiers are generated as they are consumed.

This significantly reduces memory pressure.

---

## Better Scalability

Applications processing millions of records can continue operating efficiently without exhausting available memory.

Examples include:

* Bulk imports.
* ETL pipelines.
* Data migrations.
* Event processing systems.

---

## Improved Performance

Reducing unnecessary allocations decreases:

* Garbage collection frequency.
* Memory fragmentation.
* Allocation overhead.

This often improves overall throughput.

---

## Streaming Support

Lazy sequences work naturally with LINQ and streaming APIs.

Example:

```csharp
var ids =
    Uuid7.NewGuids(1000)
          .Where(id => id != Guid.Empty)
          .Take(100);
```

The sequence is processed incrementally.

---

# Internal Implementation

Internally, batch generation is implemented using:

```csharp
internal static class Uuid7BatchGenerator
```

This type is internal and not exposed publicly.

Responsibilities:

* Validate input.
* Generate UUIDs on demand.
* Provide lazy enumeration.
* Avoid unnecessary allocations.

Simplified workflow:

```text
Caller
   │
   ▼
Uuid7.NewGuids(100)
   │
   ▼
Uuid7BatchGenerator.Generate(100)
   │
   ▼
Generate UUID #1
Yield UUID #1
Pause
Generate UUID #2
Yield UUID #2
Pause
...
```

---

# Parameter Validation

The batch generator validates the supplied count.

```csharp
ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(
    count,
    0);
```

The value must be greater than zero.

Valid:

```csharp
Uuid7.NewGuids(1);
Uuid7.NewGuids(10);
Uuid7.NewGuids(1000);
```

Invalid:

```csharp
Uuid7.NewGuids(0);
Uuid7.NewGuids(-5);
```

---

# Exception Handling

## ArgumentOutOfRangeException

Thrown when the supplied count is less than or equal to zero.

Example:

```csharp
Uuid7.NewGuids(0);
```

Produces:

```text
System.ArgumentOutOfRangeException:
'value must be greater than zero'
```

Recommended validation:

```csharp
if (count > 0)
{
    var ids = Uuid7.NewGuids(count);
}
```

---

# Batch Count Analyzer (DIXOR001)

Dixor.Identity ships with built-in Roslyn analyzers.

One analyzer specifically validates batch sizes:

```text
DIXOR001
```

The analyzer inspects calls to:

```csharp
Uuid7.NewGuids(...)
```

and reports diagnostics when an invalid constant batch size is supplied.

---

# Why Does This Analyzer Exist?

Without the analyzer:

```csharp
var ids = Uuid7.NewGuids(0);
```

The application compiles successfully.

The exception occurs only at runtime.

Runtime failures are more expensive and harder to detect.

The analyzer moves this validation to compile time.

This allows developers to fix problems before running the application.

---

# Invalid Examples

The following code produces diagnostic `DIXOR001`:

```csharp
var ids = Uuid7.NewGuids(0);
```

```csharp
var ids = Uuid7.NewGuids(-10);
```

Visual Studio displays:

```text
DIXOR001:
Batch size must be greater than zero.
```

---

# Valid Examples

```csharp
var ids = Uuid7.NewGuids(1);
```

```csharp
var ids = Uuid7.NewGuids(100);
```

```csharp
const int Count = 50;

var ids = Uuid7.NewGuids(Count);
```

---

# Compile-Time Constant Analysis

The analyzer only validates values known at compile time.

Example:

```csharp
const int Count = 0;

var ids = Uuid7.NewGuids(Count);
```

Produces:

```text
DIXOR001
```

However:

```csharp
int count = GetUserInput();

var ids = Uuid7.NewGuids(count);
```

No diagnostic is produced.

Reason:

The value is unknown during compilation.

Runtime validation still protects against invalid values.

---

# How the Analyzer Works

The analyzer performs the following steps:

1. Finds every method invocation.
2. Checks whether the method name is `NewGuids`.
3. Verifies that the containing type is `Uuid7`.
4. Retrieves the first argument.
5. Attempts to evaluate the argument as a compile-time constant.
6. Reports `DIXOR001` if the value is less than or equal to zero.

Workflow:

```text
Method Invocation
        │
        ▼
Is method NewGuids?
        │
       Yes
        │
        ▼
Belongs to Uuid7?
        │
       Yes
        │
        ▼
Argument supplied?
        │
       Yes
        │
        ▼
Compile-time constant?
        │
       Yes
        │
        ▼
Count <= 0 ?
        │
       Yes
        │
        ▼
Report DIXOR001
```

---

# Best Practices

## Prefer Batch Generation

Use batch generation when multiple identifiers are required.

Good:

```csharp
var ids = Uuid7.NewGuids(1000);
```

Avoid:

```csharp
for (int i = 0; i < 1000; i++)
{
    Uuid7.NewGuid();
}
```

---

## Stream Large Batches

Process large batches incrementally.

Good:

```csharp
foreach (var id in Uuid7.NewGuids(1_000_000))
{
    Save(id);
}
```

Avoid:

```csharp
var ids =
    Uuid7.NewGuids(1_000_000)
          .ToList();
```

unless all values must be retained.

---

## Validate External Input

When batch size originates from users or APIs:

```csharp
if (count <= 0)
{
    throw new ArgumentException(
        "Count must be greater than zero.");
}
```

---

# Summary

Batch generation provides:

* Efficient generation of multiple UUIDv7 values.
* Memory-efficient lazy evaluation.
* Reduced allocations.
* Better scalability for bulk operations.
* Improved developer productivity.
* Compile-time validation through Roslyn analyzers.

For applications generating large numbers of identifiers, batch generation offers both performance and usability benefits.

---

# Related Documentation

* [UUID Generation](./UUID7_Generation.md)
* [Monotonic Generation](./UUID7_MonotonicGeneration.md)
* [Formatting](./UUID7_Formatting.md)
* [DIXOR001](../analyzers/DIXOR001.md)

---

## Navigation

⬅ Previous: [Monotonic Generation](./UUID7_MonotonicGeneration.md)

➡ Next: [Formatting](./UUID7_Formatting.md)