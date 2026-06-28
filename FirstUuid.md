# Creating Your First UUIDv7

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Getting Started](./QuickStart.md)

---

This guide walks you through generating your very first UUID version 7 identifier using Dixor.Identity.

By the end of this guide, you will:

* Generate a UUIDv7.
* Understand what makes it different from traditional GUIDs.
* Extract the embedded timestamp.
* Validate the generated identifier.

---

# Prerequisites

Before continuing, ensure that:

* .NET 10 or later is installed.
* Dixor.Identity has been installed.

If you have not installed the package yet, see:

```text
Installation.md
```

---

# Import The Namespace

Add the following namespace:

```csharp
using Dixor.Identity.UUID7;
```

---

# Generate Your First UUID

Create a new console application and write:

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.NewGuid();

Console.WriteLine(id);
```

Example output:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

Congratulations!

You have successfully generated your first UUID version 7 identifier.

---

# Understanding The Output

A UUIDv7 looks similar to a traditional GUID:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

However, unlike a random GUID, this identifier contains:

* A timestamp.
* Cryptographically secure random data.
* Chronological ordering information.

---

# Generate Multiple UUIDs

Generate several UUIDs:

```csharp
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(Uuid7.NewGuid());
}
```

Example output:

```text
019853a6-35c4-7e58-bf91-cb7d0b1b11d2
019853a6-35c5-70c4-92ab-0f49f4ad80c4
019853a6-35c6-7612-bc40-99f65f85fdb1
019853a6-35c7-76a4-8a77-d3bb77dd5b44
019853a6-35c8-70d9-83c4-2ec58e8424c0
```

Notice how the values are naturally ordered by creation time.

---

# Extract The Embedded Timestamp

UUIDv7 stores creation time inside the identifier.

Example:

```csharp
Guid id = Uuid7.NewGuid();

DateTimeOffset timestamp =
    Uuid7.GetTimestamp(id);

Console.WriteLine(timestamp);
```

Example output:

```text
2026-06-28T14:15:30+00:00
```

This timestamp represents when the UUID was generated.

---

# Validate The UUID

You can verify that the identifier is a UUIDv7.

```csharp
Guid id = Uuid7.NewGuid();

bool isUuid7 =
    Uuid7.IsUuid7(id);

Console.WriteLine(isUuid7);
```

Output:

```text
True
```

---

# Generate A Formatted Identifier

Business applications often require contextual identifiers.

Example:

```csharp
string orderId =
    Uuid7.NewString(
        prefix: "ORD");

Console.WriteLine(orderId);
```

Example output:

```text
ORD-019853a6-35c4-7e58-bf91-cb7d0b1b11d2
```

---

# Complete Example

```csharp
using Dixor.Identity.UUID7;

Guid id = Uuid7.NewGuid();

Console.WriteLine($"Id: {id}");

Console.WriteLine(
    $"Timestamp: {Uuid7.GetTimestamp(id)}");

Console.WriteLine(
    $"Valid UUIDv7: {Uuid7.IsUuid7(id)}");
```

Example output:

```text
Id: 019853a6-35c4-7e58-bf91-cb7d0b1b11d2
Timestamp: 2026-06-28T14:15:30+00:00
Valid UUIDv7: True
```

---

# What Next?

Now that you have created your first UUIDv7, continue with:

1. QuickStart.md
2. UUID7_Overview.md
3. Generation.md
4. MonotonicGeneration.md
5. UUID7_BestPractices.md

---

# Summary

In this guide you learned how to:

* Generate a UUIDv7.
* Understand its structure.
* Extract timestamps.
* Validate identifiers.
* Create formatted identifiers.

You are now ready to begin using UUIDv7 in real applications.

---

# Related Documentation

* [Installation](./Installation.md)
* [Quick Start](./QuickStart.md)
* [Migration Guide](./MigrationGuide.md)
* [UUIDv7 Overview](../uuid7/UUID7_Overview.md)
* [UUID Generation](../uuid7/UUID7_Generation.md)

---

## Navigation

⬅ Previous: [Quick Start](./QuickStart.md)

➡ Next: [Migration Guide](./MigrationGuide.md)