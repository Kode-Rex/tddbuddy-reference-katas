import { describe, it, expect } from 'vitest';
import { Level } from '../src/Level.js';
import { SurvivorBuilder } from './SurvivorBuilder.js';

describe('Survivors', () => {
  it('New survivor has zero wounds', () => {
    const survivor = new SurvivorBuilder().build();
    expect(survivor.wounds).toBe(0);
  });

  it('New survivor has three actions per turn', () => {
    const survivor = new SurvivorBuilder().build();
    expect(survivor.actionsPerTurn).toBe(3);
  });

  it('New survivor is alive', () => {
    const survivor = new SurvivorBuilder().build();
    expect(survivor.isAlive).toBe(true);
  });

  it('New survivor starts at level Blue', () => {
    const survivor = new SurvivorBuilder().build();
    expect(survivor.level).toBe(Level.Blue);
  });

  it('Receiving a wound leaves the survivor alive with one wound', () => {
    const survivor = new SurvivorBuilder().build();
    survivor.receiveWound();
    expect(survivor.wounds).toBe(1);
    expect(survivor.isAlive).toBe(true);
  });

  it('Receiving a second wound kills the survivor', () => {
    const survivor = new SurvivorBuilder().withWounds(1).build();
    survivor.receiveWound();
    expect(survivor.isAlive).toBe(false);
    expect(survivor.wounds).toBe(2);
  });

  it('Wounding a dead survivor has no effect', () => {
    const survivor = new SurvivorBuilder().withWounds(2).build();
    survivor.receiveWound();
    expect(survivor.wounds).toBe(2);
  });
});
