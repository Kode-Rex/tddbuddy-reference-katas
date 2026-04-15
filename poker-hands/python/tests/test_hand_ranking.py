from poker_hands import HandRank

from .hand_builder import HandBuilder


def test_hand_of_five_unrelated_cards_is_ranked_as_high_card():
    assert HandBuilder.from_string("2H 5D 7C 9S KD").evaluate() == HandRank.HIGH_CARD


def test_hand_with_two_cards_of_the_same_rank_is_ranked_as_a_pair():
    assert HandBuilder.from_string("2H 2D 5C 9S KD").evaluate() == HandRank.PAIR


def test_hand_with_two_different_pairs_is_ranked_as_two_pair():
    assert HandBuilder.from_string("2H 2D 5C 5S KD").evaluate() == HandRank.TWO_PAIR


def test_hand_with_three_cards_of_the_same_rank_is_ranked_as_three_of_a_kind():
    assert HandBuilder.from_string("2H 2D 2C 5S KD").evaluate() == HandRank.THREE_OF_A_KIND


def test_hand_of_five_consecutive_ranks_is_ranked_as_a_straight():
    assert HandBuilder.from_string("2H 3D 4C 5S 6D").evaluate() == HandRank.STRAIGHT


def test_hand_of_five_cards_of_the_same_suit_is_ranked_as_a_flush():
    assert HandBuilder.from_string("2H 5H 7H 9H KH").evaluate() == HandRank.FLUSH


def test_hand_with_a_triple_and_a_pair_is_ranked_as_a_full_house():
    assert HandBuilder.from_string("2H 2D 2C 5S 5D").evaluate() == HandRank.FULL_HOUSE


def test_hand_with_four_cards_of_the_same_rank_is_ranked_as_four_of_a_kind():
    assert HandBuilder.from_string("2H 2D 2C 2S KD").evaluate() == HandRank.FOUR_OF_A_KIND


def test_hand_of_five_consecutive_ranks_in_one_suit_is_ranked_as_a_straight_flush():
    assert HandBuilder.from_string("2H 3H 4H 5H 6H").evaluate() == HandRank.STRAIGHT_FLUSH
