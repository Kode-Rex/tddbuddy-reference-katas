export class Reading {
  constructor(
    public readonly temperature: number,
    public readonly humidity: number,
    public readonly windSpeed: number,
    public readonly timestamp: Date,
  ) {}
}
