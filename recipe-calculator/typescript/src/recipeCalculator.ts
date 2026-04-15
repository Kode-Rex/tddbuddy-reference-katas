export type Recipe = Record<string, number>;

export function scale(recipe: Recipe, factor: number): Recipe {
  if (factor < 0) {
    throw new RangeError('Scale factor must be non-negative.');
  }

  const scaled: Recipe = {};
  for (const [ingredient, quantity] of Object.entries(recipe)) {
    scaled[ingredient] = quantity * factor;
  }
  return scaled;
}
