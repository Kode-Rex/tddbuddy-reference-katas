import { describe, it, expect } from 'vitest';
import { Skill } from '../src/Skill.js';
import { SurvivorBuilder } from './SurvivorBuilder.js';
import { HistoryBuilder } from './HistoryBuilder.js';

describe('Skills', () => {
  it('Reaching Yellow unlocks plus-one-action as the mandatory first skill', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withZombieKill('Alice', 7)
      .build();
    const alice = game.survivors.find(s => s.name === 'Alice')!;
    expect(alice.skills).toContain(Skill.PlusOneAction);
  });

  it('Plus-one-action skill increases actions to four', () => {
    const { game } = new HistoryBuilder()
      .withSurvivor('Alice')
      .withZombieKill('Alice', 7)
      .build();
    const alice = game.survivors.find(s => s.name === 'Alice')!;
    expect(alice.actionsPerTurn).toBe(4);
  });

  it('Hoard skill increases equipment capacity by one', () => {
    const survivor = new SurvivorBuilder().build();
    survivor.unlockSkill(Skill.Hoard);
    expect(survivor.equipmentCapacity).toBe(6);
  });
});
