import {
  DEFAULT_GRID_WIDTH,
  DEFAULT_GRID_HEIGHT,
  type Direction,
  Rover,
} from '../src/rover.js';

export class RoverBuilder {
  private x = 0;
  private y = 0;
  private heading: Direction = 'North';
  private gridWidth = DEFAULT_GRID_WIDTH;
  private gridHeight = DEFAULT_GRID_HEIGHT;
  private readonly obstacles = new Set<string>();

  at(x: number, y: number): this { this.x = x; this.y = y; return this; }
  facing(heading: Direction): this { this.heading = heading; return this; }
  onGrid(width: number, height: number): this { this.gridWidth = width; this.gridHeight = height; return this; }
  withObstacleAt(x: number, y: number): this { this.obstacles.add(`${x},${y}`); return this; }

  build(): Rover {
    return new Rover({
      x: this.x, y: this.y, heading: this.heading,
      gridWidth: this.gridWidth, gridHeight: this.gridHeight,
      obstacles: new Set(this.obstacles),
      status: 'Moving', lastObstacle: null,
    });
  }
}
