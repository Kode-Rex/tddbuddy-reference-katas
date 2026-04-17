import { describe, it, expect } from 'vitest';
import { Level } from '../src/Level.js';
import { SurvivorBuilder } from './SurvivorBuilder.js';
import { HistoryBuilder } from './HistoryBuilder.js';

describe('Experience and Levels', () => {
  it('Killing a zombie awards one experience point', () => {
    const survivor = new SurvivorBuilder().build();
    survivor.killZombie();
    expect(survivor.experience).toBe(1);
  });

  it('Survivor with seven experience is level Yellow', () => {
    const survivor = new SurvivorBuilder().withZombieKills(7).build();
    expect(survivor.level).toBe(Level.Yellow);
  });

  it('Survivor with nineteen experience is level Orange', () => {
    const survivor = new SurvivorBuilder().withZombieKills(19).build();
    expect(survivor.level).toBe(Level.Orange);
  });

  it('Survivor with forty-three experience is level Red', () => {
    const survivor = new SurvivorBuilder().withZombieKills(43).build();
    expect(survivor.level).toBe(Level.Red);
  });

  it('Game level matches the highest level among living survivors', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withSurvivor('Bob')
      .withZombieKill('Alice', 7)
      .build();
    expect(game.gameLevel).toBe(Level.Yellow);
  });
});
