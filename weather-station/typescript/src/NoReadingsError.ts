export class NoReadingsError extends Error {
  constructor() {
    super('Cannot compute statistics with no readings.');
    this.name = 'NoReadingsError';
  }
}
