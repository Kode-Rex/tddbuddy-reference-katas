export const AGE_ADULT_MINIMUM = 18;

export class Age {
  constructor(public readonly years: number) {}
  get isAdult(): boolean { return this.years >= AGE_ADULT_MINIMUM; }
}
