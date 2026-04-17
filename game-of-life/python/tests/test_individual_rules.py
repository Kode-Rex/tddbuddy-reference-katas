from .grid_builder import GridBuilder


class TestIndividualRules:
    def test_a_live_cell_with_zero_neighbors_dies(self) -> None:
        grid = GridBuilder().with_living_cell_at(5, 5).build()

        next_gen = grid.tick()

        assert not next_gen.is_alive(5, 5)

    def test_a_live_cell_with_one_neighbor_dies(self) -> None:
        grid = GridBuilder().with_living_cells_at((0, 0), (0, 1)).build()

        next_gen = grid.tick()

        assert not next_gen.is_alive(0, 0)
        assert not next_gen.is_alive(0, 1)

    def test_a_live_cell_with_two_neighbors_survives(self) -> None:
        grid = GridBuilder().with_living_cells_at((0, 0), (0, 1), (0, 2)).build()

        next_gen = grid.tick()

        assert next_gen.is_alive(0, 1)

    def test_a_live_cell_with_three_neighbors_survives(self) -> None:
        grid = (
            GridBuilder()
            .with_living_cells_at((0, 0), (0, 1), (1, 0), (1, 1))
            .build()
        )

        next_gen = grid.tick()

        assert next_gen.is_alive(0, 0)

    def test_a_live_cell_with_four_neighbors_dies(self) -> None:
        grid = (
            GridBuilder()
            .with_living_cells_at((0, 1), (1, 0), (1, 1), (1, 2), (2, 1))
            .build()
        )

        next_gen = grid.tick()

        assert not next_gen.is_alive(1, 1)

    def test_a_dead_cell_with_exactly_three_neighbors_becomes_alive(self) -> None:
        grid = GridBuilder().with_living_cells_at((0, 0), (0, 1), (1, 0)).build()

        next_gen = grid.tick()

        assert next_gen.is_alive(1, 1)
