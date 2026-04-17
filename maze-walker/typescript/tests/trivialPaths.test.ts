import { MazeBuilder } from './MazeBuilder.js';
import { WalkerBuilder } from './WalkerBuilder.js';

describe('Trivial Paths', () => {
  it('start adjacent to end finds a two-cell path', () => {
    const maze = new MazeBuilder().withLayout('SE').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toEqual([
      { row: 0, col: 0 },
      { row: 0, col: 1 },
    ]);
  });

  it('a straight horizontal corridor finds the path', () => {
    const maze = new MazeBuilder().withLayout('S..E').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toEqual([
      { row: 0, col: 0 },
      { row: 0, col: 1 },
      { row: 0, col: 2 },
      { row: 0, col: 3 },
    ]);
  });

  it('a straight vertical corridor finds the path', () => {
    const maze = new MazeBuilder().withLayout('S\n.\nE').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toEqual([
      { row: 0, col: 0 },
      { row: 1, col: 0 },
      { row: 2, col: 0 },
    ]);
  });
});
