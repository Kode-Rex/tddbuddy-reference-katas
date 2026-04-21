import { TestResult } from './TestResult.js';

export class TestSummary {
  private readonly _results: TestResult[] = [];

  get results(): readonly TestResult[] {
    return this._results;
  }

  get run(): number {
    return this._results.length;
  }

  get passed(): number {
    return this._results.filter((r) => r.status === 'PASS').length;
  }

  get failed(): number {
    return this._results.filter((r) => r.status === 'FAIL').length;
  }

  get errors(): number {
    return this._results.filter((r) => r.status === 'ERROR').length;
  }

  add(result: TestResult): void {
    this._results.push(result);
  }
}
