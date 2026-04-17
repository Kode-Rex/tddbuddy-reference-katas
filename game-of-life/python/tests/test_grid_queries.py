from game_of_life import Cell

from .grid_builder import GridBuilder


class TestGridQueries:
    def test_is_alive_returns_true_for_a_living_cell(self) -> None:
        grid = GridBuilder().with_living_cell_at(3, 4).build()

        assert grid.is_alive(3, 4)

    def test_is_alive_returns_false_for_a_dead_cell(self) -> None:
        grid = GridBuilder().with_living_cell_at(3, 4).build()

        assert not grid.is_alive(0, 0)

    def test_living_cells_returns_all_living_cells_in_the_grid(self) -> None:
        grid = GridBuilder().with_living_cells_at((1, 2), (3, 4)).build()

        assert grid.living_cells() == frozenset({Cell(1, 2), Cell(3, 4)})

    def test_an_empty_grid_has_no_living_cells(self) -> None:
        grid = GridBuilder().build()

        assert len(grid.living_cells()) == 0
