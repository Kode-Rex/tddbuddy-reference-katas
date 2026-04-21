export class AssertionFailedException extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'AssertionFailedException';
  }
}
