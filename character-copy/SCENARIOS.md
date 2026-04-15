# Character Copy — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Source** | Collaborator interface with one method `readChar()` returning a single character |
| **Destination** | Collaborator interface with one method `writeChar(c)` receiving a single character |
| **Copier** | The SUT — a procedure `copy(source, destination)` that streams characters from Source to Destination |
| **Terminator** | The newline character `'\n'` returned by Source; signals end of input and is itself **not** written |
| **StringSource** | Test fake wrapping a string; yields its characters one at a time, then yields `'\n'` as the terminator |
| **RecordingDestination** | Test fake collecting written characters into an accessible buffer |

## Domain Rules

- The copier calls `source.readChar()`, then calls `destination.writeChar(c)` with the result, and repeats.
- When `source.readChar()` returns `'\n'`, the copier stops.
- The terminating newline is **never** written to the destination.
- If the very first character read is `'\n'`, nothing is written — the copier returns immediately.
- The copier does not look ahead, buffer, or transform: every character read (except the terminator) is written exactly once in order.

## Test Scenarios

1. **Immediate newline copies nothing** — a Source that yields `'\n'` on the first read results in a Destination that recorded no characters.
2. **Single character then newline copies that character** — Source yields `'a'` then `'\n'`; Destination recorded exactly `"a"`.
3. **Multiple characters then newline copies all of them in order** — Source yields `'a'`, `'b'`, `'c'`, then `'\n'`; Destination recorded exactly `"abc"`.
4. **Copy preserves whitespace before the terminator** — Source yields `'a'`, `' '`, `'b'`, then `'\n'`; Destination recorded exactly `"a b"`.
5. **Copy does not read past the terminator** — after the copier returns, the Source has been read exactly as many times as there are characters up to and including the `'\n'` (no trailing reads).
6. **Copy writes exactly as many characters as were read before the terminator** — for a four-character payload followed by `'\n'`, the Destination recorded four characters.
