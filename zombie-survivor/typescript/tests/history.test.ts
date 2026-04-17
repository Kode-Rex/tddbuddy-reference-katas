import { describe, it, expect } from 'vitest';
import { HistoryBuilder } from './HistoryBuilder.js';

describe('History', () => {
  it('New game records a game-started event', () => {
    const { game } = new HistoryBuilder().build();
    expect(game.history).toHaveLength(1);
    expect(game.history[0]!.description).toBe('Game started.');
  });

  it('Adding a survivor records a survivor-added event', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .build();
    expect(game.history.some(e => e.description === 'Survivor added: Alice.')).toBe(true);
  });

  it('Receiving a wound records a wound-received event', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withWound('Alice')
      .build();
    expect(game.history.some(e => e.description === 'Wound received: Alice.')).toBe(true);
  });

  it('Survivor death records a survivor-died event', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withWound('Alice')
      .withWound('Alice')
      .build();
    expect(game.history.some(e => e.description === 'Survivor died: Alice.')).toBe(true);
  });

  it('Leveling up records a level-up event', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withZombieKill('Alice', 7)
      .build();
    expect(game.history.some(e => e.description === 'Level up: Alice reached Yellow.')).toBe(true);
  });

  it('Game level change records a game-level-changed event', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withZombieKill('Alice', 7)
      .build();
    expect(game.history.some(e => e.description === 'Game level changed to Yellow.')).toBe(true);
  });

  it('Game ending records a game-ended event', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withWound('Alice')
      .withWound('Alice')
      .build();
    expect(game.history.some(e => e.description === 'Game ended.')).toBe(true);
  });
});
