from bowling_game.game import score


def test_gutter_game_scores_zero():
    assert score([0] * 20) == 0
