# Blog Web App — C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-five red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Blog ──owns──> Post[]
 │               ├── Title, Body, Author, Timestamp
 │               ├── Tags : Set<string>
 │               └── Comments : Comment[]
 │
 ├── CreatePost(user, title, body) : Post
 ├── EditPost(user, postId, title, body)
 ├── DeletePost(user, postId)
 ├── AddComment(user, postId, body) : Comment
 ├── DeleteComment(user, postId, commentId)
 ├── AddTag(user, postId, tag)
 ├── RecentPosts(count) : Post[]
 ├── PostsByTag(tag) : Post[]
 └── AllTagsForUser(user) : Set<string>

Comment(id, author, body, timestamp)

IClock.Now() : DateTime
```

Five domain types, one collaborator interface, one domain exception. Each earns its keep.

## Why `Blog` Is the Aggregate Root

The kata describes a system where users interact through shared state — creating posts, commenting, tagging, querying. Making `Blog` the aggregate root keeps all mutations and authorization checks in one place. No scenario lets a `Post` or `Comment` operate independently of the blog.

## Why `UnauthorizedOperationException`

F3 convention: domain-specific exception types for invariant violations. The kata's central teaching point is authorization boundaries — "you cannot edit another user's post." Naming the exception `UnauthorizedOperationException` rather than throwing a generic `InvalidOperationException` means:

1. Tests catch the meaningful type, not a generic one
2. The error message string is byte-identical across all three languages
3. A reader sees the domain rejection in the stack trace

The message format `"User '<actor>' is not the author of post '<postId>'"` is part of the spec — it appears in SCENARIOS.md and is tested verbatim.

## Why `IClock`, Not `DateTime.Now`

Chronological ordering is central to queries (recent posts, posts by tag). Without an injected clock, two posts created in the same millisecond could appear in either order, and no test could reliably assert "this post appears above that one."

`IClock.Now()` makes the collaboration explicit. `FixedClock` in the test project provides `AdvanceTo` and `AdvanceByMinutes` — tests read "Alice posted at T+0, Bob at T+5" rather than relying on wall-clock timing.

## Why `BlogBuilder` Returns a Tuple

Most tests need a blog with existing posts, comments, and tags. The builder seeds all three in one fluent chain. It returns `(Blog, FixedClock, List<Post>)` — the blog for mutations, the clock for time control, and the post list for referencing post IDs in subsequent operations.

Posts are referenced by **index** in builder methods (`WithComment(0, ...)`, `WithTag(0, ...)`) because IDs are assigned at build time. This keeps builder calls readable without coupling to auto-incremented IDs.

## Why `Post` Is a Class, Not a Record

Unlike `Comment` (immutable, created once), `Post` is mutable — it supports `Edit`, `AddTag`, and `AddComment`. A C# `record` would imply value semantics and immutability, which contradicts the domain. `Post` is an entity with identity (its `Id`).

## Why `Comment` Is a Record

A comment is a value — id, author, body, timestamp — created once and never mutated. C# `record` gives structural equality and concise syntax. The only operation on a comment is deletion, which is handled by the owning `Post`.

## Authorization Pattern

Three operations check ownership: edit, delete, and add-tag. All three call the same `EnsureAuthorOfPost` helper, which throws `UnauthorizedOperationException` with the standardized message. Comment deletion has its own check with a comment-specific message.

This pattern makes authorization boundaries visible in the code — a reader scanning `EditPost` sees the guard clause immediately.

## Scenario Map

The twenty-five scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/BlogWebApp.Tests/BlogTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd blog-web-app/csharp
dotnet test
```
