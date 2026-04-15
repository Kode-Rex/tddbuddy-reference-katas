export function isBalanced(input: string): boolean {
  let depth = 0;
  for (const c of input) {
    if (c === '[') {
      depth++;
    } else if (c === ']') {
      depth--;
      if (depth < 0) return false;
    }
  }
  return depth === 0;
}
