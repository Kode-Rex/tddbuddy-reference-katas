from word_wrap import wrap


def test_an_empty_string_returns_an_empty_string():
    assert wrap("", 10) == ""


def test_a_whitespace_only_string_returns_an_empty_string():
    assert wrap("   \t  ", 10) == ""


def test_a_single_word_shorter_than_width_is_returned_unchanged():
    assert wrap("Hello", 10) == "Hello"


def test_a_single_word_equal_to_width_is_returned_unchanged():
    assert wrap("Hello", 5) == "Hello"


def test_two_words_that_fit_on_one_line_are_returned_unchanged():
    assert wrap("Hello World", 20) == "Hello World"


def test_two_words_break_at_the_word_boundary_when_they_do_not_fit():
    assert wrap("Hello World", 5) == "Hello\nWorld"


def test_two_words_break_at_the_word_boundary_when_the_gap_pushes_past_width():
    assert wrap("Hello World", 7) == "Hello\nWorld"


def test_three_words_across_three_lines_when_each_line_fits_one_word():
    assert wrap("Hello wonderful World", 9) == "Hello\nwonderful\nWorld"


def test_an_oversize_single_word_splits_hard_at_width():
    assert wrap("Supercalifragilisticexpialidocious", 10) == (
        "Supercalif\nragilistic\nexpialidoc\nious"
    )


def test_an_oversize_word_remainder_joins_the_next_word_when_it_fits():
    assert wrap("abcdefghij kl", 5) == "abcde\nfghij\nkl"


def test_multiple_consecutive_whitespace_characters_collapse():
    assert wrap("Hello   World", 5) == "Hello\nWorld"


def test_multi_word_multi_line_greedy_packing():
    assert wrap("The quick brown fox jumps over the lazy dog", 10) == (
        "The quick\nbrown fox\njumps over\nthe lazy\ndog"
    )
