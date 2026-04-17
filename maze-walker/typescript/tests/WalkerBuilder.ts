import { type Maze } from '../src/Maze.js';
import { Walker } from '../src/Walker.js';
import { MazeBuilder } from './MazeBuilder.js';

export class WalkerBuilder {
  private maze: Maze = new MazeBuilder().build();

  withMaze(maze: Maze): this {
    this.maze = maze;
    return this;
  }

  build(): Walker {
    return new Walker(this.maze);
  }
}
