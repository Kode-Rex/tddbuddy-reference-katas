// The six playable values in a Mastermind-style code. A const-object-as-enum
// keeps the runtime values numeric (so comparisons are trivial) while giving
// TypeScript the literal-type safety that a raw `number` would lose.
export const Peg = {
  One: 1,
  Two: 2,
  Three: 3,
  Four: 4,
  Five: 5,
  Six: 6,
} as const;
export type Peg = (typeof Peg)[keyof typeof Peg];

export const CODE_LENGTH = 4;

export type CodePegs = readonly [Peg, Peg, Peg, Peg];

export interface Secret {
  readonly pegs: CodePegs;
  scoreAgainst(guess: Guess): Feedback;
}

export interface Guess {
  readonly pegs: CodePegs;
}

export interface Feedback {
  readonly exactMatches: number;
  readonly colorMatches: number;
  readonly isWon: boolean;
  render(): string;
}

export function createSecret(pegs: CodePegs): Secret {
  return {
    pegs,
    scoreAgainst(guess: Guess): Feedback {
      return computeFeedback(pegs, guess.pegs);
    },
  };
}

export function createGuess(pegs: CodePegs): Guess {
  return { pegs };
}

export function createFeedback(exactMatches: number, colorMatches: number): Feedback {
  return {
    exactMatches,
    colorMatches,
    isWon: exactMatches === CODE_LENGTH,
    render: () => '+'.repeat(exactMatches) + '-'.repeat(colorMatches),
  };
}

// Two-pass scoring: exact matches consume positions first, then the
// remaining secret pegs are matched against the remaining guess pegs
// by value, respecting multiplicity.
function computeFeedback(secret: CodePegs, guess: CodePegs): Feedback {
  let exactMatches = 0;
  const unmatchedSecret: Peg[] = [];
  const unmatchedGuess: Peg[] = [];

  for (let i = 0; i < CODE_LENGTH; i++) {
    if (secret[i] === guess[i]) {
      exactMatches++;
    } else {
      unmatchedSecret.push(secret[i] as Peg);
      unmatchedGuess.push(guess[i] as Peg);
    }
  }

  let colorMatches = 0;
  for (const peg of unmatchedGuess) {
    const index = unmatchedSecret.indexOf(peg);
    if (index >= 0) {
      unmatchedSecret.splice(index, 1);
      colorMatches++;
    }
  }

  return createFeedback(exactMatches, colorMatches);
}
