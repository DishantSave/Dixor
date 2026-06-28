# UUID Version 7 Specification

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [Internals](https://github.com/DishantSave/Dixor/blob/main/docs/internals/Uuid7Specification.md)

---

This document provides an overview of the UUID version 7 specification implemented by Dixor.Identity.

UUID version 7 is formally defined by RFC 9562.

---

# What Is UUID Version 7?

UUID version 7 is a universally unique identifier format that combines:

* Timestamp information.
* Randomness.
* Natural sorting.

Example:

```text
01985a10-e6a3-7db0-9816-9a50e4f42d5d
```

---

# RFC 9562

UUIDv7 is standardized by:

```text
RFC 9562
Universally Unique IDentifiers (UUIDs)
```

The specification was published by the Internet Engineering Task Force (IETF).

---

# UUID Size

Every UUID occupies:

```text
128 bits
16 bytes
```

---

# Structure

```text
48-bit Unix Timestamp
4-bit Version
74-bit Random Data
2-bit Variant
```

---

# Timestamp

UUIDv7 stores:

```text
Unix time in milliseconds
```

inside:

```text
Bytes 0-5
```

Example:

```text
2026-06-28T12:00:00Z
```

becomes:

```text
1751112000000
```

milliseconds since Unix epoch.

---

# Version

UUIDv7 always stores:

```text
0111
```

which represents:

```text
Version 7
```

---

# Variant

RFC 9562 requires:

```text
10xx xxxx
```

as the variant field.

---

# Ordering

Because timestamps occupy the most significant bytes:

```text
UUID generated later
>
UUID generated earlier
```

This property enables chronological sorting.

---

# Comparison With Other UUID Versions

| Version | Ordering | Timestamp | Randomness |
| ------- | -------- | --------- | ---------- |
| v1      | Partial  | Yes       | Low        |
| v4      | No       | No        | High       |
| v6      | Yes      | Yes       | Medium     |
| v7      | Yes      | Yes       | High       |

---

# Advantages Of UUIDv7

* Naturally sortable.
* Globally unique.
* Database friendly.
* Standards compliant.
* High entropy.
* Distributed-system friendly.

---

# Database Benefits

Traditional random UUIDs often fragment indexes.

UUIDv7 minimizes fragmentation because newly generated values are generally appended near the end of indexes.

Benefits include:

* Faster inserts.
* Better cache locality.
* Reduced page splits.
* Improved clustered index performance.

---

# Monotonic Recommendations

RFC 9562 recommends monotonic behavior when generating multiple UUIDs within the same timestamp.

Dixor.Identity provides:

```csharp
Uuid7.NewMonotonic()
```

for this purpose.

---

# Compliance

Dixor.Identity follows RFC 9562 by:

* Using 48-bit Unix timestamps.
* Using version field value 7.
* Using RFC variant bits.
* Using cryptographically secure randomness.
* Preserving byte ordering requirements.

---

# Limitations

UUIDv7 guarantees uniqueness with extremely high probability.

Absolute uniqueness cannot be mathematically guaranteed in distributed systems.

However, collision probability is negligible for practical applications.

---

# Related Documentation

* [UUIDv7 Overview](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Overview.md)
* [Byte Layout](https://github.com/DishantSave/Dixor/blob/main/docs/internals/ByteLayout.md)
* [Design Decisions](https://github.com/DishantSave/Dixor/blob/main/docs/internals/DesignDecisions.md)
* [RFC 9562](https://www.rfc-editor.org/rfc/rfc9562)

---

## Navigation

⬅ Previous: [Sequential Identifier Examples](https://github.com/DishantSave/Dixor/blob/main/docs/examples/SequentialIdentifiersExamples.md)

➡ Next: [Byte Layout](https://github.com/DishantSave/Dixor/blob/main/docs/internals/ByteLayout.md)