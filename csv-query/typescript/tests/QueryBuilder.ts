import { parseCsv } from '../src/CsvParser.js';
import { Query } from '../src/Query.js';

const DefaultCsv = [
  'name,age,city,salary',
  'Alice,35,London,75000',
  'Bob,28,Paris,55000',
  'Charlie,42,London,90000',
  'Diana,31,Berlin,65000',
  'Eve,28,Paris,60000',
].join('\n');

export class QueryBuilder {
  private csv = DefaultCsv;

  withCsv(csv: string): this { this.csv = csv; return this; }

  build(): Query {
    const table = parseCsv(this.csv);
    return new Query(table);
  }
}
