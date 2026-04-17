using FluentAssertions;

namespace GameOfLife.Tests;

public class OscillatorTests
{
    [Fact]
    public void Horizontal_blinker_becomes_vertical_after_one_tick()
    {
        var horizontal = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1), (0, 2))
            .Build();

        var vertical = horizontal.Tick();

        var expected = new Cell[] { new(-1, 1), new(0, 1), new(1, 1) };
        vertical.LivingCells.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Vertical_blinker_becomes_horizontal_after_one_tick()
    {
        var vertical = new GridBuilder()
            .WithLivingCellsAt((-1, 1), (0, 1), (1, 1))
            .Build();

        var horizontal = vertical.Tick();

        var expected = new Cell[] { new(0, 0), new(0, 1), new(0, 2) };
        horizontal.LivingCells.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Blinker_returns_to_its_original_state_after_two_ticks()
    {
        var original = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1), (0, 2))
            .Build();

        var afterTwo = original.Tick().Tick();

        afterTwo.LivingCells.Should().BeEquivalentTo(original.LivingCells);
    }
}
