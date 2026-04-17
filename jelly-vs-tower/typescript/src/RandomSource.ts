export interface RandomSource {
  next(minInclusive: number, maxInclusive: number): number;
}
