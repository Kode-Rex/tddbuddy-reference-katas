# Timesheet Calc

A weekly `Timesheet` aggregates daily hour entries and classifies them as **regular** or **overtime** according to a small set of rules. `timesheet.totals()` returns a `TimesheetTotals` object carrying `regularHours`, `overtimeHours`, and `totalHours` — the split *is* the spec.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**: one primary entity (`Timesheet`), one small test-folder builder (`TimesheetBuilder`), a typed `DayOfWeek` enum distinguishing weekdays from the weekend, and named constants for the overtime threshold and standard week length. No collaborators, no object mothers, no clocks. The teaching point is the F2 builder payoff: every test opens with one readable line — `TimesheetBuilder().WithEntry(Monday, 9).WithEntry(Tuesday, 8).Build()` — that names only the variation it cares about.

## Scope — Weekly Hour Totals Only

The original TDD Buddy [Time Sheet Calculator prompt](https://www.tddbuddy.com) describes a single-day HH:mm parser: take a start time, an end time, and an optional break, return the worked duration in HH:mm. That is a single-function string-parsing kata — F1 shape — and it doesn't earn a builder.

**This reference reframes the kata to its natural F2 shape:** a weekly timesheet of daily hours, with overtime classification. A `TimesheetBuilder` with a `WithEntry(day, hours)` method is the central teaching artifact, and the entity it builds (`Timesheet`) carries the overtime rules. The HH:mm parsing bonus is listed as a stretch goal below.

### Stretch Goals (Not Implemented Here)

These are deliberately left out — each either drifts the kata back toward F1 string-parsing or tips it into F3 collaborator territory:

- **HH:mm parsing** — the original single-day start/end/break calculator in HH:mm, including inverted times (overnight shifts) and the 3/4-digit / AM-PM bonuses. Pure string-parsing; lives naturally as its own F1 kata.
- **Multi-week timesheets** — rolling 2-week / 4-week periods, period-total reports.
- **Hourly pay rate** — converting hours to `Money` with a regular rate and an overtime multiplier (typically 1.5x). Introduces a `Money` value type and a `PayRate` collaborator — F3 shape.
- **Holiday / PTO categories** — statutory holidays, paid time off, sick days as distinct entry types.
- **Persistence / submission workflow** — approval, locking, audit trail. F3 with collaborators (`ITimesheetRepository`, `IApprover`, `IClock`).

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference does satisfy.
