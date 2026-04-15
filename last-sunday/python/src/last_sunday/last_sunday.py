from calendar import monthrange
from datetime import date, timedelta


def find(year: int, month: int) -> date:
    _, days_in_month = monthrange(year, month)
    last_day = date(year, month, days_in_month)
    # Python weekday: Monday=0 .. Sunday=6
    days_to_walk_back = (last_day.weekday() - 6 + 7) % 7
    return last_day - timedelta(days=days_to_walk_back)
