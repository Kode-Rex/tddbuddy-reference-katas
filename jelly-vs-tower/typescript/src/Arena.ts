import { CombatLog } from './CombatLog.js';
import { calculateDamage } from './DamageTable.js';
import { Jelly } from './Jelly.js';
import { RandomSource } from './RandomSource.js';
import { Tower } from './Tower.js';

export class Arena {
  private readonly towers: readonly Tower[];
  private readonly jellies: readonly Jelly[];
  private readonly random: RandomSource;

  constructor(towers: readonly Tower[], jellies: readonly Jelly[], random: RandomSource) {
    this.towers = towers;
    this.jellies = jellies;
    this.random = random;
  }

  get aliveJellies(): Jelly[] {
    return this.jellies.filter(j => j.isAlive);
  }

  executeRound(): CombatLog[] {
    const logs: CombatLog[] = [];

    for (const tower of this.towers) {
      const target = this.jellies.find(j => j.isAlive);
      if (!target) break;

      const damage = calculateDamage(tower, target, this.random);
      target.takeDamage(damage);
      logs.push({ towerId: tower.id, jellyId: target.id, damage });
    }

    return logs;
  }
}
