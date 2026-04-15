export class RegistrationRejectedError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'RegistrationRejectedError';
  }
}

export class UnauthorizedError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'UnauthorizedError';
  }
}

export class NoCopiesAvailableError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'NoCopiesAvailableError';
  }
}

export class OverdueRentalError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'OverdueRentalError';
  }
}

export class NoActiveRentalError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'NoActiveRentalError';
  }
}

export class TitleNotInCatalogError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'TitleNotInCatalogError';
  }
}
