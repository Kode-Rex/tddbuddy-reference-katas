import { type Cell } from '../src/Cell.js';
import { CellType } from '../src/CellType.js';
import { Maze } from '../src/Maze.js';
import {
  NoStartCellException,
  NoEndCellException,
  MultipleStartCellsException,
  MultipleEndCellsException,
} from '../src/MazeExceptions.js';

export class MazeBuilder {
  private layout = 'SE';

  withLayout(layout: string): this {
    this.layout = layout;
    return this;
  }

  build(): Maze {
    const lines = this.layout.split('\n');
    const rows = lines.length;
    const cols = Math.max(...lines.map((l) => l.length));
    const grid: CellType[][] = [];

    let start: Cell | undefined;
    let end: Cell | undefined;
    let startCount = 0;
    let endCount = 0;

    for (let r = 0; r < rows; r++) {
      const row: CellType[] = [];
      const line = lines[r]!;
      for (let c = 0; c < cols; c++) {
        const ch = c < line.length ? line[c] : '.';
        switch (ch) {
          case '#':
            row.push(CellType.Wall);
            break;
          case 'S':
            row.push(CellType.Start);
            start = { row: r, col: c };
            startCount++;
            break;
          case 'E':
            row.push(CellType.End);
            end = { row: r, col: c };
            endCount++;
            break;
          default:
            row.push(CellType.Open);
            break;
        }
      }
      grid.push(row);
    }

    if (startCount === 0) throw new NoStartCellException();
    if (startCount > 1) throw new MultipleStartCellsException();
    if (endCount === 0) throw new NoEndCellException();
    if (endCount > 1) throw new MultipleEndCellsException();

    return new Maze(grid, start!, end!);
  }
}
