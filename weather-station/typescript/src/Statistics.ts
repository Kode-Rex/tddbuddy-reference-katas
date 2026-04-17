export class Statistics {
  constructor(
    public readonly minTemperature: number,
    public readonly maxTemperature: number,
    public readonly avgTemperature: number,
    public readonly minHumidity: number,
    public readonly maxHumidity: number,
    public readonly avgHumidity: number,
    public readonly maxWindSpeed: number,
    public readonly avgWindSpeed: number,
  ) {}
}
