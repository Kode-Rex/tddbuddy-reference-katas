import { describe, it, expect } from 'vitest';
import { copy } from '../src/characterCopy.js';
import { StringSource } from './StringSource.js';
import { RecordingDestination } from './RecordingDestination.js';

describe('copy', () => {
  it('immediate newline copies nothing', () => {
    const source = new StringSource('');
    const destination = new RecordingDestination();

    copy(source, destination);

    expect(destination.written).toBe('');
  });

  it('single character then newline copies that character', () => {
    const source = new StringSource('a');
    const destination = new RecordingDestination();

    copy(source, destination);

    expect(destination.written).toBe('a');
  });

  it('multiple characters then newline copies all of them in order', () => {
    const source = new StringSource('abc');
    const destination = new RecordingDestination();

    copy(source, destination);

    expect(destination.written).toBe('abc');
  });

  it('preserves whitespace before the terminator', () => {
    const source = new StringSource('a b');
    const destination = new RecordingDestination();

    copy(source, destination);

    expect(destination.written).toBe('a b');
  });

  it('does not read past the terminator', () => {
    const source = new StringSource('abc');
    const destination = new RecordingDestination();

    copy(source, destination);

    expect(source.readCount).toBe(4);
  });

  it('writes exactly as many characters as were read before the terminator', () => {
    const source = new StringSource('quip');
    const destination = new RecordingDestination();

    copy(source, destination);

    expect(destination.written).toHaveLength(4);
  });
});
