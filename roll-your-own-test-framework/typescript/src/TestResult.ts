export type TestStatus = 'PASS' | 'FAIL' | 'ERROR';

export interface TestResult {
  readonly name: string;
  readonly status: TestStatus;
  readonly message?: string;
}
