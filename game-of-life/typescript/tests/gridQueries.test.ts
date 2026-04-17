import { GridBuilder } from './GridBuilder.js';

describe('grid queries', () => {
  it('isAlive returns true for a living cell', () => {
    const grid = new GridBuilder()
      .withLivingCellAt(3, 4)
      .build();

    expect(grid.isAlive(3, 4)).toBe(true);
  });

  it('isAlive returns false for a dead cell', () => {
    const grid = new GridBuilder()
      .withLivingCellAt(3, 4)
      .build();

    expect(grid.isAlive(0, 0)).toBe(false);
  });

  it('livingCells returns all living cells in the grid', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([1, 2], [3, 4])
      .build();

    expect(grid.livingCells()).toEqual(
      expect.arrayContaining([
        { row: 1, col: 2 },
        { row: 3, col: 4 },
      ]),
    );
    expect(grid.livingCells()).toHaveLength(2);
  });

  it('an empty grid has no living cells', () => {
    const grid = new GridBuilder().build();

    expect(grid.livingCells()).toEqual([]);
  });
});
