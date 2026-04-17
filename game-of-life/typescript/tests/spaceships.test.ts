import { GridBuilder } from './GridBuilder.js';

describe('spaceships', () => {
  it('glider translates one cell down and right after four ticks', () => {
    const glider = new GridBuilder()
      .withLivingCellsAt([0, 1], [1, 2], [2, 0], [2, 1], [2, 2])
      .build();

    const after4 = glider.tick().tick().tick().tick();

    const expected = [
      { row: 1, col: 2 },
      { row: 2, col: 3 },
      { row: 3, col: 1 },
      { row: 3, col: 2 },
      { row: 3, col: 3 },
    ];
    expect(after4.livingCells()).toEqual(expect.arrayContaining(expected));
    expect(after4.livingCells()).toHaveLength(5);
  });
});
