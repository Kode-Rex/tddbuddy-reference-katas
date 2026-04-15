export function greet(name: string | null | ReadonlyArray<string | null>): string {
  const names = Array.isArray(name) ? name.slice() : [name];
  const resolved = names.map((n) => (n == null ? 'my friend' : n));

  const normals: string[] = [];
  const shouts: string[] = [];
  for (const n of resolved) {
    if (isShout(n)) shouts.push(n);
    else normals.push(n);
  }

  if (shouts.length === 0) return normalGreeting(normals);
  if (normals.length === 0) return shoutGreeting(shouts);
  return `${normalGreeting(normals)}. AND ${shoutGreeting(shouts)}`;
}

function normalGreeting(names: string[]): string {
  if (names.length === 1) return `Hello, ${names[0]}.`;
  if (names.length === 2) return `Hello, ${names[0]} and ${names[1]}`;
  const head = names.slice(0, -1).join(', ');
  return `Hello, ${head}, and ${names[names.length - 1]}`;
}

function shoutGreeting(names: string[]): string {
  return `HELLO ${names.join(' ')}!`;
}

function isShout(name: string): boolean {
  return /[A-Za-z]/.test(name) && name === name.toUpperCase();
}
