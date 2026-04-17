# Parking Lot — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **ParkingLot** | The aggregate. Owns a collection of `Spot`s; handles vehicle entry, exit, and fee calculation |
| **Vehicle** | Value type: `VehicleType` (motorcycle / car / bus) + `LicensePlate` (non-empty string) |
| **VehicleType** | Enumeration: `Motorcycle`, `Car`, `Bus` |
| **Spot** | A single parking space. Has a `SpotType` (motorcycle / compact / large) and an occupancy state |
| **SpotType** | Enumeration: `Motorcycle`, `Compact`, `Large` |
| **Ticket** | Issued on entry. Records the vehicle, assigned spot, and entry time. Returned on exit for fee calculation |
| **Fee** | Calculated on exit: `hours * hourly rate`. Partial hours round up (ceiling). Rate is per vehicle type |
| **Clock** | Collaborator returning "now" — injected so time-based fee tests control time without sleeping |
| **LotBuilder** | Test builder: configures spot counts by type, hourly rates, and clock |
| **VehicleBuilder** | Test builder: configures vehicle type and license plate with sensible defaults |

## Domain Rules

### Spot Allocation

- A **motorcycle** fits in any spot type: motorcycle, compact, or large.
- A **car** fits in a compact or large spot, but **not** a motorcycle spot.
- A **bus** requires a large spot only.
- When multiple spot types can serve a vehicle, the lot assigns the **smallest fitting spot type first** (motorcycle < compact < large). This prevents large spots from being consumed by smaller vehicles when tighter fits are available.
- A vehicle can only park once — attempting to park a vehicle that is already in the lot raises `VehicleAlreadyParkedException` with message `"Vehicle {plate} is already parked"`.
- When no compatible spot is available, the lot raises `NoAvailableSpotException` with message `"No available spot for vehicle {plate}"`.

### Entry and Exit

- `ParkEntry(vehicle)` assigns a spot, records entry time from the clock, and returns a `Ticket`.
- `ProcessExit(ticket)` frees the spot, calculates the fee based on elapsed time, and returns the `Fee`.
- Attempting to exit with a ticket that does not belong to this lot (or has already been processed) raises `InvalidTicketException` with message `"Ticket is not valid"`.

### Fee Calculation

- **Hourly rates by vehicle type:**
  - Motorcycle: $1/hour
  - Car: $3/hour
  - Bus: $5/hour
- Duration is calculated as `exitTime - entryTime`. Partial hours round **up** (e.g. 2 hours 1 minute = 3 hours billed).
- A stay of zero duration (entry and exit at the same clock time) is billed as **1 hour** (minimum charge).

### Lot Construction

- A lot must have at least one spot. Constructing a lot with zero total spots raises `InvalidLotConfigurationException` with message `"Lot must have at least one spot"`.

## Named Constants

- `DefaultMotorcycleRate = 1` (dollars per hour)
- `DefaultCarRate = 3` (dollars per hour)
- `DefaultBusRate = 5` (dollars per hour)

## Test Scenarios

### Lot Construction

1. **A lot with spots across all types is valid**
2. **A lot with zero total spots raises InvalidLotConfigurationException**

### Spot Allocation by Vehicle Type

3. **A motorcycle parks in a motorcycle spot when available**
4. **A car parks in a compact spot when available**
5. **A bus parks in a large spot when available**
6. **A motorcycle uses a compact spot when no motorcycle spots remain**
7. **A motorcycle uses a large spot when no motorcycle or compact spots remain**
8. **A car uses a large spot when no compact spots remain**

### Entry Produces a Ticket

9. **Parking a vehicle returns a ticket with the vehicle and assigned spot type**
10. **Parking the same vehicle twice raises VehicleAlreadyParkedException**

### No Available Spot

11. **Parking a car when only motorcycle spots remain raises NoAvailableSpotException**
12. **Parking a bus when only compact and motorcycle spots remain raises NoAvailableSpotException**
13. **Parking any vehicle when the lot is completely full raises NoAvailableSpotException**

### Exit Frees the Spot

14. **Exiting frees the spot so another vehicle of the same type can park**
15. **Exiting with an invalid ticket raises InvalidTicketException**
16. **Exiting with the same ticket twice raises InvalidTicketException**

### Fee Calculation

17. **A motorcycle parked for exactly one hour pays $1**
18. **A car parked for exactly two hours pays $6**
19. **A bus parked for exactly one hour pays $5**
20. **Partial hours round up: a car parked for 2 hours 1 minute pays $9**
21. **A stay of zero duration (entry and exit at the same time) is billed as 1 hour**

### End-to-End Worked Example

22. **A lot with 1 motorcycle spot, 1 compact spot, and 1 large spot: park motorcycle (gets motorcycle spot), park car (gets compact spot), park bus (gets large spot), lot is full; exit motorcycle at t+90min ($2); park a second car (gets freed motorcycle? no — motorcycle spot does not fit car; raises NoAvailableSpotException); park a second motorcycle (gets motorcycle spot); exit car at t+30min ($3); exit bus at t+2h ($10)**
