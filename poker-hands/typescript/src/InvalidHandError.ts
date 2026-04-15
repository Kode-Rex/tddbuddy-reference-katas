export class InvalidHandError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'InvalidHandError';
  }
}
