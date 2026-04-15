from dataclasses import dataclass
from enum import Enum
from typing import Mapping


class Day(Enum):
    MONDAY = "Monday"
    TUESDAY = "Tuesday"
    WEDNESDAY = "Wednesday"
    THURSDAY = "Thursday"
    FRIDAY = "Friday"
    SATURDAY = "Saturday"
    SUNDAY = "Sunday"


def is_weekend(day: Day) -> bool:
    return day in (Day.SATURDAY, Day.SUNDAY)


# Business numbers are named. F2 is Full-Bake — named constants win.
# Identical values across C#, TypeScript, and Python.
DAILY_OVERTIME_THRESHOLD = 8
STANDARD_WORK_WEEK_HOURS = 40

# The error message string is the spec — identical byte-for-byte across
# all three languages. The exception type is language-idiomatic (ValueError).
ERROR_HOURS_MUST_NOT_BE_NEGATIVE = "hours must not be negative"


@dataclass(frozen=True)
class TimesheetTotals:
    regular_hours: float
    overtime_hours: float

    @property
    def total_hours(self) -> float:
        return self.regular_hours + self.overtime_hours


class Timesheet:
    def __init__(self, entries: Mapping[Day, float]) -> None:
        for hours in entries.values():
            if hours < 0:
                raise ValueError(ERROR_HOURS_MUST_NOT_BE_NEGATIVE)
        self._entries: dict[Day, float] = dict(entries)

    def totals(self) -> TimesheetTotals:
        regular = 0.0
        overtime = 0.0
        for day, hours in self._entries.items():
            if is_weekend(day):
                overtime += hours
            elif hours > DAILY_OVERTIME_THRESHOLD:
                regular += DAILY_OVERTIME_THRESHOLD
                overtime += hours - DAILY_OVERTIME_THRESHOLD
            else:
                regular += hours
        return TimesheetTotals(regular_hours=regular, overtime_hours=overtime)
