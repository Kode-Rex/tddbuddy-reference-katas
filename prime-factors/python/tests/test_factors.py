from prime_factors.factors import generate


def test_one_has_no_prime_factors():
    assert generate(1) == []


def test_two_is_its_own_only_prime_factor():
    assert generate(2) == [2]
