# Poker Hands — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Card** | A single card (`Card`) with a rank and a suit |
| **Rank** | Two through Ace; ordered Two < Three < … < King < Ace |
| **Suit** | Clubs, Diamonds, Hearts, Spades; unordered |
| **Hand** | Exactly five cards (`Hand`) |
| **HandRank** | The category a hand falls into (`HighCard`, `Pair`, `TwoPair`, `ThreeOfAKind`, `Straight`, `Flush`, `FullHouse`, `FourOfAKind`, `StraightFlush`) |
| **Compare** | A three-way result: `Player1Wins`, `Player2Wins`, `Tie` |

## Domain Rules

- A `Hand` contains exactly **5 cards**; the constructor rejects any other count
- **Ranking order** (lowest → highest): HighCard < Pair < TwoPair < ThreeOfAKind < Straight < Flush < FullHouse < FourOfAKind < StraightFlush
- **Tie-breakers**:
  - `HighCard`: compare the highest card, then the next highest, etc.
  - `Pair`: compare the paired rank; if equal, compare kickers in descending order
  - `TwoPair`: compare the higher pair, then the lower pair, then the kicker
  - `ThreeOfAKind`: compare the triple's rank
  - `Straight`: compare the highest card in the straight
  - `Flush`: compare cards in descending order (same rule as HighCard)
  - `FullHouse`: compare the triple's rank
  - `FourOfAKind`: compare the quad's rank
  - `StraightFlush`: compare the highest card

## Test Scenarios

### Hand Construction

1. **A hand with five cards is valid**
2. **A hand with fewer than five cards is rejected**
3. **A hand with more than five cards is rejected**

### Ranking a Single Hand

4. **Hand of five unrelated cards is ranked as high card**
5. **Hand with two cards of the same rank is ranked as a pair**
6. **Hand with two different pairs is ranked as two pair**
7. **Hand with three cards of the same rank is ranked as three of a kind**
8. **Hand of five consecutive ranks is ranked as a straight**
9. **Hand of five cards of the same suit is ranked as a flush**
10. **Hand with a triple and a pair is ranked as a full house**
11. **Hand with four cards of the same rank is ranked as four of a kind**
12. **Hand of five consecutive ranks in one suit is ranked as a straight flush**

### Comparing Hands — Different Ranks

13. **Pair beats high card**
14. **Flush beats straight**
15. **Straight flush beats four of a kind**

### Comparing Hands — Same Rank, Different Kickers

16. **Higher high card wins when neither hand has a ranked combination**
17. **Higher pair wins when both hands have a pair**
18. **Higher kicker wins when both hands have the same pair**
19. **Higher of two pairs wins when both hands have two pair with the same lower pair**

### Ties

20. **Two hands with identical ranks and kickers tie**
