using FluentAssertions;

namespace GameOfLife.Tests;

public class GridQueryTests
{
    [Fact]
    public void IsAlive_returns_true_for_a_living_cell()
    {
        var grid = new GridBuilder()
            .WithLivingCellAt(3, 4)
            .Build();

        grid.IsAlive(3, 4).Should().BeTrue();
    }

    [Fact]
    public void IsAlive_returns_false_for_a_dead_cell()
    {
        var grid = new GridBuilder()
            .WithLivingCellAt(3, 4)
            .Build();

        grid.IsAlive(0, 0).Should().BeFalse();
    }

    [Fact]
    public void LivingCells_returns_all_living_cells_in_the_grid()
    {
        var grid = new GridBuilder()
            .WithLivingCellsAt((1, 2), (3, 4))
            .Build();

        grid.LivingCells.Should().BeEquivalentTo(
            new Cell[] { new(1, 2), new(3, 4) });
    }

    [Fact]
    public void An_empty_grid_has_no_living_cells()
    {
        var grid = new GridBuilder().Build();

        grid.LivingCells.Should().BeEmpty();
    }
}
