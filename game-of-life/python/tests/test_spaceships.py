from game_of_life import Cell

from .grid_builder import GridBuilder


class TestSpaceships:
    def test_glider_translates_one_cell_down_and_right_after_four_ticks(self) -> None:
        glider = (
            GridBuilder()
            .with_living_cells_at((0, 1), (1, 2), (2, 0), (2, 1), (2, 2))
            .build()
        )

        after4 = glider.tick().tick().tick().tick()

        expected = frozenset(
            {Cell(1, 2), Cell(2, 3), Cell(3, 1), Cell(3, 2), Cell(3, 3)}
        )
        assert after4.living_cells() == expected
