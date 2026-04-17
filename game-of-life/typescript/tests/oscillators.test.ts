import { GridBuilder } from './GridBuilder.js';

describe('oscillators', () => {
  it('horizontal blinker becomes vertical after one tick', () => {
    const horizontal = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1], [0, 2])
      .build();

    const vertical = horizontal.tick();

    const expected = [
      { row: -1, col: 1 },
      { row: 0, col: 1 },
      { row: 1, col: 1 },
    ];
    expect(vertical.livingCells()).toEqual(expect.arrayContaining(expected));
    expect(vertical.livingCells()).toHaveLength(3);
  });

  it('vertical blinker becomes horizontal after one tick', () => {
    const vertical = new GridBuilder()
      .withLivingCellsAt([-1, 1], [0, 1], [1, 1])
      .build();

    const horizontal = vertical.tick();

    const expected = [
      { row: 0, col: 0 },
      { row: 0, col: 1 },
      { row: 0, col: 2 },
    ];
    expect(horizontal.livingCells()).toEqual(expect.arrayContaining(expected));
    expect(horizontal.livingCells()).toHaveLength(3);
  });

  it('blinker returns to its original state after two ticks', () => {
    const original = new GridBuilder()
      .withLivingCellsAt([0, 0], [0, 1], [0, 2])
      .build();

    const afterTwo = original.tick().tick();

    expect(afterTwo.livingCells()).toEqual(
      expect.arrayContaining(original.livingCells()),
    );
    expect(afterTwo.livingCells()).toHaveLength(original.livingCells().length);
  });
});
