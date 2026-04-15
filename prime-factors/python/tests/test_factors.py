from prime_factors.factors import generate


def test_one_has_no_prime_factors():
    assert generate(1) == []


def test_two_is_its_own_only_prime_factor():
    assert generate(2) == [2]


def test_three_is_its_own_only_prime_factor():
    assert generate(3) == [3]


def test_four_factors_into_two_twos():
    assert generate(4) == [2, 2]


def test_six_factors_into_two_and_three():
    assert generate(6) == [2, 3]


def test_eight_factors_into_three_twos():
    assert generate(8) == [2, 2, 2]


def test_nine_factors_into_two_threes():
    assert generate(9) == [3, 3]
