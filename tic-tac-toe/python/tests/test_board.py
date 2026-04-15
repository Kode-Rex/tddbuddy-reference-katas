import pytest

from tic_tac_toe import (
    Board,
    Cell,
    CellOccupiedError,
    GameOverError,
    Outcome,
    OutOfBoundsError,
)

from tests.board_builder import BoardBuilder


def test_empty_board_reports_game_in_progress():
    board = Board()
    assert board.outcome() is Outcome.IN_PROGRESS
    assert board.current_turn() is Cell.X


def test_first_placement_puts_x_on_the_board():
    board = Board().place(0, 0)
    assert board.cell_at(0, 0) is Cell.X
    assert board.current_turn() is Cell.O
    assert board.outcome() is Outcome.IN_PROGRESS


def test_x_wins_by_completing_the_top_row():
    board = (
        BoardBuilder()
        .with_x_at(0, 0).with_x_at(0, 1)
        .with_o_at(1, 0).with_o_at(1, 1)
        .build()
    )
    assert board.place(0, 2).outcome() is Outcome.X_WINS


def test_x_wins_by_completing_the_left_column():
    board = (
        BoardBuilder()
        .with_x_at(0, 0).with_x_at(1, 0)
        .with_o_at(0, 1).with_o_at(1, 1)
        .build()
    )
    assert board.place(2, 0).outcome() is Outcome.X_WINS


def test_x_wins_on_the_main_diagonal():
    board = (
        BoardBuilder()
        .with_x_at(0, 0).with_x_at(1, 1)
        .with_o_at(0, 1).with_o_at(0, 2)
        .build()
    )
    assert board.place(2, 2).outcome() is Outcome.X_WINS


def test_o_wins_on_the_anti_diagonal():
    board = (
        BoardBuilder()
        .with_x_at(0, 0).with_x_at(1, 0).with_x_at(2, 1)
        .with_o_at(0, 2).with_o_at(1, 1)
        .build()
    )
    assert board.place(2, 0).outcome() is Outcome.O_WINS


def test_full_board_with_no_winning_line_is_a_draw():
    board = (
        BoardBuilder()
        .with_x_at(0, 0).with_o_at(0, 1).with_x_at(0, 2)
        .with_x_at(1, 0).with_x_at(1, 1).with_o_at(1, 2)
        .with_o_at(2, 0).with_x_at(2, 1).with_o_at(2, 2)
        .build()
    )
    assert board.outcome() is Outcome.DRAW


def test_placing_on_an_occupied_cell_raises_cell_occupied():
    board = BoardBuilder().with_x_at(0, 0).build()
    with pytest.raises(CellOccupiedError) as info:
        board.place(0, 0)
    assert str(info.value) == "cell already occupied"


def test_placing_with_a_row_out_of_bounds_raises_out_of_bounds():
    board = Board()
    with pytest.raises(OutOfBoundsError) as info:
        board.place(3, 0)
    assert str(info.value) == "coordinates out of bounds"
    with pytest.raises(OutOfBoundsError):
        board.place(-1, 0)


def test_placing_with_a_column_out_of_bounds_raises_out_of_bounds():
    board = Board()
    with pytest.raises(OutOfBoundsError) as info:
        board.place(0, 3)
    assert str(info.value) == "coordinates out of bounds"


def test_placing_after_a_win_raises_game_over():
    won = (
        BoardBuilder()
        .with_x_at(0, 0).with_x_at(0, 1).with_x_at(0, 2)
        .with_o_at(1, 0).with_o_at(1, 1)
        .build()
    )
    with pytest.raises(GameOverError) as info:
        won.place(2, 2)
    assert str(info.value) == "game is already over"


def test_board_builder_produces_the_board_the_test_literal_describes():
    board = BoardBuilder().with_x_at(0, 0).with_o_at(1, 1).build()
    assert board.cell_at(0, 0) is Cell.X
    assert board.cell_at(1, 1) is Cell.O
    assert board.cell_at(2, 2) is Cell.EMPTY
    assert board.outcome() is Outcome.IN_PROGRESS
    assert board.current_turn() is Cell.X

    one_ahead = BoardBuilder().with_x_at(0, 0).build()
    assert one_ahead.current_turn() is Cell.O
