export class UnknownColumnError extends Error {
  constructor(columnName: string) {
    super(`Unknown column: ${columnName}`);
    this.name = 'UnknownColumnError';
  }
}
