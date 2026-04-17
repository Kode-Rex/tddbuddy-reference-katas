import { LotBuilder } from './LotBuilder.js';
import { VehicleBuilder } from './VehicleBuilder.js';

describe('Spot Allocation by Vehicle Type', () => {
  it('a motorcycle parks in a motorcycle spot when available', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(1).withLargeSpots(1).build();
    const motorcycle = new VehicleBuilder().asMotorcycle().build();

    const ticket = lot.parkEntry(motorcycle);

    expect(ticket.spotType).toBe('motorcycle');
  });

  it('a car parks in a compact spot when available', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(1).withLargeSpots(1).build();
    const car = new VehicleBuilder().asCar().build();

    const ticket = lot.parkEntry(car);

    expect(ticket.spotType).toBe('compact');
  });

  it('a bus parks in a large spot when available', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(1).withLargeSpots(1).build();
    const bus = new VehicleBuilder().asBus().build();

    const ticket = lot.parkEntry(bus);

    expect(ticket.spotType).toBe('large');
  });

  it('a motorcycle uses a compact spot when no motorcycle spots remain', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(0).withCompactSpots(1).withLargeSpots(1).build();
    const motorcycle = new VehicleBuilder().asMotorcycle().build();

    const ticket = lot.parkEntry(motorcycle);

    expect(ticket.spotType).toBe('compact');
  });

  it('a motorcycle uses a large spot when no motorcycle or compact spots remain', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(0).withCompactSpots(0).withLargeSpots(1).build();
    const motorcycle = new VehicleBuilder().asMotorcycle().build();

    const ticket = lot.parkEntry(motorcycle);

    expect(ticket.spotType).toBe('large');
  });

  it('a car uses a large spot when no compact spots remain', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(0).withLargeSpots(1).build();
    const car = new VehicleBuilder().asCar().build();

    const ticket = lot.parkEntry(car);

    expect(ticket.spotType).toBe('large');
  });
});
