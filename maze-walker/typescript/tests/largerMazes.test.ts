import { MazeBuilder } from './MazeBuilder.js';
import { WalkerBuilder } from './WalkerBuilder.js';

describe('Larger Mazes', () => {
  it('walker solves a 5x5 maze', () => {
    const maze = new MazeBuilder()
      .withLayout('S.#..\n.#...\n...#.\n.#..E\n.....')
      .build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path.length).toBeGreaterThan(0);
    expect(path[0]).toEqual(maze.start);
    expect(path[path.length - 1]).toEqual(maze.end);
  });

  it('walker solves a maze requiring exploration', () => {
    const maze = new MazeBuilder()
      .withLayout('S..#.\n##...\n...#E')
      .build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path.length).toBeGreaterThan(0);
    expect(path[0]).toEqual(maze.start);
    expect(path[path.length - 1]).toEqual(maze.end);
  });
});
