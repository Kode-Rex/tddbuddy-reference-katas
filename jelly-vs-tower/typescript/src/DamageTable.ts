import { ColorType } from './ColorType.js';
import { Jelly } from './Jelly.js';
import { Tower } from './Tower.js';
import { RandomSource } from './RandomSource.js';

interface DamageRange {
  min: number;
  max: number;
}

const table = new Map<string, DamageRange>();

function key(tower: ColorType, level: number, jelly: ColorType): string {
  return `${tower}:${level}:${jelly}`;
}

// Blue Tower
table.set(key(ColorType.Blue, 1, ColorType.Blue), { min: 2, max: 5 });
table.set(key(ColorType.Blue, 2, ColorType.Blue), { min: 5, max: 9 });
table.set(key(ColorType.Blue, 3, ColorType.Blue), { min: 9, max: 12 });
table.set(key(ColorType.Blue, 4, ColorType.Blue), { min: 12, max: 15 });
table.set(key(ColorType.Blue, 1, ColorType.Red), { min: 0, max: 0 });
table.set(key(ColorType.Blue, 2, ColorType.Red), { min: 1, max: 1 });
table.set(key(ColorType.Blue, 3, ColorType.Red), { min: 2, max: 2 });
table.set(key(ColorType.Blue, 4, ColorType.Red), { min: 3, max: 3 });

// Red Tower
table.set(key(ColorType.Red, 1, ColorType.Blue), { min: 0, max: 0 });
table.set(key(ColorType.Red, 2, ColorType.Blue), { min: 1, max: 1 });
table.set(key(ColorType.Red, 3, ColorType.Blue), { min: 2, max: 2 });
table.set(key(ColorType.Red, 4, ColorType.Blue), { min: 3, max: 3 });
table.set(key(ColorType.Red, 1, ColorType.Red), { min: 2, max: 5 });
table.set(key(ColorType.Red, 2, ColorType.Red), { min: 5, max: 9 });
table.set(key(ColorType.Red, 3, ColorType.Red), { min: 9, max: 12 });
table.set(key(ColorType.Red, 4, ColorType.Red), { min: 12, max: 15 });

// BlueRed Tower
table.set(key(ColorType.BlueRed, 1, ColorType.Blue), { min: 2, max: 2 });
table.set(key(ColorType.BlueRed, 2, ColorType.Blue), { min: 2, max: 4 });
table.set(key(ColorType.BlueRed, 3, ColorType.Blue), { min: 4, max: 6 });
table.set(key(ColorType.BlueRed, 4, ColorType.Blue), { min: 6, max: 8 });
table.set(key(ColorType.BlueRed, 1, ColorType.Red), { min: 2, max: 2 });
table.set(key(ColorType.BlueRed, 2, ColorType.Red), { min: 2, max: 4 });
table.set(key(ColorType.BlueRed, 3, ColorType.Red), { min: 4, max: 6 });
table.set(key(ColorType.BlueRed, 4, ColorType.Red), { min: 6, max: 8 });

function resolveDamage(tower: Tower, jellyColor: ColorType, random: RandomSource): number {
  const range = table.get(key(tower.color, tower.level, jellyColor));
  if (!range) throw new Error(`No damage entry for ${tower.color}:${tower.level}:${jellyColor}`);
  return range.min === range.max ? range.min : random.next(range.min, range.max);
}

export function calculateDamage(tower: Tower, jelly: Jelly, random: RandomSource): number {
  if (jelly.color === ColorType.BlueRed) {
    const blueDamage = resolveDamage(tower, ColorType.Blue, random);
    const redDamage = resolveDamage(tower, ColorType.Red, random);
    return Math.max(blueDamage, redDamage);
  }

  return resolveDamage(tower, jelly.color, random);
}
