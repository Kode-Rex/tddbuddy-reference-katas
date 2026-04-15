import { describe, it, expect } from 'vitest';
import { convert } from '../src/metricConverter.js';

describe('convert', () => {
  it('converts kilometers to miles', () => {
    expect(convert(1, 'kilometers', 'miles')).toBeCloseTo(0.621371, 9);
  });

  it('converts kilometers to miles for a larger value', () => {
    expect(convert(10, 'kilometers', 'miles')).toBeCloseTo(6.21371, 9);
  });

  it('converts zero kilometers to zero miles', () => {
    expect(convert(0, 'kilometers', 'miles')).toBe(0);
  });

  it('converts celsius to fahrenheit for freezing point', () => {
    expect(convert(0, 'celsius', 'fahrenheit')).toBe(32);
  });

  it('converts celsius to fahrenheit for body temperature', () => {
    expect(convert(30, 'celsius', 'fahrenheit')).toBe(86);
  });

  it('converts negative celsius to fahrenheit', () => {
    expect(convert(-40, 'celsius', 'fahrenheit')).toBe(-40);
  });

  it('converts kilograms to pounds', () => {
    expect(convert(5, 'kilograms', 'pounds')).toBeCloseTo(11.0231131, 6);
  });

  it('converts zero kilograms to zero pounds', () => {
    expect(convert(0, 'kilograms', 'pounds')).toBe(0);
  });

  it('converts liters to us gallons', () => {
    expect(convert(3.785411784, 'liters', 'usGallons')).toBeCloseTo(1, 9);
  });

  it('converts liters to uk gallons', () => {
    expect(convert(4.54609, 'liters', 'ukGallons')).toBeCloseTo(1, 9);
  });

  it('converts ten liters to us gallons', () => {
    expect(convert(10, 'liters', 'usGallons')).toBeCloseTo(2.641720524, 6);
  });

  it('converts ten liters to uk gallons', () => {
    expect(convert(10, 'liters', 'ukGallons')).toBeCloseTo(2.199692483, 6);
  });

  it('rejects an unsupported conversion pair', () => {
    expect(() => convert(1, 'kilometers', 'fahrenheit')).toThrow(
      'Unsupported conversion: kilometers to fahrenheit',
    );
  });
});
