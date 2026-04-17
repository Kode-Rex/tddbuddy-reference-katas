import { Station } from '../src/Station.js';
import type { AlertThresholds } from '../src/AlertThresholds.js';
import { FixedClock } from './FixedClock.js';

interface SeededReading {
  time: Date;
  temperature: number;
  humidity: number;
  windSpeed: number;
}

export class StationBuilder {
  private _startTime: Date = new Date(Date.UTC(2026, 5, 15, 12, 0, 0));
  private _thresholds: AlertThresholds = {};
  private _seededReadings: SeededReading[] = [];

  startingAt(time: Date): this {
    this._startTime = time;
    return this;
  }

  withThresholds(thresholds: AlertThresholds): this {
    this._thresholds = thresholds;
    return this;
  }

  withReading(temperature: number, humidity: number, windSpeed: number, at?: Date): this {
    this._seededReadings.push({
      time: at ?? this._startTime,
      temperature,
      humidity,
      windSpeed,
    });
    return this;
  }

  build(): { station: Station; clock: FixedClock } {
    const clock = new FixedClock(this._startTime);
    const station = new Station(clock, this._thresholds);
    for (const { time, temperature, humidity, windSpeed } of this._seededReadings) {
      clock.advanceTo(time);
      station.record(temperature, humidity, windSpeed);
    }
    return { station, clock };
  }
}
