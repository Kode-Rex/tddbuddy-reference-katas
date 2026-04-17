import { NoAvailableSpotError } from '../src/errors.js';
import { LotBuilder } from './LotBuilder.js';
import { VehicleBuilder } from './VehicleBuilder.js';

describe('No Available Spot', () => {
  it('parking a car when only motorcycle spots remain raises NoAvailableSpotError', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(2).withCompactSpots(0).withLargeSpots(0).build();
    const car = new VehicleBuilder().asCar().withPlate('CAR-100').build();

    expect(() => lot.parkEntry(car)).toThrow(
      new NoAvailableSpotError('No available spot for vehicle CAR-100'),
    );
  });

  it('parking a bus when only compact and motorcycle spots remain raises NoAvailableSpotError', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(2).withLargeSpots(0).build();
    const bus = new VehicleBuilder().asBus().withPlate('BUS-100').build();

    expect(() => lot.parkEntry(bus)).toThrow(
      new NoAvailableSpotError('No available spot for vehicle BUS-100'),
    );
  });

  it('parking any vehicle when the lot is completely full raises NoAvailableSpotError', () => {
    const { lot } = new LotBuilder().withMotorcycleSpots(1).withCompactSpots(0).withLargeSpots(0).build();
    lot.parkEntry(new VehicleBuilder().asMotorcycle().withPlate('MC-001').build());

    expect(() => lot.parkEntry(new VehicleBuilder().asMotorcycle().withPlate('MC-002').build())).toThrow(
      new NoAvailableSpotError('No available spot for vehicle MC-002'),
    );
  });
});
