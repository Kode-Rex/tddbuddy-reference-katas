namespace UrlParts;

public record UrlParts(
    string Protocol,
    string Subdomain,
    string Domain,
    int Port,
    string Path,
    string Parameters,
    string Anchor);

public static class UrlParser
{
    private static readonly string[] TopLevelDomains =
        { ".com", ".net", ".org", ".int", ".edu", ".gov", ".mil" };

    public static UrlParts Parse(string url)
    {
        var schemeSeparator = url.IndexOf("://", System.StringComparison.Ordinal);
        var protocol = url[..schemeSeparator];
        var rest = url[(schemeSeparator + 3)..];

        var anchor = "";
        var hashIndex = rest.IndexOf('#');
        if (hashIndex >= 0)
        {
            anchor = rest[(hashIndex + 1)..];
            rest = rest[..hashIndex];
        }

        var parameters = "";
        var queryIndex = rest.IndexOf('?');
        if (queryIndex >= 0)
        {
            parameters = rest[(queryIndex + 1)..];
            rest = rest[..queryIndex];
        }

        var path = "";
        var slashIndex = rest.IndexOf('/');
        if (slashIndex >= 0)
        {
            path = rest[(slashIndex + 1)..];
            rest = rest[..slashIndex];
        }

        var authority = rest;
        var colonIndex = authority.IndexOf(':');
        string hostPart;
        int port;
        if (colonIndex >= 0)
        {
            hostPart = authority[..colonIndex];
            port = int.Parse(authority[(colonIndex + 1)..]);
        }
        else
        {
            hostPart = authority;
            port = DefaultPort(protocol);
        }

        var (subdomain, domain) = SplitHost(hostPart);

        return new UrlParts(protocol, subdomain, domain, port, path, parameters, anchor);
    }

    private static int DefaultPort(string protocol) => protocol switch
    {
        "http" => 80,
        "https" => 443,
        "ftp" => 21,
        "sftp" => 22,
        _ => throw new System.ArgumentException($"Unknown protocol: {protocol}"),
    };

    private static (string Subdomain, string Domain) SplitHost(string hostPart)
    {
        if (hostPart == "localhost") return ("", "localhost");

        foreach (var tld in TopLevelDomains)
        {
            if (!hostPart.EndsWith(tld, System.StringComparison.Ordinal)) continue;

            var beforeTld = hostPart[..^tld.Length];
            var lastDot = beforeTld.LastIndexOf('.');
            if (lastDot < 0)
            {
                return ("", beforeTld + tld);
            }
            var subdomain = beforeTld[..lastDot];
            var hostname = beforeTld[(lastDot + 1)..];
            return (subdomain, hostname + tld);
        }

        throw new System.ArgumentException($"Unrecognized host: {hostPart}");
    }
}
