export function generate(n: number): number[] {
  const factors: number[] = [];
  while (n % 2 === 0) {
    factors.push(2);
    n /= 2;
  }
  if (n > 1) factors.push(n);
  return factors;
}
