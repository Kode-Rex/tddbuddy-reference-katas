from tennis_score.match import Match


def test_start_of_match_reads_love_love():
    assert Match().score() == "Love-Love"
