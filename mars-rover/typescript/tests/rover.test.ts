import { describe, it, expect } from 'vitest';
import { UnknownCommandError, type Direction } from '../src/rover.js';
import { RoverBuilder } from './roverBuilder.js';
import { CommandBuilder } from './commandBuilder.js';

describe('Rover', () => {
  it('new rover reports its starting position, heading, and Moving status', () => {
    const rover = new RoverBuilder()
      .at(3, 4).facing('North').onGrid(100, 100).build();

    expect(rover.position).toEqual([3, 4]);
    expect(rover.heading).toBe('North');
    expect(rover.status).toBe('Moving');
    expect(rover.lastObstacle).toBeNull();
  });

  it('forward moves one square in the direction the rover is facing', () => {
    const make = (d: Direction) =>
      new RoverBuilder().at(5, 5).facing(d).onGrid(100, 100).build().execute('F');

    expect(make('North').position).toEqual([5, 4]);
    expect(make('East').position).toEqual([6, 5]);
    expect(make('South').position).toEqual([5, 6]);
    expect(make('West').position).toEqual([4, 5]);
  });

  it('backward moves one square opposite the heading', () => {
    const north = new RoverBuilder().at(5, 5).facing('North').onGrid(100, 100).build().execute('B');
    const east = new RoverBuilder().at(5, 5).facing('East').onGrid(100, 100).build().execute('B');

    expect(north.position).toEqual([5, 6]);
    expect(east.position).toEqual([4, 5]);
  });

  it('left rotates the heading counter-clockwise', () => {
    const start = new RoverBuilder().at(5, 5).facing('North').onGrid(100, 100).build();

    expect(start.execute('L').heading).toBe('West');
    expect(start.execute('LL').heading).toBe('South');
    expect(start.execute('LLL').heading).toBe('East');
    expect(start.execute('LLLL').heading).toBe('North');
    expect(start.execute('L').position).toEqual([5, 5]);
  });

  it('right rotates the heading clockwise', () => {
    const start = new RoverBuilder().at(5, 5).facing('North').onGrid(100, 100).build();

    expect(start.execute('R').heading).toBe('East');
    expect(start.execute('RR').heading).toBe('South');
    expect(start.execute('RRR').heading).toBe('West');
    expect(start.execute('RRRR').heading).toBe('North');
    expect(start.execute('R').position).toEqual([5, 5]);
  });

  it('execute applies commands in order', () => {
    const rover = new RoverBuilder().at(0, 0).facing('North').onGrid(100, 100).build();

    const after = rover.execute('FFRFF');

    expect(after.position).toEqual([2, 98]);
    expect(after.heading).toBe('East');
  });

  it('kata-brief example: (0,0) facing South FFLFF lands at (2,2)', () => {
    const rover = new RoverBuilder().at(0, 0).facing('South').onGrid(100, 100).build();

    const after = rover.execute('FFLFF');

    expect(after.position).toEqual([2, 2]);
    expect(after.heading).toBe('East');
  });

  it('wrapping across the north edge', () => {
    const rover = new RoverBuilder().at(0, 0).facing('North').onGrid(100, 100).build();
    expect(rover.execute('F').position).toEqual([0, 99]);
  });

  it('wrapping across the east edge', () => {
    const rover = new RoverBuilder().at(99, 50).facing('East').onGrid(100, 100).build();
    expect(rover.execute('F').position).toEqual([0, 50]);
  });

  it('obstacle blocks a forward move', () => {
    const rover = new RoverBuilder()
      .at(0, 0).facing('East').onGrid(100, 100)
      .withObstacleAt(1, 0).build();

    const after = rover.execute('F');

    expect(after.position).toEqual([0, 0]);
    expect(after.status).toBe('Blocked');
    expect(after.lastObstacle).toEqual([1, 0]);
  });

  it('remaining commands after block are discarded', () => {
    const rover = new RoverBuilder()
      .at(0, 0).facing('East').onGrid(100, 100)
      .withObstacleAt(2, 0).build();

    const after = rover.execute('FFRFF');

    expect(after.position).toEqual([1, 0]);
    expect(after.heading).toBe('East');
    expect(after.status).toBe('Blocked');
    expect(after.lastObstacle).toEqual([2, 0]);
  });

  it('obstacle blocks a backward move', () => {
    const rover = new RoverBuilder()
      .at(2, 0).facing('East').onGrid(100, 100)
      .withObstacleAt(1, 0).build();

    const after = rover.execute('B');

    expect(after.position).toEqual([2, 0]);
    expect(after.status).toBe('Blocked');
    expect(after.lastObstacle).toEqual([1, 0]);
  });

  it('empty command string leaves the rover unchanged', () => {
    const rover = new RoverBuilder().at(3, 4).facing('West').onGrid(100, 100).build();
    const after = rover.execute('');

    expect(after.position).toEqual([3, 4]);
    expect(after.heading).toBe('West');
    expect(after.status).toBe('Moving');
  });

  it('unknown command character raises an error', () => {
    const rover = new RoverBuilder().at(0, 0).facing('North').onGrid(100, 100).build();

    expect(() => rover.execute('FX')).toThrow(UnknownCommandError);
    expect(() => rover.execute('FX')).toThrow('unknown command');
  });

  it('builders produce the literal the test describes', () => {
    const rover = new RoverBuilder()
      .at(2, 3).facing('West').onGrid(10, 10)
      .withObstacleAt(1, 3).build();

    expect(rover.position).toEqual([2, 3]);
    expect(rover.heading).toBe('West');
    expect(rover.gridWidth).toBe(10);
    expect(rover.gridHeight).toBe(10);
    expect(rover.hasObstacleAt(1, 3)).toBe(true);
    expect(rover.hasObstacleAt(0, 0)).toBe(false);

    const commands = new CommandBuilder()
      .forward().forward().left().right().backward().build();
    expect(commands).toBe('FFLRB');
  });
});
