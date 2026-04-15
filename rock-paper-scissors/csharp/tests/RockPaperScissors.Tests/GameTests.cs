using FluentAssertions;
using Xunit;

namespace RockPaperScissors.Tests;

public class GameTests
{
    [Fact]
    public void Rock_vs_rock_is_a_draw()
    {
        Game.Decide(Play.Rock, Play.Rock).Should().Be(Outcome.Draw);
    }

    [Fact]
    public void Rock_vs_paper_loses_because_paper_covers_rock()
    {
        Game.Decide(Play.Rock, Play.Paper).Should().Be(Outcome.Lose);
    }

    [Fact]
    public void Rock_vs_scissors_wins_because_rock_crushes_scissors()
    {
        Game.Decide(Play.Rock, Play.Scissors).Should().Be(Outcome.Win);
    }

    [Fact]
    public void Paper_vs_rock_wins_because_paper_covers_rock()
    {
        Game.Decide(Play.Paper, Play.Rock).Should().Be(Outcome.Win);
    }

    [Fact]
    public void Paper_vs_paper_is_a_draw()
    {
        Game.Decide(Play.Paper, Play.Paper).Should().Be(Outcome.Draw);
    }

    [Fact]
    public void Paper_vs_scissors_loses_because_scissors_cuts_paper()
    {
        Game.Decide(Play.Paper, Play.Scissors).Should().Be(Outcome.Lose);
    }

    [Fact]
    public void Scissors_vs_rock_loses_because_rock_crushes_scissors()
    {
        Game.Decide(Play.Scissors, Play.Rock).Should().Be(Outcome.Lose);
    }

    [Fact]
    public void Scissors_vs_paper_wins_because_scissors_cuts_paper()
    {
        Game.Decide(Play.Scissors, Play.Paper).Should().Be(Outcome.Win);
    }

    [Fact]
    public void Scissors_vs_scissors_is_a_draw()
    {
        Game.Decide(Play.Scissors, Play.Scissors).Should().Be(Outcome.Draw);
    }
}
