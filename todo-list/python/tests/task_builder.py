from datetime import date
from typing import Optional

from todo_list import Task


DEFAULT_ID = 1
DEFAULT_TITLE = ""


class TaskBuilder:
    def __init__(self) -> None:
        self._id = DEFAULT_ID
        self._title = DEFAULT_TITLE
        self._done = False
        self._due: Optional[date] = None

    def with_id(self, id: int) -> "TaskBuilder":
        self._id = id
        return self

    def titled(self, title: str) -> "TaskBuilder":
        self._title = title
        return self

    def due(self, due: date) -> "TaskBuilder":
        self._due = due
        return self

    def done(self) -> "TaskBuilder":
        self._done = True
        return self

    def build(self) -> Task:
        return Task(id=self._id, title=self._title, done=self._done, due=self._due)
