using FluentAssertions;
using Xunit;

namespace UrlParts.Tests;

public class UrlParserTests
{
    [Fact]
    public void Http_root_with_www_subdomain_and_default_port()
    {
        var parts = UrlParser.Parse("http://www.tddbuddy.com");
        parts.Should().Be(new UrlParts("http", "www", "tddbuddy.com", 80, "", "", ""));
    }

    [Fact]
    public void Http_with_single_segment_subdomain_and_path()
    {
        var parts = UrlParser.Parse("http://foo.bar.com/foobar.html");
        parts.Should().Be(new UrlParts("http", "foo", "bar.com", 80, "foobar.html", "", ""));
    }

    [Fact]
    public void Https_with_explicit_port_and_multi_segment_path()
    {
        var parts = UrlParser.Parse("https://www.foobar.com:8080/download/install.exe");
        parts.Should().Be(new UrlParts("https", "www", "foobar.com", 8080, "download/install.exe", "", ""));
    }

    [Fact]
    public void Ftp_with_no_subdomain_and_explicit_port()
    {
        var parts = UrlParser.Parse("ftp://foo.com:9000/files");
        parts.Should().Be(new UrlParts("ftp", "", "foo.com", 9000, "files", "", ""));
    }

    [Fact]
    public void Https_localhost_with_path_and_anchor()
    {
        var parts = UrlParser.Parse("https://localhost/index.html#footer");
        parts.Should().Be(new UrlParts("https", "", "localhost", 443, "index.html", "", "footer"));
    }

    [Fact]
    public void Sftp_uses_default_port_22()
    {
        var parts = UrlParser.Parse("sftp://user.example.org/path");
        parts.Should().Be(new UrlParts("sftp", "user", "example.org", 22, "path", "", ""));
    }

    [Fact]
    public void Http_with_query_parameters()
    {
        var parts = UrlParser.Parse("http://api.example.com/search?q=tdd&page=2");
        parts.Should().Be(new UrlParts("http", "api", "example.com", 80, "search", "q=tdd&page=2", ""));
    }

    [Fact]
    public void Https_with_query_parameters_and_anchor()
    {
        var parts = UrlParser.Parse("https://www.site.net/page?id=42#section");
        parts.Should().Be(new UrlParts("https", "www", "site.net", 443, "page", "id=42", "section"));
    }

    [Fact]
    public void Http_localhost_with_explicit_port_and_no_path()
    {
        var parts = UrlParser.Parse("http://localhost:3000");
        parts.Should().Be(new UrlParts("http", "", "localhost", 3000, "", "", ""));
    }

    [Fact]
    public void Https_with_explicit_port_and_no_path()
    {
        var parts = UrlParser.Parse("https://www.example.gov:8443");
        parts.Should().Be(new UrlParts("https", "www", "example.gov", 8443, "", "", ""));
    }
}
