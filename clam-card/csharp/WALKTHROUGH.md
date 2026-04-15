# Clam Card — C# Walkthrough

This kata ships in **middle gear** — the C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one primary aggregate (`Card`), one value type (`Ride`), a `Zone` enum, and two small test-folder builders (`CardBuilder`, `RideBuilder`).

## Scope — Pure Domain Only

Daily per-zone cap only. No weekly/monthly caps, no return-journey discount, no bank-account collaborator, no station catalogue, no persistence. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Design at a Glance

```
Fares / CardMessages          — named constants + spec strings
Zone enum                     — A | B
Ride (readonly record struct) — (from, to, zone, fare)

Card
  ├── TravelFrom(station)     : JourneyStart   (throws on unknown)
  │     └── To(station)       : Ride           (throws on unknown)
  ├── Rides()                 : IReadOnlyList<Ride>
  └── TotalCharged()          : decimal

CardBuilder / RideBuilder (tests/)
```

## Why `TravelFrom(...).To(...)` Instead of `Travel(from, to)`

The ubiquitous language in the spec is *"Michael travels from Asterisk to Aldgate"* — a sentence, not a tuple. The two-step fluent call reads the same way at the test site:

```csharp
card.TravelFrom("Asterisk").To("Aldgate");
```

The single-method `Travel("Asterisk", "Aldgate")` is fewer characters but flattens the domain verb. The `JourneyStart` intermediate is a two-line nested class — cheap structure for a readable sentence.

## Why `Zone` Is an Enum, Not a Value Object

There are exactly two zones. `Zone A` and `Zone B` carry no additional behaviour beyond their identity; the fare tables live on `Card`, not on `Zone`. An enum is the shortest honest expression of a closed, behaviourless category. If the spec grew to include Zone C or peak/off-peak variants, a record or sealed-class hierarchy would earn its keep — but under F2 scope, it is a two-case enum.

## Why `Card` Holds a `Dictionary<string, Zone>`

Station → zone mapping is injected at construction via the builder. Two things fell out of that choice:

1. **The card carries its own network.** The spec doesn't describe a separate "transit network" service; a card knows which stations it recognises and which zone each is in. In production this would almost certainly be a collaborator — an F3 shape — but at F2 the simplest honest model is an owned dictionary.
2. **Unknown-station validation is a card responsibility.** `UnknownStationException` is thrown when a test asks the card to travel to or from a station it was never told about. Both directions check membership; the "station is not on this card's network" message is byte-identical across languages.

## Why the Cap Math Lives on `Card` (Not on `Ride`)

A `Ride` is a record of a completed journey, not a fare calculator. The cap depends on *what has already been charged today* — state that belongs to the card. The flow reads:

```
journeyZone = (fromZone == B || toZone == B) ? B : A
fare = max(0, min(singleFare, capForZone - chargedForZoneToday))
chargedForZoneToday += fare
ride = new Ride(from, to, journeyZone, fare)
```

The `max(0, …)` guard means a ride after the cap has been reached charges `$0` rather than a negative number. The `min(singleFare, …)` trims the third ride in the Zone A cap scenario from `$2.50` to `$2.00` — the remainder to the cap.

## Why Caps Are Tracked Per-Zone Independently

Re-reading the kata spec: *"No matter how many journeys are made within one of the time boundaries within a particular zone, the price will cap at that time period's fare."* The caps are per-zone, not per-card. Scenarios 8 and 9 pin this down: a card that has paid its Zone A daily cap still pays full fare on the next Zone B journey, and vice versa.

The card carries `_chargedZoneAToday` and `_chargedZoneBToday` separately. `TotalCharged()` is the sum.

## Why the Builder Has `OnDay(...)` That Does Nothing

Scenarios read as *"Michael has a Clam Card on 2024-01-01 and travels from Asterisk to Aldgate"*. `OnDay(date)` is accepted by the builder for scenario readability, but at F2 scope the daily cap resets on card construction — the date itself has no behaviour. Promoting this to a real `IClock` collaborator is the weekly/monthly cap stretch goal: the cap window's end is what the clock actually tells you.

Keeping the call in the API surface now means tests that later gain `OnDay` behaviour don't need to re-plumb the builder — the parameter is already where it needs to be.

## Why `Ride` Is a `readonly record struct`

Each ride is four small fields (two strings, an enum, a decimal) and is never mutated after creation. `record struct` gives structural equality for the ride-equality scenario without heap allocation per ride. The test asserts `Rides().Should().ContainInOrder(expectedB, expectedA)` — structural equality makes this natural.

## Named Constants for Spec Dollar Amounts

`Fares.ZoneASingleFare = 2.50m` et al. This is the F2 "named constants for business numbers" rule. Pulling the four fare numbers into a `Fares` static class also makes them trivially greppable against the spec table when weekly/monthly caps get added.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/ClamCard.Tests/CardTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd clam-card/csharp
dotnet test
```
