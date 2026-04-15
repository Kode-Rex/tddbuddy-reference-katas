export class BookNotInCatalogError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'BookNotInCatalogError';
  }
}

export class NoCopiesAvailableError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'NoCopiesAvailableError';
  }
}

export class NoActiveLoanError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'NoActiveLoanError';
  }
}
