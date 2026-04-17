import { GridBuilder } from './GridBuilder.js';

describe('empty and trivial', () => {
  it('an empty grid ticks to an empty grid', () => {
    const grid = new GridBuilder().build();

    const next = grid.tick();

    expect(next.livingCells()).toEqual([]);
  });

  it('a single living cell dies after one tick', () => {
    const grid = new GridBuilder()
      .withLivingCellAt(0, 0)
      .build();

    const next = grid.tick();

    expect(next.isAlive(0, 0)).toBe(false);
  });
});
