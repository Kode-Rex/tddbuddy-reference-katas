import { describe, expect, it } from 'vitest';
import { runAll } from '../src/TestRunner.js';
import { TestSuiteBuilder } from './TestSuiteBuilder.js';

describe('TestRunner', () => {
  // --- Discovery ---

  it('empty suite discovers zero tests', () => {
    const suite = new TestSuiteBuilder().build();

    const summary = runAll(suite);

    expect(summary.run).toBe(0);
  });

  it('suite with three tests discovers and runs all three', () => {
    const suite = new TestSuiteBuilder()
      .withPassingTest('test1')
      .withPassingTest('test2')
      .withPassingTest('test3')
      .build();

    const summary = runAll(suite);

    expect(summary.run).toBe(3);
  });

  // --- Execution and Reporting ---

  it('passing test is reported as PASS', () => {
    const suite = new TestSuiteBuilder()
      .withPassingTest('myTest')
      .build();

    const summary = runAll(suite);

    expect(summary.results).toHaveLength(1);
    expect(summary.results[0]!.status).toBe('PASS');
  });

  it('failing assertion is reported as FAIL with message', () => {
    const suite = new TestSuiteBuilder()
      .withFailingTest('myTest', 'expected 5 but got 3')
      .build();

    const summary = runAll(suite);

    expect(summary.results).toHaveLength(1);
    expect(summary.results[0]!.status).toBe('FAIL');
    expect(summary.results[0]!.message).toBe('expected 5 but got 3');
  });

  it('unhandled exception is reported as ERROR with exception info', () => {
    const suite = new TestSuiteBuilder()
      .withErroringTest('myTest', 'something went wrong')
      .build();

    const summary = runAll(suite);

    expect(summary.results).toHaveLength(1);
    expect(summary.results[0]!.status).toBe('ERROR');
    expect(summary.results[0]!.message).toContain('something went wrong');
  });

  it('multiple tests with mixed results produce correct summary counts', () => {
    const suite = new TestSuiteBuilder()
      .withPassingTest('pass1')
      .withPassingTest('pass2')
      .withFailingTest('fail1')
      .withErroringTest('error1')
      .build();

    const summary = runAll(suite);

    expect(summary.run).toBe(4);
    expect(summary.passed).toBe(2);
    expect(summary.failed).toBe(1);
    expect(summary.errors).toBe(1);
  });
});
