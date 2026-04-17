import { UnknownColumnError } from './UnknownColumnError.js';

export class Row {
  private readonly values: ReadonlyMap<string, string>;

  constructor(values: Map<string, string> | Record<string, string>) {
    this.values = values instanceof Map
      ? new Map(values)
      : new Map(Object.entries(values));
  }

  get(column: string): string {
    const value = this.values.get(column);
    if (value === undefined) throw new UnknownColumnError(column);
    return value;
  }

  get columns(): string[] {
    return [...this.values.keys()];
  }

  project(columns: readonly string[]): Row {
    const projected = new Map<string, string>();
    for (const col of columns) {
      projected.set(col, this.get(col));
    }
    return new Row(projected);
  }

  toRecord(): Record<string, string> {
    const rec: Record<string, string> = {};
    for (const [k, v] of this.values) {
      rec[k] = v;
    }
    return rec;
  }
}
