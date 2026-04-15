from dataclasses import dataclass, replace
from datetime import date
from typing import List, Optional


FIRST_TASK_ID = 1


@dataclass(frozen=True)
class Task:
    id: int
    title: str
    done: bool = False
    due: Optional[date] = None


# Message string is identical byte-for-byte across C#, TypeScript, and Python.
# The format "task <id> not found" is part of the spec (see ../SCENARIOS.md).
class TaskNotFoundError(Exception):
    def __init__(self, id: int) -> None:
        super().__init__(f"task {id} not found")
        self.id = id


class TodoList:
    def __init__(self) -> None:
        self._tasks: List[Task] = []
        self._next_id = FIRST_TASK_ID

    def add(self, title: str, due: Optional[date] = None) -> Task:
        task = Task(id=self._next_id, title=title, done=False, due=due)
        self._next_id += 1
        self._tasks.append(task)
        return task

    def mark_done(self, id: int) -> None:
        index = self._index_or_raise(id)
        existing = self._tasks[index]
        if not existing.done:
            self._tasks[index] = replace(existing, done=True)

    def remove(self, id: int) -> None:
        index = self._index_or_raise(id)
        del self._tasks[index]

    def tasks(self) -> List[Task]:
        return list(self._tasks)

    def pending(self) -> List[Task]:
        return [t for t in self._tasks if not t.done]

    def completed(self) -> List[Task]:
        return [t for t in self._tasks if t.done]

    def _index_or_raise(self, id: int) -> int:
        for i, t in enumerate(self._tasks):
            if t.id == id:
                return i
        raise TaskNotFoundError(id)
