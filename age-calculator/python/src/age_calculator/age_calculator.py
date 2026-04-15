from datetime import date


def calculate(birthdate: date, today: date) -> int:
    if birthdate > today:
        raise ValueError("birthdate is after today")

    years = today.year - birthdate.year
    if (today.month, today.day) < (birthdate.month, birthdate.day):
        years -= 1
    return years
