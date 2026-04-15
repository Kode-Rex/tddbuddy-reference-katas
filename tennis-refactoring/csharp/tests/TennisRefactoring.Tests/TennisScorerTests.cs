using FluentAssertions;
using Xunit;

namespace TennisRefactoring.Tests;

public class TennisScorerTests
{
    [Fact]
    public void Love_All_at_the_start_of_a_game()
    {
        TennisScorer.GetScore(0, 0, "Player1", "Player2").Should().Be("Love-All");
    }

    [Fact]
    public void Fifteen_All_when_both_players_have_one_point()
    {
        TennisScorer.GetScore(1, 1, "Player1", "Player2").Should().Be("Fifteen-All");
    }

    [Fact]
    public void Thirty_All_when_both_players_have_two_points()
    {
        TennisScorer.GetScore(2, 2, "Player1", "Player2").Should().Be("Thirty-All");
    }

    [Fact]
    public void Deuce_when_both_players_have_three_points()
    {
        TennisScorer.GetScore(3, 3, "Player1", "Player2").Should().Be("Deuce");
    }

    [Fact]
    public void Deuce_persists_past_Forty_All()
    {
        TennisScorer.GetScore(4, 4, "Player1", "Player2").Should().Be("Deuce");
    }

    [Fact]
    public void Fifteen_Love_when_player_1_leads_by_one_at_the_start()
    {
        TennisScorer.GetScore(1, 0, "Player1", "Player2").Should().Be("Fifteen-Love");
    }

    [Fact]
    public void Love_Fifteen_when_player_2_leads_by_one_at_the_start()
    {
        TennisScorer.GetScore(0, 1, "Player1", "Player2").Should().Be("Love-Fifteen");
    }

    [Fact]
    public void Thirty_Fifteen_when_player_1_has_two_and_player_2_has_one()
    {
        TennisScorer.GetScore(2, 1, "Player1", "Player2").Should().Be("Thirty-Fifteen");
    }

    [Fact]
    public void Forty_Fifteen_when_player_1_has_three_and_player_2_has_one()
    {
        TennisScorer.GetScore(3, 1, "Player1", "Player2").Should().Be("Forty-Fifteen");
    }

    [Fact]
    public void Advantage_to_player_1_when_they_lead_by_one_in_the_endgame()
    {
        TennisScorer.GetScore(4, 3, "Player1", "Player2").Should().Be("Advantage Player1");
    }

    [Fact]
    public void Advantage_to_player_2_when_they_lead_by_one_in_the_endgame()
    {
        TennisScorer.GetScore(3, 4, "Player1", "Player2").Should().Be("Advantage Player2");
    }

    [Fact]
    public void Advantage_persists_at_higher_equal_gap_scores()
    {
        TennisScorer.GetScore(5, 4, "Player1", "Player2").Should().Be("Advantage Player1");
    }

    [Fact]
    public void Win_for_player_1_when_they_lead_by_two_in_the_endgame()
    {
        TennisScorer.GetScore(5, 3, "Player1", "Player2").Should().Be("Win for Player1");
    }

    [Fact]
    public void Win_for_player_2_when_they_lead_by_two_in_the_endgame()
    {
        TennisScorer.GetScore(3, 5, "Player1", "Player2").Should().Be("Win for Player2");
    }

    [Fact]
    public void Player_names_are_passed_through_verbatim_into_Advantage()
    {
        TennisScorer.GetScore(4, 3, "Serena", "Venus").Should().Be("Advantage Serena");
    }

    [Fact]
    public void Player_names_are_passed_through_verbatim_into_Win()
    {
        TennisScorer.GetScore(5, 3, "Serena", "Venus").Should().Be("Win for Serena");
    }
}
