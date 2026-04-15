# Library Management — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Library ──owns──> Book[] (title, author, ISBN, copies)
    │              └── Copy (id, status: Available | CheckedOut | Reserved)
    ├──owns──> Member[]
    ├──owns──> Loan[]        (member, copy, borrowedOn, dueOn)
    ├──owns──> Reservation[] (member, isbn, reservedOn, notifiedOn)
    ├──collab──> IClock      — due dates, overdue detection, reservation expiry
    └──collab──> INotifier   — reservation-available and reservation-expired emails
```

`Library` is the aggregate root. Books, members, loans, and reservations live inside it; the outside world never mutates a `Copy` or a `Loan` directly. Two collaborators are pushed out: time (`IClock`) and notifications (`INotifier`).

## Why `Copy` Is a Separate Entity (Not Just "Available Count" on `Book`)

The video-club-rental kata tracks `TotalCopies` and `AvailableCopies` as integers on `Title`. That works when copies are interchangeable. Here they aren't: one copy can be `CheckedOut` (loaned to Alice), another `Reserved` (held for Bob after he got the notification), and a third `Available`. The **reservation flow** specifically needs a copy to sit in `Reserved` state — tied to a specific pending loan — from the moment it's returned until the reserver comes to collect it.

An integer count can't encode that. A list of `Copy` entities with their own state can. Each copy carries its own `Status` and an id; the `Book` holds the catalog-level metadata (title, author, ISBN) that all copies share.

See `src/LibraryManagement/Book.cs`, `Copy.cs`, `CopyStatus.cs`.

## Why `Loan` and `Reservation` Are Entities With Their Own State

The alternative is flag-soup on `Copy`: `BorrowedBy`, `BorrowedOn`, `ReservedBy`, `NotifiedOn` — all nullable, all meaningful in combination. The domain pays for that nullability every time it reads `Copy`.

Extracting `Loan` and `Reservation` gives each concept a dedicated home for its invariants. `Loan.FineFor(today)` is a pure query on the loan — it knows its due date and the fine rate, so the math lives with the data. `Reservation.HasExpiredAt(today)` is the same shape: the expiry rule belongs on the reservation, not smeared across `Library`.

The `Library` aggregate then reads at the level of coordination — *find the loan, close it, compute the fine, check the reservation queue* — rather than the level of nullable-field arithmetic.

See `src/LibraryManagement/Loan.cs`, `Reservation.cs`.

## Why Fine Calculation Lives on `Loan`, Not `Library`

`Loan.FineFor(DateOnly returnDate)` returns a `Money`. Two reasons it belongs on the loan:

1. **The rule depends only on the loan.** Due date is on the loan; fine rate is a constant; the computation is `(returnDate - dueOn) * FinePerDay` clamped at zero. `Library` has no role in it.
2. **The aggregate stays readable.** `Library.ReturnCopy` reads as coordination: *find the loan, close it, ask it what the fine is, update the copy, notify if reserved.* No arithmetic in the aggregate, no business number in `Library`'s source.

The named constants `Loan.LoanPeriodDays = 14` and `Loan.FinePerDay = new Money(0.10m)` live on `Loan` for the same reason — they're the loan's rules.

See `src/LibraryManagement/Loan.cs`.

## Why `expireReservations()` Is an Explicit Sweep

Reservations that have been notified expire after three days if the member doesn't check out. There are two ways to wire that up:

1. **Implicit sweep on every operation** — every call to `Reserve`, `CheckOut`, `ReturnCopy` starts by purging expired reservations. Pure query, no persistent state, but every entry point pays the scan cost and the expiration notification fires from whichever unrelated operation happened to trip it.
2. **Explicit sweep** — a separate `ExpireReservations()` method scans the reservations, drops expired ones, notifies those members, and passes the reserved copy on to the next waiter (or releases it).

This implementation chose (2), the same shape as video-club-rental's [`MarkOverdueRentals`](../../video-club-rental/csharp/WALKTHROUGH.md#why-markoverduerentals-is-an-explicit-sweep). The reason is the same: the expiration notification is a *side effect of time passing*, not of any user action. Firing it from `CheckOut` means a member who never checks out anything again never gets the expiry notice; firing it from a sweep makes the notification a real consequence of "today crossed the three-day boundary."

The cost is that tests for expiration have to advance the clock and then call `ExpireReservations()` before asserting. That visibility is the point — the production caller should also drive this sweep on a schedule (a nightly job, a login hook, whatever fits the deployment), and the tests model that exact shape.

See `src/LibraryManagement/Library.cs`.

## Why `Money` Is a Type (Not a `decimal`)

Fines are monetary values: `£0.10` per day, £1.00 total after ten days late. Asserting `fine.Should().Be(new Money(0.10m))` names what the value means — this is money, these two moneys are equal. A `decimal` could be a percentage, a points total, or a temperature; `Money` can't.

When the inevitable "different fine for reference books" or "monthly membership fee" extension lands, every site that already spells `Money` is ready.

See `src/LibraryManagement/Money.cs`.

## Why `Isbn` Is a Type (Not a `string`)

`AddBook`, `AddCopyOf`, `CheckOut`, `ReturnCopy`, `Reserve` all take an ISBN. If that parameter is `string`, any caller can pass any string — the author's name, the title, a book's subtitle — and the compiler agrees. If it's `Isbn`, the call site has to construct one, which is the moment the type system asks "is this really an ISBN?"

This is the same reasoning as `Money`: when a primitive carries a domain meaning, lift it into a type so the meaning has a home.

See `src/LibraryManagement/Isbn.cs`.

## Why `IClock` and `INotifier` Are Interfaces

Three rules pivot on time: loan due dates (today + 14 days), late fines (today - dueOn), reservation expiry (today - notifiedOn > 3 days). `DateTime.Today` would make those impossible to test deterministically. `IClock.Today()` names the collaboration, and `FixedClock.AdvanceDays(15)` in the test project is the deterministic fake.

Two rules produce outbound notifications: reservation-available (when a copy is returned to a non-empty queue) and reservation-expired (when three days pass without checkout). The behavior under test *is that the message is sent.* `INotifier` makes sending explicit; `RecordingNotifier` captures every `(member, message)` pair so tests can ask "did the available notice arrive at this member?" without caring about transport.

`notifier.AvailabilityNotificationsFor(reserver).Should().ContainSingle()` reads like a spec. That's the **Mocks as Behavioral Specifications** principle — mocks earn their keep only where the collaboration *is* the behavior.

See `src/LibraryManagement/IClock.cs`, `INotifier.cs`, and `tests/LibraryManagement.Tests/FixedClock.cs`, `RecordingNotifier.cs`.

## Why Three Builders (`BookBuilder`, `MemberBuilder`, `LibraryBuilder`)

Most scenarios need a library pre-populated with a book (and its copies) and a member. Without builders, every test writes half a dozen setup lines. With builders:

```csharp
var member = new MemberBuilder().Named("Alice").Build();
var (library, notifier, clock) = new LibraryBuilder()
    .OpenedOn(Day0)
    .WithMember(member)
    .WithBook(new BookBuilder().WithIsbn("978-...").WithCopies(2))
    .Build();
