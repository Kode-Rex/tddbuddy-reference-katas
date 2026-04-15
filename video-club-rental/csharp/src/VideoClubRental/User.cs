namespace VideoClubRental;

public class User
{
    private readonly HashSet<string> _wishlist = new(StringComparer.OrdinalIgnoreCase);

    public User(string name, string email, Age age, bool isAdmin = false)
    {
        Name = name;
        Email = email;
        Age = age;
        IsAdmin = isAdmin;
    }

    public string Name { get; }
    public string Email { get; }
    public Age Age { get; }
    public bool IsAdmin { get; }
    public int PriorityPoints { get; private set; }
    public int LoyaltyPoints { get; private set; }
    public bool HasOverdue { get; private set; }

    public IReadOnlyCollection<string> Wishlist => _wishlist;

    internal void AwardPriorityPoints(int points) => PriorityPoints += points;

    internal void DeductPriorityPoints(int points)
    {
        PriorityPoints = Math.Max(0, PriorityPoints - points);
    }

    internal void AwardLoyaltyPoints(int points) => LoyaltyPoints += points;

    internal void MarkOverdue() => HasOverdue = true;

    internal void ClearOverdue() => HasOverdue = false;

    internal void AddWish(string titleName) => _wishlist.Add(titleName);

    internal bool Wishes(string titleName) => _wishlist.Contains(titleName);

    // Test-only seeding helpers (used by builders)
    internal void SeedPriorityPoints(int points) => PriorityPoints = points;
}
