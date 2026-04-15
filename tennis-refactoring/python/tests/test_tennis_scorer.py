from tennis_refactoring import get_score


def test_love_all_at_the_start_of_a_game():
    assert get_score(0, 0, "Player1", "Player2") == "Love-All"


def test_fifteen_all_when_both_players_have_one_point():
    assert get_score(1, 1, "Player1", "Player2") == "Fifteen-All"


def test_thirty_all_when_both_players_have_two_points():
    assert get_score(2, 2, "Player1", "Player2") == "Thirty-All"


def test_deuce_when_both_players_have_three_points():
    assert get_score(3, 3, "Player1", "Player2") == "Deuce"


def test_deuce_persists_past_forty_all():
    assert get_score(4, 4, "Player1", "Player2") == "Deuce"


def test_fifteen_love_when_player_1_leads_by_one_at_the_start():
    assert get_score(1, 0, "Player1", "Player2") == "Fifteen-Love"


def test_love_fifteen_when_player_2_leads_by_one_at_the_start():
    assert get_score(0, 1, "Player1", "Player2") == "Love-Fifteen"


def test_thirty_fifteen_when_player_1_has_two_and_player_2_has_one():
    assert get_score(2, 1, "Player1", "Player2") == "Thirty-Fifteen"


def test_forty_fifteen_when_player_1_has_three_and_player_2_has_one():
    assert get_score(3, 1, "Player1", "Player2") == "Forty-Fifteen"


def test_advantage_to_player_1_when_they_lead_by_one_in_the_endgame():
    assert get_score(4, 3, "Player1", "Player2") == "Advantage Player1"


def test_advantage_to_player_2_when_they_lead_by_one_in_the_endgame():
    assert get_score(3, 4, "Player1", "Player2") == "Advantage Player2"


def test_advantage_persists_at_higher_equal_gap_scores():
    assert get_score(5, 4, "Player1", "Player2") == "Advantage Player1"


def test_win_for_player_1_when_they_lead_by_two_in_the_endgame():
    assert get_score(5, 3, "Player1", "Player2") == "Win for Player1"


def test_win_for_player_2_when_they_lead_by_two_in_the_endgame():
    assert get_score(3, 5, "Player1", "Player2") == "Win for Player2"


def test_player_names_are_passed_through_verbatim_into_advantage():
    assert get_score(4, 3, "Serena", "Venus") == "Advantage Serena"


def test_player_names_are_passed_through_verbatim_into_win():
    assert get_score(5, 3, "Serena", "Venus") == "Win for Serena"
