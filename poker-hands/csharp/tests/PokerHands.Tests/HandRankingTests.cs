using FluentAssertions;
using Xunit;

namespace PokerHands.Tests;

public class HandRankingTests
{
    [Fact]
    public void Hand_of_five_unrelated_cards_is_ranked_as_high_card()
    {
        HandBuilder.FromString("2H 5D 7C 9S KD").Evaluate().Should().Be(HandRank.HighCard);
    }

    [Fact]
    public void Hand_with_two_cards_of_the_same_rank_is_ranked_as_a_pair()
    {
        HandBuilder.FromString("2H 2D 5C 9S KD").Evaluate().Should().Be(HandRank.Pair);
    }

    [Fact]
    public void Hand_with_two_different_pairs_is_ranked_as_two_pair()
    {
        HandBuilder.FromString("2H 2D 5C 5S KD").Evaluate().Should().Be(HandRank.TwoPair);
    }

    [Fact]
    public void Hand_with_three_cards_of_the_same_rank_is_ranked_as_three_of_a_kind()
    {
        HandBuilder.FromString("2H 2D 2C 5S KD").Evaluate().Should().Be(HandRank.ThreeOfAKind);
    }

    [Fact]
    public void Hand_of_five_consecutive_ranks_is_ranked_as_a_straight()
    {
        HandBuilder.FromString("2H 3D 4C 5S 6D").Evaluate().Should().Be(HandRank.Straight);
    }

    [Fact]
    public void Hand_of_five_cards_of_the_same_suit_is_ranked_as_a_flush()
    {
        HandBuilder.FromString("2H 5H 7H 9H KH").Evaluate().Should().Be(HandRank.Flush);
    }

    [Fact]
    public void Hand_with_a_triple_and_a_pair_is_ranked_as_a_full_house()
    {
        HandBuilder.FromString("2H 2D 2C 5S 5D").Evaluate().Should().Be(HandRank.FullHouse);
    }

    [Fact]
    public void Hand_with_four_cards_of_the_same_rank_is_ranked_as_four_of_a_kind()
    {
        HandBuilder.FromString("2H 2D 2C 2S KD").Evaluate().Should().Be(HandRank.FourOfAKind);
    }

    [Fact]
    public void Hand_of_five_consecutive_ranks_in_one_suit_is_ranked_as_a_straight_flush()
    {
        HandBuilder.FromString("2H 3H 4H 5H 6H").Evaluate().Should().Be(HandRank.StraightFlush);
    }
}
