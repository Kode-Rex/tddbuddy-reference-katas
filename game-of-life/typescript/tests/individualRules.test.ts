import { GridBuilder } from './GridBuilder.js';

describe('individual rules', () => {
  it('a live cell with zero neighbors dies', () => {
    const grid = new GridBuilder()
      .withLivingCellAt(5, 5)
      .build();

    const next = grid.tick();

    expect(next.isAlive(5, 5)).toBe(false);
  });

  it('a live cell with one neighbor dies', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1])
      .build();

    const next = grid.tick();

    expect(next.isAlive(0, 0)).toBe(false);
    expect(next.isAlive(0, 1)).toBe(false);
  });

  it('a live cell with two neighbors survives', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1], [0, 2])
      .build();

    const next = grid.tick();

    expect(next.isAlive(0, 1)).toBe(true);
  });

  it('a live cell with three neighbors survives', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1], [1, 0], [1, 1])
      .build();

    const next = grid.tick();

    expect(next.isAlive(0, 0)).toBe(true);
  });

  it('a live cell with four neighbors dies', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([0, 1], [1, 0], [1, 1], [1, 2], [2, 1])
      .build();

    const next = grid.tick();

    expect(next.isAlive(1, 1)).toBe(false);
  });

  it('a dead cell with exactly three neighbors becomes alive', () => {
    const grid = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1], [1, 0])
      .build();

    const next = grid.tick();

    expect(next.isAlive(1, 1)).toBe(true);
  });
});
