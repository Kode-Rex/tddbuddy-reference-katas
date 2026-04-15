namespace LibraryManagement;

public class Member
{
    public Member(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }
}
