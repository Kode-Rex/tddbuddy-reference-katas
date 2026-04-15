import pytest

from tests.page_request_builder import PageRequestBuilder


def test_first_page_of_100_items_with_page_size_10_shows_items_1_through_10():
    request = PageRequestBuilder().total_items(100).page_size(10).page_number(1).build()
    assert request.start_item == 1
    assert request.end_item == 10
    assert request.has_previous is False
    assert request.has_next is True
    assert request.total_pages == 10


def test_middle_page_of_100_items_with_page_size_10_shows_items_41_through_50():
    request = PageRequestBuilder().total_items(100).page_size(10).page_number(5).build()
    assert request.start_item == 41
    assert request.end_item == 50
    assert request.has_previous is True
    assert request.has_next is True


def test_last_page_of_100_items_with_page_size_10_shows_items_91_through_100():
    request = PageRequestBuilder().total_items(100).page_size(10).page_number(10).build()
    assert request.start_item == 91
    assert request.end_item == 100
    assert request.has_previous is True
    assert request.has_next is False


def test_last_page_with_a_partial_window_shows_only_the_remaining_items():
    request = PageRequestBuilder().total_items(95).page_size(10).page_number(10).build()
    assert request.start_item == 91
    assert request.end_item == 95
    assert request.total_pages == 10


def test_single_item_on_one_page_reports_itself_as_start_and_end():
    request = PageRequestBuilder().total_items(1).page_size(10).page_number(1).build()
    assert request.start_item == 1
    assert request.end_item == 1
    assert request.has_previous is False
    assert request.has_next is False
    assert request.total_pages == 1


def test_zero_items_reports_no_pages_and_no_items():
    request = PageRequestBuilder().total_items(0).page_size(10).page_number(1).build()
    assert request.total_pages == 0
    assert request.start_item == 0
    assert request.end_item == 0
    assert request.has_previous is False
    assert request.has_next is False


def test_page_number_below_1_is_clamped_to_1():
    request = PageRequestBuilder().total_items(100).page_size(10).page_number(0).build()
    assert request.page_number == 1
    assert request.start_item == 1
    assert request.end_item == 10


def test_page_number_above_total_pages_is_clamped_to_the_last_page():
    request = PageRequestBuilder().total_items(100).page_size(10).page_number(99).build()
    assert request.page_number == 10
    assert request.start_item == 91
    assert request.end_item == 100


def test_page_size_of_1_gives_every_item_its_own_page():
    request = PageRequestBuilder().total_items(5).page_size(1).page_number(3).build()
    assert request.start_item == 3
    assert request.end_item == 3
    assert request.total_pages == 5
    assert request.has_previous is True
    assert request.has_next is True


def test_negative_total_items_is_rejected_at_construction():
    with pytest.raises(ValueError, match="totalItems must be >= 0"):
        PageRequestBuilder().total_items(-1).build()


def test_page_size_below_1_is_rejected_at_construction():
    with pytest.raises(ValueError, match="pageSize must be >= 1"):
        PageRequestBuilder().page_size(0).build()


def test_page_window_centers_on_the_current_page_when_there_is_room():
    first = PageRequestBuilder().total_items(100).page_size(10).page_number(1).build()
    middle = PageRequestBuilder().total_items(100).page_size(10).page_number(5).build()
    last = PageRequestBuilder().total_items(100).page_size(10).page_number(10).build()

    assert first.page_window(5) == [1, 2, 3, 4, 5]
    assert middle.page_window(5) == [3, 4, 5, 6, 7]
    assert last.page_window(5) == [6, 7, 8, 9, 10]


def test_page_window_is_clipped_when_total_pages_is_smaller_than_the_window():
    request = PageRequestBuilder().total_items(40).page_size(10).page_number(3).build()
    assert request.page_window(5) == [1, 2, 3, 4]


def test_page_window_on_an_empty_dataset_is_empty():
    request = PageRequestBuilder().total_items(0).page_size(10).page_number(1).build()
    assert request.page_window(5) == []
