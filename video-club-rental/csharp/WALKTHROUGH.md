# Video Club Rental — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-four red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
VideoClub ──owns──> User[]
    │              └── PriorityPoints, LoyaltyPoints, Wishlist
    ├──owns──> Title[] (name, totalCopies, availableCopies)
    ├──owns──> Rental[] (user, title, rentedOn, dueOn)
    ├──collab──> IClock      — rental dates, overdue detection
    └──collab──> INotifier   — welcome emails, late alerts, wishlist pings
```

`VideoClub` is the aggregate root. Users, titles, and rentals live inside it; the outside world never mutates a `Rental` directly. Two collaborators are pushed out: time (`IClock`) and notifications (`INotifier`). Everything else is internal state.

## Why `Money` Is a Type

Rental pricing is tiered: £2.50, £2.25, £1.75. Asserting `cost.Should().Be(new Money(2.50m))` names what the value means — this is money, these two moneys are equal. A `decimal` could be a percentage, a points total, or a temperature; `Money` can't.

When the inevitable "add VAT" or "apply member discount" extension lands, every site that already spells `Money` is ready. Sites spelling `decimal` would all need rediscovery.

See `src/VideoClubRental/Money.cs`.

## Why `Age` Is a Type (Not an `int`)

Two domain rules pivot on age: registration requires 18+, and the same rule applies when an admin creates another user. Rather than scatter `if (years < 18)` across the aggregate, `Age.IsAdult` names the business concept once. The threshold constant lives on `Age` (`Age.AdultMinimum = 18`) — one place to change if the club opens a U-13 tier.

This is the same reasoning as `Money`: when a primitive carries a domain rule, lift it into a type so the rule has a home.

## Why `IClock`, Not `DateTime.Today`

Rental period is fifteen days. That means returns, priority points, and the overdue-block rule all depend on dates that advance. Tests need to say: "rented on day 0, today is day 16, return is late." `DateTime.Today` makes that impossible to test deterministically — time moves, the test flakes, and worse, the test is coupled to real time, which is a collaboration it never intended.

`IClock.Today()` names the collaboration. `FixedClock.AdvanceDays(16)` in the test project is the deterministic implementation — a tiny hand-rolled fake, not a mocking library. The **Mocks as Behavioral Specifications** principle applies: when time is part of the behavior, make it an interface; when it isn't — like the rental list, which is internal state — don't.

See `src/VideoClubRental/IClock.cs` and `tests/VideoClubRental.Tests/FixedClock.cs`.

## Why `INotifier` Is an Interface

The scenarios say things like "registration dispatches a welcome email" and "donating a wishlisted title notifies the wishlisting user." The *behavior under test* is that the message is sent. If tests called a real SMTP server we'd be testing the wrong thing, and if we let the code silently `Console.WriteLine` we'd have no way to assert it happened.

`INotifier` makes sending explicit. `RecordingNotifier` captures every `(user, message)` pair so tests can ask, "did the welcome arrive at this user?" without caring about transport. `notifier.NotificationsFor(user)` reads like a spec.

Three different scenarios exercise the notifier (welcome, late alert, wishlist ping) — without this seam the kata couldn't be tested as written.

See `src/VideoClubRental/INotifier.cs` and `tests/VideoClubRental.Tests/RecordingNotifier.cs`.

## Why `PricingPolicy` Is a Static Class

The tiered-price rule is pure: inputs (new rental count, existing simultaneous count) → output (total cost). No state, no side effects. A static class with named constants (`BasePrice`, `SecondPrice`, `ThirdPrice`) makes the rule loud in the source — a reader sees the business numbers in one file rather than scattered magic decimals.

`VideoClub.Rent` delegates to `PricingPolicy.Calculate` rather than inlining the switch. Two benefits: the pricing rule has a dedicated home for future tests (e.g. "bulk member discount"), and `Rent` reads at the level of coordination — *look up the title, count existing rentals, price the new one, record it* — not the level of arithmetic.

See `src/VideoClubRental/PricingPolicy.cs`.

## Why Three Builders (User, Title, VideoClub)

Most scenarios need a club pre-populated with a user and a title. Without builders, every test writes six setup lines. With builders:

```csharp
var user = new UserBuilder().WithPriorityPoints(4).Build();
var (club, notifier, clock) = new VideoClubBuilder()
    .WithUser(user)
    .WithTitle(new TitleBuilder().Named("Jaws").Build())
    .Build();
