// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
export const BoardMessages = {
  cellOccupied: 'cell already occupied',
  outOfBounds: 'coordinates out of bounds',
  gameOver: 'game is already over',
} as const;

export const BOARD_SIZE = 3;

export type Cell = 'Empty' | 'X' | 'O';
export type Mark = 'X' | 'O';
export type Outcome = 'InProgress' | 'XWins' | 'OWins' | 'Draw';

export class CellOccupiedError extends Error {
  constructor() {
    super(BoardMessages.cellOccupied);
    this.name = 'CellOccupiedError';
  }
}

export class OutOfBoundsError extends Error {
  constructor() {
    super(BoardMessages.outOfBounds);
    this.name = 'OutOfBoundsError';
  }
}

export class GameOverError extends Error {
  constructor() {
    super(BoardMessages.gameOver);
    this.name = 'GameOverError';
  }
}

type Grid = readonly (readonly Cell[])[];

const WINNING_LINES: ReadonlyArray<ReadonlyArray<readonly [number, number]>> = [
  [[0, 0], [0, 1], [0, 2]],
  [[1, 0], [1, 1], [1, 2]],
  [[2, 0], [2, 1], [2, 2]],
  [[0, 0], [1, 0], [2, 0]],
  [[0, 1], [1, 1], [2, 1]],
  [[0, 2], [1, 2], [2, 2]],
  [[0, 0], [1, 1], [2, 2]],
  [[0, 2], [1, 1], [2, 0]],
];

function emptyGrid(): Cell[][] {
  return Array.from({ length: BOARD_SIZE }, () =>
    Array.from({ length: BOARD_SIZE }, () => 'Empty' as Cell),
  );
}

function inBounds(row: number, col: number): boolean {
  return row >= 0 && row < BOARD_SIZE && col >= 0 && col < BOARD_SIZE;
}

function countOf(grid: Grid, mark: Cell): number {
  let n = 0;
  for (let r = 0; r < BOARD_SIZE; r++)
    for (let c = 0; c < BOARD_SIZE; c++)
      if (grid[r]![c] === mark) n++;
  return n;
}

function computeOutcome(grid: Grid): Outcome {
  for (const line of WINNING_LINES) {
    const [a, b, c] = line;
    const first = grid[a![0]]![a![1]]!;
    if (first === 'Empty') continue;
    if (first === grid[b![0]]![b![1]] && first === grid[c![0]]![c![1]]) {
      return first === 'X' ? 'XWins' : 'OWins';
    }
  }
  return countOf(grid, 'Empty') === 0 ? 'Draw' : 'InProgress';
}

export class Board {
  private readonly grid: Grid;

  constructor(grid?: Grid) {
    this.grid = grid ?? emptyGrid();
  }

  cellAt(row: number, col: number): Cell {
    if (!inBounds(row, col)) throw new OutOfBoundsError();
    return this.grid[row]![col]!;
  }

  currentTurn(): Mark {
    const xs = countOf(this.grid, 'X');
    const os = countOf(this.grid, 'O');
    return xs === os ? 'X' : 'O';
  }

  outcome(): Outcome {
    return computeOutcome(this.grid);
  }

  place(row: number, col: number): Board {
    if (this.outcome() !== 'InProgress') throw new GameOverError();
    if (!inBounds(row, col)) throw new OutOfBoundsError();
    if (this.grid[row]![col] !== 'Empty') throw new CellOccupiedError();

    const next = this.grid.map((r) => r.slice());
    next[row]![col] = this.currentTurn();
    return new Board(next);
  }

  /** @internal — test-folder builder hook; production code should use `place`. */
  static fromGrid(grid: Grid): Board {
    return new Board(grid.map((r) => r.slice()));
  }
}
