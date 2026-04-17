# Laundry Reservation — TypeScript Walkthrough

Ships in **middle gear** — full implementation in one commit. The [C# walkthrough](../csharp/WALKTHROUGH.md) is the primary design-rationale document. This note captures the TypeScript-specific adaptations.

## Same Design, TypeScript Idioms

```
ReservationService
  ├── createReservation(slot, customer) → Reservation
  └── claimReservation(machineNumber, pin) → boolean

MachineApi
  ├── lock(reservationId, machineNumber, slot, pin) → boolean
  └── unlock(machineNumber, reservationId)

Collaborators (interfaces):
  ReservationRepository, EmailNotifier, SmsNotifier,
  PinGenerator, MachineSelector, MachineDevice
```

All six collaborators are `interface` types rather than C# interfaces. The recording fakes implement them directly — no class hierarchy needed.

## UUID Generation

`crypto.randomUUID()` from Node 20's built-in `node:crypto` module generates reservation IDs. No external dependency needed. This is the TypeScript equivalent of C#'s `Guid.NewGuid()`.

## No `internal` — Interface Discipline

TypeScript has no `internal` visibility modifier. All collaborator interfaces are exported alongside the domain types. The discipline is the same as the C# version: tests construct the service with recording fakes, and assertions target what those fakes recorded.

## Module Colocation

Small related types (`Customer`, `Reservation`, each interface) each get their own module file. This matches the C# one-type-per-file convention. TS idiom permits collapsing small types into one module, but keeping them separate makes the import list self-documenting.

## Scenario Map

- `tests/createReservation.test.ts` — scenarios 1–7
- `tests/machineApi.test.ts` — scenarios 8–12
- `tests/claimReservation.test.ts` — scenarios 13–21

One `it` per scenario.

## How to Run

```bash
cd laundry-reservation/typescript
npm install
npm test
```
