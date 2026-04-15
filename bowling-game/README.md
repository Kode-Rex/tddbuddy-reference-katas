# Bowling Game

**Teaching mode:** [Pedagogy](../README.md#1-pedagogy--learn-the-tdd-rhythm) — the third Pedagogy kata in the repo.

Uncle Bob's classic design-through-refactor kata. Score a game of ten-pin bowling: a pure function over a sequence of rolls (pin counts) returning an integer score. The teaching moment is not the algorithm — it's the **class you don't write**.

## What this kata teaches

The **discipline of refusing a premature abstraction.** Most people start with a `Frame` class — it's right there in the problem statement. "Ten frames per game. A frame is…" — the noun is sitting on the page. Model it, right?

The TDD progression Uncle Bob advocates says: not yet.

Walk the commit log and you'll see the Frame concept *exists in the scoring logic* (the roll cursor advances by one for a strike, by two otherwise — that's a frame boundary) but it does **not** need to exist as a class. Ten frames, one roll list, two indexes. The `Frame` the domain language named is living in an integer increment, not in an object. And the code reads better for it.

The walkthroughs explicitly call out, at two points, the moment the author was tempted to extract `Frame` and chose not to. That refusal — backed by the scorecard the tests drew — is the teaching.

## Gear arc

- **Low gear** for the opening triangulations — gutter game, all ones. Fake-it then sum.
- **Low → middle** at the spare scenario. A new rule forces the scoring loop to peek forward. The natural urge is to introduce a type; resist.
- **Middle gear** through strikes, perfect game, all spares. One scenario per cycle. The two-mode index (frame index vs roll cursor) lands as a refactor of the loop, not as a class.
- **No high gear needed.** The spec has six scenarios; the algorithm lands at scenario four and the last two are honest spec coverage.

## Why no Frame class

Three reasons, in ascending order of importance:

1. **The test list doesn't ask for one.** Every scenario is `rolls -> score`. No test inspects a frame, asks for a frame's bonus, or compares frames. A class whose only client is its author is noise.
2. **The scoring loop already models frames.** `frameIndex` advances 0→9, `rollCursor` advances by 1 (strike) or 2 (otherwise). That two-step advance *is* the frame. Naming it with a class doesn't make it clearer — it makes it one layer of indirection away.
3. **The 10th-frame special case is not a special class.** The bonus rolls are extra entries in the same roll list. They're consumed by the lookahead logic already in place. A `TenthFrame` subclass would replicate state that the list already holds.

If a future requirement asked "how many spares did the bowler get?" or "what was the score at end of frame 7?", `Frame` might earn its keep. For the six scenarios in this spec, it doesn't.

## Files

- [`SCENARIOS.md`](./SCENARIOS.md) — the shared spec (6 numbered scenarios).
- [`ARC.md`](./ARC.md) — the intended commit arc all three languages follow.
- `csharp/`, `typescript/`, `python/` — three idiomatic implementations with per-language `WALKTHROUGH.md`.

## How to read

1. Skim `SCENARIOS.md`.
2. Pick a language; open its `WALKTHROUGH.md`.
3. Walk the commit table. Stop at the two `reflect —` rows — those are the "I was about to write a Frame class and chose not to" moments.
4. Read the final SUT. Notice how short it is. That shortness is the point.
