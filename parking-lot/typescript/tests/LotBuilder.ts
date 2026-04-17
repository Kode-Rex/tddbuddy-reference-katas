import { Lot, DEFAULT_MOTORCYCLE_RATE, DEFAULT_CAR_RATE, DEFAULT_BUS_RATE } from '../src/Lot.js';
import { FixedClock } from './FixedClock.js';

export class LotBuilder {
  private motorcycleSpots = 0;
  private compactSpots = 0;
  private largeSpots = 0;
  private motorcycleRate = DEFAULT_MOTORCYCLE_RATE;
  private carRate = DEFAULT_CAR_RATE;
  private busRate = DEFAULT_BUS_RATE;
  private start = new Date(Date.UTC(2026, 0, 1));
  private clock: FixedClock | null = null;

  withMotorcycleSpots(count: number): this { this.motorcycleSpots = count; return this; }
  withCompactSpots(count: number): this { this.compactSpots = count; return this; }
  withLargeSpots(count: number): this { this.largeSpots = count; return this; }
  withMotorcycleRate(rate: number): this { this.motorcycleRate = rate; return this; }
  withCarRate(rate: number): this { this.carRate = rate; return this; }
  withBusRate(rate: number): this { this.busRate = rate; return this; }
  startingAt(start: Date): this { this.start = start; return this; }
  withClock(clock: FixedClock): this { this.clock = clock; return this; }

  build(): { lot: Lot; clock: FixedClock } {
    const clock = this.clock ?? new FixedClock(this.start);
    const lot = new Lot(
      this.motorcycleSpots, this.compactSpots, this.largeSpots, clock,
      this.motorcycleRate, this.carRate, this.busRate,
    );
    return { lot, clock };
  }
}
