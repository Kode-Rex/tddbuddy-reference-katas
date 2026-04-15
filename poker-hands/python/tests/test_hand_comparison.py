from poker_hands import Compare

from .hand_builder import HandBuilder


def test_pair_beats_high_card():
    pair = HandBuilder.from_string("2H 2D 5C 9S KD")
    high_card = HandBuilder.from_string("3H 5D 7C 9S AD")
    assert pair.compare_to(high_card) == Compare.PLAYER1_WINS


def test_flush_beats_straight():
    flush = HandBuilder.from_string("2H 5H 7H 9H KH")
    straight = HandBuilder.from_string("2H 3D 4C 5S 6D")
    assert flush.compare_to(straight) == Compare.PLAYER1_WINS


def test_straight_flush_beats_four_of_a_kind():
    straight_flush = HandBuilder.from_string("2H 3H 4H 5H 6H")
    four_of_a_kind = HandBuilder.from_string("AH AD AC AS KD")
    assert straight_flush.compare_to(four_of_a_kind) == Compare.PLAYER1_WINS
