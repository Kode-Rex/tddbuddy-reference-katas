namespace TodoList;

// Message string is identical byte-for-byte across C#, TypeScript, and Python.
// The format "task <id> not found" is part of the spec (see ../SCENARIOS.md).
public sealed class TaskNotFoundException : Exception
{
    public int Id { get; }

    public TaskNotFoundException(int id) : base($"task {id} not found")
    {
        Id = id;
    }
}
