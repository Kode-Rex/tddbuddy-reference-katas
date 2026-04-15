namespace TodoList;

// The TodoList aggregate, named TaskList in C# because:
//   1. Using TodoList here would clash with the namespace TodoList.
//   2. SCENARIOS.md treats "TodoList" as the domain concept; the C# rename to
//      TaskList keeps the type unambiguous while staying inside the domain's
//      ubiquitous language ("a list of tasks").
public sealed class TaskList
{
    private const int FirstTaskId = 1;

    private readonly List<TodoTask> _tasks = new();
    private int _nextId = FirstTaskId;

    public TodoTask Add(string title, DateOnly? due = null)
    {
        var task = new TodoTask(_nextId++, title, Done: false, Due: due);
        _tasks.Add(task);
        return task;
    }

    public void MarkDone(int id)
    {
        var index = IndexOrThrow(id);
        var existing = _tasks[index];
        if (!existing.Done)
        {
            _tasks[index] = existing with { Done = true };
        }
    }

    public void Remove(int id)
    {
        var index = IndexOrThrow(id);
        _tasks.RemoveAt(index);
    }

    public IReadOnlyList<TodoTask> Tasks() => _tasks.AsReadOnly();

    public IReadOnlyList<TodoTask> Pending() =>
        _tasks.Where(t => !t.Done).ToList().AsReadOnly();

    public IReadOnlyList<TodoTask> Completed() =>
        _tasks.Where(t => t.Done).ToList().AsReadOnly();

    private int IndexOrThrow(int id)
    {
        var index = _tasks.FindIndex(t => t.Id == id);
        if (index < 0) throw new TaskNotFoundException(id);
        return index;
    }
}
