# Bank OCR — C# Walkthrough

This kata ships in **middle/high gear** — the full C# implementation landed in one commit once the domain was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice.

Rather than stepping through twenty-two red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Parser (static)
  ├── ParseDigit(string[3])           : Digit
  └── ParseAccountNumber(string[3])   : AccountNumber

Digit (readonly record struct)         — 0-9 or Unknown
AccountNumber                          — 9 Digits; knows IsLegible, IsChecksumValid, Status
InvalidAccountNumberFormatException    — thrown for malformed input dimensions
OcrDimensions                          — named constants (DigitWidth, DigitHeight, AccountLength)
```

Two static parse functions, two domain types, one exception, one constants class. Each earns its keep.

## Why `Digit` Is a Type

A raw `int` cannot say "unknown". A raw `char` could carry `'?'` but then every site parsing digits does `if c == '?'` — the domain rule leaks into every consumer. `Digit` as a `readonly record struct` with a nullable `Value` lets callers ask `digit.IsKnown` and `account.Digits.All(d => d.IsKnown)` in ubiquitous language.

Equality is free (`record struct`), which matters for tests asserting `ParseDigit(block).Should().Be(Digit.Of(3))`. Using a plain int, the test would still pass, but it wouldn't be asserting that the result is *a domain digit with value 3* — just an int.

See `src/BankOcr/Digit.cs`.

## Why `AccountNumber` Is a Class, Not a String

Three concerns live on the account number: **what it reads as**, **whether it's legible**, and **whether its checksum validates**. A `string` can only answer the first. Bolting the other two onto a string API (`IsValid(string)`, `IsLegible(string)`) scatters domain logic across helper functions that tests would have to re-compose.

`AccountNumber` owns them. `account.Status` returns `"345882865"`, `"345882866 ERR"`, or `"34588286? ILL"` — three cases, one property, matching `SCENARIOS.md`'s status rules byte-for-byte.

The checksum uses named positions: `d1*9 + d2*8 + … + d9*1`. In code that's `Digits[i].Value * (AccountLength - i)` — the `AccountLength - i` expression is the "position weight" and reads right at a glance once `AccountLength` is named.

See `src/BankOcr/AccountNumber.cs`.

## Why `Parser` Is a Static Class

`Parser` holds no state. It's a pure function wrapped in a static class because the OCR glyph table is a compile-time lookup. Making it an instance would invite dependency-injection discussions that the domain doesn't have.

The glyph table stores each canonical digit as its **flattened 9-character grid** (top row + middle row + bottom row concatenated). Dictionary lookup by string is O(1) and the table reads as a table. Alternatives — a 10-element array indexed by digit, pattern-matching on the rows — obscure the fact that *each digit is a visual pattern* that either matches or doesn't.

`ParseDigit` validates the 3x3 block dimensions and throws `InvalidAccountNumberFormatException` on mismatch. `ParseAccountNumber` validates the 3x27 block, slices it into nine digit blocks, and delegates. Both validations live at the parse boundary — once a `Digit` or `AccountNumber` exists, its shape is guaranteed.

See `src/BankOcr/Parser.cs`.

## Why `InvalidAccountNumberFormatException`

F3 convention: domain rejections get domain names. Throwing `ArgumentException` for "this is a 2-row block" would be technically correct but force tests to catch the wrong kind of error. `InvalidAccountNumberFormatException` lets `act.Should().Throw<InvalidAccountNumberFormatException>()` read as *what went wrong in domain terms*.

See `src/BankOcr/InvalidAccountNumberFormatException.cs`.

## Why `DigitBuilder` and `AccountNumberBuilder`

Tests need two kinds of OCR input: **canonical** (for valid parses) and **deliberately malformed** (for ILL and unknown-digit scenarios). Writing `" _ "`, `"|_|"`, `"|_|"` inline in every test buries the signal.

- `DigitBuilder().ForDigit(8).Build()` — canonical glyph for 8.
- `DigitBuilder().ForDigit(8).WithRow(1, "|X|").Build()` — the same digit but with the middle row corrupted, to force `Digit.Unknown`.
- `AccountNumberBuilder().FromString("123456789").BuildRows()` — renders the 3x27 OCR block that parses back to `123456789`.
- `AccountNumberBuilder().FromString("345882865").WithDigitAt(8, garbled).BuildRows()` — canonical first eight digits, garbled ninth.

`FromString` is the ergonomic entry point; `WithDigitAt` is the escape hatch for crafting ILL scenarios without hand-writing garbled OCR strings. Both builders return the raw OCR rows (string arrays), not `AccountNumber` instances — because the thing under test is *the parser's behavior on OCR input*, not a pre-baked domain object.

See `tests/BankOcr.Tests/DigitBuilder.cs` and `AccountNumberBuilder.cs`.

## Why Named Constants Over Inline Literals

This is F3 (Full-Bake, full builders) — the opposite of F1 / Pedagogy mode. `DigitWidth = 3`, `DigitHeight = 3`, `AccountLength = 9`, `RowWidth = 27` are all named in `OcrDimensions`. Every parse function reads with domain vocabulary — `threeRows[0].Substring(i * DigitWidth, DigitWidth)` says what it does.

Inline `3`s would be technically equivalent and save three lines. They'd also leave a future reader wondering "is that 3 the digit-width, the row-height, or a coincidence?" One name closes the question.

See `src/BankOcr/Constants.cs`.

## Scope — Error Correction Deferred

Step 3 of the kata spec (suggest corrections by one-pixel edits) is flagged as a **bonus** in the scope and is not implemented here. The F3 reference covers parse, validate, and report; the correction step is a distinct algorithm that would double the scenario count without adding new domain-modeling lessons.

## Scenario Map

Twenty-two scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/BankOcr.Tests/BankOcrTests.cs`. Ten digit-parse scenarios collapse into one `[Theory]` with `[InlineData(0)] … [InlineData(9)]` — xUnit treats each inline case as a separate test, so the count matches the scenario list. The remaining twelve are individual `[Fact]`s.

## How to Run

```bash
cd bank-ocr/csharp
dotnet test
```
