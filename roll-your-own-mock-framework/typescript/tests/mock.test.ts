import { describe, expect, it } from 'vitest';
import { createMock, when, verify } from '../src/Mock.js';
import { VerificationError } from '../src/VerificationError.js';

describe('Mock Framework', () => {
  // --- Mock Creation ---

  it('create mock — methods are callable without error', () => {
    const mock = createMock();

    expect(() => mock.add(2, 3)).not.toThrow();
  });

  it('unstubbed method returns undefined', () => {
    const mock = createMock();

    const result = mock.add(2, 3);

    expect(result).toBeUndefined();
  });

  // --- Stub Configuration ---

  it('stub return value for matching args', () => {
    const mock = createMock();
    when(mock).add(2, 3).thenReturn(5);

    const result = mock.add(2, 3);

    expect(result).toBe(5);
  });

  it('stub different arg sets return their own values', () => {
    const mock = createMock();
    when(mock).add(2, 3).thenReturn(5);
    when(mock).add(1, 1).thenReturn(2);

    expect(mock.add(2, 3)).toBe(5);
    expect(mock.add(1, 1)).toBe(2);
  });

  it('unstubbed args return undefined even when other args are stubbed', () => {
    const mock = createMock();
    when(mock).add(2, 3).thenReturn(5);

    const result = mock.add(9, 9);

    expect(result).toBeUndefined();
  });

  // --- Verification — wasCalled ---

  it('verify called method passes', () => {
    const mock = createMock();
    mock.add(2, 3);

    expect(() => verify(mock).add.wasCalled()).not.toThrow();
  });

  it('verify uncalled method fails with message', () => {
    const mock = createMock();

    expect(() => verify(mock).add.wasCalled()).toThrow(VerificationError);
    expect(() => verify(mock).add.wasCalled()).toThrow(
      'expected add to be called but was never called',
    );
  });

  // --- Verification — wasCalledWith ---

  it('verify called with correct args passes', () => {
    const mock = createMock();
    mock.add(2, 3);

    expect(() => verify(mock).add.wasCalledWith(2, 3)).not.toThrow();
  });

  it('verify called with wrong args fails with message', () => {
    const mock = createMock();
    mock.add(2, 3);

    expect(() => verify(mock).add.wasCalledWith(1, 1)).toThrow(VerificationError);
    expect(() => verify(mock).add.wasCalledWith(1, 1)).toThrow(
      'expected add(1, 1) to be called but was called with (2, 3)',
    );
  });

  it('verify wasCalledWith on uncalled method fails', () => {
    const mock = createMock();

    expect(() => verify(mock).add.wasCalledWith(1, 1)).toThrow(VerificationError);
    expect(() => verify(mock).add.wasCalledWith(1, 1)).toThrow(
      'expected add(1, 1) to be called but was never called',
    );
  });

  // --- Verification — wasCalledTimes ---

  it('verify call count matches', () => {
    const mock = createMock();
    mock.add(2, 3);
    mock.add(2, 3);

    expect(() => verify(mock).add.wasCalledTimes(2)).not.toThrow();
  });

  it('verify call count mismatch fails with message', () => {
    const mock = createMock();
    mock.add(2, 3);

    expect(() => verify(mock).add.wasCalledTimes(2)).toThrow(VerificationError);
    expect(() => verify(mock).add.wasCalledTimes(2)).toThrow(
      'expected add to be called 2 times but was called 1 times',
    );
  });
});
