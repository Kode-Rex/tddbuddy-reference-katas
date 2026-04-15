# Poker Hands — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Hand ──holds──> Card[5] (rank, suit)
 │                   ├── Rank: Two < Three < … < King < Ace
 │                   └── Suit: Clubs, Diamonds, Hearts, Spades (unordered)
 ├──evaluate──> HandRank (HighCard … StraightFlush)
 └──compareTo──> Compare (Player1Wins | Player2Wins | Tie)
```

No aggregate root, no collaborators. `Hand.Evaluate()` and `Hand.CompareTo(other)` are pure functions over immutable data. The whole kata is value types and two pure queries — which is exactly why **builders carry most of the weight** here.

## Why `Card`, `Rank`, and `Suit` Are Types (Not Strings or Ints)

The kata spec writes hands as `"2H 3D 5S 9C KD"` — two-character tokens. A naive implementation could stop at strings, do all comparison by string operations, and pass every scenario. That implementation would be unreadable.

`Rank` as a backed enum (`Rank.Ace = 14`) means `Rank.Ace > Rank.King` is true by the same mechanism that makes any two ranks comparable. No lookup table, no helper function, no string switch. `Suit` as an enum makes "same suit" a reference comparison — flush detection reduces to `_cards.Select(c => c.Suit).Distinct().Count() == 1`.

`Card` as a `readonly record struct` is the natural composition: two value-typed fields, value equality for free, immutability by construction. The pair is the card; there is no "card with identity" in poker.

See `src/PokerHands/Rank.cs`, `Suit.cs`, `Card.cs`.

## Why `Hand` Validates the 5-Card Invariant in the Constructor

A poker hand is exactly five cards. The rules don't apply to four, and they don't apply to six — those aren't weaker hands, they're not hands at all. Enforcing `Cards.Length == 5` at construction means every method on `Hand` can assume the invariant. `Evaluate()` doesn't need a guard clause; `CompareTo()` doesn't either.

The rejection throws `InvalidHandException`, a **domain-specific exception type** (per F3 convention). A caller writing `try { new Hand(cards) } catch (InvalidHandException) { ... }` reads as the business rule it encodes. The cross-language message string is byte-identical: `"A hand must have exactly 5 cards (got N)"`.

See `src/PokerHands/Hand.cs`, `InvalidHandException.cs`.

## Why Two Builders: `CardBuilder` and `HandBuilder.FromString`

Most of the twenty scenarios care about a hand as a whole — "this hand is a full house", "this hand beats that hand". For those, writing out five `new Card(Rank.Two, Suit.Hearts)` expressions is pure noise. `HandBuilder.FromString("2H 3D 5S 9C KD")` is the canonical poker-hands literal from the kata brief itself; accepting it as builder input means every hand-level test reads as one line in the domain's own shorthand.

A handful of scenarios want to talk about a specific card — "this card is the Ace of Spades, check its rank and suit". For those, `new CardBuilder().OfRank(Rank.Ace).OfSuit(Suit.Spades).Build()` is the readable form. `HandBuilder` then composes those cards with `.With(card)` when the test assembles a hand card-by-card rather than from shorthand.

Both shapes coexist because they serve different readers. The shorthand form is dense and perfect for hand-level evaluation. The fluent form names the attributes when the test's point is about an attribute. Forcing every test through one shape would make half of them unreadable.

See `tests/PokerHands.Tests/CardBuilder.cs`, `HandBuilder.cs`.

## Tie-Breaker Design: A Canonical Signature per Hand

The spec has per-rank tie-breaker rules — pair compares paired rank then kickers, two-pair compares higher pair then lower pair then kicker, full house compares the triple's rank, etc. A naive implementation writes a switch on `HandRank` with a different comparator for each arm.

This implementation collapses every tie-break into one signature. `TieBreakSignature()` groups the five cards by rank, orders groups first by count (descending), then by rank (descending), and emits the ranks in that order with multiplicity preserved:

- Full house `9s over 2s` → `[9, 9, 9, 2, 2]`
- Two pair `Kings and 4s with a 7 kicker` → `[K, K, 4, 4, 7]`
- Straight `2-3-4-5-6` → `[6, 5, 4, 3, 2]`
- High card `A-K-9-5-3` → `[A, K, 9, 5, 3]`

A positional comparison of two same-rank signatures implements every tie-break rule in one loop. The triple's rank is at position 0 for a full house or three-of-a-kind; the higher pair is at position 0 for two pair; the highest card is at position 0 for high card, straight, and flush. One data shape, one comparator, nine hand categories handled.

The tradeoff: the signature is not meaningful *across* different `HandRank` values (a pair's signature might start with a King while a straight's starts with a Six, and we can't conclude from that positional compare alone which wins). So `CompareTo` checks `HandRank` first and only falls through to the signature when the categories match. That gatekeeping is the whole purpose of `HandRank` as a first-class ordered type.

See `src/PokerHands/Hand.cs` — `CompareTo`, `TieBreakSignature`, `RankGroupsByCountDescending`.

## Why `HandRank` Is a First-Class Ordered Enum

An alternative design computes a single integer score per hand (e.g. `category * 1_000_000 + signature_digits`) and compares ints. That's terse but opaque — the test that reads `hand.Score.Should().BeGreaterThan(otherHand.Score)` doesn't say *why* one beats the other.

`hand.Evaluate().Should().Be(HandRank.FullHouse)` says what the hand is. `firstHand.CompareTo(secondHand).Should().Be(Compare.Player1Wins)` says who wins. Both names live in the domain. The nine ranking categories are backed by integers (`HighCard = 1` … `StraightFlush = 9`) so ordering works, but the names are the API. This is the same discipline as `Rank` — the integer is an implementation detail; the name is the interface.

See `src/PokerHands/HandRank.cs`, `Compare.cs`.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live across five test files in `tests/PokerHands.Tests/`:

- `HandConstructionTests.cs` — scenarios 1–3
- `HandRankingTests.cs` — scenarios 4–12
- `HandComparisonTests.cs` — scenarios 13–15
- `TieBreakerTests.cs` — scenarios 16–19
- `TiesTests.cs` — scenario 20

One `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## What's Deliberately Not Modeled

The classic poker-hands problem has rich extensions the spec leaves out, and this implementation matches the spec's twenty scenarios exactly:

- **No multiple-players-per-round** — hands are compared pairwise. A three-player showdown would need an ordering, not just a binary comparator, but the scenario list doesn't ask for it.
- **No hand history or deck tracking** — each `Hand` is constructed from five cards with no awareness of the deck they came from. Duplicate cards across hands (which can't happen in a real game) are not detected; the kata's scenarios never exercise that edge.
- **No betting, pot, or game state** — `Hand` evaluates and compares; there's no `Game` or `Round` object.
- **No low-ball, wild cards, or Ace-low straights** — the spec's straight rule is "five consecutive ranks"; Ace-high straights (`T-J-Q-K-A`) work; Ace-low (`A-2-3-4-5`) does not, because the spec doesn't require it.

Every line of domain code earns its keep against a named test. Extensions are a good reader exercise — the seams are already there (`Hand.CompareTo` → `CompareHands(hands...)` fold; `Deck.Deal` wrapping `Hand` construction).

## How to Run

```bash
cd poker-hands/csharp
dotnet test
```
