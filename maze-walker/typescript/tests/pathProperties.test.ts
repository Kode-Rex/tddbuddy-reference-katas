import { CellType } from '../src/CellType.js';
import { MazeBuilder } from './MazeBuilder.js';
import { WalkerBuilder } from './WalkerBuilder.js';

function solveMaze(layout: string) {
  const maze = new MazeBuilder().withLayout(layout).build();
  const walker = new WalkerBuilder().withMaze(maze).build();
  return { maze, path: walker.findPath() };
}

describe('Path Properties', () => {
  it('the path starts at the start cell', () => {
    const { maze, path } = solveMaze('S..E');

    expect(path[0]).toEqual(maze.start);
  });

  it('the path ends at the end cell', () => {
    const { maze, path } = solveMaze('S..E');

    expect(path[path.length - 1]).toEqual(maze.end);
  });

  it('each step in the path is to an adjacent cell', () => {
    const { path } = solveMaze('S.#\n..#\n..E');

    for (let i = 1; i < path.length; i++) {
      const dr = Math.abs(path[i]!.row - path[i - 1]!.row);
      const dc = Math.abs(path[i]!.col - path[i - 1]!.col);
      expect(dr + dc).toBe(1);
    }
  });

  it('the path contains no walls', () => {
    const { maze, path } = solveMaze('S.#\n..#\n..E');

    for (const cell of path) {
      expect(maze.cellTypeAt(cell.row, cell.col)).not.toBe(CellType.Wall);
    }
  });
});
