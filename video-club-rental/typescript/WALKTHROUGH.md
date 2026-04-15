# Video Club Rental — TypeScript Walkthrough

This document complements the [C# walkthrough](../csharp/WALKTHROUGH.md). The design is the same — `VideoClub` as the aggregate, `Money` and `Age` as value types, `Clock` and `Notifier` as collaborators, three builders in `tests/`. This file names the TypeScript-specific choices.

## Structural Interfaces, Not Nominal

C# uses `IClock` and `INotifier` by name — a class must declare `: IClock` to satisfy it. TypeScript uses **structural typing**: any object with a `today(): Date` method is a `Clock`, whether it says so or not.

The implementation leans on this. `FixedClock` declares `implements Clock` for editor clarity, but nothing in `VideoClub` checks the brand — only the shape. That matters when a future test wants a throwaway clock:

```ts
const frozenClock: Clock = { today: () => new Date(Date.UTC(2026, 0, 1)) };
```

Works without ceremony. Nominal typing would have demanded a class.

## UTC Discipline for Dates

`Date` in JavaScript is a mutable 64-bit millisecond offset that formats itself in local time by default. Left alone, it makes every date-dependent test flaky: "day 15" in London is "day 14" in Los Angeles. This implementation keeps dates in **UTC** at every boundary:

- `FixedClock` stores a single `Date` that's never reformatted.
- `Rental.dueOn` is computed by adding `RENTAL_PERIOD_DAYS * MS_PER_DAY` to `rentedOn.getTime()` — pure arithmetic on UTC millis, no timezone in sight.
- `advanceDays(n)` does the same: `today.getTime() + n * MS_PER_DAY`.

`.toLocaleDateString` and `getDate` never appear. The kata doesn't format dates for display, so the question never comes up — but the discipline of UTC-only arithmetic keeps future formatting honest.

## Money Uses `number` With Rounding Guards

C# has a native `decimal`. TypeScript does not; `number` is IEEE-754 binary floating-point, and `0.1 + 0.2 !== 0.3`. For a kata whose arithmetic stops at £2.50 + £2.25 + £1.75 this is safe in practice, but `Money.plus` and `Money.equals` round to two decimal places defensively:

```ts
function round2(n: number): number { return Math.round(n * 100) / 100; }
```

This is enough for the twenty-four scenarios. A production codebase would reach for `big.js` or integer-pence representation — flag for future work, not this kata.

## Named Constants as Module-Level Exports

C# holds `BasePrice`, `PriorityAccessThreshold` etc. as static fields on `PricingPolicy` and `VideoClub`. TypeScript has no static-only classes, and free-function modules are the idiom. The constants become top-level `export const`:

```ts
export const BASE_PRICE = new Money(2.50);
export const PRIORITY_ACCESS_THRESHOLD = 5;
```

`PricingPolicy` itself is a module namespace (`import * as PricingPolicy from './PricingPolicy.js'`), which reads at call sites the same as the C# static class: `PricingPolicy.calculate(1, existing)`.

## Builders Return an Object, Not a Tuple

C# uses tuple-return for the ergonomic deconstruction `var (club, notifier, clock) = ...`. TypeScript tuples work (`[club, notifier, clock]`), but the conventional JS idiom is an **object with named fields**:

```ts
const { club, notifier, clock } = new VideoClubBuilder().build();
```

Tests read the same. Position doesn't matter; if a future scenario needs only the notifier, it destructures only the notifier.

## Test-Only Method on `User`

`user.seedPriorityPoints(n)` is marked "Test-only" in a JSDoc comment. TypeScript has no `internal` access modifier; `#private` would hide it from the builder too. The comment-plus-convention approach is the honest trade — tests need to seed state, so the method is public, and the comment names its audience.

## Scenario Map

The twenty-four scenarios live across six test files in `tests/`, one `describe` block per scenario group, matching the SCENARIOS.md section structure:

- `registration.test.ts`
- `rentalPricing.test.ts`
- `returns.test.ts`
- `priorityAccess.test.ts`
- `donations.test.ts`
- `wishlist.test.ts`

## How to Run

```bash
cd video-club-rental/typescript
npm install
npm test
```
