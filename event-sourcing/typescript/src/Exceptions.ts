export class AccountNotOpenException extends Error {
  constructor(message: string) { super(message); this.name = 'AccountNotOpenException'; }
}

export class AccountClosedException extends Error {
  constructor(message: string) { super(message); this.name = 'AccountClosedException'; }
}

export class InsufficientFundsException extends Error {
  constructor(message: string) { super(message); this.name = 'InsufficientFundsException'; }
}

export class InvalidAmountException extends Error {
  constructor(message: string) { super(message); this.name = 'InvalidAmountException'; }
}

export class NonZeroBalanceException extends Error {
  constructor(message: string) { super(message); this.name = 'NonZeroBalanceException'; }
}
