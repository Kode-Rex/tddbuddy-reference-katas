# URL Shortener — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Long URL** | The original URL a caller passes to `shorten`. Stored verbatim. |
| **Short code** | A base36 string issued sequentially by the shortener (`0`, `1`, …, `a`, `b`, …, `10`, `11`, …). |
| **Short URL** | `https://short.url/<short-code>`. The public return shape of `shorten`. |
| **Visit** | A call to `translate` with a **short URL**. Calls with a long URL do not count as visits. |
| **Statistics** | A triple of `(shortUrl, longUrl, visits)` for a known URL — retrievable by either the long or short form. |

## Domain Rules

1. **Issue order.** The first distinct long URL shortened gets code `0`, the second `1`, the tenth `a`, the thirty-seventh `10` (base36 of the 0-indexed counter).
2. **Deduplication.** `shorten(longUrl)` is idempotent: shortening the same long URL twice returns the same short URL, and the counter does not advance.
3. **Bidirectional translate.** `translate(url)` returns the short URL whether given the long URL or the short URL.
4. **Visit counting.** Only `translate(shortUrl)` increments the visit count for that short code. `translate(longUrl)` does not, and `shorten` does not.
5. **Statistics.** `statistics(url)` returns `(shortUrl, longUrl, visits)` for the mapping containing `url`, where `url` may be either the long URL or the short URL.
6. **Unknown URL.** `translate` and `statistics` on an unknown URL raise a language-idiomatic error with message `Unknown URL: <url>`.

**Short URL base:** `https://short.url/` (trailing slash included; the short code is appended directly).

## Test Scenarios

1. **Shorten first URL issues code `0`.** `shorten("https://example.com/alpha")` returns `"https://short.url/0"`.
2. **Second distinct URL issues code `1`.** After scenario 1, `shorten("https://example.com/beta")` returns `"https://short.url/1"`.
3. **Shortening a duplicate returns the existing short URL.** After scenario 1, `shorten("https://example.com/alpha")` again returns `"https://short.url/0"`.
4. **Duplicate does not advance the counter.** After shortening `alpha` twice then `beta` once, `beta` still gets `"https://short.url/1"`.
5. **Eleventh distinct URL issues code `a`.** After shortening ten distinct URLs (codes `0`–`9`), the next distinct URL returns `"https://short.url/a"`.
6. **Translate by long URL returns the short URL.** After shortening `alpha`, `translate("https://example.com/alpha")` returns `"https://short.url/0"`.
7. **Translate by short URL returns the same short URL.** After shortening `alpha`, `translate("https://short.url/0")` returns `"https://short.url/0"`.
8. **Translate by short URL increments visits.** After shortening `alpha` then calling `translate("https://short.url/0")` three times, `statistics("https://short.url/0").visits == 3`.
9. **Translate by long URL does not increment visits.** After shortening `alpha` then calling `translate("https://example.com/alpha")` three times, `statistics("https://short.url/0").visits == 0`.
10. **Shorten does not count as a visit.** After `shorten("https://example.com/alpha")` with no subsequent `translate`, `statistics(...).visits == 0`.
11. **Statistics by long URL.** After shortening `alpha` and visiting the short URL twice, `statistics("https://example.com/alpha")` returns `("https://short.url/0", "https://example.com/alpha", 2)`.
12. **Statistics by short URL.** Same setup: `statistics("https://short.url/0")` returns the same triple.
13. **Translate on unknown URL raises.** `translate("https://unknown.example/x")` on a fresh shortener raises with message `Unknown URL: https://unknown.example/x`.
14. **Statistics on unknown URL raises.** `statistics("https://unknown.example/x")` on a fresh shortener raises with the same message shape.
