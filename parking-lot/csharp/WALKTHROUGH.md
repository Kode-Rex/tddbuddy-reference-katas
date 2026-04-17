# Parking Lot — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-two red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
VehicleType (enum)         — Motorcycle, Car, Bus
SpotType (enum)            — Motorcycle, Compact, Large
Vehicle (readonly record struct)  — (Type, LicensePlate)
Ticket (sealed record)     — (Vehicle, SpotType, EntryTime)
Fee (readonly record struct)      — (Amount)

Lot
   ├── ParkEntry(vehicle) : Ticket   — allocates smallest fitting spot, records entry
   ├── ProcessExit(ticket) : Fee     — frees spot, calculates time-based fee
   └── private Dictionary<string, Ticket>  — active tickets keyed by plate

Collaborator: IClock.Now() — injected; FixedClock in tests
Defaults: MotorcycleRate = $1/hr, CarRate = $3/hr, BusRate = $5/hr
```

## Why Smallest-Fit-First Allocation

The spec says motorcycles fit in any spot, cars fit in compact or large, buses need large only. A naive implementation would just grab the first available spot — but that starves large vehicles of their only option when small vehicles consume large spots unnecessarily.

Smallest-fit-first is a single `SpotType[]` preference list per vehicle type:
- Motorcycle: `[Motorcycle, Compact, Large]`
- Car: `[Compact, Large]`
- Bus: `[Large]`

The `AllocateSpot` method walks the preference list and takes the first spot type with availability. This means a motorcycle only overflows to a compact spot when motorcycle spots are exhausted, and only to a large spot when both are gone. The end-to-end worked example exercises this: after exiting a motorcycle, a car cannot park in the freed motorcycle spot — it doesn't fit.

See `src/ParkingLot/Lot.cs`.

## Why `Vehicle` Is a Value Type

A vehicle is identified by its `(Type, LicensePlate)` pair. Two vehicles with the same plate and type are the same vehicle — there's no mutable state or identity beyond those two fields. `readonly record struct` gives structural equality, which makes ticket comparisons and duplicate-parking detection straightforward.

See `src/ParkingLot/Vehicle.cs`.

## Why `Ticket` Is a Sealed Record, Not a Value Type

Unlike `Vehicle` and `Fee`, a `Ticket` carries a `DateTime` and participates in reference-equality checks when validating exit. Using `sealed record` (reference type with value-based equality) gives the right semantics: two tickets with the same vehicle, spot, and entry time compare equal, which is exactly what `ProcessExit` needs to validate that the presented ticket matches the stored one.

See `src/ParkingLot/Ticket.cs`.

## Why `Fee` Is a Separate Type, Not a Decimal

Returning a bare `decimal` from `ProcessExit` would work, but it loses the domain name. A `Fee(decimal Amount)` value type names the concept and prevents accidental arithmetic with unrelated decimals. At this kata's scale it's a light touch — but it establishes the pattern a production system would grow into (currency, formatting, tax breakdown).

See `src/ParkingLot/Fee.cs`.

## Why `IClock`, Not `DateTime.UtcNow`

Five scenarios pivot on time elapsing: the three exact-hour fee tests, the partial-hour ceiling test, and the zero-duration minimum-charge test. The end-to-end worked example advances the clock three times across a multi-vehicle sequence. If those tests called `DateTime.UtcNow` directly, they'd either sleep or thread a stopwatch through every assertion.

`IClock.Now()` makes the collaboration explicit. `FixedClock` in the test project is the deterministic fake. This is the same pattern used in [`rate-limiter`](../../rate-limiter/csharp/WALKTHROUGH.md), [`memory-cache`](../../memory-cache/csharp/WALKTHROUGH.md#why-iclock-not-datetimeutcnow), and [`circuit-breaker`](../../circuit-breaker/csharp/WALKTHROUGH.md).

Inside `ProcessExit`, the clock is read **exactly once** per call. The same `now` is used for both duration calculation and any future auditing needs.

See `src/ParkingLot/IClock.cs` and `tests/ParkingLot.Tests/FixedClock.cs`.

## Why `LotBuilder` Returns a Tuple

Most scenarios need to **advance time after the lot exists** — park a vehicle, advance the clock, exit and check the fee. If the builder only returned the lot, the test would have no handle on the clock. Returning `(Lot, FixedClock)` gives the test exactly the two collaborators it drives.

The builder defaults all spot counts to zero so each test explicitly declares only the spots it cares about. Rate defaults use the domain constants.

See `tests/ParkingLot.Tests/LotBuilder.cs`.

## Why Four Domain Exception Types

Each rejection scenario names a different invariant:
- `InvalidLotConfigurationException` — lot with zero spots
- `VehicleAlreadyParkedException` — duplicate entry
- `NoAvailableSpotException` — no compatible spot
- `InvalidTicketException` — unknown or already-used ticket

Named exceptions put the domain rule in the type system. Tests `Should().Throw<VehicleAlreadyParkedException>()`, and a reader sees the invariant in the stack trace. The messages are **byte-identical** across the three languages; only the exception class name differs.

See `src/ParkingLot/Exceptions.cs`.

## Why Fee Calculation Uses Ceiling, Not Rounding

The spec says partial hours round up. `Math.Ceiling(elapsed.TotalHours)` handles this directly. The minimum charge of 1 hour for zero-duration stays is a separate guard (`if (hours < 1) hours = 1`) because `Math.Ceiling(0.0)` returns `0`, not `1`.

See `src/ParkingLot/Lot.cs`.

## Why Seven Test Files

The twenty-two scenarios split naturally into seven concerns:

- `LotConstructionTests.cs` — lot creation and validation (scenarios 1–2)
- `SpotAllocationTests.cs` — smallest-fit-first allocation (scenarios 3–8)
- `EntryTests.cs` — ticket issuance and duplicate detection (scenarios 9–10)
- `NoAvailableSpotTests.cs` — rejection when no spot fits (scenarios 11–13)
- `ExitTests.cs` — spot freeing and ticket validation (scenarios 14–16)
- `FeeCalculationTests.cs` — time-based fee math (scenarios 17–21)
- `WorkedExampleTests.cs` — the end-to-end scenario (scenario 22)

One `[Fact]` per scenario, test names matching the scenario titles verbatim.

## What's Deliberately Not Modeled

The kata brief's broader design could include floors/zones, reservation systems, real-time availability displays, and dynamic pricing. This reference scopes to the twenty-two core scenarios — spot allocation, entry/exit, and fee calculation. A reader extending the kata has the `SpotType` enum and `Lot` aggregate ready to grow (floors = partitioned spot pools; dynamic pricing = rate lookup by time-of-day) without the reference hiding seams under premature abstraction.

## How to Run

```bash
cd parking-lot/csharp
dotnet test
```
