from change_maker import make_change

US_COINS = (25, 10, 5, 1)
UK_COINS = (50, 20, 10, 5, 2, 1)
NORWAY_COINS = (20, 10, 5, 1)


def test_zero_amount_returns_no_coins():
    assert make_change(0, US_COINS) == []


def test_us_one_cent_is_a_single_penny():
    assert make_change(1, US_COINS) == [1]


def test_us_five_cents_is_a_single_nickel():
    assert make_change(5, US_COINS) == [5]


def test_us_twenty_five_cents_is_a_single_quarter():
    assert make_change(25, US_COINS) == [25]


def test_us_thirty_cents_is_a_quarter_and_a_nickel():
    assert make_change(30, US_COINS) == [25, 5]


def test_us_forty_one_cents_is_a_quarter_a_dime_a_nickel_and_a_penny():
    assert make_change(41, US_COINS) == [25, 10, 5, 1]


def test_us_sixty_six_cents_is_two_quarters_a_dime_a_nickel_and_a_penny():
    assert make_change(66, US_COINS) == [25, 25, 10, 5, 1]


def test_uk_forty_three_pence_is_twenty_twenty_two_one():
    assert make_change(43, UK_COINS) == [20, 20, 2, 1]


def test_uk_eighty_eight_pence_is_one_of_each_british_coin():
    assert make_change(88, UK_COINS) == [50, 20, 10, 5, 2, 1]


def test_norway_thirty_seven_ore_is_twenty_ten_five_one_one():
    assert make_change(37, NORWAY_COINS) == [20, 10, 5, 1, 1]


def test_norway_forty_ore_is_two_twenty_ore_coins():
    assert make_change(40, NORWAY_COINS) == [20, 20]
