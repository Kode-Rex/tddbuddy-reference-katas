# Clam Card

Contactless transit card. Passengers tap in and out at stations; the card charges the owner's bank account per journey. Stations belong to one of two fare zones — Zone A (cheaper) or Zone B (pricier). A journey that touches Zone B at either end is charged at the Zone B rate. Cumulative charges cap per day per zone: once a card has paid `$7.00` for Zone A journeys on a given day, further Zone A journeys that day are free; Zone B caps at `$8.00`.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. One primary aggregate (`Card`), one value-type (`Ride`), and two small test-folder builders (`CardBuilder`, `RideBuilder`) whose chained setters stage card state (a fixed clock, zone assignments) and ride records so tests read as the literal domain under test:

```csharp
var card = new CardBuilder()
    .OnDay(new DateOnly(2024, 1, 1))
    .WithZone(Zone.A, "Asterisk", "Aldgate", "Angel", "Antelope")
    .Build();

card.TravelFrom("Asterisk").To("Aldgate"); // charges $2.50
```

vs. a dictionary of stations in the SUT constructor that every test has to re-assemble. The builders skip validation of station names against the real network because they are test-folder synthesisers of card and ride state, not a network catalogue.

## Scope — Pure Domain Only

The reference covers: the card, zone-tagged stations, per-journey fare calculation (zone A, zone B, mixed-zone), and the **daily** cap per zone. **No bank-account collaborator, no weekly cap, no monthly cap, no return-journey discount, no concession fares, no transfer windows, no UI, no persistence.** These are stretch goals below.

### Stretch Goals (Not Implemented Here)

- **Weekly and monthly caps** — same shape as the daily cap, different time windows. Needs a `Clock` collaborator rich enough to reason about week/month boundaries.
- **Return-journey discount** — the bonus spec. Requires remembering the previous journey and flipping the second leg's fare if it is the inverse.
- **Bank-account debiting** — a `BankAccount` collaborator the card notifies per charge. Pushes the kata into F3.
- **Station catalogue** — production code would reject unknown stations rather than silently mis-zoning them. Out of scope here; builder assigns zones directly.
- **Cross-card transfers, concession fares, peak/off-peak** — out of scope.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference satisfies.
