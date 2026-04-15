from ip_validator import is_valid


def test_1_1_1_1_is_valid():
    assert is_valid("1.1.1.1") is True


def test_192_168_1_1_is_valid():
    assert is_valid("192.168.1.1") is True


def test_10_0_0_1_is_valid():
    assert is_valid("10.0.0.1") is True


def test_127_0_0_1_is_valid():
    assert is_valid("127.0.0.1") is True


def test_0_0_0_0_is_invalid_because_last_octet_is_zero():
    assert is_valid("0.0.0.0") is False


def test_255_255_255_255_is_invalid_because_last_octet_is_broadcast():
    assert is_valid("255.255.255.255") is False


def test_192_168_1_0_is_invalid_because_last_octet_is_zero():
    assert is_valid("192.168.1.0") is False


def test_192_168_1_255_is_invalid_because_last_octet_is_broadcast():
    assert is_valid("192.168.1.255") is False


def test_10_0_1_is_invalid_because_it_has_only_three_octets():
    assert is_valid("10.0.1") is False


def test_1_2_3_4_5_is_invalid_because_it_has_five_octets():
    assert is_valid("1.2.3.4.5") is False


def test_192_168_01_1_is_invalid_because_an_octet_has_a_leading_zero():
    assert is_valid("192.168.01.1") is False


def test_192_168_1_00_is_invalid_because_an_octet_has_leading_zeros():
    assert is_valid("192.168.1.00") is False


def test_256_1_1_1_is_invalid_because_first_octet_exceeds_255():
    assert is_valid("256.1.1.1") is False


def test_1_1_1_999_is_invalid_because_last_octet_exceeds_255():
    assert is_valid("1.1.1.999") is False


def test_address_with_negative_octet_is_invalid():
    assert is_valid("1.1.1.-1") is False


def test_address_with_non_digit_character_is_invalid():
    assert is_valid("1.1.1.a") is False


def test_address_with_empty_octet_is_invalid():
    assert is_valid("1.1..1") is False


def test_empty_string_is_invalid():
    assert is_valid("") is False
