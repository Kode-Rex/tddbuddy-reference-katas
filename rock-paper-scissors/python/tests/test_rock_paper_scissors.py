from rock_paper_scissors import Play, Outcome, decide


def test_rock_vs_rock_is_a_draw():
    assert decide(Play.ROCK, Play.ROCK) is Outcome.DRAW


def test_rock_vs_paper_loses_because_paper_covers_rock():
    assert decide(Play.ROCK, Play.PAPER) is Outcome.LOSE


def test_rock_vs_scissors_wins_because_rock_crushes_scissors():
    assert decide(Play.ROCK, Play.SCISSORS) is Outcome.WIN


def test_paper_vs_rock_wins_because_paper_covers_rock():
    assert decide(Play.PAPER, Play.ROCK) is Outcome.WIN


def test_paper_vs_paper_is_a_draw():
    assert decide(Play.PAPER, Play.PAPER) is Outcome.DRAW


def test_paper_vs_scissors_loses_because_scissors_cuts_paper():
    assert decide(Play.PAPER, Play.SCISSORS) is Outcome.LOSE


def test_scissors_vs_rock_loses_because_rock_crushes_scissors():
    assert decide(Play.SCISSORS, Play.ROCK) is Outcome.LOSE


def test_scissors_vs_paper_wins_because_scissors_cuts_paper():
    assert decide(Play.SCISSORS, Play.PAPER) is Outcome.WIN


def test_scissors_vs_scissors_is_a_draw():
    assert decide(Play.SCISSORS, Play.SCISSORS) is Outcome.DRAW
