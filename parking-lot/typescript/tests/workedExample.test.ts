import { NoAvailableSpotError } from '../src/errors.js';
import { LotBuilder } from './LotBuilder.js';
import { VehicleBuilder } from './VehicleBuilder.js';

describe('End-to-End Worked Example', () => {
  it('lot with one of each spot type: full lifecycle', () => {
    const { lot, clock } = new LotBuilder()
      .withMotorcycleSpots(1).withCompactSpots(1).withLargeSpots(1).build();

    const motorcycle = new VehicleBuilder().asMotorcycle().withPlate('MC-001').build();
    const car = new VehicleBuilder().asCar().withPlate('CAR-001').build();
    const bus = new VehicleBuilder().asBus().withPlate('BUS-001').build();

    // Park all three — fills the lot
    const mcTicket = lot.parkEntry(motorcycle);
    expect(mcTicket.spotType).toBe('motorcycle');

    const carTicket = lot.parkEntry(car);
    expect(carTicket.spotType).toBe('compact');

    const busTicket = lot.parkEntry(bus);
    expect(busTicket.spotType).toBe('large');

    // Exit motorcycle at t+90min → $2 (ceil(1.5) = 2 hours × $1)
    clock.advanceMinutes(90);
    const mcFee = lot.processExit(mcTicket);
    expect(mcFee.amount).toBe(2);

    // Try to park a second car — motorcycle spot freed, but car doesn't fit motorcycle spot
    const car2 = new VehicleBuilder().asCar().withPlate('CAR-002').build();
    expect(() => lot.parkEntry(car2)).toThrow(
      new NoAvailableSpotError('No available spot for vehicle CAR-002'),
    );

    // Park a second motorcycle — gets the freed motorcycle spot
    const mc2 = new VehicleBuilder().asMotorcycle().withPlate('MC-002').build();
    const mc2Ticket = lot.parkEntry(mc2);
    expect(mc2Ticket.spotType).toBe('motorcycle');

    // Exit car at t+120min from start → $6 (2 hours × $3)
    clock.advanceMinutes(30);
    const carFee = lot.processExit(carTicket);
    expect(carFee.amount).toBe(6);

    // Exit bus at t+2h from start → $10 (2 hours × $5)
    const busFee = lot.processExit(busTicket);
    expect(busFee.amount).toBe(10);
  });
});
