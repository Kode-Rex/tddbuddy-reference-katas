from text_justification import justify


def test_an_empty_string_returns_an_empty_list():
    assert justify("", 10) == []


def test_a_whitespace_only_string_returns_an_empty_list():
    assert justify("   \t  ", 10) == []


def test_a_single_word_shorter_than_width_is_its_own_last_line():
    assert justify("Word", 10) == ["Word"]


def test_words_that_fit_on_one_line_are_returned_unjustified():
    assert justify("Hi there", 20) == ["Hi there"]


def test_two_line_justification_distributes_uneven_padding_left_first():
    assert justify("This is a test", 12) == ["This   is  a", "test"]


def test_three_line_justification_pads_each_non_last_line_to_width():
    assert justify("This is a very long word", 10) == ["This  is a", "very  long", "word"]


def test_multiple_consecutive_whitespace_characters_collapse():
    assert justify("This   is   a   test", 12) == ["This   is  a", "test"]


def test_a_single_word_non_last_line_is_right_padded_to_width():
    assert justify("longword ab", 9) == ["longword ", "ab"]


def test_a_word_longer_than_width_stands_alone_and_may_exceed_width():
    assert justify("verylongword hi", 5) == ["verylongword", "hi"]


def test_even_space_distribution_across_equal_gaps():
    assert justify("alpha beta gamma delta epsilon", 25) == ["alpha  beta  gamma  delta", "epsilon"]