```

Setup reads like English. Defaults (title "The Pragmatic Programmer", one copy) let tests skip the irrelevant bits.

`LibraryBuilder.Build()` returns a **tuple** of `(Library, RecordingNotifier, FixedClock)`. Tests that advance time need the clock; tests that inspect notifications need the notifier. Returning a tuple keeps the builder pure — no hidden state, no reaching into the aggregate for private fields later.

See `tests/LibraryManagement.Tests/BookBuilder.cs`, `MemberBuilder.cs`, `LibraryBuilder.cs`.

## Why `Copy`, `Loan`, and `Reservation` Have `internal` Mutators

`Copy.MarkCheckedOut`, `Loan.Close`, `Reservation.MarkNotified` — these are how the aggregate advances the state machine, but they must not be callable from arbitrary code outside the domain. `internal` with `InternalsVisibleTo("LibraryManagement.Tests")` gets the access right: `Library` and its tests can drive state; the outside world can't.

The alternative — public setters — would leak the state-machine rules out of `Library` into anyone holding a `Copy` reference. With `internal`, the invariants (*copies only move Available → CheckedOut → Available or Available → Reserved → CheckedOut*) live exactly where they're enforced.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live across five test files in `tests/LibraryManagement.Tests/`:

- `BookTests.cs` — scenarios 1–5
- `MemberTests.cs` — scenarios 6–7
- `CheckoutTests.cs` — scenarios 8–10
- `ReturnTests.cs` — scenarios 11–15
- `ReservationTests.cs` — scenarios 16–20

One `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## What's Deliberately Not Modeled

The kata spec lists bonus features — patron management dashboards, e-books, search and filtering, recommendations. This implementation matches the twenty SCENARIOS exactly; every line of domain code earns its keep against a named test. Extending for bonuses is a good exercise for a reader — the seams are already there (`Member` could grow a profile; `Book` could grow a `Medium` discriminator; `Library.Find(query)` could front a search index).

## How to Run

```bash
cd library-management/csharp
dotnet test
```
