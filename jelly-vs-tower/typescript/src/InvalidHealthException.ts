export class InvalidHealthException extends Error {
  constructor(health: number) {
    super(`Health must be strictly positive, got ${health}`);
    this.name = 'InvalidHealthException';
  }
}
