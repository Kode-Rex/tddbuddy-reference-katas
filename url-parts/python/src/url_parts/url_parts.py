from dataclasses import dataclass

TOP_LEVEL_DOMAINS = (".com", ".net", ".org", ".int", ".edu", ".gov", ".mil")

DEFAULT_PORTS = {
    "http": 80,
    "https": 443,
    "ftp": 21,
    "sftp": 22,
}


@dataclass(frozen=True)
class UrlParts:
    protocol: str
    subdomain: str
    domain: str
    port: int
    path: str
    parameters: str
    anchor: str


def parse(url: str) -> UrlParts:
    scheme_separator = url.index("://")
    protocol = url[:scheme_separator]
    rest = url[scheme_separator + 3:]

    anchor = ""
    hash_index = rest.find("#")
    if hash_index >= 0:
        anchor = rest[hash_index + 1:]
        rest = rest[:hash_index]

    parameters = ""
    query_index = rest.find("?")
    if query_index >= 0:
        parameters = rest[query_index + 1:]
        rest = rest[:query_index]

    path = ""
    slash_index = rest.find("/")
    if slash_index >= 0:
        path = rest[slash_index + 1:]
        rest = rest[:slash_index]

    authority = rest
    colon_index = authority.find(":")
    if colon_index >= 0:
        host_part = authority[:colon_index]
        port = int(authority[colon_index + 1:])
    else:
        host_part = authority
        if protocol not in DEFAULT_PORTS:
            raise ValueError(f"Unknown protocol: {protocol}")
        port = DEFAULT_PORTS[protocol]

    subdomain, domain = _split_host(host_part)

    return UrlParts(
        protocol=protocol,
        subdomain=subdomain,
        domain=domain,
        port=port,
        path=path,
        parameters=parameters,
        anchor=anchor,
    )


def _split_host(host_part: str) -> tuple[str, str]:
    if host_part == "localhost":
        return "", "localhost"

    for tld in TOP_LEVEL_DOMAINS:
        if not host_part.endswith(tld):
            continue

        before_tld = host_part[: -len(tld)]
        last_dot = before_tld.rfind(".")
        if last_dot < 0:
            return "", before_tld + tld
        subdomain = before_tld[:last_dot]
        hostname = before_tld[last_dot + 1:]
        return subdomain, hostname + tld

    raise ValueError(f"Unrecognized host: {host_part}")
