from bowling_game.game import score


def test_gutter_game_scores_zero():
    assert score([0] * 20) == 0


def test_all_ones_scores_twenty():
    assert score([1] * 20) == 20


def test_one_spare_scores_the_spare_bonus():
    assert score([5, 5, 3, 0] + [0] * 16) == 16
