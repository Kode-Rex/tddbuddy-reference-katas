from balanced_brackets import is_balanced


def test_empty_string_is_balanced():
    assert is_balanced("") is True


def test_single_pair_is_balanced():
    assert is_balanced("[]") is True


def test_two_sequential_pairs_are_balanced():
    assert is_balanced("[][]") is True


def test_nested_pair_is_balanced():
    assert is_balanced("[[]]") is True


def test_deeply_nested_mixed_pairs_are_balanced():
    assert is_balanced("[[[][]]]") is True


def test_closing_before_opening_is_not_balanced():
    assert is_balanced("][") is False


def test_alternating_reversed_pairs_are_not_balanced():
    assert is_balanced("][][") is False


def test_trailing_imbalance_is_not_balanced():
    assert is_balanced("[][]][") is False


def test_a_lone_opener_is_not_balanced():
    assert is_balanced("[") is False


def test_a_lone_closer_is_not_balanced():
    assert is_balanced("]") is False


def test_unmatched_opener_at_end_is_not_balanced():
    assert is_balanced("[[]") is False


def test_unmatched_closer_at_end_is_not_balanced():
    assert is_balanced("[]]") is False
