// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
export const CardMessages = {
  unknownStation: "station is not on this card's network",
} as const;

// Named constants for spec dollar amounts.
export const ZONE_A_SINGLE_FARE = 2.5;
export const ZONE_B_SINGLE_FARE = 3.0;
export const ZONE_A_DAILY_CAP = 7.0;
export const ZONE_B_DAILY_CAP = 8.0;

export type Zone = 'A' | 'B';

export class UnknownStationError extends Error {
  constructor() {
    super(CardMessages.unknownStation);
    this.name = 'UnknownStationError';
  }
}

export interface Ride {
  readonly from: string;
  readonly to: string;
  readonly zone: Zone;
  readonly fare: number;
}

export interface CardState {
  readonly stations: ReadonlyMap<string, Zone>;
}

export class Card {
  private readonly stations: ReadonlyMap<string, Zone>;
  private readonly ridesList: Ride[] = [];
  private chargedZoneAToday = 0;
  private chargedZoneBToday = 0;

  /** @internal — test-folder builder hook; production code would inject a real network. */
  constructor(state: CardState) {
    this.stations = state.stations;
  }

  rides(): readonly Ride[] {
    return this.ridesList;
  }

  totalCharged(): number {
    return this.chargedZoneAToday + this.chargedZoneBToday;
  }

  travelFrom(station: string): JourneyStart {
    this.ensureKnown(station);
    return new JourneyStart(this, station);
  }

  /** @internal */
  completeJourney(from: string, to: string): Ride {
    this.ensureKnown(to);
    const fromZone = this.stations.get(from)!;
    const toZone = this.stations.get(to)!;
    const journeyZone: Zone = fromZone === 'B' || toZone === 'B' ? 'B' : 'A';

    const fare = this.chargeForZone(journeyZone);
    const ride: Ride = { from, to, zone: journeyZone, fare };
    this.ridesList.push(ride);
    return ride;
  }

  private chargeForZone(zone: Zone): number {
    const singleFare = zone === 'A' ? ZONE_A_SINGLE_FARE : ZONE_B_SINGLE_FARE;
    const cap = zone === 'A' ? ZONE_A_DAILY_CAP : ZONE_B_DAILY_CAP;
    const charged = zone === 'A' ? this.chargedZoneAToday : this.chargedZoneBToday;

    const remaining = cap - charged;
    const fare = Math.max(0, Math.min(singleFare, remaining));

    if (zone === 'A') this.chargedZoneAToday += fare;
    else this.chargedZoneBToday += fare;

    return fare;
  }

  private ensureKnown(station: string): void {
    if (!this.stations.has(station)) {
      throw new UnknownStationError();
    }
  }
}

export class JourneyStart {
  constructor(private readonly card: Card, private readonly from: string) {}

  to(station: string): Ride {
    return this.card.completeJourney(this.from, station);
  }
}
