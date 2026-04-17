import { VehicleAlreadyParkedError } from '../src/errors.js';
import { LotBuilder } from './LotBuilder.js';
import { VehicleBuilder } from './VehicleBuilder.js';

describe('Entry Produces a Ticket', () => {
  it('parking a vehicle returns a ticket with the vehicle and assigned spot type', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(1).withLargeSpots(1).build();
    const car = new VehicleBuilder().asCar().withPlate('CAR-100').build();

    const ticket = lot.parkEntry(car);

    expect(ticket.vehicle).toEqual(car);
    expect(ticket.spotType).toBe('compact');
  });

  it('parking the same vehicle twice raises VehicleAlreadyParkedError', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(2).withLargeSpots(1).build();
    const car = new VehicleBuilder().asCar().withPlate('CAR-100').build();
    lot.parkEntry(car);

    expect(() => lot.parkEntry(car)).toThrow(
      new VehicleAlreadyParkedError('Vehicle CAR-100 is already parked'),
    );
  });
});
