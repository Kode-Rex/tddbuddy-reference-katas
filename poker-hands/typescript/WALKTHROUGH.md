# Poker Hands — TypeScript Walkthrough

This walkthrough is a **delta** from the [C# walkthrough](../csharp/WALKTHROUGH.md). The design is the same — read that document first for the rationale behind `Card`/`Rank`/`Suit` as types, the 5-card invariant, the two-builder shape, and the canonical tie-break signature. This file covers what's idiomatic to TypeScript and where the shapes diverge.

## Same Design, Different Idiom

- **`Rank` and `HandRank`** are numeric enums (`Ace = 14`, `StraightFlush = 9`). The integer backing makes `Rank.Ace > Rank.King` and `myRank > theirRank` work by the same natural operator the C# enums get. No comparator function required.
- **`Suit` and `Compare`** are string enums. They're unordered and we want readable debug output; string-backed values print as their name.
- **`Card` is an `interface` with a factory function** `card(rank, suit)` rather than a record struct. TypeScript's structural typing gives us the same value-equality-in-spirit (two cards with the same rank and suit are interchangeable) without a class.
- **`Hand` is a class** because constructor validation is the natural TypeScript pattern for enforcing the 5-card invariant. The private `readonly` cards field mirrors the C# implementation.
- **`InvalidHandError` extends `Error`** — the domain-specific exception type convention, with the name property set for stack-trace readability. The message string `"A hand must have exactly 5 cards (got N)"` is byte-identical to the C# and Python versions.

## Why No Collaborators and No `Money`

Unlike `bank-account` or `library-management`, poker-hands has no time-dependent logic (no clocks) and no notifications. It also has no monetary values — the kata is pure combinatorics over five cards. That's why the TS implementation has no `FixedClock` / `RecordingNotifier` / `Money` equivalents. Every line of `src/` is a pure function or an immutable value type.

## `noUncheckedIndexedAccess` and the `!` Operator

The `tsconfig.json` enables `noUncheckedIndexedAccess`, which makes `array[i]` return `T | undefined`. Inside `Hand.compareTo`, the signature arrays are guaranteed same length (both are evaluated from 5-card hands), so `mySig[i]!` is a local assertion that narrowing-per-iteration would verify at runtime. The alternative — a `for...of` zip — reads worse and buys no safety. The `!` markers are the price of strict indexing; they're confined to the inner loop where the invariant is local and obvious.

## Builders

- **`CardBuilder`** — `.ofRank(Rank.Ace).ofSuit(Suit.Spades).build()`. Fluent, defaults to Two of Clubs.
- **`HandBuilder`** — fluent `.with(card)` accumulator plus the static `HandBuilder.fromString("2H 3D 5S 9C KD")` factory that delegates to `Hand.parse`. Same two-shape decision as the C# implementation: shorthand form for hand-level evaluation, card-by-card form for tests that care about individual card properties.

See `tests/CardBuilder.ts`, `tests/HandBuilder.ts`.

## Scenario Map

Twenty scenarios across five `*.test.ts` files in `tests/`:

- `handConstruction.test.ts` — scenarios 1–3
- `handRanking.test.ts` — scenarios 4–12
- `handComparison.test.ts` — scenarios 13–15
- `tieBreakers.test.ts` — scenarios 16–19
- `ties.test.ts` — scenario 20

## How to Run

```bash
cd poker-hands/typescript
npm install
npx vitest run
```