```

Setup reads like English. Defaults (age 30, email `alex@example.com`, 3 copies of "The Godfather") let tests skip the irrelevant bits. Each builder owns the construction rules for its type; the `VideoClubBuilder` composes them.

Crucially, `VideoClubBuilder.Build()` returns **a tuple** of `(VideoClub, RecordingNotifier, FixedClock)`. Tests that advance time need the clock; tests that inspect notifications need the notifier. Returning a tuple keeps the builder pure — no hidden state, no reaching into the aggregate for private fields later.

See `tests/VideoClubRental.Tests/UserBuilder.cs`, `TitleBuilder.cs`, `VideoClubBuilder.cs`.

## Why `User` Has `internal` Mutators

`User.AwardPriorityPoints`, `DeductPriorityPoints`, `MarkOverdue` — these are how the aggregate mutates users, but they must not be callable by arbitrary code outside the domain. `internal` with `InternalsVisibleTo("VideoClubRental.Tests")` gets the access right: the aggregate and its tests can seed state, the outside world can't.

The alternative — public setters or a god-like `User.ApplyEvent(...)` — leaks the domain rule out of `VideoClub` into anyone who holds a `User` reference. With `internal`, the invariants ("points never go below zero", "priority is awarded on on-time return") live exactly where they're enforced.

## Why Returns Branch on `IsLateAt(today)`

`Rental.IsLateAt(DateOnly)` is a pure query on the rental. `VideoClub.ReturnTitle` asks the rental "are you late, given today?" and then awards or deducts accordingly. This keeps `VideoClub` at the coordination level and puts the rental's own invariant on the rental.

After the return, the aggregate checks whether *any* of the user's remaining rentals are still late — if none are, the overdue block clears. This is what the scenario "returning the overdue title unblocks renting" asserts, and it's one line of LINQ because the data model is right.

## Why `MarkOverdueRentals` Is an Explicit Sweep

The overdue-block rule says "a user with an overdue rental cannot rent another title." There are two ways to wire that up:

1. **Implicit sweep on every operation** — `Rent` opens by scanning the user's active rentals against `Clock.Today()`, sets a transient flag, then proceeds. Pure query, no persistent state, but every entry point pays the scan cost and the late-alert notification has to fire from `Rent` (an awkward seam).
2. **Explicit sweep + persistent flag** — a separate `MarkOverdueRentals()` method scans the catalog, mutates `User.HasOverdue`, and dispatches late alerts. `Rent` then reads the flag and rejects.

This implementation chose (2). The reason is that the late alert is a *notification side effect* — it should fire once when the rental crosses the boundary, not every time the user touches the system. Coupling the alert to `Rent` means a user who never tries to rent again never gets alerted; coupling it to a sweep makes the alert a real consequence of "today crossed the due date," not "today, the user happened to call Rent."

The cost is that tests for the overdue-block scenarios have to advance the clock and then call `MarkOverdueRentals` before exercising `Rent`. Tests in `ReturnTests.cs` do this explicitly, which makes the coordination visible in the test rather than hidden inside `Rent`. That visibility is the point — the production caller should also drive this sweep on a schedule (a daily job, a login hook, whatever fits the deployment), and the tests model that exact shape.

The persistent `HasOverdue` flag also lets the return flow clear it by rescanning remaining rentals — a one-line check that wouldn't exist if the flag didn't exist.

## Scenario Map

The twenty-four scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live across six test files in `tests/VideoClubRental.Tests/`:

- `RegistrationTests.cs` — scenarios 1–5
- `RentalPricingTests.cs` — scenarios 6–10
- `ReturnTests.cs` — scenarios 11–15
- `PriorityAccessTests.cs` — scenarios 16–18
- `DonationTests.cs` — scenarios 19–21
- `WishlistTests.cs` — scenarios 22–24

One `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## What's Deliberately Not Modeled

The kata spec lists bonus features — wrong-return detection, rental history, recommendations, membership tiers. This implementation matches the twenty-four SCENARIOS exactly; every line of domain code earns its keep against a named test. Extending for bonuses is a good exercise for a reader — the seams are already there (`Rental` could persist into a history list; `User` could grow a `Tier` computed from loyalty points; `Donate` could reject unknown donors).

## How to Run

```bash
cd video-club-rental/csharp
dotnet test
```
