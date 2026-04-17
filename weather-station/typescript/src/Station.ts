import type { Clock } from './Clock.js';
import type { AlertThresholds } from './AlertThresholds.js';
import { Reading } from './Reading.js';
import { Statistics } from './Statistics.js';
import { Alert } from './Alert.js';
import { InvalidReadingError } from './InvalidReadingError.js';
import { NoReadingsError } from './NoReadingsError.js';

export class Station {
  private readonly _readings: Reading[] = [];

  constructor(
    private readonly clock: Clock,
    private readonly thresholds: AlertThresholds = {},
  ) {}

  get readings(): readonly Reading[] {
    return this._readings;
  }

  record(temperature: number, humidity: number, windSpeed: number): Alert[] {
    if (humidity < 0 || humidity > 100) {
      throw new InvalidReadingError('Humidity must be between 0 and 100.');
    }
    if (windSpeed < 0) {
      throw new InvalidReadingError('Wind speed must be non-negative.');
    }

    const reading = new Reading(temperature, humidity, windSpeed, this.clock.now());
    this._readings.push(reading);

    return this.evaluateAlerts(reading);
  }

  getStatistics(): Statistics {
    if (this._readings.length === 0) {
      throw new NoReadingsError();
    }

    const temps = this._readings.map((r) => r.temperature);
    const humidities = this._readings.map((r) => r.humidity);
    const winds = this._readings.map((r) => r.windSpeed);

    return new Statistics(
      Math.min(...temps),
      Math.max(...temps),
      temps.reduce((a, b) => a + b, 0) / temps.length,
      Math.min(...humidities),
      Math.max(...humidities),
      humidities.reduce((a, b) => a + b, 0) / humidities.length,
      Math.max(...winds),
      winds.reduce((a, b) => a + b, 0) / winds.length,
    );
  }

  private evaluateAlerts(reading: Reading): Alert[] {
    const alerts: Alert[] = [];

    if (
      this.thresholds.highTemperatureCeiling !== undefined &&
      reading.temperature > this.thresholds.highTemperatureCeiling
    ) {
      alerts.push(
        new Alert(
          `High temperature alert: ${reading.temperature} exceeds ceiling of ${this.thresholds.highTemperatureCeiling}.`,
        ),
      );
    }

    if (
      this.thresholds.lowTemperatureFloor !== undefined &&
      reading.temperature < this.thresholds.lowTemperatureFloor
    ) {
      alerts.push(
        new Alert(
          `Low temperature alert: ${reading.temperature} is below floor of ${this.thresholds.lowTemperatureFloor}.`,
        ),
      );
    }

    if (
      this.thresholds.highWindSpeedLimit !== undefined &&
      reading.windSpeed > this.thresholds.highWindSpeedLimit
    ) {
      alerts.push(
        new Alert(
          `High wind alert: ${reading.windSpeed} exceeds limit of ${this.thresholds.highWindSpeedLimit}.`,
        ),
      );
    }

    return alerts;
  }
}
