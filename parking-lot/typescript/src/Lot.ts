import type { Clock } from './Clock.js';
import type { Fee } from './Fee.js';
import type { SpotType } from './SpotType.js';
import type { Ticket } from './Ticket.js';
import type { Vehicle } from './Vehicle.js';
import type { VehicleType } from './VehicleType.js';
import {
  InvalidLotConfigurationError,
  InvalidTicketError,
  NoAvailableSpotError,
  VehicleAlreadyParkedError,
} from './errors.js';

export const DEFAULT_MOTORCYCLE_RATE = 1;
export const DEFAULT_CAR_RATE = 3;
export const DEFAULT_BUS_RATE = 5;

const MOTORCYCLE_PREFERENCE: readonly SpotType[] = ['motorcycle', 'compact', 'large'];
const CAR_PREFERENCE: readonly SpotType[] = ['compact', 'large'];
const BUS_PREFERENCE: readonly SpotType[] = ['large'];

const PREFERENCE_BY_VEHICLE: Record<VehicleType, readonly SpotType[]> = {
  motorcycle: MOTORCYCLE_PREFERENCE,
  car: CAR_PREFERENCE,
  bus: BUS_PREFERENCE,
};

const MS_PER_HOUR = 3_600_000;

export class Lot {
  private readonly clock: Clock;
  private readonly availableSpots: Map<SpotType, number>;
  private readonly activeTickets = new Map<string, Ticket>();
  private readonly rates: Map<VehicleType, number>;

  constructor(
    motorcycleSpots: number,
    compactSpots: number,
    largeSpots: number,
    clock: Clock,
    motorcycleRate = DEFAULT_MOTORCYCLE_RATE,
    carRate = DEFAULT_CAR_RATE,
    busRate = DEFAULT_BUS_RATE,
  ) {
    if (motorcycleSpots + compactSpots + largeSpots <= 0) {
      throw new InvalidLotConfigurationError('Lot must have at least one spot');
    }

    this.clock = clock;
    this.availableSpots = new Map<SpotType, number>([
      ['motorcycle', motorcycleSpots],
      ['compact', compactSpots],
      ['large', largeSpots],
    ]);
    this.rates = new Map<VehicleType, number>([
      ['motorcycle', motorcycleRate],
      ['car', carRate],
      ['bus', busRate],
    ]);
  }

  parkEntry(vehicle: Vehicle): Ticket {
    if (this.activeTickets.has(vehicle.licensePlate)) {
      throw new VehicleAlreadyParkedError(
        `Vehicle ${vehicle.licensePlate} is already parked`,
      );
    }

    const spotType = this.allocateSpot(vehicle);
    const now = this.clock.now();
    const ticket: Ticket = { vehicle, spotType, entryTime: now };
    this.activeTickets.set(vehicle.licensePlate, ticket);
    return ticket;
  }

  processExit(ticket: Ticket): Fee {
    const stored = this.activeTickets.get(ticket.vehicle.licensePlate);
    if (
      !stored ||
      stored.spotType !== ticket.spotType ||
      stored.entryTime.getTime() !== ticket.entryTime.getTime()
    ) {
      throw new InvalidTicketError('Ticket is not valid');
    }

    this.activeTickets.delete(ticket.vehicle.licensePlate);
    const current = this.availableSpots.get(ticket.spotType) ?? 0;
    this.availableSpots.set(ticket.spotType, current + 1);

    const now = this.clock.now();
    const elapsedMs = now.getTime() - ticket.entryTime.getTime();
    let hours = Math.ceil(elapsedMs / MS_PER_HOUR);
    if (hours < 1) hours = 1;

    const rate = this.rates.get(ticket.vehicle.type) ?? 0;
    return { amount: hours * rate };
  }

  private allocateSpot(vehicle: Vehicle): SpotType {
    const preference = PREFERENCE_BY_VEHICLE[vehicle.type];

    for (const spotType of preference) {
      const available = this.availableSpots.get(spotType) ?? 0;
      if (available > 0) {
        this.availableSpots.set(spotType, available - 1);
        return spotType;
      }
    }

    throw new NoAvailableSpotError(
      `No available spot for vehicle ${vehicle.licensePlate}`,
    );
  }
}
