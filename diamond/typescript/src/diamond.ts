const A_CODE = 'A'.charCodeAt(0);
const Z_CODE = 'Z'.charCodeAt(0);

export function print(letter: string): string {
  if (letter.length !== 1) {
    throw new Error('letter must be a single A-Z character');
  }
  const normalized = letter.toUpperCase();
  const code = normalized.charCodeAt(0);
  if (code < A_CODE || code > Z_CODE) {
    throw new Error('letter must be a single A-Z character');
  }

  const n = code - A_CODE;
  const rows: string[] = [];
  for (let r = 0; r <= 2 * n; r++) {
    const offset = r <= n ? r : 2 * n - r;
    rows.push(buildRow(offset, n));
  }
  return rows.join('\n');
}

function buildRow(offset: number, n: number): string {
  const rowLetter = String.fromCharCode(A_CODE + offset);
  const leading = ' '.repeat(n - offset);
  if (offset === 0) {
    return leading + rowLetter;
  }
  const inner = ' '.repeat(2 * offset - 1);
  return leading + rowLetter + inner + rowLetter;
}
