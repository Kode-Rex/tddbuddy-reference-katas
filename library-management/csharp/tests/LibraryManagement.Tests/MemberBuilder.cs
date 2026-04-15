namespace LibraryManagement.Tests;

public class MemberBuilder
{
    private static int _nextId = 1000;
    private int _id = System.Threading.Interlocked.Increment(ref _nextId);
    private string _name = "Alex Reader";

    public MemberBuilder Named(string name) { _name = name; return this; }
    public MemberBuilder WithId(int id) { _id = id; return this; }

    public Member Build() => new(_id, _name);
}
