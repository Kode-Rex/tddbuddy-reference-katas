import { MazeBuilder } from './MazeBuilder.js';
import { WalkerBuilder } from './WalkerBuilder.js';

describe('Shortest Path', () => {
  it('walker finds the shortest path around a wall', () => {
    const maze = new MazeBuilder().withLayout('S.\n#.\nE.').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toEqual([
      { row: 0, col: 0 },
      { row: 0, col: 1 },
      { row: 1, col: 1 },
      { row: 2, col: 1 },
      { row: 2, col: 0 },
    ]);
  });

  it('walker picks the shortest of two possible routes', () => {
    const maze = new MazeBuilder().withLayout('S.E\n...').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toHaveLength(3);
    expect(path[0]).toEqual({ row: 0, col: 0 });
    expect(path[2]).toEqual({ row: 0, col: 2 });
  });

  it('walker navigates a winding corridor', () => {
    const maze = new MazeBuilder().withLayout('S.#\n.#.\n..E').build();
    const walker = new WalkerBuilder().withMaze(maze).build();

    const path = walker.findPath();

    expect(path).toEqual([
      { row: 0, col: 0 },
      { row: 1, col: 0 },
      { row: 2, col: 0 },
      { row: 2, col: 1 },
      { row: 2, col: 2 },
    ]);
  });
});
