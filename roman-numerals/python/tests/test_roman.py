from roman_numerals.roman import to_roman


def test_1_is_I():
    assert to_roman(1) == "I"


def test_2_is_II():
    assert to_roman(2) == "II"


def test_3_is_III():
    assert to_roman(3) == "III"
