export function trim(input: string): string {
  let result = '';
  let lineStart = 0;
  let i = 0;
  while (i < input.length) {
    const ch = input[i];
    if (ch === '\n') {
      result += rightTrim(input, lineStart, i) + '\n';
      lineStart = i + 1;
      i++;
    } else if (ch === '\r' && input[i + 1] === '\n') {
      result += rightTrim(input, lineStart, i) + '\r\n';
      lineStart = i + 2;
      i += 2;
    } else {
      i++;
    }
  }
  result += rightTrim(input, lineStart, input.length);
  return result;
}

function rightTrim(input: string, start: number, end: number): string {
  let trimmedEnd = end;
  while (trimmedEnd > start) {
    const c = input[trimmedEnd - 1];
    if (c !== ' ' && c !== '\t') break;
    trimmedEnd--;
  }
  return input.slice(start, trimmedEnd);
}
