# Weather Station — C# Walkthrough

This kata ships in **middle/high gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-four red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Station ──owns──> Reading[]
   │
   ├── Record(temp, humidity, windSpeed) : Alert[]
   ├── GetStatistics() : Statistics
   └── Readings : IReadOnlyList<Reading>
```

Five domain types. Each earns its keep.

## Why `Reading` Is a Record Struct

A reading is a value — immutable once captured, compared by content. `readonly record struct` gives value equality, pattern-matching destructuring, and zero-allocation storage on the stack. There's no identity to a reading; two observations with the same temperature, humidity, wind speed, and timestamp are the same reading.

See `src/WeatherStation/Reading.cs`.

## Why `IClock`, Not `DateTime.Now`

Four scenarios assert on the **timestamp** recorded with a reading. If those tests called `DateTime.Now` directly, they'd be correct today and flaky forever after. More importantly, they'd couple the test to real time, which is a collaboration the test doesn't intend to have.

`IClock.Now()` makes the collaboration explicit. `FixedClock` in the test project is the mock — a tiny deterministic implementation. The tests read "at noon on June 15, I recorded 25 C" instead of "at whatever now is, I recorded something."

See `src/WeatherStation/IClock.cs` and `tests/WeatherStation.Tests/FixedClock.cs`.

## Why Domain-Specific Exceptions

`InvalidReadingException` and `NoReadingsException` name the rejection in domain language. A stack trace that says `InvalidReadingException: Humidity must be between 0 and 100.` tells the reader what went wrong without opening code. Throwing a generic `InvalidOperationException` hides the domain under mechanism.

The exception messages are **byte-identical across C#, TypeScript, and Python** — the business rule doesn't change because the language does.

See `src/WeatherStation/InvalidReadingException.cs` and `src/WeatherStation/NoReadingsException.cs`.

## Why `StationBuilder`

Most tests need a station with pre-recorded history. Without a builder, every test arranges a clock, creates a station, records readings one by one. With `new StationBuilder().WithReading(30m, 50m, 10m).Build()`, setup is one line that reads like English.

The builder returns **both** the station and its clock as a tuple. Tests that want to advance time after setup need clock access. Returning a tuple keeps the builder pure.

`ReadingBuilder` exists alongside `StationBuilder` for tests that need to construct specific reading parameters without a full station context — primarily for validation edge-case scenarios.

See `tests/WeatherStation.Tests/StationBuilder.cs` and `tests/WeatherStation.Tests/ReadingBuilder.cs`.

## Why `Record` Returns Alerts

The alternative is a separate `CheckAlerts()` method the caller must remember to call. Returning alerts from `Record` makes the alert evaluation automatic and the API impossible to misuse — you can't record a reading without seeing its alerts.

`AlertThresholds` is a value type with nullable fields. No threshold configured means no alert evaluation for that dimension. This avoids boolean flags or sentinel values.

See `src/WeatherStation/Station.cs` and `src/WeatherStation/AlertThresholds.cs`.

## Why `Statistics` Is a Record Struct

Statistics are a projection — a snapshot computed from the reading history at query time. Making it a record struct means callers get value equality for free (useful in tests) and the struct communicates "this is data, not behavior."

See `src/WeatherStation/Statistics.cs`.

## Scenario Map

The twenty-four scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/WeatherStation.Tests/StationTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd weather-station/csharp
dotnet test
```
