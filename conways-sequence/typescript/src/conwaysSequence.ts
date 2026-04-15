export function nextTerm(term: string): string {
  let output = '';
  let i = 0;
  while (i < term.length) {
    const digit = term[i];
    let runLength = 1;
    while (i + runLength < term.length && term[i + runLength] === digit) {
      runLength++;
    }
    output += String(runLength) + digit;
    i += runLength;
  }
  return output;
}

export function lookAndSay(seed: string, iterations: number): string {
  if (iterations < 0) {
    throw new Error('iterations must be non-negative');
  }
  let term = seed;
  for (let step = 0; step < iterations; step++) {
    term = nextTerm(term);
  }
  return term;
}
