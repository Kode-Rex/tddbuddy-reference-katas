# Parking Lot

A multi-type parking lot with spot allocation by vehicle size and time-based fee calculation. Great for practicing **aggregate design**, **spot-allocation strategy (smallest-fit-first)**, **injected clocks for time-based logic**, and **domain exception types** with test data builders.

## What this kata teaches

- **Test Data Builders** — `LotBuilder().WithMotorcycleSpots(n).WithCompactSpots(n).WithLargeSpots(n).WithClock(clock).Build()` returns a `(ParkingLot, Clock)` tuple so tests drive entry/exit timestamps deterministically. `VehicleBuilder().AsMotorcycle().WithPlate("M-001").Build()` produces vehicles with sensible defaults.
- **Injected Clock** — `Clock` is a collaborator interface; `FixedClock` in tests lets fee-calculation tests advance simulated wall-time without sleeping.
- **Spot Allocation Strategy** — smallest-fit-first: motorcycles prefer motorcycle spots over compact over large; cars prefer compact over large. This preserves large spots for vehicles that need them.
- **Value Types** — `Vehicle` (type + plate), `Ticket` (vehicle + spot + entry time), `Fee` (amount in dollars) are immutable values.
- **Domain Exception Types** — `VehicleAlreadyParkedException`, `NoAvailableSpotException`, `InvalidTicketException`, `InvalidLotConfigurationException` name the rejection in the type system rather than throwing generic exceptions. Messages are byte-identical across all three languages.
- **Fee Calculation** — per-hour rates by vehicle type, partial-hour ceiling, minimum one-hour charge. The clock collaborator makes these scenarios deterministic.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
