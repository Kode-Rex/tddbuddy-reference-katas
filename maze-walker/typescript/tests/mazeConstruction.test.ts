import { CellType } from '../src/CellType.js';
import {
  NoStartCellException,
  NoEndCellException,
  MultipleStartCellsException,
  MultipleEndCellsException,
} from '../src/MazeExceptions.js';
import { MazeBuilder } from './MazeBuilder.js';

describe('Maze Construction', () => {
  it('a maze can be built from a string-art representation', () => {
    const maze = new MazeBuilder().withLayout('S.E').build();

    expect(maze.rows).toBe(1);
    expect(maze.cols).toBe(3);
    expect(maze.start).toEqual({ row: 0, col: 0 });
    expect(maze.end).toEqual({ row: 0, col: 2 });
  });

  it('a maze identifies walls correctly', () => {
    const maze = new MazeBuilder().withLayout('S#E').build();

    expect(maze.cellTypeAt(0, 1)).toBe(CellType.Wall);
  });

  it('a maze without a start cell throws NoStartCellException', () => {
    expect(() => new MazeBuilder().withLayout('..E').build()).toThrow(
      NoStartCellException,
    );
    expect(() => new MazeBuilder().withLayout('..E').build()).toThrow(
      'Maze must have exactly one start cell.',
    );
  });

  it('a maze without an end cell throws NoEndCellException', () => {
    expect(() => new MazeBuilder().withLayout('S..').build()).toThrow(
      NoEndCellException,
    );
    expect(() => new MazeBuilder().withLayout('S..').build()).toThrow(
      'Maze must have exactly one end cell.',
    );
  });

  it('a maze with multiple start cells throws MultipleStartCellsException', () => {
    expect(() => new MazeBuilder().withLayout('S.S\n..E').build()).toThrow(
      MultipleStartCellsException,
    );
    expect(() => new MazeBuilder().withLayout('S.S\n..E').build()).toThrow(
      'Maze must have exactly one start cell.',
    );
  });

  it('a maze with multiple end cells throws MultipleEndCellsException', () => {
    expect(() => new MazeBuilder().withLayout('S.E\n..E').build()).toThrow(
      MultipleEndCellsException,
    );
    expect(() => new MazeBuilder().withLayout('S.E\n..E').build()).toThrow(
      'Maze must have exactly one end cell.',
    );
  });
});
