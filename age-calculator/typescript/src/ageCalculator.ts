export function calculate(birthdate: Date, today: Date): number {
  if (birthdate.getTime() > today.getTime()) {
    throw new Error('birthdate is after today');
  }

  let years = today.getUTCFullYear() - birthdate.getUTCFullYear();
  const todayMonthDay = today.getUTCMonth() * 100 + today.getUTCDate();
  const birthdayMonthDay = birthdate.getUTCMonth() * 100 + birthdate.getUTCDate();
  if (todayMonthDay < birthdayMonthDay) {
    years -= 1;
  }
  return years;
}
