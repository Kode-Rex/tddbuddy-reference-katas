import pytest

from mars_rover import (
    Direction,
    MovementStatus,
    UnknownCommandError,
)

from tests.command_builder import CommandBuilder
from tests.rover_builder import RoverBuilder


def test_new_rover_reports_its_starting_position_heading_and_moving_status():
    rover = RoverBuilder().at(3, 4).facing(Direction.NORTH).on_grid(100, 100).build()

    assert rover.position == (3, 4)
    assert rover.heading is Direction.NORTH
    assert rover.status is MovementStatus.MOVING
    assert rover.last_obstacle is None


def test_forward_moves_one_square_in_the_direction_the_rover_is_facing():
    def step(d: Direction):
        return RoverBuilder().at(5, 5).facing(d).on_grid(100, 100).build().execute("F")

    assert step(Direction.NORTH).position == (5, 4)
    assert step(Direction.EAST).position == (6, 5)
    assert step(Direction.SOUTH).position == (5, 6)
    assert step(Direction.WEST).position == (4, 5)


def test_backward_moves_one_square_opposite_the_heading():
    north = RoverBuilder().at(5, 5).facing(Direction.NORTH).on_grid(100, 100).build().execute("B")
    east = RoverBuilder().at(5, 5).facing(Direction.EAST).on_grid(100, 100).build().execute("B")

    assert north.position == (5, 6)
    assert east.position == (4, 5)


def test_left_rotates_the_heading_counter_clockwise():
    start = RoverBuilder().at(5, 5).facing(Direction.NORTH).on_grid(100, 100).build()

    assert start.execute("L").heading is Direction.WEST
    assert start.execute("LL").heading is Direction.SOUTH
    assert start.execute("LLL").heading is Direction.EAST
    assert start.execute("LLLL").heading is Direction.NORTH
    assert start.execute("L").position == (5, 5)


def test_right_rotates_the_heading_clockwise():
    start = RoverBuilder().at(5, 5).facing(Direction.NORTH).on_grid(100, 100).build()

    assert start.execute("R").heading is Direction.EAST
    assert start.execute("RR").heading is Direction.SOUTH
    assert start.execute("RRR").heading is Direction.WEST
    assert start.execute("RRRR").heading is Direction.NORTH
    assert start.execute("R").position == (5, 5)


def test_execute_applies_commands_in_order():
    rover = RoverBuilder().at(0, 0).facing(Direction.NORTH).on_grid(100, 100).build()

    after = rover.execute("FFRFF")

    assert after.position == (2, 98)
    assert after.heading is Direction.EAST


def test_kata_brief_example_zero_zero_facing_south_ffLff_lands_at_two_two():
    rover = RoverBuilder().at(0, 0).facing(Direction.SOUTH).on_grid(100, 100).build()

    after = rover.execute("FFLFF")

    assert after.position == (2, 2)
    assert after.heading is Direction.EAST


def test_wrapping_across_the_north_edge():
    rover = RoverBuilder().at(0, 0).facing(Direction.NORTH).on_grid(100, 100).build()
    assert rover.execute("F").position == (0, 99)


def test_wrapping_across_the_east_edge():
    rover = RoverBuilder().at(99, 50).facing(Direction.EAST).on_grid(100, 100).build()
    assert rover.execute("F").position == (0, 50)


def test_obstacle_blocks_a_forward_move():
    rover = (
        RoverBuilder()
        .at(0, 0).facing(Direction.EAST).on_grid(100, 100)
        .with_obstacle_at(1, 0).build()
    )
    after = rover.execute("F")

    assert after.position == (0, 0)
    assert after.status is MovementStatus.BLOCKED
    assert after.last_obstacle == (1, 0)


def test_remaining_commands_after_block_are_discarded():
    rover = (
        RoverBuilder()
        .at(0, 0).facing(Direction.EAST).on_grid(100, 100)
        .with_obstacle_at(2, 0).build()
    )
    after = rover.execute("FFRFF")

    assert after.position == (1, 0)
    assert after.heading is Direction.EAST
    assert after.status is MovementStatus.BLOCKED
    assert after.last_obstacle == (2, 0)


def test_obstacle_blocks_a_backward_move():
    rover = (
        RoverBuilder()
        .at(2, 0).facing(Direction.EAST).on_grid(100, 100)
        .with_obstacle_at(1, 0).build()
    )
    after = rover.execute("B")

    assert after.position == (2, 0)
    assert after.status is MovementStatus.BLOCKED
    assert after.last_obstacle == (1, 0)


def test_empty_command_string_leaves_the_rover_unchanged():
    rover = RoverBuilder().at(3, 4).facing(Direction.WEST).on_grid(100, 100).build()
    after = rover.execute("")

    assert after.position == (3, 4)
    assert after.heading is Direction.WEST
    assert after.status is MovementStatus.MOVING


def test_unknown_command_character_raises_an_error():
    rover = RoverBuilder().at(0, 0).facing(Direction.NORTH).on_grid(100, 100).build()

    with pytest.raises(UnknownCommandError) as info:
        rover.execute("FX")
    assert str(info.value) == "unknown command"


def test_builders_produce_the_literal_the_test_describes():
    rover = (
        RoverBuilder()
        .at(2, 3).facing(Direction.WEST).on_grid(10, 10)
        .with_obstacle_at(1, 3).build()
    )

    assert rover.position == (2, 3)
    assert rover.heading is Direction.WEST
    assert rover.grid_width == 10
    assert rover.grid_height == 10
    assert rover.has_obstacle_at(1, 3)
    assert not rover.has_obstacle_at(0, 0)

    commands = CommandBuilder().forward().forward().left().right().backward().build()
    assert commands == "FFLRB"
