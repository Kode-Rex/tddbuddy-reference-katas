import { Calculator } from '../src/calculator.js';

export class CalculatorBuilder {
  private keys = '';

  pressKeys(keys: string): this {
    this.keys += keys;
    return this;
  }

  build(): Calculator {
    const calculator = new Calculator();
    for (const key of this.keys) calculator.press(key);
    return calculator;
  }
}

export function aCalculator(): CalculatorBuilder {
  return new CalculatorBuilder();
}
