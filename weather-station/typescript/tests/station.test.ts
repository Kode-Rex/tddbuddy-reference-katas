import { describe, expect, it } from 'vitest';
import { InvalidReadingError } from '../src/InvalidReadingError.js';
import { NoReadingsError } from '../src/NoReadingsError.js';
import { StationBuilder } from './StationBuilder.js';

const June15Noon = new Date(Date.UTC(2026, 5, 15, 12, 0, 0));

describe('Station', () => {
  // --- Empty Station ---

  it('new station has no readings', () => {
    const { station } = new StationBuilder().build();
    expect(station.readings).toEqual([]);
  });

  it('requesting statistics from a station with no readings is rejected', () => {
    const { station } = new StationBuilder().build();
    expect(() => station.getStatistics()).toThrow(NoReadingsError);
    expect(() => station.getStatistics()).toThrow('Cannot compute statistics with no readings.');
  });

  // --- Recording Readings ---

  it('recording a valid reading increases the reading count', () => {
    const { station } = new StationBuilder().build();
    station.record(25, 60, 15);
    expect(station.readings).toHaveLength(1);
  });

  it('recording a reading captures the clock timestamp', () => {
    const { station } = new StationBuilder().startingAt(June15Noon).build();
    station.record(25, 60, 15);
    expect(station.readings[0]?.timestamp).toEqual(June15Noon);
  });

  it('recording a reading with humidity below zero is rejected', () => {
    const { station } = new StationBuilder().build();
    expect(() => station.record(25, -1, 10)).toThrow(InvalidReadingError);
    expect(() => station.record(25, -1, 10)).toThrow('Humidity must be between 0 and 100.');
  });

  it('recording a reading with humidity above 100 is rejected', () => {
    const { station } = new StationBuilder().build();
    expect(() => station.record(25, 101, 10)).toThrow(InvalidReadingError);
    expect(() => station.record(25, 101, 10)).toThrow('Humidity must be between 0 and 100.');
  });

  it('recording a reading with negative wind speed is rejected', () => {
    const { station } = new StationBuilder().build();
    expect(() => station.record(25, 50, -1)).toThrow(InvalidReadingError);
    expect(() => station.record(25, 50, -1)).toThrow('Wind speed must be non-negative.');
  });

  it('rejected reading leaves the station unchanged', () => {
    const { station } = new StationBuilder().withReading(20, 50, 10).build();
    const before = station.readings.length;
    try { station.record(25, -1, 10); } catch { /* expected */ }
    expect(station.readings.length).toBe(before);
  });

  // --- Temperature Statistics ---

  it('minimum temperature is the lowest recorded temperature', () => {
    const { station } = new StationBuilder()
      .withReading(30, 50, 10)
      .withReading(15, 55, 12)
      .withReading(25, 60, 8)
      .build();
    expect(station.getStatistics().minTemperature).toBe(15);
  });

  it('maximum temperature is the highest recorded temperature', () => {
    const { station } = new StationBuilder()
      .withReading(30, 50, 10)
      .withReading(15, 55, 12)
      .withReading(25, 60, 8)
      .build();
    expect(station.getStatistics().maxTemperature).toBe(30);
  });

  it('average temperature is the mean of all recorded temperatures', () => {
    const { station } = new StationBuilder()
      .withReading(30, 50, 10)
      .withReading(15, 55, 12)
      .withReading(25, 60, 8)
      .build();
    expect(station.getStatistics().avgTemperature).toBeCloseTo(23.333, 2);
  });

  it('a single reading makes min, max, and average equal', () => {
    const { station } = new StationBuilder()
      .withReading(22, 65, 5)
      .build();
    const stats = station.getStatistics();
    expect(stats.minTemperature).toBe(22);
    expect(stats.maxTemperature).toBe(22);
    expect(stats.avgTemperature).toBe(22);
  });

  // --- Humidity Statistics ---

  it('minimum humidity is the lowest recorded humidity', () => {
    const { station } = new StationBuilder()
      .withReading(20, 30, 10)
      .withReading(22, 80, 12)
      .withReading(21, 55, 8)
      .build();
    expect(station.getStatistics().minHumidity).toBe(30);
  });

  it('maximum humidity is the highest recorded humidity', () => {
    const { station } = new StationBuilder()
      .withReading(20, 30, 10)
      .withReading(22, 80, 12)
      .withReading(21, 55, 8)
      .build();
    expect(station.getStatistics().maxHumidity).toBe(80);
  });

  it('average humidity is the mean of all recorded humidities', () => {
    const { station } = new StationBuilder()
      .withReading(20, 30, 10)
      .withReading(22, 80, 12)
      .withReading(21, 55, 8)
      .build();
    expect(station.getStatistics().avgHumidity).toBe(55);
  });

  // --- Wind Statistics ---

  it('maximum wind speed is the highest recorded wind speed', () => {
    const { station } = new StationBuilder()
      .withReading(20, 50, 10)
      .withReading(22, 55, 45)
      .withReading(21, 60, 20)
      .build();
    expect(station.getStatistics().maxWindSpeed).toBe(45);
  });

  it('average wind speed is the mean of all recorded wind speeds', () => {
    const { station } = new StationBuilder()
      .withReading(20, 50, 10)
      .withReading(22, 55, 45)
      .withReading(21, 60, 20)
      .build();
    expect(station.getStatistics().avgWindSpeed).toBe(25);
  });

  // --- Alerts ---

  it('high temperature alert fires when temperature exceeds the ceiling', () => {
    const { station } = new StationBuilder()
      .withThresholds({ highTemperatureCeiling: 35 })
      .build();
    const alerts = station.record(40, 50, 10);
    expect(alerts).toHaveLength(1);
    expect(alerts[0]?.message).toBe('High temperature alert: 40 exceeds ceiling of 35.');
  });

  it('no high temperature alert when temperature is at or below the ceiling', () => {
    const { station } = new StationBuilder()
      .withThresholds({ highTemperatureCeiling: 35 })
      .build();
    const alerts = station.record(35, 50, 10);
    expect(alerts).toHaveLength(0);
  });

  it('low temperature alert fires when temperature drops below the floor', () => {
    const { station } = new StationBuilder()
      .withThresholds({ lowTemperatureFloor: 0 })
      .build();
    const alerts = station.record(-5, 50, 10);
    expect(alerts).toHaveLength(1);
    expect(alerts[0]?.message).toBe('Low temperature alert: -5 is below floor of 0.');
  });

  it('no low temperature alert when temperature is at or above the floor', () => {
    const { station } = new StationBuilder()
      .withThresholds({ lowTemperatureFloor: 0 })
      .build();
    const alerts = station.record(0, 50, 10);
    expect(alerts).toHaveLength(0);
  });

  it('high wind alert fires when wind speed exceeds the limit', () => {
    const { station } = new StationBuilder()
      .withThresholds({ highWindSpeedLimit: 50 })
      .build();
    const alerts = station.record(20, 50, 60);
    expect(alerts).toHaveLength(1);
    expect(alerts[0]?.message).toBe('High wind alert: 60 exceeds limit of 50.');
  });

  it('no high wind alert when wind speed is at or below the limit', () => {
    const { station } = new StationBuilder()
      .withThresholds({ highWindSpeedLimit: 50 })
      .build();
    const alerts = station.record(20, 50, 50);
    expect(alerts).toHaveLength(0);
  });

  it('multiple alerts can fire for a single reading', () => {
    const { station } = new StationBuilder()
      .withThresholds({ highTemperatureCeiling: 35, highWindSpeedLimit: 50 })
      .build();
    const alerts = station.record(40, 50, 60);
    expect(alerts).toHaveLength(2);
    const messages = alerts.map((a) => a.message);
    expect(messages).toContain('High temperature alert: 40 exceeds ceiling of 35.');
    expect(messages).toContain('High wind alert: 60 exceeds limit of 50.');
  });
});
