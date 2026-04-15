from prime_factors.factors import generate


def test_one_has_no_prime_factors():
    assert generate(1) == []
