# Tic-Tac-Toe

Two-player tic-tac-toe on a 3x3 board. `Board.place(row, col)` returns a new `Board`; `Board.outcome()` reports `InProgress | XWins | OWins | Draw`. Invalid moves (occupied cells, out-of-bounds coordinates, playing after the game is over) raise domain exceptions. Turn order is implicit — `X` always moves first, then `O`, alternating — so `place` takes only the coordinates, not a mark.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. One primary entity (`Board`), one small test-folder builder (`BoardBuilder`, 10–30 lines) whose chained setters read as a direct literal of the board state under test. No collaborators, no object mothers. The teaching point is that tests set up a specific board — *"X on the top row with O blocking the centre"* — by naming exactly the marks that matter, not by replaying the move sequence that got there:

```
new BoardBuilder().WithXAt(0,0).WithXAt(0,1).WithOAt(1,1).Build()
```

vs.

```
new Board().Place(0,0).Place(1,1).Place(0,1)  // whose turn now? which marks won? harder to read
```

The builder's payoff is that each test opens with a readable snapshot of the board it cares about.

## Scope — Pure Domain Only

The reference covers the board, turn tracking, move validation, and win/draw detection. **No UI, no CLI, no persistence, no AI opponent.** Those would each introduce collaborators (renderers, input parsers, strategy interfaces) and tip the kata into F3.

### Stretch Goals (Not Implemented Here)

- **Rendering** — ASCII board printing / HTML view
- **CLI or interactive loop** — argument parsing, human input
- **Computer opponent** — minimax, heuristic, or random-move strategies
- **Undo / history** — replay the move list
- **Board sizes other than 3x3** — generalised n-in-a-row

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference satisfies.
