import { describe, it, expect } from 'vitest';
import { Calculator } from '../src/calculator.js';
import { aCalculator } from './calculatorBuilder.js';

describe('Calculator', () => {
  it('a fresh calculator displays zero', () => {
    expect(aCalculator().build().display).toBe('0');
  });

  it('pressing a single digit displays that digit', () => {
    expect(aCalculator().pressKeys('7').build().display).toBe('7');
  });

  it('pressing multiple digits builds a multi-digit operand', () => {
    expect(aCalculator().pressKeys('123').build().display).toBe('123');
  });

  it('leading zero is replaced by the first non-zero digit', () => {
    expect(aCalculator().pressKeys('05').build().display).toBe('5');
  });

  it('addition of two operands', () => {
    expect(aCalculator().pressKeys('1+2=').build().display).toBe('3');
  });

  it('subtraction of two operands', () => {
    expect(aCalculator().pressKeys('9-4=').build().display).toBe('5');
  });

  it('multiplication of two operands', () => {
    expect(aCalculator().pressKeys('6*7=').build().display).toBe('42');
  });

  it('integer division truncates toward zero', () => {
    expect(aCalculator().pressKeys('7/2=').build().display).toBe('3');
  });

  it('division by zero enters the error state', () => {
    expect(aCalculator().pressKeys('5/0=').build().display).toBe('Error');
  });

  it('consecutive operators enter the error state', () => {
    expect(aCalculator().pressKeys('1++2=').build().display).toBe('Error');
  });

  it('error is sticky — further keys are ignored', () => {
    expect(aCalculator().pressKeys('5/0=123+4=').build().display).toBe('Error');
  });

  it('clear resets from the error state', () => {
    expect(aCalculator().pressKeys('5/0=C').build().display).toBe('0');
  });

  it('clear resets from a normal state', () => {
    expect(aCalculator().pressKeys('42C').build().display).toBe('0');
  });

  it('equals with no pending operator is a no-op', () => {
    expect(aCalculator().pressKeys('42=').build().display).toBe('42');
  });

  it('repeated equals reapplies the last operator and operand', () => {
    expect(aCalculator().pressKeys('2+3==').build().display).toBe('8');
  });

  it('chained operators evaluate left-to-right', () => {
    expect(aCalculator().pressKeys('2+3*4=').build().display).toBe('20');
  });

  it('operator after equals continues from the result', () => {
    expect(aCalculator().pressKeys('2+3=*4=').build().display).toBe('20');
  });

  it('a new digit after equals starts a fresh calculation', () => {
    expect(aCalculator().pressKeys('2+3=7').build().display).toBe('7');
    expect(aCalculator().pressKeys('2+3=7+1=').build().display).toBe('8');
  });

  it('negative results display with a leading minus', () => {
    expect(aCalculator().pressKeys('3-9=').build().display).toBe('-6');
  });

  it('an unknown key raises an argument error', () => {
    const calculator = new Calculator();
    expect(() => calculator.press('x')).toThrow('unknown key: x');
  });
});
