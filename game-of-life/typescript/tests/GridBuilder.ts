import { type Cell } from '../src/Cell.js';
import { Grid } from '../src/Grid.js';

export class GridBuilder {
  private readonly livingCells: Cell[] = [];

  withLivingCellAt(row: number, col: number): this {
    this.livingCells.push({ row, col });
    return this;
  }

  withLivingCellsAt(...cells: [number, number][]): this {
    for (const [row, col] of cells) {
      this.livingCells.push({ row, col });
    }
    return this;
  }

  build(): Grid {
    return new Grid(this.livingCells);
  }
}
