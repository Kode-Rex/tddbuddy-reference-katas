import { describe, it, expect } from 'vitest';
import { Survivor } from '../src/Survivor.js';
import { DuplicateSurvivorNameException } from '../src/Exceptions.js';
import { HistoryBuilder } from './HistoryBuilder.js';

describe('Game', () => {
  it('New game starts with zero survivors', () => {
    const { game } = new HistoryBuilder().build();
    expect(game.survivors.length).toBe(0);
  });

  it('Adding a survivor increases the survivor count', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .build();
    expect(game.survivors.length).toBe(1);
  });

  it('Adding a survivor with a duplicate name is rejected', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .build();
    expect(() => game.addSurvivor(new Survivor('Alice')))
      .toThrow(DuplicateSurvivorNameException);
    expect(() => game.addSurvivor(new Survivor('Alice')))
      .toThrow("A survivor named 'Alice' already exists.");
  });

  it('Game ends when all survivors are dead', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withWound('Alice')
      .withWound('Alice')
      .build();
    expect(game.hasEnded).toBe(true);
  });
});
