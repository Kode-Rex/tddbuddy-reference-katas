const ONES = [
  'zero', 'one', 'two', 'three', 'four',
  'five', 'six', 'seven', 'eight', 'nine',
  'ten', 'eleven', 'twelve', 'thirteen', 'fourteen',
  'fifteen', 'sixteen', 'seventeen', 'eighteen', 'nineteen',
] as const;

const TENS = [
  '', '', 'twenty', 'thirty', 'forty',
  'fifty', 'sixty', 'seventy', 'eighty', 'ninety',
] as const;

export function toWords(n: number): string {
  if (n === 0) return 'zero';

  const parts: string[] = [];

  const thousands = Math.floor(n / 1000);
  if (thousands > 0) {
    parts.push(`${ONES[thousands]} thousand`);
    n %= 1000;
  }

  const hundreds = Math.floor(n / 100);
  if (hundreds > 0) {
    parts.push(`${ONES[hundreds]} hundred`);
    n %= 100;
  }

  if (n > 0) {
    parts.push(belowHundred(n));
  }

  return parts.join(' ');
}

function belowHundred(n: number): string {
  if (n < 20) return ONES[n]!;
  const tens = Math.floor(n / 10);
  const ones = n % 10;
  return ones === 0 ? TENS[tens]! : `${TENS[tens]}-${ONES[ones]}`;
}
