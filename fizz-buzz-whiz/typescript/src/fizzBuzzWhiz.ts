export function say(n: number): string {
  const divisibleByThree = n % 3 === 0;
  const divisibleByFive = n % 5 === 0;

  if (divisibleByThree && divisibleByFive) return 'FizzBuzz';
  if (divisibleByThree) return 'Fizz';
  if (divisibleByFive) return 'Buzz';
  return String(n);
}
