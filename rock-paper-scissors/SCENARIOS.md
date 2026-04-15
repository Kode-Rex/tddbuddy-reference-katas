# Rock Paper Scissors — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Play** | One of `rock`, `paper`, `scissors` — a gesture a player throws |
| **Outcome** | One of `win`, `lose`, `draw` — the result from **player 1's** perspective |
| **decide** | The pure function that maps two plays to an outcome |

## Domain Rules

- Rock beats Scissors
- Scissors beats Paper
- Paper beats Rock
- Equal plays are a draw
- The outcome is always reported from **player 1's** perspective

## Test Scenarios

Full 3×3 matrix as discrete scenarios — the domain has only nine cases, so every one is listed.

1. **rock vs rock is a draw**
2. **rock vs paper loses** — paper covers rock
3. **rock vs scissors wins** — rock crushes scissors
4. **paper vs rock wins** — paper covers rock
5. **paper vs paper is a draw**
6. **paper vs scissors loses** — scissors cuts paper
7. **scissors vs rock loses** — rock crushes scissors
8. **scissors vs paper wins** — scissors cuts paper
9. **scissors vs scissors is a draw**
