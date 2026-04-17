import { InvalidLotConfigurationError } from '../src/errors.js';
import { LotBuilder } from './LotBuilder.js';

describe('Lot Construction', () => {
  it('a lot with spots across all types is valid', () => {
    expect(() =>
      new LotBuilder().withMotorcycleSpots(2).withCompactSpots(3).withLargeSpots(1).build(),
    ).not.toThrow();
  });

  it('a lot with zero total spots raises InvalidLotConfigurationError', () => {
    expect(() =>
      new LotBuilder().withMotorcycleSpots(0).withCompactSpots(0).withLargeSpots(0).build(),
    ).toThrow(new InvalidLotConfigurationError('Lot must have at least one spot'));
  });
});
