from url_parts import UrlParts, parse


def test_http_root_with_www_subdomain_and_default_port():
    assert parse("http://www.tddbuddy.com") == UrlParts(
        protocol="http", subdomain="www", domain="tddbuddy.com",
        port=80, path="", parameters="", anchor="",
    )


def test_http_with_single_segment_subdomain_and_path():
    assert parse("http://foo.bar.com/foobar.html") == UrlParts(
        protocol="http", subdomain="foo", domain="bar.com",
        port=80, path="foobar.html", parameters="", anchor="",
    )


def test_https_with_explicit_port_and_multi_segment_path():
    assert parse("https://www.foobar.com:8080/download/install.exe") == UrlParts(
        protocol="https", subdomain="www", domain="foobar.com",
        port=8080, path="download/install.exe", parameters="", anchor="",
    )


def test_ftp_with_no_subdomain_and_explicit_port():
    assert parse("ftp://foo.com:9000/files") == UrlParts(
        protocol="ftp", subdomain="", domain="foo.com",
        port=9000, path="files", parameters="", anchor="",
    )


def test_https_localhost_with_path_and_anchor():
    assert parse("https://localhost/index.html#footer") == UrlParts(
        protocol="https", subdomain="", domain="localhost",
        port=443, path="index.html", parameters="", anchor="footer",
    )


def test_sftp_uses_default_port_22():
    assert parse("sftp://user.example.org/path") == UrlParts(
        protocol="sftp", subdomain="user", domain="example.org",
        port=22, path="path", parameters="", anchor="",
    )


def test_http_with_query_parameters():
    assert parse("http://api.example.com/search?q=tdd&page=2") == UrlParts(
        protocol="http", subdomain="api", domain="example.com",
        port=80, path="search", parameters="q=tdd&page=2", anchor="",
    )


def test_https_with_query_parameters_and_anchor():
    assert parse("https://www.site.net/page?id=42#section") == UrlParts(
        protocol="https", subdomain="www", domain="site.net",
        port=443, path="page", parameters="id=42", anchor="section",
    )


def test_http_localhost_with_explicit_port_and_no_path():
    assert parse("http://localhost:3000") == UrlParts(
        protocol="http", subdomain="", domain="localhost",
        port=3000, path="", parameters="", anchor="",
    )


def test_https_with_explicit_port_and_no_path():
    assert parse("https://www.example.gov:8443") == UrlParts(
        protocol="https", subdomain="www", domain="example.gov",
        port=8443, path="", parameters="", anchor="",
    )
