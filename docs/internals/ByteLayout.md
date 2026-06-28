# UUIDv7 Byte Layout

> 📚 Documentation Navigation
>
> **Home:** [README.md](../../README.md)
> **Section:** [Internals](https://github.com/DishantSave/Dixor/blob/main/docs/internals/Uuid7Specification.md)

---

This document explains how UUID version 7 values are represented internally within Dixor.Identity.

Understanding the byte layout is important for developers who want to understand:

* How UUIDv7 achieves chronological ordering.
* How timestamps are embedded.
* How version and variant bits are stored.
* How timestamps are later extracted.
* How RFC 9562 defines UUIDv7.

---

# Overview

A UUID always consists of **128 bits (16 bytes)**.

UUID version 7 divides these bits into multiple sections:

| Section        | Size    |
| -------------- | ------- |
| Unix Timestamp | 48 bits |
| Version        | 4 bits  |
| Random Data    | 74 bits |
| Variant        | 2 bits  |

The timestamp occupies the most significant bits, allowing UUIDs to sort chronologically.

---

# Complete Byte Layout

The following diagram illustrates the internal structure.

```text
Byte Index

 0        1        2        3        4        5
+--------+--------+--------+--------+--------+--------+
|                                                    |
|         48-bit Unix Timestamp (Milliseconds)       |
|                                                    |
+--------+--------+--------+--------+--------+--------+

 6                     7
+----------------+----------------+
| Ver | Random   |    Random      |
| 0111| 12 bits  |    8 bits      |
+----------------+----------------+

 8                     9
+----------------+----------------+
|Var | Random    |    Random      |
|10xx| 6 bits    |    8 bits      |
+----------------+----------------+

10       11       12       13       14       15
+--------+--------+--------+--------+--------+--------+
|                                                        |
|                 Random / Monotonic Data               |
|                                                        |
+--------+--------+--------+--------+--------+--------+
```

---

# Timestamp Section

The first six bytes store a Unix timestamp.

```text
Bytes 0-5
```

Size:

```text
48 bits
```

Unit:

```text
Milliseconds since Unix Epoch
```

Unix Epoch:

```text
1970-01-01T00:00:00Z
```

Example:

```text
01985A10E6A3
```

This hexadecimal value represents the timestamp embedded inside the UUID.

---

# Why Is The Timestamp Stored First?

Placing the timestamp at the beginning ensures that UUIDs generated later sort after UUIDs generated earlier.

Example:

```text
01985A10-E6A3-...
01985A10-E6A4-...
01985A10-E6A5-...
```

Sorting these strings naturally produces chronological order.

This behavior greatly improves database performance.

---

# Version Field

UUID versions are identified using four bits.

UUIDv7 always uses:

```text
0111
```

which equals:

```text
7
```

Location:

```text
Most significant four bits of byte 6
```

Implementation:

```csharp
bytes[6] &= 0x0F;
bytes[6] |= 0x70;
```

Binary:

```text
xxxx xxxx
0111 xxxx
```

---

# Variant Field

RFC 9562 requires UUIDs to use the RFC variant.

Location:

```text
Most significant two bits of byte 8
```

Required value:

```text
10
```

Implementation:

```csharp
bytes[8] &= 0x3F;
bytes[8] |= 0x80;
```

Binary:

```text
xxxx xxxx
10xx xxxx
```

---

# Random Data Section

After storing the timestamp, version and variant bits, remaining bits are filled using cryptographically secure randomness.

Implementation:

```csharp
RandomNumberGenerator.Fill(bytes[6..]);
```

Randomness provides:

* Global uniqueness.
* Collision resistance.
* Unpredictability.
* Security.

---

# Monotonic UUID Layout

Monotonic UUID generation slightly modifies the layout.

The final two bytes are reserved for a sequence counter.

```text
Bytes 14-15
```

Example:

```text
+--------+--------+
| Counter High    |
+--------+--------+
| Counter Low     |
+--------+--------+
```

This ensures ordering within the same millisecond.

---

# Endianness

RFC 9562 stores timestamps using:

```text
Big-endian order
```

Also known as:

```text
Network byte order
```

Example:

```text
Timestamp = 0x01985A10E6A3

Byte[0] = 0x01
Byte[1] = 0x98
Byte[2] = 0x5A
Byte[3] = 0x10
Byte[4] = 0xE6
Byte[5] = 0xA3
```

Dixor.Identity internally converts between .NET Guid representation and RFC byte ordering using `GuidByteConverter`.

---

# Visual Representation

```text
Byte:  0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F

       +---------------------------+
       |       Timestamp           |
       +---------------------------+

                           +-------+
                           | Ver=7 |
                           +-------+

                                 +----------------------+
                                 | Random Bits          |
                                 +----------------------+

                                       +-----+
                                       | Var |
                                       +-----+

                                             +----------------+
                                             | Random Data    |
                                             +----------------+
```

---

# Why Understanding The Layout Matters

Understanding the layout enables developers to:

* Extract timestamps.
* Validate UUID versions.
* Debug generation issues.
* Understand sorting behavior.
* Build interoperable systems.
* Integrate with external platforms.

---

# Related Documentation

* [UUIDv7 Specification](https://github.com/DishantSave/Dixor/blob/main/docs/internals/Uuid7Specification.md)
* [Design Decisions](https://github.com/DishantSave/Dixor/blob/main/docs/internals/DesignDecisions.md)
* [UUID Generation](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_Generation.md)
* [Timestamp Extraction](https://github.com/DishantSave/Dixor/blob/main/docs/uuid7/UUID7_TimestampExtraction.md)

---

## Navigation

⬅ Previous: [UUIDv7 Specification](https://github.com/DishantSave/Dixor/blob/main/docs/internals/Uuid7Specification.md)

➡ Next: [Design Decisions](https://github.com/DishantSave/Dixor/blob/main/docs/internals/DesignDecisions.md)