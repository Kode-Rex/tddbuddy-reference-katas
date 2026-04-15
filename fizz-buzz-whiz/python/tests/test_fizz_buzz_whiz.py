from fizz_buzz_whiz import say


def test_one_returns_the_number_as_a_string():
    assert say(1) == "1"


def test_two_returns_the_number_as_a_string():
    assert say(2) == "2"


def test_three_is_divisible_by_three_and_returns_fizz():
    assert say(3) == "Fizz"


def test_five_is_divisible_by_five_and_returns_buzz():
    assert say(5) == "Buzz"


def test_six_is_divisible_by_three_and_returns_fizz():
    assert say(6) == "Fizz"


def test_ten_is_divisible_by_five_and_returns_buzz():
    assert say(10) == "Buzz"


def test_fifteen_is_divisible_by_both_three_and_five_and_returns_fizzbuzz():
    assert say(15) == "FizzBuzz"


def test_thirty_is_divisible_by_both_three_and_five_and_returns_fizzbuzz():
    assert say(30) == "FizzBuzz"
