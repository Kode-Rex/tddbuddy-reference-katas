import { describe, expect, it } from 'vitest';
import { UnknownColumnError } from '../src/UnknownColumnError.js';
import { QueryBuilder } from './QueryBuilder.js';

describe('CsvQuery', () => {
  // --- Parsing ---

  it('parsing csv produces rows with correct column values', () => {
    const query = new QueryBuilder().build();
    const rows = query.rows;
    expect(rows[0]!.get('name')).toBe('Alice');
    expect(rows[0]!.get('age')).toBe('35');
    expect(rows[0]!.get('city')).toBe('London');
    expect(rows[0]!.get('salary')).toBe('75000');
  });

  it('parsing csv with only a header row produces zero rows', () => {
    const query = new QueryBuilder().withCsv('name,age,city,salary').build();
    expect(query.rows).toHaveLength(0);
  });

  it('parsing csv with a single data row produces one row', () => {
    const query = new QueryBuilder().withCsv('name,age\nAlice,35').build();
    expect(query.rows).toHaveLength(1);
    expect(query.rows[0]!.get('name')).toBe('Alice');
  });

  it('parsing quoted fields strips quotes and preserves commas', () => {
    const query = new QueryBuilder()
      .withCsv('name,age,city\n"Smith, Jr.",45,London')
      .build();
    expect(query.rows[0]!.get('name')).toBe('Smith, Jr.');
  });

  // --- Select ---

  it('selecting a single column returns only that column', () => {
    const query = new QueryBuilder().build();
    const rows = query.select('name').rows;
    expect(rows[0]!.columns).toEqual(['name']);
    expect(rows[0]!.get('name')).toBe('Alice');
    expect(rows[4]!.get('name')).toBe('Eve');
  });

  it('selecting multiple columns returns them in requested order', () => {
    const query = new QueryBuilder().build();
    const rows = query.select('city', 'name').rows;
    expect(rows[0]!.columns).toEqual(['city', 'name']);
    expect(rows[0]!.get('city')).toBe('London');
    expect(rows[0]!.get('name')).toBe('Alice');
  });

  it('selecting an unknown column raises UnknownColumnError', () => {
    const query = new QueryBuilder().build();
    expect(() => query.select('invalid_column')).toThrow(UnknownColumnError);
    expect(() => query.select('invalid_column')).toThrow('Unknown column: invalid_column');
  });

  // --- Where — equality and inequality ---

  it('where equal filters to matching rows', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('city', '=', 'London').rows;
    expect(rows).toHaveLength(2);
    expect(rows[0]!.get('name')).toBe('Alice');
    expect(rows[1]!.get('name')).toBe('Charlie');
  });

  it('where not-equal excludes matching rows', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('city', '!=', 'London').rows;
    expect(rows).toHaveLength(3);
    expect(rows.map(r => r.get('name'))).toEqual(['Bob', 'Diana', 'Eve']);
  });

  it('where on a value with no matches returns empty', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('city', '=', 'Tokyo').rows;
    expect(rows).toHaveLength(0);
  });

  // --- Where — numeric comparisons ---

  it('where greater-than compares numerically', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('age', '>', '30').rows;
    expect(rows).toHaveLength(3);
    expect(rows.map(r => r.get('name'))).toEqual(['Alice', 'Charlie', 'Diana']);
  });

  it('where less-than compares numerically', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('age', '<', '30').rows;
    expect(rows).toHaveLength(2);
    expect(rows.map(r => r.get('name'))).toEqual(['Bob', 'Eve']);
  });

  it('where greater-than-or-equal includes the boundary', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('age', '>=', '35').rows;
    expect(rows).toHaveLength(2);
    expect(rows.map(r => r.get('name'))).toEqual(['Alice', 'Charlie']);
  });

  it('where less-than-or-equal includes the boundary', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('age', '<=', '28').rows;
    expect(rows).toHaveLength(2);
    expect(rows.map(r => r.get('name'))).toEqual(['Bob', 'Eve']);
  });

  // --- Where — string fallback ---

  it('where compares as strings when values are non-numeric', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('city', '>', 'London').rows;
    expect(rows).toHaveLength(2);
    expect(rows.map(r => r.get('name'))).toEqual(['Bob', 'Eve']);
  });

  // --- OrderBy ---

  it('orderBy ascending sorts numerically', () => {
    const query = new QueryBuilder().build();
    const rows = query.orderBy('age', 'asc').rows;
    expect(rows.map(r => r.get('name'))).toEqual(['Bob', 'Eve', 'Diana', 'Alice', 'Charlie']);
  });

  it('orderBy descending sorts numerically', () => {
    const query = new QueryBuilder().build();
    const rows = query.orderBy('salary', 'desc').rows;
    expect(rows.map(r => r.get('name'))).toEqual(['Charlie', 'Alice', 'Diana', 'Eve', 'Bob']);
  });

  it('orderBy sorts strings lexicographically', () => {
    const query = new QueryBuilder().build();
    const rows = query.orderBy('name', 'asc').rows;
    expect(rows.map(r => r.get('name'))).toEqual(['Alice', 'Bob', 'Charlie', 'Diana', 'Eve']);
  });

  // --- Limit ---

  it('limit restricts the result set to N rows', () => {
    const query = new QueryBuilder().build();
    const rows = query.limit(2).rows;
    expect(rows).toHaveLength(2);
    expect(rows[0]!.get('name')).toBe('Alice');
    expect(rows[1]!.get('name')).toBe('Bob');
  });

  it('limit larger than row count returns all rows', () => {
    const query = new QueryBuilder().build();
    const rows = query.limit(100).rows;
    expect(rows).toHaveLength(5);
  });

  // --- Count ---

  it('count returns the total number of rows', () => {
    const query = new QueryBuilder().build();
    expect(query.count()).toBe(5);
  });

  it('count after where returns the filtered count', () => {
    const query = new QueryBuilder().build();
    expect(query.where('city', '=', 'Paris').count()).toBe(2);
  });

  // --- Chaining ---

  it('where then select returns filtered projected rows', () => {
    const query = new QueryBuilder().build();
    const rows = query.where('age', '>', '30').select('name').rows;
    expect(rows).toHaveLength(3);
    expect(rows.map(r => r.get('name'))).toEqual(['Alice', 'Charlie', 'Diana']);
  });

  it('where then orderBy then limit chains correctly', () => {
    const query = new QueryBuilder().build();
    const rows = query
      .where('age', '>=', '35')
      .orderBy('salary', 'desc')
      .limit(1)
      .rows;
    expect(rows).toHaveLength(1);
    expect(rows[0]!.get('name')).toBe('Charlie');
  });

  it('where then count returns zero when no rows match', () => {
    const query = new QueryBuilder().build();
    expect(query.where('city', '=', 'Tokyo').count()).toBe(0);
  });
});
