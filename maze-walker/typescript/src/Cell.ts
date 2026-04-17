/**
 * A coordinate in the maze. Compared by string key for set membership.
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

/** Return the four cardinal neighbors (up, down, left, right). */
export function cardinalNeighbors(cell: Cell): Cell[] {
  return [
    { row: cell.row - 1, col: cell.col },
    { row: cell.row + 1, col: cell.col },
    { row: cell.row, col: cell.col - 1 },
    { row: cell.row, col: cell.col + 1 },
  ];
}
