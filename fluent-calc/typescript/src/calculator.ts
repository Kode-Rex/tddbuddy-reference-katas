type Op = 'plus' | 'minus';

interface Operation {
  kind: Op;
  operand: number;
}

export class Calculator {
  private value = 0;
  private seeded = false;
  private readonly undoStack: Operation[] = [];
  private readonly redoStack: Operation[] = [];

  seed(n: number): this {
    if (this.seeded) return this;
    this.value = n;
    this.seeded = true;
    return this;
  }

  plus(n: number): this {
    return this.apply({ kind: 'plus', operand: n });
  }

  minus(n: number): this {
    return this.apply({ kind: 'minus', operand: n });
  }

  undo(): this {
    const operation = this.undoStack.pop();
    if (operation === undefined) return this;
    this.value = reverse(this.value, operation);
    this.redoStack.push(operation);
    return this;
  }

  redo(): this {
    const operation = this.redoStack.pop();
    if (operation === undefined) return this;
    this.value = forward(this.value, operation);
    this.undoStack.push(operation);
    return this;
  }

  save(): this {
    this.undoStack.length = 0;
    this.redoStack.length = 0;
    return this;
  }

  result(): number {
    return this.value;
  }

  private apply(operation: Operation): this {
    if (!this.seeded) return this;
    this.value = forward(this.value, operation);
    this.undoStack.push(operation);
    this.redoStack.length = 0;
    return this;
  }
}

function forward(value: number, operation: Operation): number {
  return operation.kind === 'plus' ? value + operation.operand : value - operation.operand;
}

function reverse(value: number, operation: Operation): number {
  return operation.kind === 'plus' ? value - operation.operand : value + operation.operand;
}
