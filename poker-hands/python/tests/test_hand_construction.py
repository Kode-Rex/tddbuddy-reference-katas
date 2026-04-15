import pytest

from poker_hands import Card, InvalidHandError, Rank, Suit

from .card_builder import CardBuilder
from .hand_builder import HandBuilder


def test_a_hand_with_five_cards_is_valid():
    ace_of_spades = CardBuilder().of_rank(Rank.ACE).of_suit(Suit.SPADES).build()
    hand = (
        HandBuilder()
        .with_card(ace_of_spades)
        .with_card(Card(Rank.KING, Suit.SPADES))
        .with_card(Card(Rank.QUEEN, Suit.SPADES))
        .with_card(Card(Rank.JACK, Suit.SPADES))
        .with_card(Card(Rank.TEN, Suit.SPADES))
        .build()
    )

    assert len(hand.cards) == 5
    assert hand.cards[0] == ace_of_spades


def test_a_hand_with_fewer_than_five_cards_is_rejected():
    with pytest.raises(InvalidHandError, match=r"^A hand must have exactly 5 cards \(got 4\)$"):
        HandBuilder.from_string("2H 3D 5S 9C")


def test_a_hand_with_more_than_five_cards_is_rejected():
    with pytest.raises(InvalidHandError, match=r"^A hand must have exactly 5 cards \(got 6\)$"):
        HandBuilder.from_string("2H 3D 5S 9C KD AH")
