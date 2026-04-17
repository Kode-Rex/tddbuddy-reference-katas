# Laundry Reservation — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **ReservationService** | The aggregate root that orchestrates reservation creation and claiming |
| **Reservation** | A booking of a specific machine at a specific time for a customer |
| **Customer** | A patron identified by email and cell phone number |
| **Machine** | One of 25 washing machines, identified by number (1–25) |
| **Pin** | A 5-digit numeric code the patron enters at the IoT device to unlock a machine |
| **MachineApi** | Collaborator that wraps the device SDK; sends lock/unlock instructions to machines |
| **MachineDevice** | The SDK interface for a single IoT device; `Lock` and `Unlock` |
| **EmailNotifier** | Collaborator that sends reservation confirmation emails |
| **SmsNotifier** | Collaborator that sends SMS messages (new PIN after failed attempts) |
| **ReservationRepository** | Collaborator that persists and retrieves reservations |
| **PinGenerator** | Collaborator that produces 5-digit PINs; injected so tests control the value |
| **MachineSelector** | Collaborator that picks an available machine number; injected so tests control the choice |
| **Clock** | Collaborator that returns the current date/time — injected for determinism |

## Domain Rules

- A new reservation requires a **date/time**, **cell phone number**, and **email address**
- A reservation is assigned a **unique reservation ID** (GUID), a **machine number** (1–25), and a **5-digit PIN**
- On creation, a **confirmation email** is sent with the machine number, reservation ID, and PIN
- On creation, the selected machine is **locked** via the Machine API with the reservation ID, date/time, and PIN
- On creation, the reservation is **saved** to the repository
- A customer may only have **one active reservation** at a time; creating a second is rejected
- **Claiming** a reservation requires the machine number and PIN
- A successful claim **marks the reservation as used** in the repository and **unlocks** the machine
- **Failed PIN attempts** are tracked per machine; after **5 consecutive failures**, an SMS is sent with a new PIN
- After 5 failures, the reservation is **updated** with the new PIN, and the Machine API is called to **re-lock** with the new PIN
- The failure counter **resets** after a successful claim or after a new PIN is issued

## Test Scenarios

### Create Reservation

1. **Creating a reservation assigns a unique reservation ID**
2. **Creating a reservation assigns a machine number from the selector**
3. **Creating a reservation assigns a five-digit PIN from the generator**
4. **Creating a reservation sends a confirmation email with machine number, reservation ID, and PIN**
5. **Creating a reservation saves the reservation to the repository**
6. **Creating a reservation locks the machine via the Machine API**
7. **Creating a second reservation for the same customer is rejected**

### Machine API

8. **Locking a machine delegates to the device with reservation ID, date/time, and PIN**
9. **Locking a machine returns true when the device accepts the lock**
10. **Locking a machine returns false when the device rejects the lock**
11. **Locking a machine with an existing reservation ID updates the PIN and date/time**
12. **Unlocking a machine delegates to the device with the reservation ID**

### Claim Reservation

13. **Claiming with the correct PIN marks the reservation as used**
14. **Claiming with the correct PIN unlocks the machine**
15. **Claiming with an incorrect PIN does not unlock the machine**
16. **Claiming with an incorrect PIN does not mark the reservation as used**
17. **Five consecutive incorrect PINs sends an SMS with a new PIN**
18. **Five consecutive incorrect PINs updates the reservation with the new PIN**
19. **Five consecutive incorrect PINs re-locks the machine with the new PIN**
20. **A successful claim resets the failure counter**
21. **The failure counter resets after a new PIN is issued allowing five more attempts**
