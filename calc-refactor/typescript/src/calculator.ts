// Identical byte-for-byte across C#, TypeScript, and Python.
// See ../../SCENARIOS.md for the characterization contract.

export const Keys = {
  equals: '=',
  clear: 'C',
} as const;

export const DisplayStrings = {
  zero: '0',
  error: 'Error',
} as const;

export const ErrorMessages = {
  unknownKey: (key: string): string => `unknown key: ${key}`,
} as const;

type Operator = '+' | '-' | '*' | '/';

function isDigit(c: string): boolean {
  return c.length === 1 && c >= '0' && c <= '9';
}

function isOperator(c: string): c is Operator {
  return c === '+' || c === '-' || c === '*' || c === '/';
}

// Integer division truncating toward zero — matches C# `/` and Python's
// `int(a/b)` rounding, so "7/2=" shows "3" and "-7/2=" shows "-3" in all
// three languages.
function truncDiv(a: number, b: number): number {
  return Math.trunc(a / b);
}

export class Calculator {
  private _display: string = DisplayStrings.zero;
  private left = 0;
  private pendingOperator: Operator | null = null;
  private rememberedRight = 0;
  private hasRememberedRight = false;
  private enteringOperand = false;
  private justEvaluated = false;
  private inError = false;

  get display(): string {
    return this._display;
  }

  press(key: string): void {
    if (this.inError) {
      if (key === Keys.clear) this.reset();
      return;
    }

    if (isDigit(key)) { this.pressDigit(key); return; }
    if (isOperator(key)) { this.pressOperator(key); return; }
    if (key === Keys.equals) { this.pressEquals(); return; }
    if (key === Keys.clear) { this.reset(); return; }

    throw new Error(ErrorMessages.unknownKey(key));
  }

  private pressDigit(key: string): void {
    if (!this.enteringOperand || this.justEvaluated) {
      if (this.justEvaluated) {
        // A digit after `=` starts a fresh calculation — drop the
        // accumulator and the pending operator so the new operand
        // is not folded into the remembered state.
        this.left = 0;
        this.pendingOperator = null;
        this.hasRememberedRight = false;
      }
      this._display = key;
      this.enteringOperand = true;
      this.justEvaluated = false;
      return;
    }

    this._display = this._display === DisplayStrings.zero ? key : this._display + key;
  }

  private pressOperator(key: Operator): void {
    if (this.enteringOperand || this.justEvaluated) {
      if (this.pendingOperator !== null && this.enteringOperand) {
        this.left = this.apply(this.left, this.pendingOperator, parseInt(this._display, 10));
        this._display = this.left.toString();
        if (this.inError) return;
      } else {
        this.left = parseInt(this._display, 10);
      }
      this.pendingOperator = key;
      this.enteringOperand = false;
      this.justEvaluated = false;
      this.hasRememberedRight = false;
      return;
    }

    // Two operators in a row — the legacy `1++2` path.
    this.enterError();
  }

  private pressEquals(): void {
    if (this.pendingOperator === null) return;

    const right = this.hasRememberedRight && !this.enteringOperand
      ? this.rememberedRight
      : parseInt(this._display, 10);

    const result = this.apply(this.left, this.pendingOperator, right);
    if (this.inError) return;

    this.rememberedRight = right;
    this.hasRememberedRight = true;
    this.left = result;
    this._display = result.toString();
    this.enteringOperand = false;
    this.justEvaluated = true;
  }

  private apply(left: number, op: Operator, right: number): number {
    switch (op) {
      case '+': return left + right;
      case '-': return left - right;
      case '*': return left * right;
      case '/':
        if (right === 0) { this.enterError(); return 0; }
        return truncDiv(left, right);
    }
  }

  private enterError(): void {
    this.inError = true;
    this._display = DisplayStrings.error;
  }

  private reset(): void {
    this._display = DisplayStrings.zero;
    this.left = 0;
    this.pendingOperator = null;
    this.rememberedRight = 0;
    this.hasRememberedRight = false;
    this.enteringOperand = false;
    this.justEvaluated = false;
    this.inError = false;
  }
}
