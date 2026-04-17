import { InvalidTicketError } from '../src/errors.js';
import { LotBuilder } from './LotBuilder.js';
import { VehicleBuilder } from './VehicleBuilder.js';

describe('Exit Frees the Spot', () => {
  it('exiting frees the spot so another vehicle of the same type can park', () => {
    const { lot, clock } = new LotBuilder().withMotorcycleSpots(0).withCompactSpots(1).withLargeSpots(0).build();
    const car1 = new VehicleBuilder().asCar().withPlate('CAR-001').build();
    const car2 = new VehicleBuilder().asCar().withPlate('CAR-002').build();

    const ticket = lot.parkEntry(car1);
    clock.advanceHours(1);
    lot.processExit(ticket);

    expect(() => lot.parkEntry(car2)).not.toThrow();
  });

  it('exiting with an invalid ticket raises InvalidTicketError', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(1).withLargeSpots(1).build();
    const fakeTicket = {
      vehicle: { type: 'car' as const, licensePlate: 'FAKE-001' },
      spotType: 'compact' as const,
      entryTime: new Date(Date.UTC(2026, 0, 1)),
    };

    expect(() => lot.processExit(fakeTicket)).toThrow(
      new InvalidTicketError('Ticket is not valid'),
    );
  });

  it('exiting with the same ticket twice raises InvalidTicketError', () => {
    const { lot, clock } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(1).withLargeSpots(1).build();
    const car = new VehicleBuilder().asCar().build();
    const ticket = lot.parkEntry(car);

    clock.advanceHours(1);
    lot.processExit(ticket);

    expect(() => lot.processExit(ticket)).toThrow(
      new InvalidTicketError('Ticket is not valid'),
    );
  });
});
