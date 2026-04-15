namespace VideoClubRental.Tests;

public class UserBuilder
{
    private string _name = "Alex Member";
    private string _email = "alex@example.com";
    private Age _age = new(30);
    private bool _isAdmin;
    private int _priorityPoints;

    public UserBuilder Named(string name) { _name = name; return this; }
    public UserBuilder WithEmail(string email) { _email = email; return this; }
    public UserBuilder Aged(int years) { _age = new Age(years); return this; }
    public UserBuilder AsAdmin() { _isAdmin = true; return this; }
    public UserBuilder WithPriorityPoints(int points) { _priorityPoints = points; return this; }

    public User Build()
    {
        var user = new User(_name, _email, _age, _isAdmin);
        if (_priorityPoints > 0) user.SeedPriorityPoints(_priorityPoints);
        return user;
    }
}
