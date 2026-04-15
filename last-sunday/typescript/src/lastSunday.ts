export function find(year: number, month: number): Date {
  const lastDay = new Date(Date.UTC(year, month, 0));
  const daysToWalkBack = (lastDay.getUTCDay() - 0 + 7) % 7;
  lastDay.setUTCDate(lastDay.getUTCDate() - daysToWalkBack);
  return lastDay;
}
