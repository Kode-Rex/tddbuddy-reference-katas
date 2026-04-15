namespace TodoList;

// Named TodoTask (not Task) to avoid collision with System.Threading.Tasks.Task.
// The SCENARIOS.md spec refers to this entity as "Task" in domain language; the
// C# rename is purely to keep the type name unambiguous at the call site.
public sealed record TodoTask(int Id, string Title, bool Done, DateOnly? Due);
