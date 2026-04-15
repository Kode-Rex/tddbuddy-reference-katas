# Bank OCR

Dave Thomas's classic OCR kata: parse 3x3 ASCII-art digit blocks into 9-digit account numbers, then validate with a checksum and report status. Excellent for practicing **domain types over strings**, **value-object builders**, and **layered parsing** (digit → account → status).

## What this kata teaches

- **Test Data Builders** — `DigitBuilder` constructs arbitrary 3x3 grids (including malformed ones); `AccountNumberBuilder.fromString("123456789")` renders the canonical OCR layout for a number.
- **Domain Types** — `Digit` and `AccountNumber` are types, not strings. A `Digit` either holds a numeric value or is unknown. An `AccountNumber` knows whether it's legible and whether its checksum is valid.
- **Domain Exceptions** — malformed input (wrong row count, wrong row width) throws `InvalidAccountNumberFormatException`. The exception names the *rejection*, not the mechanism.
- **Named Constants** — `DigitWidth = 3`, `DigitHeight = 3`, `AccountLength = 9`. Full-bake mode: when the literal has a name in the spec, the code uses it.
- **Layered Parsing** — `parseDigit` and `parseAccountNumber` compose; checksum and status reporting are separate concerns stacked on top.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
