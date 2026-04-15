using FluentAssertions;
using Xunit;

namespace UrlShortener.Tests;

public class ShortenerTests
{
    [Fact]
    public void Shorten_first_url_issues_code_zero()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha").Should().Be("https://short.url/0");
    }

    [Fact]
    public void Second_distinct_url_issues_code_one()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Shorten("https://example.com/beta").Should().Be("https://short.url/1");
    }

    [Fact]
    public void Shortening_a_duplicate_returns_the_existing_short_url()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Shorten("https://example.com/alpha").Should().Be("https://short.url/0");
    }

    [Fact]
    public void Duplicate_does_not_advance_the_counter()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Shorten("https://example.com/alpha");
        shortener.Shorten("https://example.com/beta").Should().Be("https://short.url/1");
    }

    [Fact]
    public void Eleventh_distinct_url_issues_code_a()
    {
        var shortener = new Shortener();
        for (var i = 0; i < 10; i++)
        {
            shortener.Shorten($"https://example.com/url-{i}");
        }
        shortener.Shorten("https://example.com/url-10").Should().Be("https://short.url/a");
    }

    [Fact]
    public void Translate_by_long_url_returns_the_short_url()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Translate("https://example.com/alpha").Should().Be("https://short.url/0");
    }

    [Fact]
    public void Translate_by_short_url_returns_the_same_short_url()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Translate("https://short.url/0").Should().Be("https://short.url/0");
    }

    [Fact]
    public void Translate_by_short_url_increments_visits()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Translate("https://short.url/0");
        shortener.Translate("https://short.url/0");
        shortener.Translate("https://short.url/0");
        shortener.Statistics("https://short.url/0").Visits.Should().Be(3);
    }

    [Fact]
    public void Translate_by_long_url_does_not_increment_visits()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Translate("https://example.com/alpha");
        shortener.Translate("https://example.com/alpha");
        shortener.Translate("https://example.com/alpha");
        shortener.Statistics("https://short.url/0").Visits.Should().Be(0);
    }

    [Fact]
    public void Shorten_does_not_count_as_a_visit()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Statistics("https://short.url/0").Visits.Should().Be(0);
    }

    [Fact]
    public void Statistics_by_long_url()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Translate("https://short.url/0");
        shortener.Translate("https://short.url/0");
        shortener.Statistics("https://example.com/alpha")
            .Should().Be(new UrlStatistics("https://short.url/0", "https://example.com/alpha", 2));
    }

    [Fact]
    public void Statistics_by_short_url()
    {
        var shortener = new Shortener();
        shortener.Shorten("https://example.com/alpha");
        shortener.Translate("https://short.url/0");
        shortener.Translate("https://short.url/0");
        shortener.Statistics("https://short.url/0")
            .Should().Be(new UrlStatistics("https://short.url/0", "https://example.com/alpha", 2));
    }

    [Fact]
    public void Translate_on_unknown_url_raises()
    {
        var shortener = new Shortener();
        var act = () => shortener.Translate("https://unknown.example/x");
        act.Should().Throw<ArgumentException>()
            .WithMessage("Unknown URL: https://unknown.example/x");
    }

    [Fact]
    public void Statistics_on_unknown_url_raises()
    {
        var shortener = new Shortener();
        var act = () => shortener.Statistics("https://unknown.example/x");
        act.Should().Throw<ArgumentException>()
            .WithMessage("Unknown URL: https://unknown.example/x");
    }
}
