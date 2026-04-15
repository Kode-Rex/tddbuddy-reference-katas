namespace LibraryManagement;

public class Library
{
    private const string ReservationAvailableTemplate = "'{0}' is now available to borrow";
    private const string ReservationExpiredTemplate = "Your reservation for '{0}' has expired";

    private readonly IClock _clock;
    private readonly INotifier _notifier;
    private readonly Dictionary<string, Book> _books = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Member> _members = new();
    private readonly List<Loan> _loans = new();
    private readonly List<Reservation> _reservations = new();
    private int _nextMemberId = 1;

    public Library(IClock clock, INotifier notifier)
    {
        _clock = clock;
        _notifier = notifier;
    }

    public IReadOnlyCollection<Book> Books => _books.Values;
    public IReadOnlyList<Member> Members => _members;
    public IReadOnlyList<Loan> Loans => _loans;
    public IReadOnlyList<Reservation> Reservations => _reservations;

    public Book AddBook(string title, string author, Isbn isbn)
    {
        if (_books.TryGetValue(isbn.Value, out var existing))
        {
            existing.AddCopy();
            return existing;
        }
        var book = new Book(title, author, isbn);
        book.AddCopy();
        _books[isbn.Value] = book;
        return book;
    }

    public Book AddCopyOf(Isbn isbn)
    {
        var book = RequireBook(isbn);
        book.AddCopy();
        return book;
    }

    public void RemoveCopy(Isbn isbn)
    {
        var book = RequireBook(isbn);
        book.RemoveOneCopy();
        if (book.CopyCount == 0)
        {
            _books.Remove(isbn.Value);
        }
    }

    public Member Register(string name)
    {
        var member = new Member(_nextMemberId++, name);
        _members.Add(member);
        return member;
    }

    public Loan CheckOut(Member member, Isbn isbn)
    {
        var book = RequireBook(isbn);
        var today = _clock.Today();

        // Head-of-queue check: if there's a notified reservation for this member, satisfy it.
        var reservedCopy = book.FindReservedCopy();
        if (reservedCopy is not null)
        {
            var head = HeadReservationFor(isbn);
            if (head is null || !ReferenceEquals(head.Member, member))
            {
                throw new NoCopiesAvailableException($"No copies of '{isbn}' are available");
            }
            _reservations.Remove(head);
            reservedCopy.MarkCheckedOut();
            var reservedLoan = new Loan(member, reservedCopy, today);
            _loans.Add(reservedLoan);
            return reservedLoan;
        }

        var available = book.FindAvailableCopy()
            ?? throw new NoCopiesAvailableException($"No copies of '{isbn}' are available");

        available.MarkCheckedOut();
        var loan = new Loan(member, available, today);
        _loans.Add(loan);
        return loan;
    }

    public Money ReturnCopy(Member member, Isbn isbn)
    {
        var loan = _loans.FirstOrDefault(l =>
            !l.IsClosed && ReferenceEquals(l.Member, member) && l.Copy.Isbn.Equals(isbn))
            ?? throw new NoActiveLoanException($"Member has no active loan of '{isbn}'");

        var today = _clock.Today();
        loan.Close(today);
        _loans.Remove(loan);
        var fine = loan.FineFor(today);

        var head = HeadReservationFor(isbn);
        if (head is not null)
        {
            loan.Copy.MarkReserved();
            head.MarkNotified(today);
            _notifier.Send(head.Member, string.Format(ReservationAvailableTemplate, BookTitle(isbn)));
        }
        else
        {
            loan.Copy.MarkAvailable();
        }

        return fine;
    }

    public void Reserve(Member member, Isbn isbn)
    {
        RequireBook(isbn);
        _reservations.Add(new Reservation(member, isbn, _clock.Today()));
    }

    public void ExpireReservations()
    {
        var today = _clock.Today();
        var expired = _reservations.Where(r => r.HasExpiredAt(today)).ToList();
        foreach (var reservation in expired)
        {
            _reservations.Remove(reservation);
            _notifier.Send(
                reservation.Member,
                string.Format(ReservationExpiredTemplate, BookTitle(reservation.Isbn)));

            // Pass the reserved copy to the next waiter (if any); else release it.
            var book = _books.TryGetValue(reservation.Isbn.Value, out var b) ? b : null;
            var reservedCopy = book?.FindReservedCopy();
            if (reservedCopy is null) continue;

            var next = HeadReservationFor(reservation.Isbn);
            if (next is not null)
            {
                next.MarkNotified(today);
                _notifier.Send(next.Member, string.Format(ReservationAvailableTemplate, BookTitle(reservation.Isbn)));
            }
            else
            {
                reservedCopy.MarkAvailable();
            }
        }
    }

    public IReadOnlyList<Reservation> ReservationsFor(Isbn isbn) =>
        _reservations.Where(r => r.Isbn.Equals(isbn)).ToList();

    private Reservation? HeadReservationFor(Isbn isbn) =>
        _reservations.FirstOrDefault(r => r.Isbn.Equals(isbn));

    private Book RequireBook(Isbn isbn) =>
        _books.TryGetValue(isbn.Value, out var book)
            ? book
            : throw new BookNotInCatalogException($"Book '{isbn}' is not in the catalog");

    private string BookTitle(Isbn isbn) =>
        _books.TryGetValue(isbn.Value, out var book) ? book.Title : isbn.Value;

    internal void SeedMember(Member member)
    {
        _members.Add(member);
        if (member.Id >= _nextMemberId) _nextMemberId = member.Id + 1;
    }

    internal Book SeedBook(Book book, int copyCount)
    {
        _books[book.Isbn.Value] = book;
        for (var i = 0; i < copyCount; i++) book.AddCopy();
        return book;
    }
}
