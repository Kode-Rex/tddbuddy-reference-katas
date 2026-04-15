import pytest

from bingo import NumberOutOfRangeError, WinPatterns

from tests.card_builder import CardBuilder


# A complete 5x5 card used by scenarios that need every cell populated.
# Column ranges are honoured for realism even though the builder does
# not enforce them: B 1-15, I 16-30, N 31-45 (free at (2,2)), G 46-60, O 61-75.
def a_full_card() -> CardBuilder:
    return (
        CardBuilder()
        .with_number_at(0, 0, 3).with_number_at(0, 1, 17).with_number_at(0, 2, 33).with_number_at(0, 3, 48).with_number_at(0, 4, 62)
        .with_number_at(1, 0, 8).with_number_at(1, 1, 22).with_number_at(1, 2, 38).with_number_at(1, 3, 52).with_number_at(1, 4, 67)
        .with_number_at(2, 0, 11).with_number_at(2, 1, 27)                              .with_number_at(2, 3, 55).with_number_at(2, 4, 70)
        .with_number_at(3, 0, 4).with_number_at(3, 1, 19).with_number_at(3, 2, 41).with_number_at(3, 3, 58).with_number_at(3, 4, 73)
        .with_number_at(4, 0, 15).with_number_at(4, 1, 30).with_number_at(4, 2, 45).with_number_at(4, 3, 60).with_number_at(4, 4, 75)
    )


def test_blank_card_reports_no_win_and_no_marks():
    card = a_full_card().build()

    assert card.has_won() is False
    assert card.winning_pattern() == WinPatterns.NONE
    for r in range(5):
        for c in range(5):
            if not (r == 2 and c == 2):
                assert card.is_marked(r, c) is False


def test_free_space_starts_marked():
    card = a_full_card().build()
    assert card.is_marked(2, 2) is True


def test_marking_a_number_that_is_on_the_card_marks_the_matching_cell():
    card = a_full_card().build()

    card.mark(3)

    assert card.is_marked(0, 0) is True
    assert card.is_marked(0, 1) is False


def test_marking_a_number_not_on_the_card_is_a_silent_no_op():
    card = a_full_card().build()

    card.mark(42)  # 42 is in the I-column range but not placed on this card

    for r in range(5):
        for c in range(5):
            if not (r == 2 and c == 2):
                assert card.is_marked(r, c) is False


def test_marking_a_number_outside_1_to_75_raises():
    card = a_full_card().build()

    with pytest.raises(NumberOutOfRangeError) as info:
        card.mark(0)
    assert str(info.value) == "called number must be between 1 and 75"

    with pytest.raises(NumberOutOfRangeError) as info:
        card.mark(76)
    assert str(info.value) == "called number must be between 1 and 75"


def test_completing_row_0_wins_on_that_row():
    card = a_full_card().build()

    for n in (3, 17, 33, 48, 62):
        card.mark(n)

    assert card.has_won() is True
    assert card.winning_pattern() == WinPatterns.row(0)


def test_completing_column_4_wins_on_that_column():
    card = a_full_card().build()

    for n in (62, 67, 70, 73, 75):
        card.mark(n)

    assert card.winning_pattern() == WinPatterns.column(4)


def test_completing_the_main_diagonal_wins_on_diagonal_main():
    card = a_full_card().build()

    for n in (3, 22, 58, 75):  # (2,2) is the free space
        card.mark(n)

    assert card.winning_pattern() == WinPatterns.DIAGONAL_MAIN


def test_completing_the_anti_diagonal_wins_on_diagonal_anti():
    card = a_full_card().build()

    for n in (62, 52, 19, 15):  # (2,2) is the free space
        card.mark(n)

    assert card.winning_pattern() == WinPatterns.DIAGONAL_ANTI


def test_four_marks_in_a_row_is_not_a_win():
    card = a_full_card().build()

    for n in (3, 17, 33, 48):
        card.mark(n)

    assert card.has_won() is False
    assert card.winning_pattern() == WinPatterns.NONE


def test_winning_pattern_scan_order_is_rows_then_columns_then_diagonals():
    card = (
        CardBuilder()
        .with_number_at(0, 0, 3).with_number_at(0, 1, 17).with_number_at(0, 2, 33).with_number_at(0, 3, 48).with_number_at(0, 4, 62)
        .with_number_at(1, 0, 8)
        .with_number_at(2, 0, 11)
        .with_number_at(3, 0, 4)
        .with_number_at(4, 0, 15)
        .with_mark_at(0, 0).with_mark_at(0, 1).with_mark_at(0, 2).with_mark_at(0, 3).with_mark_at(0, 4)
        .with_mark_at(1, 0).with_mark_at(2, 0).with_mark_at(3, 0).with_mark_at(4, 0)
        .build()
    )

    assert card.winning_pattern() == WinPatterns.row(0)


def test_card_builder_produces_the_card_the_test_literal_describes():
    card = CardBuilder().with_number_at(0, 0, 3).build()

    assert card.number_at(0, 0) == 3
    assert card.number_at(2, 2) is None
    assert card.is_marked(2, 2) is True
    assert card.is_marked(0, 0) is False

    card.mark(3)
    assert card.is_marked(0, 0) is True
