using FluentAssertions;
using Xunit;

namespace TodoList.Tests;

public class TaskListTests
{
    [Fact]
    public void A_new_todo_list_has_no_tasks()
    {
        var list = new TaskList();

        list.Tasks().Should().BeEmpty();
        list.Pending().Should().BeEmpty();
        list.Completed().Should().BeEmpty();
    }

    [Fact]
    public void Adding_a_task_returns_it_with_an_auto_assigned_id_of_1()
    {
        var list = new TaskList();

        var task = list.Add("buy milk");

        task.Should().Be(new TaskBuilder().WithId(1).Titled("buy milk").Build());
    }

    [Fact]
    public void Adding_a_task_with_a_due_date_records_the_due_date()
    {
        var list = new TaskList();
        var due = new DateOnly(2026, 4, 20);

        var task = list.Add("ship release", due);

        task.Should().Be(new TaskBuilder().WithId(1).Titled("ship release").Due(due).Build());
    }

    [Fact]
    public void Ids_are_assigned_sequentially_in_insertion_order()
    {
        var list = new TaskList();

        var a = list.Add("a");
        var b = list.Add("b");
        var c = list.Add("c");

        a.Id.Should().Be(1);
        b.Id.Should().Be(2);
        c.Id.Should().Be(3);
    }

    [Fact]
    public void All_tasks_listing_returns_tasks_in_insertion_order()
    {
        var list = new TaskList();
        list.Add("a");
        list.Add("b");
        list.Add("c");

        list.Tasks().Select(t => t.Title).Should().Equal("a", "b", "c");
    }

    [Fact]
    public void Marking_a_task_as_complete_flips_its_done_flag()
    {
        var list = new TaskList();
        list.Add("a");

        list.MarkDone(1);

        list.Tasks()[0].Done.Should().BeTrue();
    }

    [Fact]
    public void Pending_filter_excludes_completed_tasks()
    {
        var list = new TaskList();
        list.Add("a");
        list.Add("b");
        list.Add("c");
        list.MarkDone(2);

        list.Pending().Select(t => t.Title).Should().Equal("a", "c");
    }

    [Fact]
    public void Completed_filter_excludes_pending_tasks()
    {
        var list = new TaskList();
        list.Add("a");
        list.Add("b");
        list.Add("c");
        list.MarkDone(2);

        list.Completed().Select(t => t.Title).Should().Equal("b");
    }

    [Fact]
    public void Removing_a_task_deletes_it_from_the_list()
    {
        var list = new TaskList();
        list.Add("a");
        list.Add("b");

        list.Remove(2);

        list.Tasks().Select(t => t.Id).Should().Equal(1);
    }

    [Fact]
    public void Ids_are_not_reused_after_removal()
    {
        var list = new TaskList();
        list.Add("a");
        list.Add("b");
        list.Add("c");

        list.Remove(2);
        var next = list.Add("d");

        next.Id.Should().Be(4);
    }

    [Fact]
    public void Marking_an_unknown_id_raises_task_not_found()
    {
        var list = new TaskList();

        var act = () => list.MarkDone(99);

        act.Should().Throw<TaskNotFoundException>().WithMessage("task 99 not found");
    }

    [Fact]
    public void Removing_an_unknown_id_raises_task_not_found()
    {
        var list = new TaskList();

        var act = () => list.Remove(99);

        act.Should().Throw<TaskNotFoundException>().WithMessage("task 99 not found");
    }

    [Fact]
    public void Marking_an_already_completed_task_is_idempotent()
    {
        var list = new TaskList();
        list.Add("a");
        list.MarkDone(1);

        var act = () => list.MarkDone(1);

        act.Should().NotThrow();
        list.Tasks()[0].Done.Should().BeTrue();
    }
}
