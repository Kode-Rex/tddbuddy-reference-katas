import { describe, it, expect } from 'vitest';
import { find } from '../src/lastSunday.js';

const utc = (year: number, month: number, day: number): Date =>
  new Date(Date.UTC(year, month - 1, day));

describe('find (last Sunday of a month)', () => {
  it('returns 2013-01-27 for January 2013 (31-day month ending Thursday)', () => {
    expect(find(2013, 1)).toEqual(utc(2013, 1, 27));
  });

  it('returns 2013-02-24 for February 2013 (non-leap February)', () => {
    expect(find(2013, 2)).toEqual(utc(2013, 2, 24));
  });

  it('returns 2013-03-31 for March 2013 (last day itself is Sunday)', () => {
    expect(find(2013, 3)).toEqual(utc(2013, 3, 31));
  });

  it('returns 2013-04-28 for April 2013 (30-day month)', () => {
    expect(find(2013, 4)).toEqual(utc(2013, 4, 28));
  });

  it('returns 2013-06-30 for June 2013 (30-day month, last day is Sunday)', () => {
    expect(find(2013, 6)).toEqual(utc(2013, 6, 30));
  });

  it('returns 2013-12-29 for December 2013 (year-end boundary)', () => {
    expect(find(2013, 12)).toEqual(utc(2013, 12, 29));
  });

  it('returns 2020-02-23 for February 2020 (leap February ending Saturday)', () => {
    expect(find(2020, 2)).toEqual(utc(2020, 2, 23));
  });

  it('returns 2032-02-29 for February 2032 (leap day itself is Sunday)', () => {
    expect(find(2032, 2)).toEqual(utc(2032, 2, 29));
  });

  it('returns 2100-02-28 for February 2100 (century non-leap February)', () => {
    expect(find(2100, 2)).toEqual(utc(2100, 2, 28));
  });

  it('returns 2000-12-31 for December 2000 (four-hundred-divisible leap year)', () => {
    expect(find(2000, 12)).toEqual(utc(2000, 12, 31));
  });
});
