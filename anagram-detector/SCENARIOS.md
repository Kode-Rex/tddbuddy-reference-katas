# Anagram Detector — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

An **anagram key** for a string is the sequence of its letters, lowercased, with
all non-letter characters (spaces, punctuation, digits) removed, sorted
alphabetically. Two strings are **anagrams** when they share the same anagram
key **and** their normalized letter content is not identical to the other
string's original content (i.e. a word is never an anagram of itself).

The rules, applied in order:

1. **Case-insensitive** — `"Cat"` and `"tac"` share the key `act`.
2. **Spaces and punctuation ignored** — `"Astronomer"` and `"Moon starer"` both key to `aemnoooorrst`.
3. **A word is not an anagram of itself** — compared by original input equality.
4. **Empty letter content is not an anagram** — two empty strings share the empty key, but the "not itself" rule rejects them; an empty string is also not considered an anagram of any other empty-letter input.
5. **Single-letter mismatch rejected** — `"aab"` vs `"abb"` differ in letter counts.

## API

Three functions form the kata surface:

- **`areAnagrams(a, b) → bool`** — pair detection (Step 1).
- **`findAnagrams(subject, candidates) → list`** — filter candidates to those that are anagrams of the subject, preserving input order (Step 2).
- **`groupAnagrams(words) → list of lists`** — partition words into anagram sets, preserving first-occurrence order of both groups and their members (Step 3).

`findAnagrams` and `groupAnagrams` reuse the pair-detection rules for *what counts as an anagram*, with one exception: grouping a word with itself trivially keeps it in its own set (a single-element group is valid output), since `groupAnagrams` partitions rather than pairs.

## Test Scenarios

### Pair detection — `areAnagrams`

1. **`"listen"` and `"silent"` are anagrams** — canonical example, same letters rearranged
2. **`"hello"` and `"world"` are not anagrams** — different letter sets
3. **`"cat"` and `"tac"` are anagrams** — three-letter rearrangement
4. **`"cat"` and `"cat"` are not anagrams** — a word is not an anagram of itself
5. **`"Cat"` and `"tac"` are anagrams** — case-insensitive comparison
6. **`""` and `""` are not anagrams** — empty inputs fail the not-itself rule
7. **`"a"` and `"a"` are not anagrams** — single-letter identical pair
8. **`"ab"` and `"ba"` are anagrams** — minimal rearrangement
9. **`"aab"` and `"abb"` are not anagrams** — letter counts differ
10. **`"Astronomer"` and `"Moon starer"` are anagrams** — case and whitespace normalized
11. **`"rail safety"` and `"fairy tales"` are anagrams** — multi-word phrases

### Find anagrams — `findAnagrams`

12. **`"listen"` against `["silent", "tinsel"]` returns both** — every candidate matches
13. **`"listen"` against `["hello", "world"]` returns an empty list** — no candidate matches
14. **`"master"` against `["stream", "maters", "pigeon"]` returns `["stream", "maters"]`** — mixed matches preserve input order

### Group anagrams — `groupAnagrams`

15. **`["eat", "tea", "ate"]` groups into one set of three** — all share key `aet`
16. **`["abc", "def"]` groups into two singletons** — no shared key
17. **`[]` returns an empty list** — no words, no groups
18. **`["eat", "tea", "tan", "ate", "nat", "bat"]` groups into `[[eat, tea, ate], [tan, nat], [bat]]`** — three groups, first-occurrence order preserved for both groups and members
