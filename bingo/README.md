# Bingo

Classic US Bingo card — a 5x5 grid with numbers drawn from column-specific ranges (B 1–15, I 16–30, N 31–45, G 46–60, O 61–75) and a FREE space in the centre. `Card.mark(number)` flips the cell holding that number; `Card.winningPattern()` reports the first completed line (row, column, or diagonal) or `None`. Marking a number outside the 1–75 range is rejected; marking a number the card does not contain is a no-op — the caller calls numbers at large, some land on the card, some do not.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. One primary entity (`Card`), one small test-folder builder (`CardBuilder`, 15–30 lines) whose chained setters place specific numbers at specific `(row, col)` coordinates so the test reads as the literal board under test:

```
new CardBuilder()
    .WithNumberAt(0, 0, 3).WithNumberAt(0, 1, 17).WithNumberAt(0, 2, 33)
    .WithNumberAt(0, 3, 48).WithNumberAt(0, 4, 62)
    .Build()
```

vs. relying on random generation and hunting for numbers that happen to land in a row. The builder skips column-range validation because it is a test-folder synthesiser of card states, not a card-generation service.

## Scope — Pure Domain Only

The reference covers the card, the numbering rules, the free space, marking called numbers, and detecting the first winning line. **No card generator, no caller/draw service, no UI, no persistence, no networked multiplayer.** Random card generation (the "25 unique numbers within column ranges" rule from the spec) is a card *factory* concern — it would introduce an `IRandom` collaborator and tip the kata into F3.

### Stretch Goals (Not Implemented Here)

- **Card generator** — random card construction honouring the column ranges
- **Caller / draw service** — a `Caller` that emits unique numbers 1–75 without repeats
- **Game loop** — orchestrates draws and card checks until someone calls "Bingo"
- **Win-pattern variants** — blackout, four-corners, postage-stamp, X pattern
- **UK Bingo** — the 9x3 variant with 5 filled spaces per row
- **Persistence / multiplayer** — storing cards, syncing marks across players

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference satisfies.
