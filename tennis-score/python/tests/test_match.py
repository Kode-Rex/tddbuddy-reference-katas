from tennis_score.match import Match


def test_start_of_match_reads_love_love():
    assert Match().score() == "Love-Love"


def test_one_point_to_player_one_reads_fifteen_love():
    match = Match()
    match.point_won_by(1)
    assert match.score() == "15-Love"
