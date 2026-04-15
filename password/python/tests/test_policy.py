from tests.policy_builder import PolicyBuilder


def test_policy_with_min_length_8_accepts_an_8_character_password():
    policy = PolicyBuilder().min_length(8).build()
    assert policy.validate("abcd1234").ok is True


def test_policy_with_min_length_8_rejects_a_6_character_password():
    policy = PolicyBuilder().min_length(8).build()
    result = policy.validate("abc123")
    assert result.ok is False
    assert result.failures == ["minimum length"]


def test_policy_requiring_a_digit_accepts_a_password_with_a_digit():
    policy = PolicyBuilder().requires_digit().build()
    assert policy.validate("password1").ok is True


def test_policy_requiring_a_digit_rejects_a_password_with_no_digit():
    policy = PolicyBuilder().requires_digit().build()
    result = policy.validate("password")
    assert result.ok is False
    assert result.failures == ["requires digit"]


def test_policy_requiring_a_symbol_accepts_a_password_with_a_symbol():
    policy = PolicyBuilder().requires_symbol().build()
    assert policy.validate("password!").ok is True


def test_policy_requiring_a_symbol_rejects_a_password_with_no_symbol():
    policy = PolicyBuilder().requires_symbol().build()
    result = policy.validate("password1")
    assert result.ok is False
    assert result.failures == ["requires symbol"]


def test_policy_requiring_uppercase_rejects_an_all_lowercase_password():
    policy = PolicyBuilder().requires_upper().build()
    result = policy.validate("password1")
    assert result.ok is False
    assert result.failures == ["requires uppercase"]


def test_policy_requiring_lowercase_rejects_an_all_uppercase_password():
    policy = PolicyBuilder().requires_lower().build()
    result = policy.validate("PASSWORD1")
    assert result.ok is False
    assert result.failures == ["requires lowercase"]


def test_policy_with_multiple_requirements_reports_every_failed_rule():
    policy = (
        PolicyBuilder()
        .min_length(8)
        .requires_digit()
        .requires_symbol()
        .requires_upper()
        .requires_lower()
        .build()
    )

    assert policy.validate("Abcd123!").ok is True

    result = policy.validate("abc")
    assert result.ok is False
    assert result.failures == [
        "minimum length",
        "requires digit",
        "requires symbol",
        "requires uppercase",
    ]


def test_policy_builder_default_is_min_length_8_with_no_character_class_requirements():
    policy = PolicyBuilder().build()
    assert policy.validate("abcdefgh").ok is True
    result = policy.validate("abcdefg")
    assert result.ok is False
    assert result.failures == ["minimum length"]
