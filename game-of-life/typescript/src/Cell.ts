/**
 * A coordinate on the infinite plane.
 * Cells are compared by their string key (`row,col`) for set membership.
 */
export interface Cell {
  readonly row: number;
  readonly col: number;
}

/** Canonical string key for set-based equality. */
export function cellKey(cell: Cell): string {
  return `${cell.row},${cell.col}`;
}

/** Parse a cell key back into a Cell. */
export function parseKey(key: string): Cell {
  const [row, col] = key.split(',').map(Number);
  return { row: row!, col: col! };
}

/** Return the eight orthogonal and diagonal neighbors of a cell. */
export function neighbors(cell: Cell): Cell[] {
  const result: Cell[] = [];
  for (let dr = -1; dr <= 1; dr++) {
    for (let dc = -1; dc <= 1; dc++) {
      if (dr === 0 && dc === 0) continue;
      result.push({ row: cell.row + dr, col: cell.col + dc });
    }
  }
  return result;
}
