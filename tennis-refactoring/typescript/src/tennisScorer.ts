// Clean tennis scorer — characterization-equivalent to the legacy
// getScore function from the kata brief. Three named branches
// (equal / endgame / in-play) replace the original's nested if/else
// over raw point counts; POINT_NAMES lifts the magic inline array
// into a named module-level lookup. See ../SCENARIOS.md for the contract.

// The first point count at which the game enters the endgame —
// Advantage or Win. Below this threshold, unequal scores render as
// "<Point>-<Point>" (in-play); at or above, the leader's name appears.
const ENDGAME_THRESHOLD = 4;

// The first point count at which equal scores collapse to "Deuce"
// rather than "<Point>-All". Legacy: `p1Score >= 3` when p1Score == p2Score.
const DEUCE_THRESHOLD = 3;

const POINT_NAMES = ['Love', 'Fifteen', 'Thirty', 'Forty'] as const;

function pointName(score: number): string {
  return POINT_NAMES[score]!;
}

export function getScore(
  p1Score: number,
  p2Score: number,
  p1Name: string,
  p2Name: string,
): string {
  if (p1Score === p2Score) return equalScore(p1Score);
  if (p1Score >= ENDGAME_THRESHOLD || p2Score >= ENDGAME_THRESHOLD) {
    return endgameScore(p1Score, p2Score, p1Name, p2Name);
  }
  return inPlayScore(p1Score, p2Score);
}

function equalScore(score: number): string {
  if (score >= DEUCE_THRESHOLD) return 'Deuce';
  return `${pointName(score)}-All`;
}

function endgameScore(
  p1Score: number,
  p2Score: number,
  p1Name: string,
  p2Name: string,
): string {
  const diff = p1Score - p2Score;
  if (diff === 1) return `Advantage ${p1Name}`;
  if (diff === -1) return `Advantage ${p2Name}`;
  if (diff >= 2) return `Win for ${p1Name}`;
  return `Win for ${p2Name}`;
}

function inPlayScore(p1Score: number, p2Score: number): string {
  return `${pointName(p1Score)}-${pointName(p2Score)}`;
}
