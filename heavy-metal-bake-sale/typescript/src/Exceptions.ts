export class OutOfStockException extends Error {
  constructor(productName: string) {
    super(`${productName} is out of stock`);
    this.name = 'OutOfStockException';
  }
}

export class InsufficientPaymentException extends Error {
  constructor() {
    super('Not enough money.');
    this.name = 'InsufficientPaymentException';
  }
}

export class UnknownPurchaseCodeException extends Error {
  constructor(code: string) {
    super(`Unknown purchase code: ${code}`);
    this.name = 'UnknownPurchaseCodeException';
  }
}
