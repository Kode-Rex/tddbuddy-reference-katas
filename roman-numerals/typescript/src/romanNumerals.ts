const mapping: ReadonlyArray<readonly [number, string]> = [
  [5, 'V'],
  [4, 'IV'],
  [1, 'I'],
];

export function toRoman(n: number): string {
  let result = '';
  for (const [value, symbol] of mapping) {
    while (n >= value) {
      result += symbol;
      n -= value;
    }
  }
  return result;
}
