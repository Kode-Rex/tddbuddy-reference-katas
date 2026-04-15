import { parse } from './delimiterParser';

export function add(numbers: string): number {
  if (numbers === '') return 0;
  const { delimiter, body } = parse(numbers);
  const parsed = body.split(delimiter).map((t) => parseInt(t, 10));
  const negatives = parsed.filter((n) => n < 0);
  if (negatives.length > 0) {
    throw new Error(`negatives not allowed: ${negatives.join(', ')}`);
  }
  return parsed.reduce((sum, n) => sum + n, 0);
}
