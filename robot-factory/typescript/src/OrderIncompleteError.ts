export class OrderIncompleteError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'OrderIncompleteError';
  }
}
