from anagram_detector import are_anagrams, find_anagrams, group_anagrams


def test_listen_and_silent_are_anagrams():
    assert are_anagrams("listen", "silent") is True


def test_hello_and_world_are_not_anagrams():
    assert are_anagrams("hello", "world") is False


def test_cat_and_tac_are_anagrams():
    assert are_anagrams("cat", "tac") is True


def test_a_word_is_not_an_anagram_of_itself():
    assert are_anagrams("cat", "cat") is False


def test_comparison_is_case_insensitive():
    assert are_anagrams("Cat", "tac") is True


def test_empty_strings_are_not_anagrams():
    assert are_anagrams("", "") is False


def test_single_identical_letters_are_not_anagrams():
    assert are_anagrams("a", "a") is False


def test_ab_and_ba_are_anagrams():
    assert are_anagrams("ab", "ba") is True


def test_aab_and_abb_are_not_anagrams_because_letter_counts_differ():
    assert are_anagrams("aab", "abb") is False


def test_astronomer_and_moon_starer_are_anagrams_ignoring_case_and_whitespace():
    assert are_anagrams("Astronomer", "Moon starer") is True


def test_rail_safety_and_fairy_tales_are_anagrams_across_multi_word_phrases():
    assert are_anagrams("rail safety", "fairy tales") is True


def test_find_anagrams_returns_all_matching_candidates():
    assert find_anagrams("listen", ["silent", "tinsel"]) == ["silent", "tinsel"]


def test_find_anagrams_returns_empty_when_no_candidate_matches():
    assert find_anagrams("listen", ["hello", "world"]) == []


def test_find_anagrams_preserves_input_order_for_mixed_matches():
    assert find_anagrams("master", ["stream", "maters", "pigeon"]) == ["stream", "maters"]


def test_group_anagrams_collects_words_sharing_one_anagram_key():
    assert group_anagrams(["eat", "tea", "ate"]) == [["eat", "tea", "ate"]]


def test_group_anagrams_returns_singletons_when_no_keys_are_shared():
    assert group_anagrams(["abc", "def"]) == [["abc"], ["def"]]


def test_group_anagrams_returns_an_empty_list_for_no_words():
    assert group_anagrams([]) == []


def test_group_anagrams_preserves_first_occurrence_order_of_groups_and_members():
    assert group_anagrams(["eat", "tea", "tan", "ate", "nat", "bat"]) == [
        ["eat", "tea", "ate"],
        ["tan", "nat"],
        ["bat"],
    ]
