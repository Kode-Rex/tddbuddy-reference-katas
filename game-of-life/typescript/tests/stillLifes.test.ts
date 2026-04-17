import { GridBuilder } from './GridBuilder.js';

describe('still lifes', () => {
  it('block is stable after one tick', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1], [1, 0], [1, 1])
      .build();

    const next = grid.tick();

    expect(next.livingCells()).toEqual(
      expect.arrayContaining(grid.livingCells()),
    );
    expect(next.livingCells()).toHaveLength(grid.livingCells().length);
  });

  it('block remains stable after multiple ticks', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1], [1, 0], [1, 1])
      .build();

    const after5 = grid.tick().tick().tick().tick().tick();

    expect(after5.livingCells()).toEqual(
      expect.arrayContaining(grid.livingCells()),
    );
    expect(after5.livingCells()).toHaveLength(grid.livingCells().length);
  });
});
