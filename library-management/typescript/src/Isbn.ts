export class Isbn {
  constructor(public readonly value: string) {}
  equals(other: Isbn): boolean { return this.value === other.value; }
  toString(): string { return this.value; }
}
