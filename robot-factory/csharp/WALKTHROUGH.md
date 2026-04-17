# Robot Factory ��� C# Walkthrough

This kata ships in **middle gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice.

Rather than stepping through twenty red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Factory ──uses──> IPartSupplier[]
   │                    │
   ├── CostRobot(order)     ──queries──> GetQuote(type, option)
   └── PurchaseRobot(order) ──calls───> Purchase(type, option)

RobotOrder ──contains──> { PartType → PartOption }
CostBreakdown ──contains──> PartQuote[] + Total
```

Five pieces. Each earns its keep.

## Why `IPartSupplier` Is an Interface

The kata spec says parts come from external suppliers via web services. That's a collaboration boundary — production code will call HTTP endpoints; test code substitutes `FakePartSupplier` instances that hold an in-memory catalog. The interface has three members: `Name`, `GetQuote`, and `Purchase`.

`FakePartSupplier` in the test project is not a mocking-library mock. It's a hand-rolled fake with a `PurchaseLog` so tests can verify *which* suppliers were actually called during purchase. This is the **Mocks as Behavioral Specifications** principle: the purchase calls are the behavior under test.

## Why `RobotOrder` Validates Itself

An incomplete order (missing one or more part types) is a domain invariant violation, not a caller concern. `RobotOrder.Validate()` checks that all five `PartType` values are present and throws `OrderIncompleteException` with a message naming the missing types.

The factory calls `Validate()` as its first step in both `CostRobot` and `PurchaseRobot`. Tests that exercise the incomplete-order path use `RobotOrderBuilder().Without(PartType.Power)` to surgically remove one part type from an otherwise-complete order.

## Why `Money` Is a Type

Same rationale as every F3 kata in this repo: asserting `quote.Price.Should().Be(new Money(42m))` says "this is a monetary quantity" rather than "this is a decimal that happens to represent money." The `IComparable<Money>` implementation lets LINQ's `MinBy` compare prices naturally.

## Why Two Builders

`RobotOrderBuilder` defaults to a complete, valid order (all five part types configured). Tests that need a specific configuration override one or two options; tests that need an invalid order call `.Without(PartType.X)`. One line of setup, clear intent.

`SupplierBuilder` constructs `FakePartSupplier` instances with a named catalog. Tests declare which parts a supplier carries and at what price, in a fluent chain that reads as a supplier specification.

Together, the two builders let each test scenario read as a single paragraph: "given these suppliers with these catalogs, when I cost this order, the cheapest parts come from the expected sources."

## Why Domain Exceptions

`OrderIncompleteException` and `PartNotAvailableException` are named for the domain rejection, not the mechanism. A reader scanning the test file sees `Throw<PartNotAvailableException>().WithMessage("Part not available: InfraredVision")` and knows exactly what domain rule was violated. The message strings are byte-identical across all three language implementations.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/RobotFactory.Tests/RobotFactoryTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim.

## How to Run

```bash
cd robot-factory/csharp
dotnet test
```
