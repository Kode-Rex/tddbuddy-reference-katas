# Weather Station

Weather data collection and statistical analysis kata: recording readings, computing min/max/average statistics, and firing threshold-based alerts. Excellent for practicing **test data builders** over an aggregate that accumulates time-series observations with meaningful invariants.

## What this kata teaches

- **Test Data Builders** — `ReadingBuilder` and `StationBuilder` make scenario setup one line each.
- **Domain Types** — `Temperature`, `Humidity`, `WindSpeed`, and `Reading` carry meaning; raw numbers don't.
- **Invariant Testing** — every validation rule (humidity range, non-negative wind) has a test that asserts the *rejection*, not just the success path.
- **Aggregate Statistics** — `Station` is the aggregate root that owns its reading history and computes statistics as a pure query.
- **Collaborator Injection** — `Clock` is injected so tests control time deterministically.
- **Alert Thresholds** — configurable limits with domain-specific exception messages, byte-identical across languages.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
