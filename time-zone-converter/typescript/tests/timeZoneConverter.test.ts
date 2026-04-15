import { describe, it, expect } from 'vitest';
import { convert, type LocalDateTime, type Offset } from '../src/timeZoneConverter.js';

const local = (year: number, month: number, day: number, hour: number, minute: number, second: number = 0): LocalDateTime =>
  ({ year, month, day, hour, minute, second });

const offset = (hours: number, minutes: number = 0): Offset => ({ hours, minutes });

const UTC = offset(0);

describe('convert (time zone converter over fixed UTC offsets)', () => {
  it('identity conversion between UTC and UTC returns the same local time', () => {
    expect(convert(local(2024, 6, 15, 12, 0), UTC, UTC)).toEqual(local(2024, 6, 15, 12, 0));
  });

  it('westward conversion from UTC to -05:00 subtracts five hours', () => {
    expect(convert(local(2024, 6, 15, 12, 0), UTC, offset(-5))).toEqual(local(2024, 6, 15, 7, 0));
  });

  it('eastward conversion from UTC to +09:00 adds nine hours', () => {
    expect(convert(local(2024, 6, 15, 12, 0), UTC, offset(9))).toEqual(local(2024, 6, 15, 21, 0));
  });

  it('cross-zone conversion without UTC transits via the shared instant', () => {
    expect(convert(local(2024, 6, 15, 9, 0), offset(-5), offset(9))).toEqual(local(2024, 6, 15, 23, 0));
  });

  it('forward conversion across midnight rolls into the next day', () => {
    expect(convert(local(2024, 6, 15, 22, 0), UTC, offset(5))).toEqual(local(2024, 6, 16, 3, 0));
  });

  it('backward conversion across midnight rolls into the previous day', () => {
    expect(convert(local(2024, 6, 15, 2, 0), UTC, offset(-5))).toEqual(local(2024, 6, 14, 21, 0));
  });

  it('half-hour offset +05:30 handles non-integer-hour zones', () => {
    expect(convert(local(2024, 6, 15, 12, 0), UTC, offset(5, 30))).toEqual(local(2024, 6, 15, 17, 30));
  });

  it('quarter-hour offset +05:45 handles forty-five-minute zones', () => {
    expect(convert(local(2024, 6, 15, 12, 0), UTC, offset(5, 45))).toEqual(local(2024, 6, 15, 17, 45));
  });

  it('forward conversion across month boundary rolls June into July', () => {
    expect(convert(local(2024, 6, 30, 23, 30), UTC, offset(2))).toEqual(local(2024, 7, 1, 1, 30));
  });

  it('backward conversion across year boundary rolls into the previous year', () => {
    expect(convert(local(2024, 1, 1, 1, 0), UTC, offset(-5))).toEqual(local(2023, 12, 31, 20, 0));
  });

  it('forward conversion across leap day rolls February 29th into March 1st', () => {
    expect(convert(local(2024, 2, 29, 23, 0), UTC, offset(2))).toEqual(local(2024, 3, 1, 1, 0));
  });

  it('international date line swing from +12:00 to -12:00 steps back one day', () => {
    expect(convert(local(2024, 6, 15, 10, 0), offset(12), offset(-12))).toEqual(local(2024, 6, 14, 10, 0));
  });
});
