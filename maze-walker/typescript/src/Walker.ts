import { type Cell, cellKey, parseKey, cardinalNeighbors } from './Cell.js';
import { type Maze } from './Maze.js';

/**
 * Navigates a maze using BFS to find the shortest path from start to end.
 */
export class Walker {
  private readonly maze: Maze;

  constructor(maze: Maze) {
    this.maze = maze;
  }

  /**
   * Finds the shortest path from start to end using BFS.
   * Returns an empty array when no path exists.
   */
  findPath(): Cell[] {
    const { start, end } = this.maze;
    const startKey = cellKey(start);
    const endKey = cellKey(end);

    const visited = new Set<string>([startKey]);
    const queue: string[] = [startKey];
    const cameFrom = new Map<string, string>();

    let head = 0;
    while (head < queue.length) {
      const currentKey = queue[head]!;
      head++;

      if (currentKey === endKey) {
        return this.reconstructPath(cameFrom, startKey, endKey);
      }

      const current = parseKey(currentKey);
      for (const neighbor of cardinalNeighbors(current)) {
        if (!this.maze.isWalkable(neighbor)) continue;

        const nk = cellKey(neighbor);
        if (visited.has(nk)) continue;

        visited.add(nk);
        cameFrom.set(nk, currentKey);
        queue.push(nk);
      }
    }

    return [];
  }

  private reconstructPath(
    cameFrom: Map<string, string>,
    startKey: string,
    endKey: string,
  ): Cell[] {
    const path: Cell[] = [];
    let current = endKey;

    while (current !== startKey) {
      path.push(parseKey(current));
      current = cameFrom.get(current)!;
    }

    path.push(parseKey(startKey));
    path.reverse();
    return path;
  }
}
