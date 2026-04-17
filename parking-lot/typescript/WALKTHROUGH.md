# Parking Lot — TypeScript Walkthrough

Same design as the [C# walkthrough](../csharp/WALKTHROUGH.md). This file notes what's idiomatic to TypeScript and what deliberately diverges.

## Idiomatic Deltas

- **`VehicleType` and `SpotType` are string literal unions**, not enums. `'motorcycle' | 'car' | 'bus'` is the TS idiom — exhaustive switch narrowing works identically, and the values are self-documenting in test output without an enum-to-string conversion step.
- **`Vehicle`, `Ticket`, and `Fee` are plain interfaces**, not classes. TS interfaces with `readonly` fields give the same immutable-value shape as C#'s record structs, but without needing constructor boilerplate. Structural equality works naturally with Vitest's `toEqual`.
- **`Lot` uses `Map<SpotType, number>`** for available spots and `Map<string, Ticket>` for active tickets. Maps are the idiomatic TS collection for keyed lookup — equivalent to C#'s `Dictionary`.
- **Millisecond arithmetic** for durations. JavaScript has no `TimeSpan`; the clock returns `Date` and fee calculation uses `getTime()` differences divided by `MS_PER_HOUR`. `FixedClock` offers `advanceHours` and `advanceMinutes` convenience methods so tests read in domain units.
- **Ticket equality** in `processExit` compares `spotType` (string equality), `vehicle.licensePlate` (string equality), and `entryTime.getTime()` (epoch ms equality). TS has no structural record equality, so the comparison is explicit field-by-field.
- **Error classes** extend `Error` and set `this.name` — `VehicleAlreadyParkedError`, `NoAvailableSpotError`, `InvalidTicketError`, `InvalidLotConfigurationError`. Messages are byte-identical to C# and Python.

## Shared Design (see C# walkthrough for rationale)

- Smallest-fit-first spot allocation via preference arrays.
- `Clock` collaborator + `FixedClock` test double.
- `LotBuilder` returns `{ lot, clock }` so tests drive both.
- `VehicleBuilder` with `asMotorcycle()` / `asCar()` / `asBus()` convenience methods.
- Four domain exception types with byte-identical messages.
- Seven test files mirroring the C# split.

## How to Run

```bash
cd parking-lot/typescript
npm install
npx vitest run
```
