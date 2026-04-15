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

    [Fact]
    public void Two_points_each_reads_thirty_thirty()
    {
        var match = new Match();
        match.PointWonBy(1);
        match.PointWonBy(2);
        match.PointWonBy(1);
        match.PointWonBy(2);
        match.Score().Should().Be("30-30");
    }

    [Fact]
    public void Three_points_each_reads_deuce()
    {
        var match = new Match();
        for (var i = 0; i < 3; i++)
        {
            match.PointWonBy(1);
            match.PointWonBy(2);
        }
        match.Score().Should().Be("Deuce");
    }

    [Fact]
    public void Four_three_reads_advantage_player_one()
    {
        var match = new Match();
        for (var i = 0; i < 3; i++)
        {
            match.PointWonBy(1);
            match.PointWonBy(2);
        }
        match.PointWonBy(1);
        match.Score().Should().Be("Advantage Player 1");
    }

    [Fact]
    public void Four_two_reads_game_player_one()
    {
        var match = new Match();
        match.PointWonBy(1);
        match.PointWonBy(1);
        match.PointWonBy(2);
        match.PointWonBy(1);
        match.PointWonBy(2);
        match.PointWonBy(1);
        match.Score().Should().Be("Game Player 1");
    }
}
