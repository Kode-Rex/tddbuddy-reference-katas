import { AssertionFailedException } from './AssertionFailedException.js';

export function assertEqual(expected: unknown, actual: unknown): void {
  if (expected !== actual) {
    throw new AssertionFailedException(`expected ${String(expected)} but got ${String(actual)}`);
  }
}

export function assertTrue(condition: boolean): void {
  if (!condition) {
    throw new AssertionFailedException('expected true');
  }
}

export function assertThrows(fn: () => void): void {
  try {
    fn();
  } catch {
    return;
  }
  throw new AssertionFailedException('expected exception');
}
