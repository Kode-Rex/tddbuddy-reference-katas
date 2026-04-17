import type { Row } from './Row.js';
import { UnknownColumnError } from './UnknownColumnError.js';

export class CsvTable {
  readonly headers: readonly string[];
  readonly rows: readonly Row[];

  constructor(headers: readonly string[], rows: readonly Row[]) {
    this.headers = headers;
    this.rows = rows;
  }

  validateColumn(column: string): void {
    if (!this.headers.includes(column)) {
      throw new UnknownColumnError(column);
    }
  }
}
