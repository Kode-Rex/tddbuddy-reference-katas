namespace PokerHands;

public class Hand
{
    public const int HandSize = 5;

    private static readonly IReadOnlyDictionary<char, Rank> RankCodes = new Dictionary<char, Rank>
    {
        ['2'] = Rank.Two,
        ['3'] = Rank.Three,
        ['4'] = Rank.Four,
        ['5'] = Rank.Five,
        ['6'] = Rank.Six,
        ['7'] = Rank.Seven,
        ['8'] = Rank.Eight,
        ['9'] = Rank.Nine,
        ['T'] = Rank.Ten,
        ['J'] = Rank.Jack,
        ['Q'] = Rank.Queen,
        ['K'] = Rank.King,
        ['A'] = Rank.Ace,
    };

    private static readonly IReadOnlyDictionary<char, Suit> SuitCodes = new Dictionary<char, Suit>
    {
        ['C'] = Suit.Clubs,
        ['D'] = Suit.Diamonds,
        ['H'] = Suit.Hearts,
        ['S'] = Suit.Spades,
    };

    private readonly Card[] _cards;

    public Hand(IEnumerable<Card> cards)
    {
        var array = cards.ToArray();
        if (array.Length != HandSize)
        {
            throw new InvalidHandException($"A hand must have exactly 5 cards (got {array.Length})");
        }
        _cards = array;
    }

    public IReadOnlyList<Card> Cards => _cards;

    /// <summary>
    /// Parse a hand from shorthand notation like "2H 3D 5S 9C KD".
    /// Each token is a two-character card: rank code (2-9, T, J, Q, K, A) + suit code (C, D, H, S).
    /// </summary>
    public static Hand Parse(string shorthand)
    {
        if (shorthand is null) throw new InvalidHandException("A hand must have exactly 5 cards (got 0)");
        var tokens = shorthand.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var cards = tokens.Select(ParseCard);
        return new Hand(cards);
    }

    private static Card ParseCard(string token)
    {
        if (token.Length != 2)
        {
            throw new InvalidHandException($"Invalid card token '{token}'");
        }
        if (!RankCodes.TryGetValue(token[0], out var rank))
        {
            throw new InvalidHandException($"Invalid rank code '{token[0]}'");
        }
        if (!SuitCodes.TryGetValue(token[1], out var suit))
        {
            throw new InvalidHandException($"Invalid suit code '{token[1]}'");
        }
        return new Card(rank, suit);
    }

    public HandRank Evaluate()
    {
        var groups = RankGroupsByCountDescending();
        var counts = groups.Select(g => g.Count).ToArray();
        var isFlush = _cards.Select(c => c.Suit).Distinct().Count() == 1;
        var isStraight = IsStraight();

        if (isStraight && isFlush) return HandRank.StraightFlush;
        if (counts[0] == 4) return HandRank.FourOfAKind;
        if (counts[0] == 3 && counts.Length > 1 && counts[1] == 2) return HandRank.FullHouse;
        if (isFlush) return HandRank.Flush;
        if (isStraight) return HandRank.Straight;
        if (counts[0] == 3) return HandRank.ThreeOfAKind;
        if (counts[0] == 2 && counts.Length > 1 && counts[1] == 2) return HandRank.TwoPair;
        if (counts[0] == 2) return HandRank.Pair;
        return HandRank.HighCard;
    }

    public Compare CompareTo(Hand other)
    {
        var rank = Evaluate();
        var otherRank = other.Evaluate();
        if (rank > otherRank) return Compare.Player1Wins;
        if (rank < otherRank) return Compare.Player2Wins;

        var mySignature = TieBreakSignature();
        var theirSignature = other.TieBreakSignature();
        for (var i = 0; i < mySignature.Count; i++)
        {
            if (mySignature[i] > theirSignature[i]) return Compare.Player1Wins;
            if (mySignature[i] < theirSignature[i]) return Compare.Player2Wins;
        }
        return Compare.Tie;
    }

    /// <summary>
    /// Canonical tie-break signature: ranks ordered first by group size (descending), then by rank (descending).
    /// Four of a kind [9,9,9,9,2] → [9,9,9,9,2]. Full house 9s over 2s → [9,9,9,2,2].
    /// Two pair Ks and 4s with a 7 kicker → [K,K,4,4,7]. This ordering means a positional
    /// comparison correctly implements every tie-breaker rule in SCENARIOS.md.
    /// </summary>
    private IReadOnlyList<Rank> TieBreakSignature()
    {
        return RankGroupsByCountDescending().SelectMany(g => Enumerable.Repeat(g.Rank, g.Count)).ToList();
    }

    private IReadOnlyList<(Rank Rank, int Count)> RankGroupsByCountDescending()
    {
        return _cards
            .GroupBy(c => c.Rank)
            .Select(g => (Rank: g.Key, Count: g.Count()))
            .OrderByDescending(g => g.Count)
            .ThenByDescending(g => g.Rank)
            .ToList();
    }

    private bool IsStraight()
    {
        var ordered = _cards.Select(c => (int)c.Rank).OrderBy(r => r).ToArray();
        for (var i = 1; i < ordered.Length; i++)
        {
            if (ordered[i] != ordered[i - 1] + 1) return false;
        }
        return true;
    }
}
