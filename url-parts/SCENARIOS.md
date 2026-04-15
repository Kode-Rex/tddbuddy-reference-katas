# URL Parts — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Protocol** | One of `http`, `https`, `ftp`, `sftp`. |
| **Subdomain** | Everything before the registrable hostname (e.g. `www` in `www.foo.com`). Empty string when the host has no subdomain, or when the host is `localhost`. |
| **Domain** | The registrable hostname joined with its top-level domain (e.g. `foo.com`). For `localhost` the domain is the bare string `localhost`. |
| **Port** | Numeric port. When absent from the URL, defaulted from the protocol: `http` → 80, `https` → 443, `ftp` → 21, `sftp` → 22. |
| **Path** | The path after the authority, with the leading `/` stripped. Empty string when absent. |
| **Parameters** | The query string after `?`, not including the `?`. Empty string when absent. |
| **Anchor** | The fragment after `#`, not including the `#`. Empty string when absent. |

**Recognized top-level domains:** `.com`, `.net`, `.org`, `.int`, `.edu`, `.gov`, `.mil`.

## Domain Rules

1. A URL begins with `protocol "://"` and the protocol is one of the four listed above.
2. After `://` comes the authority (`[subdomain.]host[.tld][:port]`), then optionally a path beginning with `/`, then optionally `?parameters`, then optionally `#anchor`.
3. The **anchor** is split off first (everything after the first `#`), then the **parameters** (everything after the first `?` in what remains), then the **path** (everything after the first `/` in what remains).
4. The authority splits on `:` into host-part and port. An absent port is filled from the protocol's default.
5. The host-part is split into subdomain + domain:
   - If the host-part is exactly `localhost`, then subdomain is empty and domain is `localhost`.
   - Otherwise, find the trailing segment matching one of the recognized TLDs. The portion before the TLD is split on the **last** `.`: the tail is the registrable hostname, the head (possibly empty) is the subdomain. The domain is the registrable hostname plus the TLD.

No regular expressions. Split on `://`, `#`, `?`, `/`, `:`, and `.`; check the rules.

## Test Scenarios

1. **`http://www.tddbuddy.com`** → protocol `http`, subdomain `www`, domain `tddbuddy.com`, port `80`, path `""`, parameters `""`, anchor `""`.
2. **`http://foo.bar.com/foobar.html`** → protocol `http`, subdomain `foo`, domain `bar.com`, port `80`, path `foobar.html`, parameters `""`, anchor `""`.
3. **`https://www.foobar.com:8080/download/install.exe`** → protocol `https`, subdomain `www`, domain `foobar.com`, port `8080`, path `download/install.exe`, parameters `""`, anchor `""`.
4. **`ftp://foo.com:9000/files`** → protocol `ftp`, subdomain `""`, domain `foo.com`, port `9000`, path `files`, parameters `""`, anchor `""`.
5. **`https://localhost/index.html#footer`** → protocol `https`, subdomain `""`, domain `localhost`, port `443`, path `index.html`, parameters `""`, anchor `footer`.
6. **`sftp://user.example.org/path`** → protocol `sftp`, subdomain `user`, domain `example.org`, port `22`, path `path`, parameters `""`, anchor `""`.
7. **`http://api.example.com/search?q=tdd&page=2`** → protocol `http`, subdomain `api`, domain `example.com`, port `80`, path `search`, parameters `q=tdd&page=2`, anchor `""`.
8. **`https://www.site.net/page?id=42#section`** → protocol `https`, subdomain `www`, domain `site.net`, port `443`, path `page`, parameters `id=42`, anchor `section`.
9. **`http://localhost:3000`** → protocol `http`, subdomain `""`, domain `localhost`, port `3000`, path `""`, parameters `""`, anchor `""`.
10. **`https://www.example.gov:8443`** → protocol `https`, subdomain `www`, domain `example.gov`, port `8443`, path `""`, parameters `""`, anchor `""`.
