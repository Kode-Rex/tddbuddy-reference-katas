// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
export const RoverMessages = {
  unknownCommand: 'unknown command',
} as const;

export const DEFAULT_GRID_WIDTH = 100;
export const DEFAULT_GRID_HEIGHT = 100;

export type Direction = 'North' | 'East' | 'South' | 'West';
export type Command = 'Forward' | 'Backward' | 'Left' | 'Right';
export type MovementStatus = 'Moving' | 'Blocked';

export type Coord = readonly [number, number];

export class UnknownCommandError extends Error {
  constructor() {
    super(RoverMessages.unknownCommand);
    this.name = 'UnknownCommandError';
  }
}

const LEFT_OF: Record<Direction, Direction> = {
  North: 'West',
  West: 'South',
  South: 'East',
  East: 'North',
};

const RIGHT_OF: Record<Direction, Direction> = {
  North: 'East',
  East: 'South',
  South: 'West',
  West: 'North',
};

const STEP_OF: Record<Direction, Coord> = {
  North: [0, -1],
  South: [0, 1],
  East: [1, 0],
  West: [-1, 0],
};

function parseCommand(ch: string): Command {
  switch (ch) {
    case 'F': return 'Forward';
    case 'B': return 'Backward';
    case 'L': return 'Left';
    case 'R': return 'Right';
    default: throw new UnknownCommandError();
  }
}

function mod(value: number, modulus: number): number {
  return ((value % modulus) + modulus) % modulus;
}

export interface RoverParams {
  x: number;
  y: number;
  heading: Direction;
  gridWidth: number;
  gridHeight: number;
  obstacles: ReadonlySet<string>; // encoded "x,y"
  status: MovementStatus;
  lastObstacle: Coord | null;
}

function encodeObstacle(x: number, y: number): string {
  return `${x},${y}`;
}

export class Rover {
  readonly x: number;
  readonly y: number;
  readonly heading: Direction;
  readonly gridWidth: number;
  readonly gridHeight: number;
  readonly status: MovementStatus;
  readonly lastObstacle: Coord | null;
  private readonly obstacles: ReadonlySet<string>;

  constructor(params: RoverParams) {
    this.x = params.x;
    this.y = params.y;
    this.heading = params.heading;
    this.gridWidth = params.gridWidth;
    this.gridHeight = params.gridHeight;
    this.obstacles = params.obstacles;
    this.status = params.status;
    this.lastObstacle = params.lastObstacle;
  }

  get position(): Coord {
    return [this.x, this.y];
  }

  hasObstacleAt(x: number, y: number): boolean {
    return this.obstacles.has(encodeObstacle(x, y));
  }

  execute(commands: string): Rover {
    let x = this.x;
    let y = this.y;
    let heading = this.heading;
    let status = this.status;
    let lastObstacle = this.lastObstacle;

    for (const ch of commands) {
      const cmd = parseCommand(ch);
      if (status === 'Blocked') break;

      if (cmd === 'Left') {
        heading = LEFT_OF[heading];
      } else if (cmd === 'Right') {
        heading = RIGHT_OF[heading];
      } else {
        const sign = cmd === 'Forward' ? 1 : -1;
        const [dx, dy] = STEP_OF[heading];
        const nx = mod(x + dx * sign, this.gridWidth);
        const ny = mod(y + dy * sign, this.gridHeight);
        if (this.obstacles.has(encodeObstacle(nx, ny))) {
          status = 'Blocked';
          lastObstacle = [nx, ny];
        } else {
          x = nx;
          y = ny;
        }
      }
    }

    return new Rover({
      x, y, heading,
      gridWidth: this.gridWidth, gridHeight: this.gridHeight,
      obstacles: this.obstacles,
      status, lastObstacle,
    });
  }
}
