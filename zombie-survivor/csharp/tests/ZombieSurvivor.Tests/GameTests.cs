using FluentAssertions;
using Xunit;

namespace ZombieSurvivor.Tests;

public class GameTests
{
    [Fact]
    public void New_game_starts_with_zero_survivors()
    {
        var (game, _) = new HistoryBuilder().Build();

        game.Survivors.Should().BeEmpty();
    }

    [Fact]
    public void Adding_a_survivor_increases_the_survivor_count()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .Build();

        game.Survivors.Should().HaveCount(1);
    }

    [Fact]
    public void Adding_a_survivor_with_a_duplicate_name_is_rejected()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .Build();

        var act = () => game.AddSurvivor(new Survivor("Alice"));

        act.Should().Throw<DuplicateSurvivorNameException>()
            .WithMessage("A survivor named 'Alice' already exists.");
    }

    [Fact]
    public void Game_ends_when_all_survivors_are_dead()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithWound("Alice")
            .WithWound("Alice")
            .Build();

        game.HasEnded.Should().BeTrue();
    }
}
