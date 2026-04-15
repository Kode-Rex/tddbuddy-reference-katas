using FluentAssertions;
using Xunit;
using static CodeBreaker.Peg;

namespace CodeBreaker.Tests;

public class FeedbackTests
{
    [Fact]
    public void Secret_1234_vs_guess_with_no_shared_pegs_has_no_matches()
    {
        var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
        var guess = new GuessBuilder().With(Five, Six, Five, Six).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("");
        feedback.ExactMatches.Should().Be(0);
        feedback.ColorMatches.Should().Be(0);
    }

    [Fact]
    public void Secret_1234_vs_guess_1566_has_one_exact_match()
    {
        var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
        var guess = new GuessBuilder().With(One, Five, Six, Six).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("+");
        feedback.ExactMatches.Should().Be(1);
        feedback.ColorMatches.Should().Be(0);
    }

    [Fact]
    public void Secret_1234_vs_guess_1234_is_a_win()
    {
        var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
        var guess = new GuessBuilder().With(One, Two, Three, Four).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("++++");
        feedback.ExactMatches.Should().Be(4);
        feedback.IsWon.Should().BeTrue();
    }

    [Fact]
    public void Secret_1234_vs_guess_4321_has_four_color_matches()
    {
        var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
        var guess = new GuessBuilder().With(Four, Three, Two, One).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("----");
        feedback.ExactMatches.Should().Be(0);
        feedback.ColorMatches.Should().Be(4);
    }

    [Fact]
    public void Secret_1234_vs_guess_1243_has_two_exact_and_two_color_matches()
    {
        var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
        var guess = new GuessBuilder().With(One, Two, Four, Three).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("++--");
        feedback.ExactMatches.Should().Be(2);
        feedback.ColorMatches.Should().Be(2);
    }

    [Fact]
    public void Secret_1234_vs_guess_2135_has_one_exact_and_two_color_matches()
    {
        var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
        var guess = new GuessBuilder().With(Two, One, Three, Five).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("+--");
        feedback.ExactMatches.Should().Be(1);
        feedback.ColorMatches.Should().Be(2);
    }

    [Fact]
    public void Secret_1124_vs_guess_5166_counts_the_duplicate_peg_only_once()
    {
        var secret = new SecretBuilder().With(One, One, Two, Four).Build();
        var guess = new GuessBuilder().With(Five, One, Six, Six).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("+");
        feedback.ExactMatches.Should().Be(1);
        feedback.ColorMatches.Should().Be(0);
    }

    [Fact]
    public void Secret_1122_vs_guess_2211_has_four_color_matches_no_exact()
    {
        var secret = new SecretBuilder().With(One, One, Two, Two).Build();
        var guess = new GuessBuilder().With(Two, Two, One, One).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("----");
        feedback.ExactMatches.Should().Be(0);
        feedback.ColorMatches.Should().Be(4);
    }

    [Fact]
    public void Secret_1111_vs_guess_1112_counts_three_exacts_and_ignores_the_non_matching_peg()
    {
        var secret = new SecretBuilder().With(One, One, One, One).Build();
        var guess = new GuessBuilder().With(One, One, One, Two).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("+++");
        feedback.ExactMatches.Should().Be(3);
        feedback.ColorMatches.Should().Be(0);
    }

    [Fact]
    public void Secret_1111_vs_guess_2111_counts_three_exacts_at_positions_2_through_4()
    {
        var secret = new SecretBuilder().With(One, One, One, One).Build();
        var guess = new GuessBuilder().With(Two, One, One, One).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("+++");
        feedback.ExactMatches.Should().Be(3);
        feedback.ColorMatches.Should().Be(0);
    }

    [Fact]
    public void Secret_1234_vs_guess_1111_counts_one_exact_and_no_color_matches()
    {
        var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
        var guess = new GuessBuilder().With(One, One, One, One).Build();

        var feedback = secret.ScoreAgainst(guess);

        feedback.Render().Should().Be("+");
        feedback.ExactMatches.Should().Be(1);
        feedback.ColorMatches.Should().Be(0);
    }

    [Fact]
    public void Feedback_with_four_exact_matches_is_won_any_other_feedback_is_not()
    {
        new Feedback(4, 0).IsWon.Should().BeTrue();
        new Feedback(0, 4).IsWon.Should().BeFalse();
        new Feedback(3, 1).IsWon.Should().BeFalse();
        new Feedback(0, 0).IsWon.Should().BeFalse();
    }
}
