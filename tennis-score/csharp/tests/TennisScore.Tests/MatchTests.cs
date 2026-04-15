using FluentAssertions;

namespace TennisScore.Tests;

public class MatchTests
{
    [Fact]
    public void Start_of_match_reads_love_love()
    {
        new Match().Score().Should().Be("Love-Love");
    }
}
