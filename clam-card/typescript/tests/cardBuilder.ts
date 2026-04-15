import { Card, Ride, Zone } from '../src/card.js';

// Test-folder synthesisers. Stage the card's network (zones → stations)
// and construct Ride literals for equality assertions.
export class CardBuilder {
  private readonly stations = new Map<string, Zone>();

  onDay(_date: Date): this {
    return this;
  }

  withZone(zone: Zone, ...stations: string[]): this {
    for (const s of stations) this.stations.set(s, zone);
    return this;
  }

  build(): Card {
    return new Card({ stations: this.stations });
  }
}

export class RideBuilder {
  private fromStation = '';
  private toStation = '';
  private zone: Zone = 'A';
  private fare = 0;

  from(station: string): this { this.fromStation = station; return this; }
  to(station: string): this { this.toStation = station; return this; }
  chargedAt(zone: Zone): this { this.zone = zone; return this; }
  withFare(fare: number): this { this.fare = fare; return this; }

  build(): Ride {
    return { from: this.fromStation, to: this.toStation, zone: this.zone, fare: this.fare };
  }
}
