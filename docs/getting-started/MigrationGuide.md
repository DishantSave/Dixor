# Migration Guide

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)  
> **Section:** [Getting Started](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/QuickStart.md)

---

# Migrating To Dixor.Identity

This guide explains how to migrate existing applications to Dixor.Identity and UUID version 7.

Common migration scenarios include:

* Migrating from `Guid.NewGuid()`.
* Migrating from UUIDv4.
* Migrating from third-party identifier libraries.
* Migrating existing database schemas.

---

# Why Migrate?

Traditional random GUIDs can cause:

* Database fragmentation.
* Poor index locality.
* Lack of chronological ordering.
* Additional timestamp columns.

UUIDv7 solves these issues.

---

# Migrating From Guid.NewGuid()

Existing code:

```csharp
Guid id = Guid.NewGuid();
```

Replace with:

```csharp
Guid id = Uuid7.NewGuid();
```

No additional changes are required.

---

# Before

```csharp
Order order = new()
{
    Id = Guid.NewGuid()
};
```

---

# After

```csharp
Order order = new()
{
    Id = Uuid7.NewGuid()
};
```

---

# Migrating From UUIDv4

Most applications can migrate seamlessly.

Existing database column:

```sql
UNIQUEIDENTIFIER
```

can remain unchanged.

No schema modification is required.

---

# Mixed Environments

Applications may temporarily contain both:

* UUIDv4 values.
* UUIDv7 values.

Use validation:

```csharp
if (Uuid7.IsUuid7(id))
{
}
```

to identify UUIDv7 values.

---

# Migrating Existing Data

Existing identifiers should generally remain unchanged.

Recommended approach:

* Preserve historical identifiers.
* Generate UUIDv7 only for new records.

Example:

```text
Existing Records → UUIDv4
New Records      → UUIDv7
```

---

# Migrating Database Keys

Most databases already support UUID storage.

No schema changes are usually required.

Examples:

| Database   | Existing Type    |
| ---------- | ---------------- |
| SQL Server | UNIQUEIDENTIFIER |
| PostgreSQL | UUID             |
| MySQL      | BINARY(16)       |
| SQLite     | BLOB             |

---

# Migrating From Sequential GUID Libraries

Existing:

```csharp
Guid id =
    SequentialGuid.Create();
```

Replace:

```csharp
Guid id =
    Uuid7.NewMonotonicGuid();
```

---

# Migrating From ULID Libraries

Existing:

```csharp
Ulid id = Ulid.NewUlid();
```

Replace:

```csharp
Guid id = Uuid7.NewGuid();
```

Benefits:

* Native .NET Guid support.
* RFC 9562 compliance.
* Database compatibility.

---

# Updating Validation Logic

Existing:

```csharp
Guid.TryParse(input, out Guid id);
```

Recommended:

```csharp
Uuid7.IsValid(input);
```

or:

```csharp
Uuid7.TryParse(input, out Guid id);
```

---

# Updating Unit Tests

Before:

```csharp
Assert.NotEqual(
    Guid.Empty,
    id);
```

After:

```csharp
Assert.True(
    Uuid7.IsUuid7(id));
```

---

# Recommended Migration Strategy

1. Install Dixor.Identity.
2. Replace new identifier generation.
3. Keep existing identifiers unchanged.
4. Update tests.
5. Monitor database performance.
6. Gradually adopt advanced features.

---

# Migration Checklist

* [ ] Install Dixor.Identity.
* [ ] Replace Guid.NewGuid().
* [ ] Update unit tests.
* [ ] Validate external input.
* [ ] Preserve historical identifiers.
* [ ] Monitor indexes.
* [ ] Review analyzer diagnostics.

---

# Summary

Migrating to Dixor.Identity is typically straightforward.

In most applications, migration requires only replacing:

```csharp
Guid.NewGuid()
```

with:

```csharp
Uuid7.NewGuid()
```

while keeping existing database schemas unchanged.

The result is improved database performance, chronological ordering and RFC 9562 compliance.

---

# Related Documentation

* [Installation](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/Installation.md)
* [Quick Start](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/QuickStart.md)
* [Generate Your First UUID](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/FirstUuid.md)
* [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)
* [UUID Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Generation.md)

---

## Navigation

⬅ Previous: [Generate Your First UUID](https://github.com/DishantSave/Dixor/blob/main/docs/getting-started/FirstUuid.md)

➡ Next: [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)