export type Unit =
  | 'kilometers'
  | 'miles'
  | 'celsius'
  | 'fahrenheit'
  | 'kilograms'
  | 'pounds'
  | 'liters'
  | 'usGallons'
  | 'ukGallons';

export function convert(value: number, from: Unit, to: Unit): number {
  const key = `${from}->${to}` as const;
  switch (key) {
    case 'kilometers->miles':
      return value * 0.621371;
    case 'celsius->fahrenheit':
      return value * 1.8 + 32;
    case 'kilograms->pounds':
      return value / 0.45359237;
    case 'liters->usGallons':
      return value / 3.785411784;
    case 'liters->ukGallons':
      return value / 4.54609;
    default:
      throw new Error(`Unsupported conversion: ${from} to ${to}`);
  }
}
