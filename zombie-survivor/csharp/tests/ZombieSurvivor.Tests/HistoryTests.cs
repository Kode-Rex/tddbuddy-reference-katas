using FluentAssertions;
using Xunit;

namespace ZombieSurvivor.Tests;

public class HistoryTests
{
    [Fact]
    public void New_game_records_a_game_started_event()
    {
        var (game, _) = new HistoryBuilder().Build();

        game.History.Should().ContainSingle()
            .Which.Description.Should().Be("Game started.");
    }

    [Fact]
    public void Adding_a_survivor_records_a_survivor_added_event()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .Build();

        game.History.Should().Contain(e => e.Description == "Survivor added: Alice.");
    }

    [Fact]
    public void Receiving_a_wound_records_a_wound_received_event()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithWound("Alice")
            .Build();

        game.History.Should().Contain(e => e.Description == "Wound received: Alice.");
    }

    [Fact]
    public void Survivor_death_records_a_survivor_died_event()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithWound("Alice")
            .WithWound("Alice")
            .Build();

        game.History.Should().Contain(e => e.Description == "Survivor died: Alice.");
    }

    [Fact]
    public void Leveling_up_records_a_level_up_event()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithZombieKill("Alice", 7)
            .Build();

        game.History.Should().Contain(e => e.Description == "Level up: Alice reached Yellow.");
    }

    [Fact]
    public void Game_level_change_records_a_game_level_changed_event()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithZombieKill("Alice", 7)
            .Build();

        game.History.Should().Contain(e => e.Description == "Game level changed to Yellow.");
    }

    [Fact]
    public void Game_ending_records_a_game_ended_event()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithWound("Alice")
            .WithWound("Alice")
            .Build();

        game.History.Should().Contain(e => e.Description == "Game ended.");
    }
}
