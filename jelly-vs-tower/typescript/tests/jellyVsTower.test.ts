import { describe, expect, it } from 'vitest';
import { ColorType } from '../src/ColorType.js';
import { calculateDamage } from '../src/DamageTable.js';
import { InvalidHealthException } from '../src/InvalidHealthException.js';
import { InvalidLevelException } from '../src/InvalidLevelException.js';
import { Arena } from '../src/Arena.js';
import { TowerBuilder } from './TowerBuilder.js';
import { JellyBuilder } from './JellyBuilder.js';
import { FixedRandomSource } from './FixedRandomSource.js';

describe('Jelly vs Tower', () => {
  // ── Jelly Creation and Health ──────────────────────────────

  it('jelly starts alive with the given health', () => {
    const jelly = new JellyBuilder().withHealth(20).build();
    expect(jelly.health).toBe(20);
    expect(jelly.isAlive).toBe(true);
  });

  it('jelly with zero health is rejected', () => {
    expect(() => new JellyBuilder().withHealth(0).build())
      .toThrow(InvalidHealthException);
    expect(() => new JellyBuilder().withHealth(0).build())
      .toThrow('Health must be strictly positive, got 0');
  });

  it('jelly with negative health is rejected', () => {
    expect(() => new JellyBuilder().withHealth(-5).build())
      .toThrow(InvalidHealthException);
    expect(() => new JellyBuilder().withHealth(-5).build())
      .toThrow('Health must be strictly positive, got -5');
  });

  it('jelly dies when health reaches zero', () => {
    const jelly = new JellyBuilder().withHealth(5).build();
    jelly.takeDamage(5);
    expect(jelly.isAlive).toBe(false);
    expect(jelly.health).toBe(0);
  });

  it('jelly dies when health drops below zero', () => {
    const jelly = new JellyBuilder().withHealth(3).build();
    jelly.takeDamage(10);
    expect(jelly.isAlive).toBe(false);
    expect(jelly.health).toBe(0);
  });

  // ── Tower Creation and Validation ─────────────────────────

  it('tower is created with a color and level', () => {
    const tower = new TowerBuilder().withColor(ColorType.Red).withLevel(3).build();
    expect(tower.color).toBe(ColorType.Red);
    expect(tower.level).toBe(3);
  });

  it('tower with level below 1 is rejected', () => {
    expect(() => new TowerBuilder().withLevel(0).build())
      .toThrow(InvalidLevelException);
    expect(() => new TowerBuilder().withLevel(0).build())
      .toThrow('Tower level must be between 1 and 4, got 0');
  });

  it('tower with level above 4 is rejected', () => {
    expect(() => new TowerBuilder().withLevel(5).build())
      .toThrow(InvalidLevelException);
    expect(() => new TowerBuilder().withLevel(5).build())
      .toThrow('Tower level must be between 1 and 4, got 5');
  });

  // ── Damage Lookup — Blue Tower ────────────────────────────

  it('blue tower level 1 deals 2 to 5 damage to a blue jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.Blue).withLevel(1).build();
    const jelly = new JellyBuilder().withColor(ColorType.Blue).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(2))).toBe(2);
    expect(calculateDamage(tower, jelly, new FixedRandomSource(5))).toBe(5);
  });

  it('blue tower level 1 deals 0 damage to a red jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.Blue).withLevel(1).build();
    const jelly = new JellyBuilder().withColor(ColorType.Red).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(0))).toBe(0);
  });

  it('blue tower level 4 deals 12 to 15 damage to a blue jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.Blue).withLevel(4).build();
    const jelly = new JellyBuilder().withColor(ColorType.Blue).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(12))).toBe(12);
    expect(calculateDamage(tower, jelly, new FixedRandomSource(15))).toBe(15);
  });

  it('blue tower level 2 deals 1 damage to a red jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.Blue).withLevel(2).build();
    const jelly = new JellyBuilder().withColor(ColorType.Red).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(1))).toBe(1);
  });

  // ── Damage Lookup — Red Tower ─────────────────────────────

  it('red tower level 3 deals 9 to 12 damage to a red jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.Red).withLevel(3).build();
    const jelly = new JellyBuilder().withColor(ColorType.Red).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(9))).toBe(9);
    expect(calculateDamage(tower, jelly, new FixedRandomSource(12))).toBe(12);
  });

  it('red tower level 2 deals 1 damage to a blue jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.Red).withLevel(2).build();
    const jelly = new JellyBuilder().withColor(ColorType.Blue).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(1))).toBe(1);
  });

  it('red tower level 1 deals 0 damage to a blue jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.Red).withLevel(1).build();
    const jelly = new JellyBuilder().withColor(ColorType.Blue).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(0))).toBe(0);
  });

  // ── Damage Lookup — BlueRed Tower ─────────────────────────

  it('blueRed tower level 4 deals 6 to 8 damage to a blue jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.BlueRed).withLevel(4).build();
    const jelly = new JellyBuilder().withColor(ColorType.Blue).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(6))).toBe(6);
    expect(calculateDamage(tower, jelly, new FixedRandomSource(8))).toBe(8);
  });

  it('blueRed tower level 4 deals 6 to 8 damage to a red jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.BlueRed).withLevel(4).build();
    const jelly = new JellyBuilder().withColor(ColorType.Red).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(6))).toBe(6);
    expect(calculateDamage(tower, jelly, new FixedRandomSource(8))).toBe(8);
  });

  it('blueRed tower level 1 deals 2 damage to a blue jelly', () => {
    const tower = new TowerBuilder().withColor(ColorType.BlueRed).withLevel(1).build();
    const jelly = new JellyBuilder().withColor(ColorType.Blue).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(2))).toBe(2);
  });

  // ── BlueRed Jelly — Takes Higher Column ───────────────────

  it('blueRed jelly takes the higher of blue and red column damage', () => {
    const tower = new TowerBuilder().withColor(ColorType.Blue).withLevel(1).build();
    const jelly = new JellyBuilder().withColor(ColorType.BlueRed).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(3))).toBe(3);
  });

  it('blueRed jelly hit by blueRed tower uses both columns and takes the higher', () => {
    const tower = new TowerBuilder().withColor(ColorType.BlueRed).withLevel(2).build();
    const jelly = new JellyBuilder().withColor(ColorType.BlueRed).withHealth(100).build();
    expect(calculateDamage(tower, jelly, new FixedRandomSource(3))).toBe(3);
  });

  // ── Combat Flow ───────────────────────────────────────────

  it('tower attacks the first alive jelly and produces a combat log', () => {
    const tower = new TowerBuilder().withId('T1').withColor(ColorType.Blue).withLevel(1).build();
    const jelly = new JellyBuilder().withId('J1').withColor(ColorType.Blue).withHealth(20).build();
    const arena = new Arena([tower], [jelly], new FixedRandomSource(3));
    const logs = arena.executeRound();
    expect(logs).toHaveLength(1);
    expect(logs[0]!.towerId).toBe('T1');
    expect(logs[0]!.jellyId).toBe('J1');
    expect(logs[0]!.damage).toBe(3);
    expect(jelly.health).toBe(17);
  });

  it('dead jellies are skipped — tower attacks the next alive jelly', () => {
    const tower = new TowerBuilder().withId('T1').withColor(ColorType.Blue).withLevel(4).build();
    const deadJelly = new JellyBuilder().withId('J1').withColor(ColorType.Blue).withHealth(1).build();
    deadJelly.takeDamage(1);
    const aliveJelly = new JellyBuilder().withId('J2').withColor(ColorType.Blue).withHealth(20).build();
    const arena = new Arena([tower], [deadJelly, aliveJelly], new FixedRandomSource(12));
    const logs = arena.executeRound();
    expect(logs).toHaveLength(1);
    expect(logs[0]!.jellyId).toBe('J2');
  });

  it('tower attack does nothing when no alive jellies remain', () => {
    const tower = new TowerBuilder().withColor(ColorType.Blue).withLevel(1).build();
    const jelly = new JellyBuilder().withHealth(1).build();
    jelly.takeDamage(1);
    const arena = new Arena([tower], [jelly], new FixedRandomSource(3));
    const logs = arena.executeRound();
    expect(logs).toHaveLength(0);
  });

  it('multiple towers each attack in a single round', () => {
    const tower1 = new TowerBuilder().withId('T1').withColor(ColorType.Blue).withLevel(1).build();
    const tower2 = new TowerBuilder().withId('T2').withColor(ColorType.Red).withLevel(1).build();
    const jelly = new JellyBuilder().withId('J1').withColor(ColorType.Blue).withHealth(100).build();
    const arena = new Arena([tower1, tower2], [jelly], new FixedRandomSource(3));
    const logs = arena.executeRound();
    expect(logs).toHaveLength(2);
    expect(logs[0]!.towerId).toBe('T1');
    expect(logs[1]!.towerId).toBe('T2');
  });

  it('jelly killed during a round is removed before the next tower attacks', () => {
    const tower1 = new TowerBuilder().withId('T1').withColor(ColorType.Blue).withLevel(4).build();
    const tower2 = new TowerBuilder().withId('T2').withColor(ColorType.Blue).withLevel(1).build();
    const jelly1 = new JellyBuilder().withId('J1').withColor(ColorType.Blue).withHealth(1).build();
    const jelly2 = new JellyBuilder().withId('J2').withColor(ColorType.Blue).withHealth(20).build();
    const arena = new Arena([tower1, tower2], [jelly1, jelly2], new FixedRandomSource(12));
    const logs = arena.executeRound();
    expect(logs).toHaveLength(2);
    expect(logs[0]!.jellyId).toBe('J1');
    expect(logs[1]!.jellyId).toBe('J2');
    expect(jelly1.isAlive).toBe(false);
  });
});
