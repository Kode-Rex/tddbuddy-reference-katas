import { AssertionFailedException } from './AssertionFailedException.js';
import { TestSummary } from './TestSummary.js';

export type TestSuite = Record<string, () => void>;

export function runAll(suite: TestSuite): TestSummary {
  const summary = new TestSummary();

  for (const [name, testFn] of Object.entries(suite)) {
    try {
      testFn();
      summary.add({ name, status: 'PASS' });
    } catch (error) {
      if (error instanceof AssertionFailedException) {
        summary.add({ name, status: 'FAIL', message: error.message });
      } else {
        const message =
          error instanceof Error
            ? `${error.constructor.name}: ${error.message}`
            : String(error);
        summary.add({ name, status: 'ERROR', message });
      }
    }
  }

  return summary;
}
