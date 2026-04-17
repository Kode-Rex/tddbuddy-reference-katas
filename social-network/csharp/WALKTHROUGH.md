# Social Network — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through eighteen red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Network ──owns──> User[]
   │               └── Following : Set<string>
   ├── Post(user, content)
   ├── Follow(follower, followee)
   ├── Timeline(user) : Post[]    — own posts, reverse chrono
   └── Wall(user) : Post[]        — own + followed, reverse chrono

Post(author, content, timestamp)

IClock.Now() : DateTime
```

Three domain types, one collaborator interface. Each earns its keep.

## Why `Network` Is the Aggregate Root

The kata describes a system where users interact through shared state — posting, following, reading timelines and walls. Making `Network` the aggregate root keeps all mutations in one place. There's no scenario where a `User` or `Post` operates independently of the network.

This mirrors the kata's console-application framing: "Commands always start with the user's name" and route through a single in-memory system. `Network` is that system.

## Why `IClock`, Not `DateTime.Now`

Timeline and wall ordering is the kata's core behavior. If tests called `DateTime.Now` directly, two posts created in the same millisecond could appear in either order, and no test could reliably assert "this post appears above that one."

`IClock.Now()` makes the collaboration explicit. `FixedClock` in the test project is a tiny deterministic implementation with `AdvanceTo` and `AdvanceByMinutes` — tests read "Alice posted at T+0, Bob at T+5" rather than "Alice and Bob posted at... whatever time the CI runner reached these lines."

See `src/SocialNetwork/IClock.cs` and `tests/SocialNetwork.Tests/FixedClock.cs`.

## Why `NetworkBuilder`

Most tests need a network with some existing posts and follows. Without a builder, every test writes five setup lines arranging users, clock advances, and post calls. With `new NetworkBuilder().WithPost("Alice", "Hello!", 0).WithFollow("Charlie", "Alice").Build()`, setup is one fluent chain that reads like a scenario description.

The builder returns both the `Network` and the `FixedClock`. Tests that need to advance time after setup (e.g. "post after following") get the clock from the tuple and call `AdvanceByMinutes`.

See `tests/SocialNetwork.Tests/NetworkBuilder.cs`.

## Timeline vs Wall — The Core Distinction

**Timeline** shows a single user's own posts, most recent first. It's a simple author filter plus reverse sort.

**Wall** shows the user's own posts plus all posts from users they follow, most recent first. It computes a "visible authors" set (self + following), filters posts by that set, and sorts.

This is the kata's central behavioral distinction and the one tests must make crystal clear. The test names spell it out: "Timeline does not include posts from other users" vs "Wall includes posts from followed users."

## Why `Post` Is a Record

A `Post` is a value — author, content, timestamp — created once, never mutated. C# `record` gives structural equality for free, which means tests can assert on post values without custom comparers.

## Why `Follow` Returns `bool` but `Post` Returns `void`

`Follow` has a business rule (can't follow yourself, idempotent). Returning `bool` lets tests assert on the result directly. `Post` always succeeds (the kata spec says "assume correct commands"), so `void` is the honest return type — there's nothing to report.

## Scenario Map

The eighteen scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/SocialNetwork.Tests/NetworkTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd social-network/csharp
dotnet test
```
