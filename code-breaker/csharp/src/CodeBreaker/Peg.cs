namespace CodeBreaker;

// The six playable values in a Mastermind-style code.
// Kept as a typed enum (rather than raw int) so invalid pegs cannot be constructed.
public enum Peg
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
}
