export class VehicleAlreadyParkedError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'VehicleAlreadyParkedError';
  }
}

export class NoAvailableSpotError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'NoAvailableSpotError';
  }
}

export class InvalidTicketError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'InvalidTicketError';
  }
}

export class InvalidLotConfigurationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'InvalidLotConfigurationError';
  }
}
