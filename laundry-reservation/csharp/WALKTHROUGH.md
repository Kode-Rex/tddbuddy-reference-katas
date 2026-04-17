# Laundry Reservation — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice.

Rather than stepping through twenty-one red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
ReservationService
  ├── CreateReservation(slot, customer) → Reservation
  └── ClaimReservation(machineNumber, pin) → bool

MachineApi
  ├── Lock(reservationId, machineNumber, slot, pin) → bool
  └── Unlock(machineNumber, reservationId)

Collaborators:
  IReservationRepository, IEmailNotifier, ISmsNotifier,
  IPinGenerator, IMachineSelector, IMachineDevice
```

## Why This Is a Test-Double Kata

The domain logic in `ReservationService` is thin — it orchestrates six collaborators. The *behavior* is the sequence of calls: save, email, lock; or count failures, SMS, re-lock. Without test doubles, there is nothing to assert on. Every test in this kata asserts on what was *sent to* a collaborator, not on a return value.

This makes it an ideal kata for practicing the **Mocks as Behavioral Specifications** principle: when the collaboration *is* the behavior, each collaborator gets an interface and a recording fake.

## Why Recording Fakes, Not a Mocking Library

`RecordingEmailNotifier`, `RecordingSmsNotifier`, `RecordingMachineDevice` — each is a hand-written fake that records what it received. Tests assert on the `.Sent` or `.LockCalls` lists.

A mocking library (Moq, NSubstitute) could do the same with fewer lines. But hand-written fakes are:

1. **Readable in isolation.** You can open `RecordingEmailNotifier.cs` and understand it without knowing the mocking library's API.
2. **Debuggable.** Set a breakpoint in `Send()` and watch the test arrange → act → assert flow.
3. **Shared across languages.** The same pattern translates to TypeScript and Python without importing a mocking framework.

For a kata that teaches test doubles, making the doubles explicit is the point.

## Why `ReservationServiceBuilder`

Most tests need a `ReservationService` wired to specific fakes. Without the builder, each test would construct six fakes and pass them to the constructor — a setup wall that obscures the scenario.

`ReservationServiceBuilder` provides sensible defaults (machine 7, PIN 12345) and exposes the recording fakes as properties so tests can assert on them. The `.Build()` method returns a tuple of `(Service, MachineApi)`, keeping the builder pure.

## Why `DuplicateReservationException`

The kata spec says "a user may only have a single active reservation at a time." A bool return would work, but naming the exception — `DuplicateReservationException` with the message `"Customer 'alice@example.com' already has an active reservation."` — makes the rejection visible in stack traces and test names.

The message is byte-identical across C#, TypeScript, and Python. The exception type is language-idiomatic.

## Why Failure Counting Lives in `ReservationService`

The PIN failure counter could live on `Reservation`, on `MachineApi`, or on the service. It lives on the service because:

1. The counter is a **process concern**, not a domain invariant. A reservation doesn't "know" about failed PIN attempts — the service orchestrating the IoT flow does.
2. The reset-on-success and reset-on-new-PIN logic both involve calling collaborators (SMS, re-lock). Putting the counter where the collaborator calls happen avoids reaching back into the reservation to trigger side effects.

## Scenario Map

The twenty-one scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live across three test classes:

- `CreateReservationTests` — scenarios 1–7
- `MachineApiTests` — scenarios 8–12
- `ClaimReservationTests` — scenarios 13–21

One `[Fact]` per scenario, test names matching the scenario titles verbatim.

## How to Run

```bash
cd laundry-reservation/csharp
dotnet test
```
