export class BreakerThresholdInvalidError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'BreakerThresholdInvalidError';
  }
}

export class BreakerTimeoutInvalidError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'BreakerTimeoutInvalidError';
  }
}

export class CircuitOpenError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'CircuitOpenError';
  }
}
