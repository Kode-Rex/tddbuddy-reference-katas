from datetime import date

import pytest

from todo_list import TaskNotFoundError, TodoList
from tests.task_builder import TaskBuilder


def test_a_new_todo_list_has_no_tasks():
    todo = TodoList()
    assert todo.tasks() == []
    assert todo.pending() == []
    assert todo.completed() == []


def test_adding_a_task_returns_it_with_an_auto_assigned_id_of_1():
    todo = TodoList()
    task = todo.add("buy milk")
    assert task == TaskBuilder().with_id(1).titled("buy milk").build()


def test_adding_a_task_with_a_due_date_records_the_due_date():
    todo = TodoList()
    task = todo.add("ship release", date(2026, 4, 20))
    assert task == (
        TaskBuilder().with_id(1).titled("ship release").due(date(2026, 4, 20)).build()
    )


def test_ids_are_assigned_sequentially_in_insertion_order():
    todo = TodoList()
    a = todo.add("a")
    b = todo.add("b")
    c = todo.add("c")
    assert [a.id, b.id, c.id] == [1, 2, 3]


def test_all_tasks_listing_returns_tasks_in_insertion_order():
    todo = TodoList()
    todo.add("a")
    todo.add("b")
    todo.add("c")
    assert [t.title for t in todo.tasks()] == ["a", "b", "c"]


def test_marking_a_task_as_complete_flips_its_done_flag():
    todo = TodoList()
    todo.add("a")
    todo.mark_done(1)
    assert todo.tasks()[0].done is True


def test_pending_filter_excludes_completed_tasks():
    todo = TodoList()
    todo.add("a")
    todo.add("b")
    todo.add("c")
    todo.mark_done(2)
    assert [t.title for t in todo.pending()] == ["a", "c"]


def test_completed_filter_excludes_pending_tasks():
    todo = TodoList()
    todo.add("a")
    todo.add("b")
    todo.add("c")
    todo.mark_done(2)
    assert [t.title for t in todo.completed()] == ["b"]


def test_removing_a_task_deletes_it_from_the_list():
    todo = TodoList()
    todo.add("a")
    todo.add("b")
    todo.remove(2)
    assert [t.id for t in todo.tasks()] == [1]


def test_ids_are_not_reused_after_removal():
    todo = TodoList()
    todo.add("a")
    todo.add("b")
    todo.add("c")
    todo.remove(2)
    new_task = todo.add("d")
    assert new_task.id == 4


def test_marking_an_unknown_id_raises_task_not_found():
    todo = TodoList()
    with pytest.raises(TaskNotFoundError, match="task 99 not found"):
        todo.mark_done(99)


def test_removing_an_unknown_id_raises_task_not_found():
    todo = TodoList()
    with pytest.raises(TaskNotFoundError, match="task 99 not found"):
        todo.remove(99)


def test_marking_an_already_completed_task_is_idempotent():
    todo = TodoList()
    todo.add("a")
    todo.mark_done(1)
    todo.mark_done(1)
    assert todo.tasks()[0].done is True
