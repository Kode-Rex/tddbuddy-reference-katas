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


def _play_game(match: Match, winner: int) -> None:
    for _ in range(4):
        match.point_won_by(winner)


def test_six_four_in_games_reads_set_player_one():
    match = Match()
    for _ in range(4):
        _play_game(match, 1)
        _play_game(match, 2)
    _play_game(match, 1)
    _play_game(match, 1)
    assert match.score() == "Set Player 1"


def test_two_sets_to_player_one_reads_match_player_one():
    match = Match()
    # Set 1: 6-4
    for _ in range(4):
        _play_game(match, 1)
        _play_game(match, 2)
    _play_game(match, 1)
    _play_game(match, 1)
    # Set 2: 6-3
    for _ in range(3):
        _play_game(match, 1)
        _play_game(match, 2)
    _play_game(match, 1)
    _play_game(match, 1)
    _play_game(match, 1)
    assert match.score() == "Match Player 1"
