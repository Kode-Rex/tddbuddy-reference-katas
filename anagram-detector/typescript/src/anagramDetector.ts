function key(s: string): string {
  return s
    .toLowerCase()
    .split('')
    .filter((c) => c >= 'a' && c <= 'z')
    .sort()
    .join('');
}

export function areAnagrams(a: string, b: string): boolean {
  if (a === b) return false;
  const keyA = key(a);
  const keyB = key(b);
  if (keyA.length === 0 || keyB.length === 0) return false;
  return keyA === keyB;
}

export function findAnagrams(subject: string, candidates: readonly string[]): string[] {
  return candidates.filter((candidate) => areAnagrams(subject, candidate));
}

export function groupAnagrams(words: readonly string[]): string[][] {
  const keyToGroup = new Map<string, string[]>();
  const order: string[] = [];
  for (const word of words) {
    const k = key(word);
    let group = keyToGroup.get(k);
    if (group === undefined) {
      group = [];
      keyToGroup.set(k, group);
      order.push(k);
    }
    group.push(word);
  }
  return order.map((k) => keyToGroup.get(k)!);
}
