from timesheet_calc import Day, Timesheet


class TimesheetBuilder:
    def __init__(self) -> None:
        self._entries: dict[Day, float] = {}

    def with_entry(self, day: Day, hours: float) -> "TimesheetBuilder":
        self._entries[day] = hours
        return self

    def build(self) -> Timesheet:
        return Timesheet(self._entries)
