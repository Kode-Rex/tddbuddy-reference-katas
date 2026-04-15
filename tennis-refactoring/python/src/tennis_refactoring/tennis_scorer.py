# Clean tennis scorer — characterization-equivalent to the legacy getScore
# function from the kata brief. Three named branches (equal / endgame /
# in-play) replace the original's nested if/else over raw point counts;
# POINT_NAMES lifts the magic ["Love","Fifteen","Thirty","Forty"] array
# into a named module-level tuple. See ../../SCENARIOS.md for the contract.

# The first point count at which the game enters the endgame —
# Advantage or Win. Below this threshold, unequal scores render as
# "<Point>-<Point>" (in-play); at or above, the leader's name appears.
ENDGAME_THRESHOLD = 4

# The first point count at which equal scores collapse to "Deuce"
# rather than "<Point>-All". Legacy: `p1Score >= 3` when p1Score == p2Score.
DEUCE_THRESHOLD = 3

POINT_NAMES = ("Love", "Fifteen", "Thirty", "Forty")


def _point_name(score: int) -> str:
    return POINT_NAMES[score]


def get_score(p1_score: int, p2_score: int, p1_name: str, p2_name: str) -> str:
    if p1_score == p2_score:
        return _equal_score(p1_score)
    if p1_score >= ENDGAME_THRESHOLD or p2_score >= ENDGAME_THRESHOLD:
        return _endgame_score(p1_score, p2_score, p1_name, p2_name)
    return _in_play_score(p1_score, p2_score)


def _equal_score(score: int) -> str:
    if score >= DEUCE_THRESHOLD:
        return "Deuce"
    return f"{_point_name(score)}-All"


def _endgame_score(p1_score: int, p2_score: int, p1_name: str, p2_name: str) -> str:
    diff = p1_score - p2_score
    if diff == 1:
        return f"Advantage {p1_name}"
    if diff == -1:
        return f"Advantage {p2_name}"
    if diff >= 2:
        return f"Win for {p1_name}"
    return f"Win for {p2_name}"


def _in_play_score(p1_score: int, p2_score: int) -> str:
    return f"{_point_name(p1_score)}-{_point_name(p2_score)}"
