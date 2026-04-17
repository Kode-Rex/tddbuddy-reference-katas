/** A coordinate on the board. */
export interface Position {
  readonly x: number;
  readonly y: number;
}

export function positionKey(p: Position): string {
  return `${p.x},${p.y}`;
}

export function positionsEqual(a: Position, b: Position): boolean {
  return a.x === b.x && a.y === b.y;
}
