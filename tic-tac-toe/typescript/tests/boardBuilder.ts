import { BOARD_SIZE, Board, type Cell } from '../src/board.js';

export class BoardBuilder {
  private readonly grid: Cell[][] = Array.from({ length: BOARD_SIZE }, () =>
    Array.from({ length: BOARD_SIZE }, () => 'Empty' as Cell),
  );

  withXAt(row: number, col: number): this { this.grid[row]![col] = 'X'; return this; }
  withOAt(row: number, col: number): this { this.grid[row]![col] = 'O'; return this; }

  build(): Board { return Board.fromGrid(this.grid); }
}
