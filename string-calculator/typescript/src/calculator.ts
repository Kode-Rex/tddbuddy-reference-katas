import { parse } from './delimiterParser';

export function add(numbers: string): number {
  if (numbers === '') return 0;
  const { delimiter, body } = parse(numbers);
  return body.split(delimiter).reduce((sum, token) => sum + parseInt(token, 10), 0);
}
