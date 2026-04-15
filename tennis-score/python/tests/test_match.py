from tennis_score.match import Match


def test_start_of_match_reads_love_love():
    assert Match().score() == "Love-Love"


def test_one_point_to_player_one_reads_fifteen_love():
    match = Match()
    match.point_won_by(1)
    assert match.score() == "15-Love"


def test_two_points_each_reads_thirty_thirty():
    match = Match()
    match.point_won_by(1)
    match.point_won_by(2)
    match.point_won_by(1)
    match.point_won_by(2)
    assert match.score() == "30-30"


def test_three_points_each_reads_deuce():
    match = Match()
    for _ in range(3):
        match.point_won_by(1)
        match.point_won_by(2)
    assert match.score() == "Deuce"


def test_four_three_reads_advantage_player_one():
    match = Match()
    for _ in range(3):
        match.point_won_by(1)
        match.point_won_by(2)
    match.point_won_by(1)
    assert match.score() == "Advantage Player 1"


def test_four_two_reads_game_player_one():
    match = Match()
    match.point_won_by(1)
    match.point_won_by(1)
    match.point_won_by(2)
    match.point_won_by(1)
    match.point_won_by(2)
    match.point_won_by(1)
    assert match.score() == "Game Player 1"
