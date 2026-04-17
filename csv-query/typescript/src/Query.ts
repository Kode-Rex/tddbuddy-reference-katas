import type { CsvTable } from './CsvTable.js';
import type { Row } from './Row.js';

function isNumeric(value: string): boolean {
  return value.trim() !== '' && !isNaN(Number(value));
}

function evaluate(cellValue: string, op: string, filterValue: string): boolean {
  if (isNumeric(cellValue) && isNumeric(filterValue)) {
    const cell = Number(cellValue);
    const filter = Number(filterValue);
    switch (op) {
      case '=': return cell === filter;
      case '!=': return cell !== filter;
      case '>': return cell > filter;
      case '<': return cell < filter;
      case '>=': return cell >= filter;
      case '<=': return cell <= filter;
      default: return false;
    }
  }

  const cmp = cellValue < filterValue ? -1 : cellValue > filterValue ? 1 : 0;
  switch (op) {
    case '=': return cmp === 0;
    case '!=': return cmp !== 0;
    case '>': return cmp > 0;
    case '<': return cmp < 0;
    case '>=': return cmp >= 0;
    case '<=': return cmp <= 0;
    default: return false;
  }
}

export class Query {
  private readonly table: CsvTable;
  private currentRows: Row[];

  constructor(table: CsvTable) {
    this.table = table;
    this.currentRows = [...table.rows];
  }

  select(...columns: string[]): this {
    for (const col of columns) this.table.validateColumn(col);
    this.currentRows = this.currentRows.map(r => r.project(columns));
    return this;
  }

  where(column: string, op: string, value: string): this {
    this.table.validateColumn(column);
    this.currentRows = this.currentRows.filter(r => evaluate(r.get(column), op, value));
    return this;
  }

  orderBy(column: string, direction: string): this {
    this.table.validateColumn(column);
    const asc = direction.toLowerCase() === 'asc';
    this.currentRows.sort((a, b) => {
      const va = a.get(column);
      const vb = b.get(column);
      let cmp: number;
      if (isNumeric(va) && isNumeric(vb)) {
        cmp = Number(va) - Number(vb);
      } else {
        cmp = va < vb ? -1 : va > vb ? 1 : 0;
      }
      return asc ? cmp : -cmp;
    });
    return this;
  }

  limit(n: number): this {
    this.currentRows = this.currentRows.slice(0, n);
    return this;
  }

  count(): number {
    return this.currentRows.length;
  }

  get rows(): readonly Row[] {
    return this.currentRows;
  }
}
