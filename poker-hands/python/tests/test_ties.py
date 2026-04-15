from poker_hands import Compare

from .hand_builder import HandBuilder


def test_two_hands_with_identical_ranks_and_kickers_tie():
    player1 = HandBuilder.from_string("2H 3D 5S 9C KD")
    player2 = HandBuilder.from_string("2D 3H 5C 9S KH")
    assert player1.compare_to(player2) == Compare.TIE
