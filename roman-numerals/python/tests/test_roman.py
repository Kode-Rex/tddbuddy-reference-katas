from roman_numerals.roman import to_roman


def test_1_is_I():
    assert to_roman(1) == "I"


def test_2_is_II():
    assert to_roman(2) == "II"


def test_3_is_III():
    assert to_roman(3) == "III"


def test_5_is_V():
    assert to_roman(5) == "V"


def test_4_is_IV():
    assert to_roman(4) == "IV"


def test_10_is_X():
    assert to_roman(10) == "X"
