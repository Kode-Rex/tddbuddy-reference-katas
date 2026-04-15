# Bank OCR — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Digit** | A single parsed digit — either a numeric value 0–9 or **unknown** (rendered as `?`) |
| **Account Number** | Nine digits parsed from a 3-row OCR block; may be legible or contain unknowns |
| **OCR Block** | The 3-row, 27-column ASCII-art input representing one account number |
| **Checksum** | The modular arithmetic over digit positions that determines numeric validity |
| **Status** | The reported outcome per account: `VALID`, `ERR` (bad checksum), `ILL` (illegible) |

### Named Constants

| Name | Value | Meaning |
|------|-------|---------|
| `DigitWidth` | 3 | Columns per digit |
| `DigitHeight` | 3 | Rows per digit (and per OCR block) |
| `AccountLength` | 9 | Digits per account number |

## Digit Glyphs

Each digit is three rows of three characters. The canonical set:

```text
 _     _  _     _  _  _  _  _
| |  | _| _||_||_ |_   ||_||_|
|_|  ||_  _|  | _||_|  ||_| _|

 0  1  2  3  4  5  6  7  8  9
```

## Domain Rules

- An **OCR block** is exactly **3 rows** of exactly **27 characters** each (9 digits × 3 columns).
- Malformed OCR input (wrong row count or wrong row width) is rejected with `InvalidAccountNumberFormatException`.
- Each 3x3 sub-block matching a canonical digit glyph parses to that digit; anything else parses to **unknown** (`?`).
- An account number is **legible** iff all nine digits parsed as known values.
- The **checksum** for a legible account `d1 d2 … d9` is `(d1*9 + d2*8 + … + d9*1) mod 11 == 0`.
- **Status reporting:**
  - Legible and checksum valid → `"<number>"` (e.g. `"345882865"`)
  - Legible but checksum invalid → `"<number> ERR"` (e.g. `"345882866 ERR"`)
  - Any digit unknown → `"<number-with-?> ILL"` (e.g. `"34588286? ILL"`)

## Test Scenarios

### Digit Parsing

1. **Parses the glyph for zero**
2. **Parses the glyph for one**
3. **Parses the glyph for two**
4. **Parses the glyph for three**
5. **Parses the glyph for four**
6. **Parses the glyph for five**
7. **Parses the glyph for six**
8. **Parses the glyph for seven**
9. **Parses the glyph for eight**
10. **Parses the glyph for nine**
11. **A non-canonical glyph parses as unknown**

### Account Number Parsing

12. **Parses a full 9-digit account number from a 3x27 block**
13. **An account with one unreadable digit parses with an unknown in that position**
14. **Rejects an OCR block with the wrong number of rows**
15. **Rejects an OCR block with the wrong row width**

### Checksum Validation

16. **A legible account with a valid checksum reports as valid**
17. **A legible account with an invalid checksum reports as invalid**
18. **An account containing an unknown digit is not considered for checksum**

### Status Reporting

19. **Status for a valid account is just the number**
20. **Status for a bad-checksum account appends `ERR`**
21. **Status for an illegible account appends `ILL`**

### Builders

22. **`AccountNumberBuilder.fromString` renders a 3x27 OCR block matching the canonical glyphs**
