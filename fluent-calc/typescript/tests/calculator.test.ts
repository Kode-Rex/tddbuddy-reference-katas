import { describe, it, expect } from 'vitest';
import { Calculator } from '../src/calculator.js';

describe('Calculator', () => {
  it('a new calculator\'s result is zero', () => {
    expect(new Calculator().result()).toBe(0);
  });

  it('seeding sets the starting value', () => {
    expect(new Calculator().seed(10).result()).toBe(10);
  });

  it('plus adds to the seeded value', () => {
    expect(new Calculator().seed(10).plus(5).result()).toBe(15);
  });

  it('minus subtracts from the seeded value', () => {
    expect(new Calculator().seed(10).minus(4).result()).toBe(6);
  });

  it('operations chain in order', () => {
    expect(new Calculator().seed(10).plus(5).plus(5).result()).toBe(20);
  });

  it('subsequent seed calls are ignored', () => {
    expect(new Calculator().seed(10).seed(99).plus(5).result()).toBe(15);
  });

  it('plus before seed is ignored', () => {
    expect(new Calculator().plus(5).seed(10).result()).toBe(10);
  });

  it('undo reverses the most recent operation', () => {
    expect(new Calculator().seed(10).plus(5).undo().result()).toBe(10);
  });

  it('undo twice reverses two operations', () => {
    expect(new Calculator().seed(10).plus(5).minus(2).undo().undo().result()).toBe(10);
  });

  it('undo with nothing to undo is a no-op', () => {
    expect(new Calculator().undo().result()).toBe(0);
    expect(new Calculator().seed(10).undo().undo().result()).toBe(10);
  });

  it('redo replays the most recently undone operation', () => {
    expect(
      new Calculator().seed(10).plus(5).minus(2).undo().undo().redo().result(),
    ).toBe(15);
  });

  it('redo with nothing to redo is a no-op', () => {
    expect(new Calculator().seed(10).plus(5).redo().result()).toBe(15);
  });

  it('a new operation after undo clears the redo stack', () => {
    expect(
      new Calculator().seed(10).plus(5).undo().plus(3).redo().result(),
    ).toBe(13);
  });

  it('the full undo/redo example from the spec', () => {
    expect(
      new Calculator().seed(10).plus(5).minus(2).undo().undo().redo().result(),
    ).toBe(15);
  });

  it('save clears history so undo has no effect', () => {
    expect(
      new Calculator()
        .seed(10)
        .plus(5)
        .minus(2)
        .save()
        .undo()
        .redo()
        .undo()
        .plus(5)
        .result(),
    ).toBe(18);
  });
});
