from bowling_game.game import score


def test_gutter_game_scores_zero():
    assert score([0] * 20) == 0


def test_all_ones_scores_twenty():
    assert score([1] * 20) == 20
