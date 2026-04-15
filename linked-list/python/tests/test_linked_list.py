import pytest

from linked_list import LinkedList


def list_of(*values: int) -> LinkedList[int]:
    list_ = LinkedList[int]()
    for value in values:
        list_.append(value)
    return list_


def test_a_new_list_is_empty():
    list_ = LinkedList[int]()

    assert list_.size() == 0
    assert list_.to_array() == []


def test_appending_to_an_empty_list_yields_a_single_element():
    list_ = LinkedList[int]()

    list_.append(1)

    assert list_.size() == 1
    assert list_.to_array() == [1]


def test_appending_preserves_insertion_order():
    list_ = LinkedList[int]()

    list_.append(1)
    list_.append(2)
    list_.append(3)

    assert list_.to_array() == [1, 2, 3]


def test_prepending_to_an_empty_list_yields_a_single_element():
    list_ = LinkedList[int]()

    list_.prepend(1)

    assert list_.to_array() == [1]


def test_prepending_puts_the_value_at_the_front():
    list_ = LinkedList[int]()
    list_.append(1)
    list_.append(2)

    list_.prepend(0)

    assert list_.to_array() == [0, 1, 2]


def test_get_returns_the_value_at_the_given_index():
    list_ = list_of(0, 1, 2)

    assert list_.get(0) == 0
    assert list_.get(2) == 2


def test_get_on_an_out_of_range_index_raises():
    list_ = list_of(0, 1, 2)

    with pytest.raises(IndexError, match="index out of range: 5"):
        list_.get(5)


def test_get_on_a_negative_index_raises():
    list_ = list_of(0, 1, 2)

    with pytest.raises(IndexError, match="index out of range: -1"):
        list_.get(-1)


def test_remove_returns_the_value_and_shifts_subsequent_elements():
    list_ = list_of(0, 1, 2)

    removed = list_.remove(1)

    assert removed == 1
    assert list_.to_array() == [0, 2]


def test_remove_the_head_returns_the_first_value_and_leaves_the_tail():
    list_ = list_of(0, 2)

    removed = list_.remove(0)

    assert removed == 0
    assert list_.to_array() == [2]


def test_remove_the_only_element_leaves_an_empty_list():
    list_ = list_of(2)

    removed = list_.remove(0)

    assert removed == 2
    assert list_.size() == 0
    assert list_.to_array() == []


def test_remove_on_an_empty_list_raises():
    list_ = LinkedList[int]()

    with pytest.raises(IndexError, match="index out of range: 0"):
        list_.remove(0)


def test_contains_finds_an_existing_value():
    list_ = list_of(2)

    assert list_.contains(2) is True
    assert list_.contains(99) is False


def test_index_of_returns_the_first_occurrence():
    list_ = list_of(2)

    assert list_.index_of(2) == 0
    assert list_.index_of(99) == -1


def test_insert_at_the_head_shifts_existing_elements():
    list_ = list_of(2)

    list_.insert_at(0, 5)

    assert list_.to_array() == [5, 2]


def test_insert_at_the_middle_shifts_subsequent_elements():
    list_ = list_of(5, 2)

    list_.insert_at(1, 7)

    assert list_.to_array() == [5, 7, 2]


def test_insert_at_size_is_equivalent_to_append():
    list_ = list_of(5, 7, 2)

    list_.insert_at(3, 9)

    assert list_.to_array() == [5, 7, 2, 9]


def test_insert_at_an_out_of_range_index_raises():
    list_ = list_of(5, 7, 2)

    with pytest.raises(IndexError, match="index out of range: 10"):
        list_.insert_at(10, 9)

    with pytest.raises(IndexError, match="index out of range: -1"):
        list_.insert_at(-1, 9)
