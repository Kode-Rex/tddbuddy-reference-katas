import { AssertionFailedException } from '../src/AssertionFailedException.js';
import { TestSuite } from '../src/TestRunner.js';

export class TestSuiteBuilder {
  private readonly tests: Record<string, () => void> = {};

  withPassingTest(name: string = 'passingTest'): this {
    this.tests[name] = () => {};
    return this;
  }

  withFailingTest(name: string = 'failingTest', message: string = 'expected 5 but got 3'): this {
    this.tests[name] = () => {
      throw new AssertionFailedException(message);
    };
    return this;
  }

  withErroringTest(name: string = 'erroringTest', message: string = 'something went wrong'): this {
    this.tests[name] = () => {
      throw new Error(message);
    };
    return this;
  }

  build(): TestSuite {
    return { ...this.tests };
  }
}
