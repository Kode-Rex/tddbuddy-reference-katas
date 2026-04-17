export class InvalidReadingError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'InvalidReadingError';
  }
}
