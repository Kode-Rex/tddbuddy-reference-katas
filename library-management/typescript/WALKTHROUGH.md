# Library Management — TypeScript Walkthrough

Ships in **middle gear** — full implementation in one commit. The [C# walkthrough](../csharp/WALKTHROUGH.md) is the primary design-rationale document. This note captures the TypeScript-specific adaptations.

## Same Design, TypeScript Idioms

```
Library ──owns──> Book[] (title, author, Isbn, copies)
    │              └── Copy (id, status: CopyStatus enum)
    ├──owns──> Member[]
    ├──owns──> Loan[]        (member, copy, borrowedOn, dueOn)
    ├──owns──> Reservation[] (member, isbn, reservedOn, notifiedOn)
    ├──collab──> Clock       — injected interface
    └──collab──> Notifier    — injected interface
```

`Library` is the aggregate; `Clock` and `Notifier` are `interface` types rather than C# interfaces; everything else reads identically.

## Date Discipline

Loan due dates and reservation expiry both do arithmetic in days. TypeScript's `Date` is a millisecond instant, so every offset is computed via `new Date(t.getTime() + days * MS_PER_DAY)` and every comparison uses `getTime()`. Tests construct `new Date(Date.UTC(2026, 0, 1))` to avoid local-timezone drift, which would otherwise make `+14 days` non-deterministic across developer machines.

This is the same discipline as video-club-rental; see its walkthrough for the longer argument.

## Module-Level Constants, `SCREAMING_CASE`

`LOAN_PERIOD_DAYS`, `FINE_PER_DAY`, and `RESERVATION_EXPIRY_DAYS` live at module scope where TypeScript idiom puts them, not as class statics. Tests import them so assertions read `clock.advanceDays(LOAN_PERIOD_DAYS + 1)` — the rule is named, not magic-numbered.

See `src/Loan.ts` and `src/Reservation.ts`.

## `Money` as a Class, Not a Branded Primitive

A branded `type Money = number & { readonly __brand: 'Money' }` is lighter, but adding/comparing monies still bottoms out on raw arithmetic. A class with `plus`, `times`, `equals` keeps the operations named and rounds to two decimal places on every constructor exit — enough precision for fines.

See `src/Money.ts`.

## No `internal` — `readonly` and Method Intent

TypeScript doesn't have C#'s `internal`; `Copy.markCheckedOut`, `Loan.close`, `Reservation.markNotified` are public methods. The discipline is that only `Library` calls them. `readonly` on ctor parameters prevents the simplest leaks (callers can't write `copy.isbn = ...`). This is weaker than C#'s compiler-enforced boundary, but the naming convention makes the intent plain to readers.

## Scenario Map

- `tests/books.test.ts` — scenarios 1–5
- `tests/members.test.ts` — scenarios 6–7
- `tests/checkouts.test.ts` — scenarios 8–10
- `tests/returns.test.ts` — scenarios 11–15
- `tests/reservations.test.ts` — scenarios 16–20

One `it` per scenario.

## How to Run

```bash
cd library-management/typescript
npm install
npm test
```
