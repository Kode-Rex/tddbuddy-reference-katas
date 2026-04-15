# Poker Hands

Rank 5-card poker hands and compare two hands to decide the winner. An algorithms kata where **test data builders** turn "write down a specific hand" from a miserable exercise in card array literals into a readable sentence.

## What this kata teaches

- **Test Data Builders** — `CardBuilder`, `HandBuilder` make every test read like "a hand of four kings and an ace."
- **Named Domain Constants** — `Rank.Ace`, `Suit.Spades`; no magic strings or enums reduced to ints.
- **Ranking as a First-Class Type** — `HandRank` (`StraightFlush`, `FourOfAKind`, …) is returned from evaluation, not compared via nested ifs.
- **Tie-Breaking Tested Independently** — rank-equal hands exercise the kicker logic in isolation from base ranking.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
