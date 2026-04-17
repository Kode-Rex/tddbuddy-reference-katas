export class PartNotAvailableError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'PartNotAvailableError';
  }
}
