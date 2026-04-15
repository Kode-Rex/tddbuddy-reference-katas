from code_breaker import Feedback, Peg
from tests.guess_builder import GuessBuilder
from tests.secret_builder import SecretBuilder

ONE, TWO, THREE, FOUR, FIVE, SIX = (
    Peg.ONE,
    Peg.TWO,
    Peg.THREE,
    Peg.FOUR,
    Peg.FIVE,
    Peg.SIX,
)


def test_secret_1234_vs_guess_with_no_shared_pegs_has_no_matches():
    secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
    guess = GuessBuilder().with_pegs(FIVE, SIX, FIVE, SIX).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == ""
    assert feedback.exact_matches == 0
    assert feedback.color_matches == 0


def test_secret_1234_vs_guess_1566_has_one_exact_match():
    secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
    guess = GuessBuilder().with_pegs(ONE, FIVE, SIX, SIX).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "+"
    assert feedback.exact_matches == 1
    assert feedback.color_matches == 0


def test_secret_1234_vs_guess_1234_is_a_win():
    secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
    guess = GuessBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "++++"
    assert feedback.exact_matches == 4
    assert feedback.is_won is True


def test_secret_1234_vs_guess_4321_has_four_color_matches():
    secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
    guess = GuessBuilder().with_pegs(FOUR, THREE, TWO, ONE).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "----"
    assert feedback.exact_matches == 0
    assert feedback.color_matches == 4


def test_secret_1234_vs_guess_1243_has_two_exact_and_two_color_matches():
    secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
    guess = GuessBuilder().with_pegs(ONE, TWO, FOUR, THREE).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "++--"
    assert feedback.exact_matches == 2
    assert feedback.color_matches == 2


def test_secret_1234_vs_guess_2135_has_one_exact_and_two_color_matches():
    secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
    guess = GuessBuilder().with_pegs(TWO, ONE, THREE, FIVE).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "+--"
    assert feedback.exact_matches == 1
    assert feedback.color_matches == 2


def test_secret_1124_vs_guess_5166_counts_the_duplicate_peg_only_once():
    secret = SecretBuilder().with_pegs(ONE, ONE, TWO, FOUR).build()
    guess = GuessBuilder().with_pegs(FIVE, ONE, SIX, SIX).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "+"
    assert feedback.exact_matches == 1
    assert feedback.color_matches == 0


def test_secret_1122_vs_guess_2211_has_four_color_matches_no_exact():
    secret = SecretBuilder().with_pegs(ONE, ONE, TWO, TWO).build()
    guess = GuessBuilder().with_pegs(TWO, TWO, ONE, ONE).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "----"
    assert feedback.exact_matches == 0
    assert feedback.color_matches == 4


def test_secret_1111_vs_guess_1112_counts_three_exacts_and_ignores_the_non_matching_peg():
    secret = SecretBuilder().with_pegs(ONE, ONE, ONE, ONE).build()
    guess = GuessBuilder().with_pegs(ONE, ONE, ONE, TWO).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "+++"
    assert feedback.exact_matches == 3
    assert feedback.color_matches == 0


def test_secret_1111_vs_guess_2111_counts_three_exacts_at_positions_2_through_4():
    secret = SecretBuilder().with_pegs(ONE, ONE, ONE, ONE).build()
    guess = GuessBuilder().with_pegs(TWO, ONE, ONE, ONE).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "+++"
    assert feedback.exact_matches == 3
    assert feedback.color_matches == 0


def test_secret_1234_vs_guess_1111_counts_one_exact_and_no_color_matches():
    secret = SecretBuilder().with_pegs(ONE, TWO, THREE, FOUR).build()
    guess = GuessBuilder().with_pegs(ONE, ONE, ONE, ONE).build()

    feedback = secret.score_against(guess)

    assert feedback.render() == "+"
    assert feedback.exact_matches == 1
    assert feedback.color_matches == 0


def test_feedback_with_four_exact_matches_is_won_any_other_feedback_is_not():
    assert Feedback(exact_matches=4, color_matches=0).is_won is True
    assert Feedback(exact_matches=0, color_matches=4).is_won is False
    assert Feedback(exact_matches=3, color_matches=1).is_won is False
    assert Feedback(exact_matches=0, color_matches=0).is_won is False
