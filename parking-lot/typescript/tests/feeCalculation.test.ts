import { LotBuilder } from './LotBuilder.js';
import { VehicleBuilder } from './VehicleBuilder.js';

describe('Fee Calculation', () => {
  it('a motorcycle parked for exactly one hour pays $1', () => {
    const { lot, clock } = new LotBuilder().withMotorcycleSpots(1).build();
    const motorcycle = new VehicleBuilder().asMotorcycle().build();

    const ticket = lot.parkEntry(motorcycle);
    clock.advanceHours(1);
    const fee = lot.processExit(ticket);

    expect(fee.amount).toBe(1);
  });

  it('a car parked for exactly two hours pays $6', () => {
    const { lot, clock } = new LotBuilder().withCompactSpots(1).build();
    const car = new VehicleBuilder().asCar().build();

    const ticket = lot.parkEntry(car);
    clock.advanceHours(2);
    const fee = lot.processExit(ticket);

    expect(fee.amount).toBe(6);
  });

  it('a bus parked for exactly one hour pays $5', () => {
    const { lot, clock } = new LotBuilder().withLargeSpots(1).build();
    const bus = new VehicleBuilder().asBus().build();

    const ticket = lot.parkEntry(bus);
    clock.advanceHours(1);
    const fee = lot.processExit(ticket);

    expect(fee.amount).toBe(5);
  });

  it('partial hours round up: a car parked for 2 hours 1 minute pays $9', () => {
    const { lot, clock } = new LotBuilder().withCompactSpots(1).build();
    const car = new VehicleBuilder().asCar().build();

    const ticket = lot.parkEntry(car);
    clock.advanceHours(2);
    clock.advanceMinutes(1);
    const fee = lot.processExit(ticket);

    expect(fee.amount).toBe(9);
  });

  it('a stay of zero duration is billed as 1 hour', () => {
    const { lot } = new LotBuilder().withCompactSpots(1).build();
    const car = new VehicleBuilder().asCar().build();

    const ticket = lot.parkEntry(car);
    const fee = lot.processExit(ticket);

    expect(fee.amount).toBe(3);
  });
});
