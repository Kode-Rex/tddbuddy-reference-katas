using FluentAssertions;

namespace GameOfLife.Tests;

public class IndividualRuleTests
{
    [Fact]
    public void A_live_cell_with_zero_neighbors_dies()
    {
        var grid = new GridBuilder()
            .WithLivingCellAt(5, 5)
            .Build();

        var next = grid.Tick();

        next.IsAlive(5, 5).Should().BeFalse();
    }

    [Fact]
    public void A_live_cell_with_one_neighbor_dies()
    {
        var grid = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1))
            .Build();

        var next = grid.Tick();

        next.IsAlive(0, 0).Should().BeFalse();
        next.IsAlive(0, 1).Should().BeFalse();
    }

    [Fact]
    public void A_live_cell_with_two_neighbors_survives()
    {
        // Three cells in a row: center has two neighbors
        var grid = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1), (0, 2))
            .Build();

        var next = grid.Tick();

        next.IsAlive(0, 1).Should().BeTrue();
    }

    [Fact]
    public void A_live_cell_with_three_neighbors_survives()
    {
        // L-shape: (0,0) has neighbors at (0,1), (1,0), (1,1)
        var grid = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1), (1, 0), (1, 1))
            .Build();

        var next = grid.Tick();

        next.IsAlive(0, 0).Should().BeTrue();
    }

    [Fact]
    public void A_live_cell_with_four_neighbors_dies()
    {
        // Cross shape: center at (1,1) with four orthogonal neighbors
        var grid = new GridBuilder()
            .WithLivingCellsAt((0, 1), (1, 0), (1, 1), (1, 2), (2, 1))
            .Build();

        var next = grid.Tick();

        next.IsAlive(1, 1).Should().BeFalse();
    }

    [Fact]
    public void A_dead_cell_with_exactly_three_neighbors_becomes_alive()
    {
        // Three cells in an L: dead cell at (1,1) has three living neighbors
        var grid = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1), (1, 0))
            .Build();

        var next = grid.Tick();

        next.IsAlive(1, 1).Should().BeTrue();
    }
}
