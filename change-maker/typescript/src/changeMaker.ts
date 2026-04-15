export function makeChange(amount: number, denominations: readonly number[]): number[] {
  const coins: number[] = [];
  let remaining = amount;
  for (const coin of denominations) {
    while (remaining >= coin) {
      coins.push(coin);
      remaining -= coin;
    }
  }
  return coins;
}
