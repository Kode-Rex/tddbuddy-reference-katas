export class NoStartCellException extends Error {
  constructor() {
    super('Maze must have exactly one start cell.');
    this.name = 'NoStartCellException';
  }
}

export class NoEndCellException extends Error {
  constructor() {
    super('Maze must have exactly one end cell.');
    this.name = 'NoEndCellException';
  }
}

export class MultipleStartCellsException extends Error {
  constructor() {
    super('Maze must have exactly one start cell.');
    this.name = 'MultipleStartCellsException';
  }
}

export class MultipleEndCellsException extends Error {
  constructor() {
    super('Maze must have exactly one end cell.');
    this.name = 'MultipleEndCellsException';
  }
}
