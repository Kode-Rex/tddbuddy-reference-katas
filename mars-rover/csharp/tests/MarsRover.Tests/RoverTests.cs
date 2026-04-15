using FluentAssertions;
using Xunit;

namespace MarsRover.Tests;

public class RoverTests
{
    [Fact]
    public void New_rover_reports_its_starting_position_heading_and_moving_status()
    {
        var rover = new RoverBuilder()
            .At(3, 4).Facing(Direction.North).OnGrid(100, 100).Build();

        rover.Position.Should().Be((3, 4));
        rover.Heading.Should().Be(Direction.North);
        rover.Status.Should().Be(MovementStatus.Moving);
        rover.LastObstacle.Should().BeNull();
    }

    [Fact]
    public void Forward_moves_one_square_in_the_direction_the_rover_is_facing()
    {
        var facing = new Func<Direction, Rover>(d =>
            new RoverBuilder().At(5, 5).Facing(d).OnGrid(100, 100).Build().Execute("F"));

        facing(Direction.North).Position.Should().Be((5, 4));
        facing(Direction.East).Position.Should().Be((6, 5));
        facing(Direction.South).Position.Should().Be((5, 6));
        facing(Direction.West).Position.Should().Be((4, 5));
    }

    [Fact]
    public void Backward_moves_one_square_opposite_the_heading()
    {
        var north = new RoverBuilder().At(5, 5).Facing(Direction.North).OnGrid(100, 100).Build().Execute("B");
        var east = new RoverBuilder().At(5, 5).Facing(Direction.East).OnGrid(100, 100).Build().Execute("B");

        north.Position.Should().Be((5, 6));
        east.Position.Should().Be((4, 5));
    }

    [Fact]
    public void Left_rotates_the_heading_counter_clockwise()
    {
        var start = new RoverBuilder().At(5, 5).Facing(Direction.North).OnGrid(100, 100).Build();

        start.Execute("L").Heading.Should().Be(Direction.West);
        start.Execute("LL").Heading.Should().Be(Direction.South);
        start.Execute("LLL").Heading.Should().Be(Direction.East);
        start.Execute("LLLL").Heading.Should().Be(Direction.North);
        start.Execute("L").Position.Should().Be((5, 5));
    }

    [Fact]
    public void Right_rotates_the_heading_clockwise()
    {
        var start = new RoverBuilder().At(5, 5).Facing(Direction.North).OnGrid(100, 100).Build();

        start.Execute("R").Heading.Should().Be(Direction.East);
        start.Execute("RR").Heading.Should().Be(Direction.South);
        start.Execute("RRR").Heading.Should().Be(Direction.West);
        start.Execute("RRRR").Heading.Should().Be(Direction.North);
        start.Execute("R").Position.Should().Be((5, 5));
    }

    [Fact]
    public void Execute_applies_commands_in_order()
    {
        var rover = new RoverBuilder().At(0, 0).Facing(Direction.North).OnGrid(100, 100).Build();

        var after = rover.Execute("FFRFF");

        after.Position.Should().Be((2, 98));
        after.Heading.Should().Be(Direction.East);
    }

    [Fact]
    public void Kata_brief_example_zero_zero_facing_south_ffLff_lands_at_two_two()
    {
        var rover = new RoverBuilder().At(0, 0).Facing(Direction.South).OnGrid(100, 100).Build();

        var after = rover.Execute("FFLFF");

        after.Position.Should().Be((2, 2));
        after.Heading.Should().Be(Direction.East);
    }

    [Fact]
    public void Wrapping_across_the_north_edge()
    {
        var rover = new RoverBuilder().At(0, 0).Facing(Direction.North).OnGrid(100, 100).Build();

        rover.Execute("F").Position.Should().Be((0, 99));
    }

    [Fact]
    public void Wrapping_across_the_east_edge()
    {
        var rover = new RoverBuilder().At(99, 50).Facing(Direction.East).OnGrid(100, 100).Build();

        rover.Execute("F").Position.Should().Be((0, 50));
    }

    [Fact]
    public void Obstacle_blocks_a_forward_move()
    {
        var rover = new RoverBuilder()
            .At(0, 0).Facing(Direction.East).OnGrid(100, 100)
            .WithObstacleAt(1, 0).Build();

        var after = rover.Execute("F");

        after.Position.Should().Be((0, 0));
        after.Status.Should().Be(MovementStatus.Blocked);
        after.LastObstacle.Should().Be((1, 0));
    }

    [Fact]
    public void Remaining_commands_after_block_are_discarded()
    {
        var rover = new RoverBuilder()
            .At(0, 0).Facing(Direction.East).OnGrid(100, 100)
            .WithObstacleAt(2, 0).Build();

        var after = rover.Execute("FFRFF");

        after.Position.Should().Be((1, 0));
        after.Heading.Should().Be(Direction.East);
        after.Status.Should().Be(MovementStatus.Blocked);
        after.LastObstacle.Should().Be((2, 0));
    }

    [Fact]
    public void Obstacle_blocks_a_backward_move()
    {
        var rover = new RoverBuilder()
            .At(2, 0).Facing(Direction.East).OnGrid(100, 100)
            .WithObstacleAt(1, 0).Build();

        var after = rover.Execute("B");

        after.Position.Should().Be((2, 0));
        after.Status.Should().Be(MovementStatus.Blocked);
        after.LastObstacle.Should().Be((1, 0));
    }

    [Fact]
    public void Empty_command_string_leaves_the_rover_unchanged()
    {
        var rover = new RoverBuilder().At(3, 4).Facing(Direction.West).OnGrid(100, 100).Build();

        var after = rover.Execute("");

        after.Position.Should().Be((3, 4));
        after.Heading.Should().Be(Direction.West);
        after.Status.Should().Be(MovementStatus.Moving);
    }

    [Fact]
    public void Unknown_command_character_raises_an_error()
    {
        var rover = new RoverBuilder().At(0, 0).Facing(Direction.North).OnGrid(100, 100).Build();

        var act = () => rover.Execute("FX");

        act.Should().Throw<UnknownCommandException>().WithMessage("unknown command*");
    }

    [Fact]
    public void Builders_produce_the_literal_the_test_describes()
    {
        var rover = new RoverBuilder()
            .At(2, 3).Facing(Direction.West).OnGrid(10, 10)
            .WithObstacleAt(1, 3).Build();

        rover.Position.Should().Be((2, 3));
        rover.Heading.Should().Be(Direction.West);
        rover.GridWidth.Should().Be(10);
        rover.GridHeight.Should().Be(10);
        rover.Obstacles.Should().ContainSingle().Which.Should().Be((1, 3));

        var commands = new CommandBuilder()
            .Forward().Forward().Left().Right().Backward().Build();
        commands.Should().Be("FFLRB");
    }
}
