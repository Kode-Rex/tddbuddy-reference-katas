namespace TodoList.Tests;

// Test-folder builder for constructing expected TodoTask values in assertions.
// The TaskList aggregate is the runtime source of truth; this builder exists
// so tests read one fluent line when naming "the task I expect to see".
public sealed class TaskBuilder
{
    private const int DefaultId = 1;
    private const string DefaultTitle = "";

    private int _id = DefaultId;
    private string _title = DefaultTitle;
    private bool _done;
    private DateOnly? _due;

    public TaskBuilder WithId(int id) { _id = id; return this; }
    public TaskBuilder Titled(string title) { _title = title; return this; }
    public TaskBuilder Due(DateOnly due) { _due = due; return this; }
    public TaskBuilder Done() { _done = true; return this; }

    public TodoTask Build() => new(_id, _title, _done, _due);
}
