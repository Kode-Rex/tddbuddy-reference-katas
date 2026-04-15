from numbers_to_words import to_words


def test_zero_is_spelled_out():
    assert to_words(0) == "zero"


def test_five_is_spelled_out():
    assert to_words(5) == "five"


def test_eight_is_spelled_out():
    assert to_words(8) == "eight"


def test_ten_is_spelled_out():
    assert to_words(10) == "ten"


def test_nineteen_is_a_single_word():
    assert to_words(19) == "nineteen"


def test_twenty_is_spelled_out():
    assert to_words(20) == "twenty"


def test_twenty_one_is_hyphenated():
    assert to_words(21) == "twenty-one"


def test_seventy_seven_is_hyphenated():
    assert to_words(77) == "seventy-seven"


def test_ninety_nine_is_hyphenated():
    assert to_words(99) == "ninety-nine"


def test_one_hundred_names_the_leading_one():
    assert to_words(100) == "one hundred"


def test_three_hundred_three_has_no_and():
    assert to_words(303) == "three hundred three"


def test_five_hundred_fifty_five_keeps_the_hyphen_in_the_tens():
    assert to_words(555) == "five hundred fifty-five"


def test_two_thousand_omits_trailing_zeros():
    assert to_words(2000) == "two thousand"


def test_two_thousand_four_hundred_skips_the_tens_and_ones():
    assert to_words(2400) == "two thousand four hundred"


def test_three_thousand_four_hundred_sixty_six_is_fully_spelled_out():
    assert to_words(3466) == "three thousand four hundred sixty-six"
