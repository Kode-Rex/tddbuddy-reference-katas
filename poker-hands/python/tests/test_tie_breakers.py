from poker_hands import Compare

from .hand_builder import HandBuilder


def test_higher_high_card_wins_when_neither_hand_has_a_ranked_combination():
    ace_high = HandBuilder.from_string("2C 3H 4S 8C AH")
    king_high = HandBuilder.from_string("2H 3D 5S 9C KD")
    assert ace_high.compare_to(king_high) == Compare.PLAYER1_WINS


def test_higher_pair_wins_when_both_hands_have_a_pair():
    kings_pair = HandBuilder.from_string("KH KD 5C 9S 3D")
    twos_pair = HandBuilder.from_string("2H 2D 5C 9S AD")
    assert kings_pair.compare_to(twos_pair) == Compare.PLAYER1_WINS


def test_higher_kicker_wins_when_both_hands_have_the_same_pair():
    sevens_with_ace_kicker = HandBuilder.from_string("7H 7D AC 4S 2D")
    sevens_with_king_kicker = HandBuilder.from_string("7C 7S KH 4D 2H")
    assert sevens_with_ace_kicker.compare_to(sevens_with_king_kicker) == Compare.PLAYER1_WINS


def test_higher_of_two_pairs_wins_when_both_hands_have_two_pair_with_the_same_lower_pair():
    aces_and_twos = HandBuilder.from_string("AH AD 2C 2S KD")
    kings_and_twos = HandBuilder.from_string("KH KC 2H 2D QS")
    assert aces_and_twos.compare_to(kings_and_twos) == Compare.PLAYER1_WINS
