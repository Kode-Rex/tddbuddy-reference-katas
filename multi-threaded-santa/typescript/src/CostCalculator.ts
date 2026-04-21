/**
 * Calculates the cookie cost: elves x elapsed seconds = cookies.
 */
export function calculateCookies(
  elfCount: number,
  elapsedSeconds: number,
): number {
  return elfCount * elapsedSeconds;
}
