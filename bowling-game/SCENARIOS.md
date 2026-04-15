# Bowling Game — Scenarios

The shared specification all three language implementations satisfy. Six scenarios, ordered by the gear they land at. The last two pass on arrival once the scoring loop's two-mode index is in place.

## Domain Rules

- A game has **ten frames**. Each frame is up to two rolls, except the tenth.
- **Open frame** — score is the sum of pins knocked down in that frame's rolls.
- **Spare** — two rolls in a frame totalling 10. Score is `10 + next roll`.
- **Strike** — one roll knocking down all 10 pins. Score is `10 + next two rolls`.
- **Tenth frame** — if a spare, one bonus roll; if a strike, two bonus rolls. Bonus rolls count toward the tenth frame's score but do not themselves begin a new frame.
- The SUT is pure: a sequence of rolls in, an integer score out. No validation beyond what the six scenarios specify (which is none).

## Scenarios

1. **Gutter game scores zero.** Twenty rolls of `0` → `0`.
2. **All ones scores twenty.** Twenty rolls of `1` → `20`.
3. **One spare scores the spare bonus.** `5, 5, 3, 0, 0, 0, ...` (14 zeros) → `16` — the spare is worth `10 + 3 = 13`, plus the next frame's `3`.
4. **One strike scores the strike bonus.** `10, 3, 4, 0, 0, ...` (16 zeros) → `24` — the strike is worth `10 + 3 + 4 = 17`, plus the next frame's `3 + 4 = 7`.
5. **Perfect game scores 300.** Twelve rolls of `10` → `300` — ten strikes in frames 1–10, plus two bonus rolls in the tenth.
6. **All spares scores 150.** `5, 5` repeated ten times, plus one bonus `5` → `150`.

## Out of Scope

Behavior not listed above is undefined by this spec. No validation of pin counts, frame totals, roll counts, or roll ordering. If a future requirement names any of those, a scenario is added here first.
