export class EmptyReportError extends Error {
  constructor() { super('Cannot submit an empty report'); this.name = 'EmptyReportError'; }
}

export class ReportExceedsMaximumError extends Error {
  constructor() { super('Report total exceeds the $5,000 maximum'); this.name = 'ReportExceedsMaximumError'; }
}

export class InvalidStatusTransitionError extends Error {
  constructor(message: string) { super(message); this.name = 'InvalidStatusTransitionError'; }
}

export class ExpenseNotFoundError extends Error {
  constructor() { super('Expense not found'); this.name = 'ExpenseNotFoundError'; }
}

export class InvalidAmountError extends Error {
  constructor() { super('Amount must be positive'); this.name = 'InvalidAmountError'; }
}

export class FinalizedReportError extends Error {
  constructor() { super('Cannot modify a finalized report'); this.name = 'FinalizedReportError'; }
}
