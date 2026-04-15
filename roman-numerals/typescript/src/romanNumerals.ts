const lookup: Record<number, string> = {
  1: 'I',
  2: 'II',
  3: 'III',
};

export function toRoman(n: number): string {
  return lookup[n]!;
}
