export class LineItemNotFoundError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'LineItemNotFoundError';
  }
}
