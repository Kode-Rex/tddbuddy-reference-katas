# IP Validator — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

A string is a valid **host-assignable IPv4 address** when *all* of the following hold:

1. It consists of exactly **four** decimal octets separated by single `.` characters.
2. Each octet is a non-empty sequence of ASCII digits (`0`–`9`) only.
3. Each octet has **no leading zeros**: `0` is allowed; `01`, `00`, `007` are not.
4. Each octet's numeric value is between `0` and `255` inclusive.
5. The **final** octet is neither `0` (network address) nor `255` (broadcast address).

Any other input — including empty strings, strings with non-digit characters, strings with the wrong number of octets, or strings with out-of-range octets — is invalid.

No regular expressions. Split on `.`, parse each octet with the language's idiomatic integer parser, check the rules.

## Test Scenarios

1. **`1.1.1.1` is valid** — four in-range octets, last is `1`
2. **`192.168.1.1` is valid** — typical private-network host address
3. **`10.0.0.1` is valid** — private-network host address with zeros in the middle octets
4. **`127.0.0.1` is valid** — loopback host address
5. **`0.0.0.0` is invalid** — last octet is `0` (network address)
6. **`255.255.255.255` is invalid** — last octet is `255` (broadcast address)
7. **`192.168.1.0` is invalid** — last octet is `0`
8. **`192.168.1.255` is invalid** — last octet is `255`
9. **`10.0.1` is invalid** — only three octets
10. **`1.2.3.4.5` is invalid** — five octets
11. **`192.168.01.1` is invalid** — octet has a leading zero
12. **`192.168.1.00` is invalid** — octet has leading zeros
13. **`256.1.1.1` is invalid** — first octet exceeds `255`
14. **`1.1.1.999` is invalid** — last octet far exceeds `255`
15. **`1.1.1.-1` is invalid** — negative sign is not a digit
16. **`1.1.1.a` is invalid** — non-digit character in an octet
17. **`1.1..1` is invalid** — empty octet between adjacent dots
18. **`` (empty string) is invalid** — no octets at all
