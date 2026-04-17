import { MazeBuilder } from './MazeBuilder.js';
import { WalkerBuilder } from './WalkerBuilder.js';

describe('No Path', () => {
  it('a wall between start and end returns an empty path', () => {
    const maze = new MazeBuilder().withLayout('S#E').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toEqual([]);
  });

  it('a maze with no reachable end returns an empty path', () => {
    const maze = new MazeBuilder().withLayout('S.#\n.##\n##E').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toEqual([]);
  });
});
