export function score(rolls: number[]): number {
  let total = 0;
  let rollIndex = 0;
  for (let frameIndex = 0; frameIndex < 10; frameIndex++) {
    if (rolls[rollIndex] === 10) {
      total += 10 + rolls[rollIndex + 1]! + rolls[rollIndex + 2]!;
      rollIndex += 1;
    } else if (rolls[rollIndex]! + rolls[rollIndex + 1]! === 10) {
      total += 10 + rolls[rollIndex + 2]!;
      rollIndex += 2;
    } else {
      total += rolls[rollIndex]! + rolls[rollIndex + 1]!;
      rollIndex += 2;
    }
  }
  return total;
}
