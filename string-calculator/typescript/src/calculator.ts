export function add(numbers: string): number {
  if (numbers === '') return 0;
  return numbers.split(/[,\n]/).reduce((sum, token) => sum + parseInt(token, 10), 0);
}
