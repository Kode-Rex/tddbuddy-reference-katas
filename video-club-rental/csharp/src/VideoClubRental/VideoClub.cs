namespace VideoClubRental;

public class VideoClub
{
    public const int PriorityAccessThreshold = 5;
    public const int OnTimeReturnAward = 2;
    public const int LateReturnPenalty = 2;
    public const int DonationLoyaltyAward = 10;

    private const string WelcomeMessage = "Welcome to the video club";
    private const string LateAlertTemplate = "Your rental of '{0}' is overdue";
    private const string WishlistAvailableTemplate = "'{0}' is now available";

    private readonly IClock _clock;
    private readonly INotifier _notifier;
    private readonly Dictionary<string, User> _users = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Title> _titles = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Rental> _rentals = new();

    public VideoClub(IClock clock, INotifier notifier)
    {
        _clock = clock;
        _notifier = notifier;
    }

    public IReadOnlyCollection<User> Users => _users.Values;
    public IReadOnlyCollection<Title> Titles => _titles.Values;

    public User Register(string name, string email, Age age)
    {
        if (!age.IsAdult)
            throw new InvalidOperationException($"User must be at least {Age.AdultMinimum} to register");

        var user = new User(name, email, age);
        _users[email] = user;
        _notifier.Send(user, WelcomeMessage);
        return user;
    }

    public User CreateUser(User admin, string name, string email, Age age)
    {
        if (!admin.IsAdmin)
            throw new InvalidOperationException("Only admin users may create other users");
        return Register(name, email, age);
    }

    internal void SeedUser(User user) => _users[user.Email] = user;

    public Title AddTitle(Title title)
    {
        _titles[title.Name] = title;
        return title;
    }

    public Money Rent(User user, string titleName)
    {
        if (user.HasOverdue)
            throw new InvalidOperationException("User has an overdue rental and cannot rent");

        var title = RequireTitle(titleName);
        var existingRentals = ActiveRentalsFor(user).Count;
        var cost = PricingPolicy.Calculate(newRentalCount: 1, existingRentalCount: existingRentals);

        title.CheckOut();
        _rentals.Add(new Rental(user, title, _clock.Today()));
        return cost;
    }

    public void ReturnTitle(User user, string titleName)
    {
        var rental = ActiveRentalsFor(user).FirstOrDefault(r =>
            string.Equals(r.Title.Name, titleName, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"User has no active rental of '{titleName}'");

        var today = _clock.Today();
        _rentals.Remove(rental);
        rental.Title.CheckIn();

        if (rental.IsLateAt(today))
        {
            user.DeductPriorityPoints(LateReturnPenalty);
            _notifier.Send(user, string.Format(LateAlertTemplate, rental.Title.Name));
        }
        else
        {
            user.AwardPriorityPoints(OnTimeReturnAward);
        }

        if (!ActiveRentalsFor(user).Any(r => r.IsLateAt(today)))
            user.ClearOverdue();
    }

    public void MarkOverdueRentals()
    {
        var today = _clock.Today();
        foreach (var rental in _rentals.Where(r => r.IsLateAt(today)))
            rental.User.MarkOverdue();
    }

    public bool HasPriorityAccess(User user) => user.PriorityPoints >= PriorityAccessThreshold;

    public void AddToWishlist(User user, string titleName) => user.AddWish(titleName);

    public void Donate(User donor, string titleName)
    {
        if (_titles.TryGetValue(titleName, out var existing))
            existing.AddCopy();
        else
            AddTitle(new Title(titleName, totalCopies: 1));

        donor.AwardLoyaltyPoints(DonationLoyaltyAward);

        foreach (var user in _users.Values.Where(u => u.Wishes(titleName)))
            _notifier.Send(user, string.Format(WishlistAvailableTemplate, titleName));
    }

    private Title RequireTitle(string titleName) =>
        _titles.TryGetValue(titleName, out var title)
            ? title
            : throw new InvalidOperationException($"Title '{titleName}' is not in the catalog");

    private List<Rental> ActiveRentalsFor(User user) =>
        _rentals.Where(r => ReferenceEquals(r.User, user)).ToList();
}
