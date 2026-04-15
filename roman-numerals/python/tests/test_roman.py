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


def test_9_is_IX():
    assert to_roman(9) == "IX"


def test_40_is_XL():
    assert to_roman(40) == "XL"


def test_90_is_XC():
    assert to_roman(90) == "XC"


def test_400_is_CD():
    assert to_roman(400) == "CD"


def test_900_is_CM():
    assert to_roman(900) == "CM"


def test_1000_is_M():
    assert to_roman(1000) == "M"


def test_1984_is_MCMLXXXIV():
    assert to_roman(1984) == "MCMLXXXIV"
