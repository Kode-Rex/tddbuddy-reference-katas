export class InvalidLevelException extends Error {
  constructor(level: number) {
    super(`Tower level must be between 1 and 4, got ${level}`);
    this.name = 'InvalidLevelException';
  }
}
