export function isValid(input: string): boolean {
  if (input.length === 0) return false;

  const octets = input.split('.');
  if (octets.length !== 4) return false;

  for (let i = 0; i < 4; i++) {
    const value = parseOctet(octets[i]!);
    if (value === null) return false;
    if (i === 3 && (value === 0 || value === 255)) return false;
  }

  return true;
}

function parseOctet(octet: string): number | null {
  if (octet.length === 0) return null;
  if (octet.length > 1 && octet[0] === '0') return null;

  let value = 0;
  for (const c of octet) {
    if (c < '0' || c > '9') return null;
    value = value * 10 + (c.charCodeAt(0) - '0'.charCodeAt(0));
    if (value > 255) return null;
  }
  return value;
}
