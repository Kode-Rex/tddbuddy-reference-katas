# Laundry Reservation

IoT-powered laundry booking: reserve a machine, receive a PIN, and claim the machine at the facility. This kata is a **test-double kata** — the domain logic orchestrates multiple collaborators (email, SMS, device SDK, persistence), and every interaction is verified through fakes and recording doubles.

## What this kata teaches

- **Test Data Builders** — `ReservationBuilder(slot, service, customer)` constructs reservations with sensible defaults; `CustomerBuilder` provides named patrons.
- **Collaborator Interfaces** — `IEmailNotifier`, `ISmsNotifier`, `IMachineDevice`, `IReservationRepository`, `IPinGenerator`, `IMachineSelector` — each injected, each faked in tests.
- **Mocks as Behavioral Specifications** — tests assert on what was sent to the email notifier, what lock instruction reached the device, what was persisted. The collaborator calls *are* the behavior.
- **Domain Exceptions** — `DuplicateReservationException` names the invariant violation; identical message across languages.
- **Failure Counting** — five consecutive bad PINs triggers an SMS and PIN reset, exercising state tracking across multiple interactions.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
