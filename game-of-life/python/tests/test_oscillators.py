from game_of_life import Cell

from .grid_builder import GridBuilder


class TestOscillators:
    def test_horizontal_blinker_becomes_vertical_after_one_tick(self) -> None:
        horizontal = (
            GridBuilder().with_living_cells_at((0, 0), (0, 1), (0, 2)).build()
        )

        vertical = horizontal.tick()

        expected = frozenset({Cell(-1, 1), Cell(0, 1), Cell(1, 1)})
        assert vertical.living_cells() == expected

    def test_vertical_blinker_becomes_horizontal_after_one_tick(self) -> None:
        vertical = (
            GridBuilder().with_living_cells_at((-1, 1), (0, 1), (1, 1)).build()
        )

        horizontal = vertical.tick()

        expected = frozenset({Cell(0, 0), Cell(0, 1), Cell(0, 2)})
        assert horizontal.living_cells() == expected

    def test_blinker_returns_to_its_original_state_after_two_ticks(self) -> None:
        original = (
            GridBuilder().with_living_cells_at((0, 0), (0, 1), (0, 2)).build()
        )

        after_two = original.tick().tick()

        assert after_two.living_cells() == original.living_cells()
