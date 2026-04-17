import { CsvTable } from './CsvTable.js';
import { Row } from './Row.js';

export function parseCsv(csv: string): CsvTable {
  const lines = csv.split('\n').filter(l => l.length > 0);
  if (lines.length === 0) return new CsvTable([], []);

  const headers = parseFields(lines[0]!);
  const rows: Row[] = [];

  for (let i = 1; i < lines.length; i++) {
    const fields = parseFields(lines[i]!);
    const values = new Map<string, string>();
    for (let j = 0; j < headers.length; j++) {
      values.set(headers[j]!, j < fields.length ? fields[j]! : '');
    }
    rows.push(new Row(values));
  }

  return new CsvTable(headers, rows);
}

function parseFields(line: string): string[] {
  const fields: string[] = [];
  let current = '';
  let inQuotes = false;

  for (const ch of line) {
    if (ch === '"') {
      inQuotes = !inQuotes;
    } else if (ch === ',' && !inQuotes) {
      fields.push(current.trim());
      current = '';
    } else {
      current += ch;
    }
  }

  fields.push(current.trim());
  return fields;
}
