import { describe, it, expect } from 'vitest';
import {
  Board,
  CellOccupiedError,
  GameOverError,
  OutOfBoundsError,
} from '../src/board.js';
import { BoardBuilder } from './boardBuilder.js';

describe('Board', () => {
  it('empty board reports game in progress', () => {
    const board = new Board();
    expect(board.outcome()).toBe('InProgress');
    expect(board.currentTurn()).toBe('X');
  });

  it('first placement puts X on the board', () => {
    const board = new Board().place(0, 0);
    expect(board.cellAt(0, 0)).toBe('X');
    expect(board.currentTurn()).toBe('O');
    expect(board.outcome()).toBe('InProgress');
  });

  it('X wins by completing the top row', () => {
    const board = new BoardBuilder()
      .withXAt(0, 0).withXAt(0, 1)
      .withOAt(1, 0).withOAt(1, 1)
      .build();

    expect(board.place(0, 2).outcome()).toBe('XWins');
  });

  it('X wins by completing the left column', () => {
    const board = new BoardBuilder()
      .withXAt(0, 0).withXAt(1, 0)
      .withOAt(0, 1).withOAt(1, 1)
      .build();

    expect(board.place(2, 0).outcome()).toBe('XWins');
  });

  it('X wins on the main diagonal', () => {
    const board = new BoardBuilder()
      .withXAt(0, 0).withXAt(1, 1)
      .withOAt(0, 1).withOAt(0, 2)
      .build();

    expect(board.place(2, 2).outcome()).toBe('XWins');
  });

  it('O wins on the anti-diagonal', () => {
    const board = new BoardBuilder()
      .withXAt(0, 0).withXAt(1, 0).withXAt(2, 1)
      .withOAt(0, 2).withOAt(1, 1)
      .build();

    expect(board.place(2, 0).outcome()).toBe('OWins');
  });

  it('full board with no winning line is a draw', () => {
    const board = new BoardBuilder()
      .withXAt(0, 0).withOAt(0, 1).withXAt(0, 2)
      .withXAt(1, 0).withXAt(1, 1).withOAt(1, 2)
      .withOAt(2, 0).withXAt(2, 1).withOAt(2, 2)
      .build();

    expect(board.outcome()).toBe('Draw');
  });

  it('placing on an occupied cell raises cell-occupied', () => {
    const board = new BoardBuilder().withXAt(0, 0).build();
    expect(() => board.place(0, 0)).toThrow(CellOccupiedError);
    expect(() => board.place(0, 0)).toThrow('cell already occupied');
  });

  it('placing with a row out of bounds raises out-of-bounds', () => {
    const board = new Board();
    expect(() => board.place(3, 0)).toThrow(OutOfBoundsError);
    expect(() => board.place(3, 0)).toThrow('coordinates out of bounds');
    expect(() => board.place(-1, 0)).toThrow(OutOfBoundsError);
  });

  it('placing with a column out of bounds raises out-of-bounds', () => {
    const board = new Board();
    expect(() => board.place(0, 3)).toThrow(OutOfBoundsError);
    expect(() => board.place(0, 3)).toThrow('coordinates out of bounds');
  });

  it('placing after a win raises game-over', () => {
    const won = new BoardBuilder()
      .withXAt(0, 0).withXAt(0, 1).withXAt(0, 2)
      .withOAt(1, 0).withOAt(1, 1)
      .build();

    expect(() => won.place(2, 2)).toThrow(GameOverError);
    expect(() => won.place(2, 2)).toThrow('game is already over');
  });

  it('BoardBuilder produces the board the test literal describes', () => {
    const board = new BoardBuilder().withXAt(0, 0).withOAt(1, 1).build();
    expect(board.cellAt(0, 0)).toBe('X');
    expect(board.cellAt(1, 1)).toBe('O');
    expect(board.cellAt(2, 2)).toBe('Empty');
    expect(board.outcome()).toBe('InProgress');
    expect(board.currentTurn()).toBe('X');

    const oneAhead = new BoardBuilder().withXAt(0, 0).build();
    expect(oneAhead.currentTurn()).toBe('O');
  });
});
