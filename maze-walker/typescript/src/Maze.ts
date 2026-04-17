import { type Cell } from './Cell.js';
import { CellType } from './CellType.js';

/**
 * Immutable rectangular grid of cells with exactly one start and one end.
 */
export class Maze {
  private readonly grid: CellType[][];
  readonly rows: number;
  readonly cols: number;
  readonly start: Cell;
  readonly end: Cell;

  constructor(grid: CellType[][], start: Cell, end: Cell) {
    this.grid = grid;
    this.rows = grid.length;
    this.cols = grid[0]?.length ?? 0;
    this.start = start;
    this.end = end;
  }

  /** Returns the cell type at the given position, or undefined if out of bounds. */
  cellTypeAt(row: number, col: number): CellType | undefined {
    if (row < 0 || row >= this.rows || col < 0 || col >= this.cols) {
      return undefined;
    }
    return this.grid[row]![col];
  }

  /** Returns true if the given cell is within bounds and not a wall. */
  isWalkable(cell: Cell): boolean {
    const type = this.cellTypeAt(cell.row, cell.col);
    return type !== undefined && type !== CellType.Wall;
  }
}
