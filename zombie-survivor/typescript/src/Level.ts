export enum Level {
  Blue = 'Blue',
  Yellow = 'Yellow',
  Orange = 'Orange',
  Red = 'Red',
}

export function levelFor(xp: number): Level {
  if (xp >= 43) return Level.Red;
  if (xp >= 19) return Level.Orange;
  if (xp >= 7) return Level.Yellow;
  return Level.Blue;
}

const LevelOrder: Record<Level, number> = {
  [Level.Blue]: 0,
  [Level.Yellow]: 1,
  [Level.Orange]: 2,
  [Level.Red]: 3,
};

export function maxLevel(a: Level, b: Level): Level {
  return LevelOrder[a] >= LevelOrder[b] ? a : b;
}
