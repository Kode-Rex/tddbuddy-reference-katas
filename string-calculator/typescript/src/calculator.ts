export function add(numbers: string): number {
  if (numbers === '') return 0;

  let delimiter: RegExp = /[,\n]/;
  let body = numbers;

  if (numbers.startsWith('//')) {
    const headerEnd = numbers.indexOf('\n');
    const header = numbers.substring(2, headerEnd);
    delimiter = new RegExp(header);
    body = numbers.substring(headerEnd + 1);
  }

  return body.split(delimiter).reduce((sum, token) => sum + parseInt(token, 10), 0);
}
