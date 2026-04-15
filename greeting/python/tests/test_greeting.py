from greeting import greet


def test_a_single_name_is_greeted_by_name():
    assert greet("Bob") == "Hello, Bob."


def test_a_null_name_is_greeted_as_my_friend():
    assert greet(None) == "Hello, my friend."


def test_an_all_caps_name_is_shouted_back():
    assert greet("JERRY") == "HELLO JERRY!"


def test_two_names_are_joined_with_and():
    assert greet(["Jill", "Jane"]) == "Hello, Jill and Jane"


def test_three_or_more_names_are_joined_with_an_oxford_comma():
    assert greet(["Amy", "Brian", "Charlotte"]) == "Hello, Amy, Brian, and Charlotte"


def test_mixed_normal_and_shouted_names_split_into_two_greetings():
    assert greet(["Amy", "BRIAN", "Charlotte"]) == "Hello, Amy and Charlotte. AND HELLO BRIAN!"
