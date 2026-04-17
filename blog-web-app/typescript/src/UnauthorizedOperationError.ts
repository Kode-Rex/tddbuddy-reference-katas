export class UnauthorizedOperationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'UnauthorizedOperationError';
  }
}
