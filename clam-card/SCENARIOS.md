# Clam Card — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **the transit-card domain only**: zone-tagged stations, per-journey fare calculation, and the daily per-zone cap. Weekly and monthly caps, the return-journey discount (bonus), a bank-account collaborator, station-catalogue validation, and persistence concerns are **out of scope** — see the top-level [`README.md`](README.md#scope--pure-domain-only) for the full stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Card** | A Clam Card. Tracks cumulative per-zone charges per day; calculates and records the fare for each completed journey. Exposes `travelFrom(station).to(station)` returning a `Ride`, `totalCharged()`, and `rides()`. |
| **Ride** | A completed journey. Carries the `from` station, `to` station, `zone` (the zone the fare was charged at), and `fare` (the amount actually charged for this ride — potentially `$0` if a daily cap was already reached). |
| **Station** | A named stop belonging to exactly one `Zone`. Stations unknown to the card raise `UnknownStationException` / `UnknownStationError`. |
| **Zone** | `A` or `B`. Zone A is cheaper; Zone B is pricier. A journey touching Zone B at either end is billed at the Zone B rate. |
| **Journey Zone** | The zone the fare is charged at. `JourneyZone(from, to) = B` if either endpoint is Zone B, else `A`. |
| **Single Fare** | The per-journey price before caps: Zone A = `$2.50`, Zone B = `$3.00`. |
| **Daily Cap** | Per zone per day: Zone A = `$7.00`, Zone B = `$8.00`. Once the cap is reached for a zone that day, further journeys *at that journey-zone* on that day charge `$0`. A journey's fare is `min(singleFare, capForZone - alreadyChargedForZoneToday)`. |
| **CardBuilder** | Test-folder fluent builder. Sets the card's current day (`onDay(date)`) and assigns stations to zones (`withZone(zone, stations...)`). |
| **RideBuilder** | Test-folder fluent builder. Constructs a `Ride` literal for ride-equality scenarios. |

## Domain Rules

- A journey's **journey-zone** is `B` if the `from` or `to` station is in Zone B; otherwise `A`.
- The **single fare** for a journey is `$2.50` if journey-zone is A, `$3.00` if journey-zone is B.
- The card tracks **per-zone cumulative charges on the current day**. When the cumulative total for a zone reaches that zone's cap, subsequent rides at that journey-zone on that day are free. A ride whose single fare would push the cumulative total past the cap is charged only the remainder to the cap (may be less than the single fare).
- The **daily cap is per journey-zone**, not per physical station zone. A card that has spent `$7.00` on Zone A journeys still pays full fare on its next Zone B journey; the two caps are independent.
- Named constants (both for readability and for the 6–8 spec-sourced dollar amounts):
  - `ZoneASingleFare = 2.50`
  - `ZoneBSingleFare = 3.00`
  - `ZoneADailyCap = 7.00`
  - `ZoneBDailyCap = 8.00`
- Touching a station the card does not know raises `UnknownStationException` (C#) / `UnknownStationError` (TS / Python).
- Fare math is exact to the cent. C# uses `decimal`; TS uses integer cents internally; Python uses `Decimal`. Tests compare fares in dollars (e.g. `2.50m`, `2.5`, `Decimal("2.50")`).

### Exception Messages

The exception message strings are **identical byte-for-byte** across all three languages; the exception type names differ by language idiom.

| Rule | Message |
|------|---------|
| station not known to this card | `"station is not on this card's network"` |

## Test Scenarios

### Single-journey fares

1. **One-way Zone A journey is charged the Zone A single fare** — Michael travels from Asterisk to Aldgate (both Zone A). He is charged `$2.50`.
2. **One-way Zone A-to-B journey is charged the Zone B single fare** — Michael travels from Asterisk (A) to Barbican (B). He is charged `$3.00`.
3. **One-way Zone B-to-A journey is charged the Zone B single fare** — Bison (B) to Asterisk (A) is a Zone B journey; charged `$3.00`.
4. **One-way Zone B journey is charged the Zone B single fare** — Bison (B) to Barbican (B) is charged `$3.00`.

### Multiple journeys, no cap reached

5. **Two single journeys accumulate on `totalCharged`** — Asterisk→Aldgate (`$2.50`) then Asterisk→Balham (`$3.00`) totals `$5.50`. The card's `rides()` lists the two journeys in order, each with its own fare.

### Daily caps

6. **Zone A daily cap is `$7.00`** — four consecutive Zone A journeys (all A-to-A) are charged `$2.50`, `$2.50`, `$2.00`, `$0.00`. The third journey is trimmed from `$2.50` to `$2.00` to meet the cap; the fourth is free.
7. **Zone B daily cap is `$8.00`** — four consecutive Zone B journeys are charged `$3.00`, `$3.00`, `$2.00`, `$0.00`. The third is trimmed to the cap remainder; the fourth is free.
8. **Reaching the Zone A cap does not affect Zone B fares** — after paying `$7.00` in Zone A journeys, a Zone B journey still costs `$3.00`.
9. **Reaching the Zone B cap does not affect Zone A fares** — after paying `$8.00` in Zone B journeys, a Zone A journey still costs `$2.50`.

### Validation

10. **Travelling from an unknown station raises** — `travelFrom("Moonbase")` raises with message `"station is not on this card's network"`.
11. **Travelling to an unknown station raises** — `travelFrom("Asterisk").to("Moonbase")` raises with the same message.

### Ride record

12. **Each ride records its zone and fare** — after Asterisk→Barbican and Asterisk→Aldgate, `rides()` contains two `Ride` records with zones `B` and `A` and fares `$3.00` and `$2.50` respectively.
