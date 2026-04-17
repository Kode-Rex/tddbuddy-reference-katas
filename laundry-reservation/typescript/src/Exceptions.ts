export class DuplicateReservationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'DuplicateReservationError';
  }
}
