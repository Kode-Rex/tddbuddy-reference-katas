export class InvalidAccountNumberFormatException extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'InvalidAccountNumberFormatException';
  }
}
