using FluentAssertions;

namespace TennisScore.Tests;

public class MatchTests
{
    [Fact]
    public void Start_of_match_reads_love_love()
    {
        new Match().Score().Should().Be("Love-Love");
    }

    [Fact]
    public void One_point_to_player_one_reads_fifteen_love()
    {
        var match = new Match();
        match.PointWonBy(1);
        match.Score().Should().Be("15-Love");
    }
}
