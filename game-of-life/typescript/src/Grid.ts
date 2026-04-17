import { type Cell, cellKey, parseKey, neighbors } from './Cell.js';

/**
 * Immutable set of living cells on an unbounded infinite plane.
 * `tick()` applies the four GoL rules and returns the next generation.
 */
export class Grid {
  private readonly cells: ReadonlySet<string>;

  constructor(livingCells: Iterable<Cell>) {
    const keys = new Set<string>();
    for (const cell of livingCells) {
      keys.add(cellKey(cell));
    }
    this.cells = keys;
  }

  isAlive(row: number, col: number): boolean {
    return this.cells.has(cellKey({ row, col }));
  }

  livingCells(): Cell[] {
    return [...this.cells].map(parseKey);
  }

  tick(): Grid {
    const neighborCounts = new Map<string, number>();

    for (const key of this.cells) {
      const cell = parseKey(key);
      for (const neighbor of neighbors(cell)) {
        const nk = cellKey(neighbor);
        neighborCounts.set(nk, (neighborCounts.get(nk) ?? 0) + 1);
      }
    }

    const nextGeneration: Cell[] = [];

    for (const [key, count] of neighborCounts) {
      const isAlive = this.cells.has(key);
      if (count === 3 || (count === 2 && isAlive)) {
        nextGeneration.push(parseKey(key));
      }
    }

    return new Grid(nextGeneration);
  }
}
