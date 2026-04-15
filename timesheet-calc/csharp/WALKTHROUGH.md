# Timesheet Calc — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one primary entity (`Timesheet`), one small test-folder builder (`TimesheetBuilder`), one typed `Day` enum, one record for totals, and a pair of named-constants classes. Read the repo [Gears section](../../README.md#gears--bridging-tdd-and-bdd) for why middle gear is the deliberate choice.

## Scope — Weekly Hour Totals Only

The original TDD Buddy *Time Sheet Calculator* prompt is a single-day HH:mm parser: start time, end time, optional break, return duration in HH:mm. That is a string-parsing kata — it doesn't earn a builder, and shipping it as F2 would be a lie about what the tier teaches. **We reframed the kata to its natural F2 shape:** a weekly timesheet of day-keyed hour entries, classified as regular vs overtime. The `WithEntry(day, hours)` builder is the teaching artifact; the overtime rules live on the entity it builds. HH:mm parsing, hourly pay, and PTO categories are called out as stretch goals in the kata [`README.md`](../README.md#stretch-goals-not-implemented-here).

## The Design at a Glance

```
Day (enum) ─── IsWeekend() ─┐
                            │
TimesheetBuilder (tests/) ──WithEntry──> Timesheet ──Totals──> TimesheetTotals
                                         (validates hours >= 0)    RegularHours
                                                                   OvertimeHours
                                                                   TotalHours (derived)
```

Four source files under `src/TimesheetCalc/` (the enum + extension, the constants, the result record, the entity) and one builder under `tests/TimesheetCalc.Tests/`. That is the whole F2 surface.

## Why `Day` Is Our Own Enum (Not `System.DayOfWeek`)

`System.DayOfWeek` starts at `Sunday = 0` — a calendar convention, not a business-week convention. The domain vocabulary here orders Monday through Sunday with weekdays first and weekends last; exposing `IsWeekend()` as a first-class operation on the enum makes the classification rule read out loud. Aliasing to `System.DayOfWeek` would force every test to think in platform-calendar terms and would bury `IsWeekend()` in a helper.

The enum is flat (no underlying numeric semantics depended on) — this is domain classification, not date arithmetic.

## Why `OvertimeRules` Has Named Constants

`DailyOvertimeThreshold = 8` and `StandardWorkWeekHours = 40` are named for two reasons. First, F2 is Full-Bake mode (the opposite of F1's inline-literals policy) — business numbers that carry meaning get names. Second, the two constants are related by a rule (`5 * DailyOvertimeThreshold == StandardWorkWeekHours`) that is part of the spec, and naming them lets the test for the full-week scenario assert against `StandardWorkWeekHours` instead of a bare `40`.

## Why `Timesheet` Validates In Its Constructor, Not In The Builder

The rejection rule — `hours >= 0` — is a property of a valid `Timesheet`, not of one particular construction path. Every path that builds a timesheet (the public constructor, the test builder, any future JSON deserializer) must enforce it, so the check lives on the entity. The builder is a test-folder convenience; enforcing the invariant in the builder alone would let production code route around it.

The exception type is `ArgumentException` (C#-idiomatic). The message string — `"hours must not be negative"` — is identical byte-for-byte across C#, TypeScript, and Python, codified in `ErrorMessages`. Cross-language message-string parity is how this kata declares that the rejection is part of the spec, not an implementation accident.

## Why `TimesheetTotals` Is A Record With A Derived Property

`TotalHours` is always `RegularHours + OvertimeHours`. Storing it as a third field would invite drift between the sum and the parts. Deriving it as a computed property keeps the invariant enforced by construction — you cannot build an inconsistent `TimesheetTotals`.

Record equality is a free win for test diagnostics: `FluentAssertions` can print a full `TimesheetTotals` on a mismatch without custom `ToString()`.

## Why `TimesheetBuilder` Exists — The F2 Signature Pattern

Twelve scenarios need twelve slightly different timesheets. Without a builder, a five-day full-week scenario reads:

```csharp
var timesheet = new Timesheet(new Dictionary<Day, double>
{
    [Day.Monday] = 8,
    [Day.Tuesday] = 8,
    [Day.Wednesday] = 8,
    [Day.Thursday] = 8,
    [Day.Friday] = 8,
});
```

With the builder:

```csharp
var timesheet = new TimesheetBuilder()
    .WithEntry(Day.Monday, 8)
    .WithEntry(Day.Tuesday, 8)
    .WithEntry(Day.Wednesday, 8)
    .WithEntry(Day.Thursday, 8)
    .WithEntry(Day.Friday, 8)
    .Build();
```

The builder does not save lines on the full-week scenario — five `WithEntry` calls is still five lines. What it *does* save is the cognitive load of the dictionary-literal ceremony and the collection type. Each line names one thing the scenario cares about: a day and a count. The builder also models **entry replacement** naturally — `_entries[day] = hours` makes scenario 10 (later entries replace earlier ones) a property of the builder's dictionary semantics, not a feature the test has to set up around.

Fifteen lines of builder. That is the F2 budget and this builder spends it well. No object mother, no tuple return, no collaborator injection.

## What Is Deliberately Not Modeled

- **No `IClock`** — hours are numbers, not wall-clock times; no temporal collaboration.
- **No `Money`** — there is no pay rate here.
- **No `HH:mm` parsing** — string parsing is F1 territory; see the stretch-goals list.
- **No multi-week period** — every `Timesheet` is one week.
- **No entry category** — hours are hours. Holiday, PTO, sick-day classifications would tip the kata into F3.

Every omission points at an F3 extension. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/TimesheetCalc.Tests/TimesheetTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles (modulo C# underscore convention).

## How to Run

```bash
cd timesheet-calc/csharp
dotnet test
```
