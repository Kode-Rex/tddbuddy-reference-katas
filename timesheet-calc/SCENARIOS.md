# Timesheet Calc — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **weekly hour classification only**: given a set of day→hours entries over a single week, how many hours are regular vs overtime. HH:mm parsing, multi-week rollups, pay calculation, PTO/holiday categories, and submission workflow are **out of scope** — see the top-level [`README.md`](README.md#scope--weekly-hour-totals-only) for the stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Timesheet** | An immutable collection of daily `Entry` records over a single week. Each day of the week appears at most once. Exposes `totals()` which returns a `TimesheetTotals`. |
| **Entry** | A single `(day, hours)` pair. `hours` is a non-negative number (fractional hours allowed). `day` is a `DayOfWeek`. |
| **DayOfWeek** | A typed enum of the seven days — `Monday`, `Tuesday`, `Wednesday`, `Thursday`, `Friday`, `Saturday`, `Sunday`. Monday through Friday are **weekdays**; Saturday and Sunday are the **weekend**. |
| **TimesheetTotals** | The computed split of hours across the whole week: `regularHours`, `overtimeHours`, `totalHours`. `totalHours == regularHours + overtimeHours` is an invariant. |
| **TimesheetBuilder** | Test-folder fluent builder that produces a `Timesheet`. `WithEntry(day, hours)` adds (or replaces) the entry for that day; `Build()` produces the `Timesheet`. Exists so tests read one line per entry that names exactly the variation the scenario cares about. |

## Domain Rules

- **Overtime threshold (weekday)** — on any weekday, hours worked **beyond 8** are overtime. An entry of 10 hours on Monday is 8 regular + 2 overtime.
- **Weekend hours are always overtime** — Saturday and Sunday contribute 0 regular hours and all-hours overtime, regardless of count.
- **Standard work week** — 40 regular hours (5 weekdays × 8 hours). This is the natural ceiling on weekly regular hours: a full Monday–Friday at 8 hours each produces 40 regular / 0 overtime / 40 total.
- **No entries, no hours** — a timesheet with no entries totals 0 regular / 0 overtime / 0 total.
- **Invariant** — `totalHours == regularHours + overtimeHours` always holds. `totalHours` equals the sum of every entry's hours.
- **Hours are non-negative** — attempting to build a timesheet with a negative hour count raises a language-idiomatic error (C# `ArgumentException`, TS `Error`, Python `ValueError`). The message string is identical across languages: `"hours must not be negative"`.
- **Fractional hours are permitted** — `8.5` hours on Monday is 8 regular + 0.5 overtime. Arithmetic uses the language's native floating-point / decimal type; tests assert with tolerance where needed.
- **Entry replacement** — `WithEntry(Monday, 8)` followed by `WithEntry(Monday, 10)` on the same builder keeps only the second entry (10 hours on Monday).

## Named Constants

Identical across all three languages. Pulled into named constants because F2 is Full-Bake mode — business numbers are named.

| Constant | Value | Meaning |
|----------|-------|---------|
| `DailyOvertimeThreshold` | `8` | Hours worked on a single weekday beyond this count are overtime. |
| `StandardWorkWeekHours` | `40` | The sum of `DailyOvertimeThreshold` across the five weekdays. Exposed as the natural ceiling on regular hours. |

## Test Scenarios

1. **An empty timesheet totals zero across the board** — `Timesheet` with no entries has `regularHours=0`, `overtimeHours=0`, `totalHours=0`.
2. **A single 8-hour weekday is all regular** — `WithEntry(Monday, 8)` gives `regular=8`, `overtime=0`, `total=8`.
3. **A single weekday under 8 hours is all regular** — `WithEntry(Tuesday, 6)` gives `regular=6`, `overtime=0`, `total=6`.
4. **Weekday hours beyond 8 spill into overtime** — `WithEntry(Monday, 10)` gives `regular=8`, `overtime=2`, `total=10`.
5. **Fractional weekday overtime is tracked** — `WithEntry(Monday, 8.5)` gives `regular=8`, `overtime=0.5`, `total=8.5`.
6. **Saturday hours are all overtime** — `WithEntry(Saturday, 4)` gives `regular=0`, `overtime=4`, `total=4`.
7. **Sunday hours are all overtime** — `WithEntry(Sunday, 6)` gives `regular=0`, `overtime=6`, `total=6`.
8. **A full Monday-to-Friday at 8 hours each totals the standard 40-hour week** — five `WithEntry(<weekday>, 8)` calls give `regular=40`, `overtime=0`, `total=40`.
9. **A mixed week combines weekday overtime with weekend overtime** — Monday 9, Tuesday 8, Wednesday 8, Thursday 8, Friday 10, Saturday 5 gives `regular=40` (8 per weekday, capped), `overtime=1+0+0+0+2+5 = 8`, `total=48`.
10. **Later entries for the same day replace earlier entries** — `WithEntry(Monday, 8)` then `WithEntry(Monday, 10)` yields the 10-hour Monday result (`regular=8`, `overtime=2`, `total=10`).
11. **A negative hours entry is rejected with the identical cross-language message** — building with `WithEntry(Monday, -1)` raises the language-idiomatic error with message `"hours must not be negative"`.
12. **A zero-hour entry is valid and contributes nothing** — `WithEntry(Monday, 0)` gives `regular=0`, `overtime=0`, `total=0` (distinct from "no entry at all" only in that Monday is recorded; the totals are the same).
