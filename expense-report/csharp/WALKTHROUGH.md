# Expense Report — C# Walkthrough

This kata ships in **middle/high gear** — the full C# implementation landed in one commit once the design was understood. Read the [Gears section of the repo README](../../README.md#gears--bridging-tdd-and-bdd) for why that's a deliberate choice, not a corner cut.

Rather than stepping through twenty-two red/green cycles, this walkthrough explains **why the design came out the shape it did** and where each teaching pattern lives.

## The Design at a Glance

```
Report ──owns──> ExpenseItem[]
   │
   ├── EmployeeName : string
   ├── Status : ReportStatus (Draft / Pending / Approved / Rejected)
   ├── Total : Money
   ├── RequiresApproval : bool
   ├── AddExpense(item) — guards against finalized + invalid amounts
   ├── RemoveExpense(item)
   ├── Submit() — guards against empty + over maximum
   ├── Approve() / Reject(reason) / Reopen()
   └── PrintSummary() : string

ExpenseItem
   ├── Description, Amount, Category
   └── IsOverLimit : bool  (delegates to SpendingPolicy)

SpendingPolicy — static lookup of per-item limits + report-level constants
```

## Why `Money` Is a Type

Same rationale as the Bank Account kata. Asserting `report.Total.Should().Be(new Money(1657m))` is semantically richer than asserting on a `decimal`. The type closes the "amount of what?" gap and is ready for currency or rounding extensions.

See `src/ExpenseReport/Money.cs`.

## Why `SpendingPolicy` Is Static

The per-item limits and report-level thresholds are pure domain constants. They don't vary at runtime, so injecting them would add indirection without value. If limits became configurable per-company, `SpendingPolicy` would graduate to an interface — but the tests would not change, because they already assert through domain-language methods (`IsOverLimit`, `RequiresApproval`) rather than hard-coding the numbers.

See `src/ExpenseReport/SpendingPolicy.cs`.

## Why Domain-Specific Exceptions

The spec names six distinct rejections: empty report, exceeds maximum, invalid status transition, expense not found, invalid amount, finalized report. Each gets its own exception class with a **byte-identical message** shared across C#, TypeScript, and Python. Tests catch the meaningful type — `act.Should().Throw<EmptyReportException>()` — and a reader sees the domain in the stack trace.

This is the opposite of throwing `InvalidOperationException` for everything. The exception name **is** the domain rejection.

See `src/ExpenseReport/Exceptions.cs`.

## Why `ReportBuilder` and `ExpenseItemBuilder`

Most tests need a report in a specific lifecycle state (Draft, Pending, Approved, Rejected) with specific expenses. Without builders, every test writes five-to-ten setup lines. With `new ReportBuilder().WithExpense(b => b.AsMeal(30m)).Submitted().Build()`, setup is one line that reads like English.

The `ExpenseItemBuilder` has convenience methods (`AsMeal`, `AsTravel`, `AsEquipment`) that set both the category and a sensible default amount/description. This keeps the test focused on the scenario's distinguishing feature — the amount that's over limit, the status transition that fails.

See `tests/ExpenseReport.Tests/ReportBuilder.cs` and `tests/ExpenseReport.Tests/ExpenseItemBuilder.cs`.

## Why the State Machine Lives in `Report`

An alternative design would extract a `StatusMachine` class. The state transitions here are simple enough — four states, five transitions, each guarded by a one-line check — that extracting them would scatter the domain without reducing complexity. If the workflow grew (e.g., multi-level approval), the extraction would earn its keep.

## Why `PrintSummary` Is a Pure Query

Same as Bank Account's `PrintStatement`: the summary reads from the expense list and formats it. No mutation, no side effect. Tests set up expenses once and assert on the string.

## Scenario Map

Twenty-two scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/ExpenseReport.Tests/ReportTests.cs`, one `[Fact]` per scenario (the per-item limits scenario uses `[Theory]` with five inline data rows). Test names match the scenario titles verbatim.

## How to Run

```bash
cd expense-report/csharp
dotnet test
```
