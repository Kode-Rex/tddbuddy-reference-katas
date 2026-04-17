# Expense Report

Business-logic kata with a rich state machine: categorized expenses, per-category spending limits, approval workflows, and formatted summaries. Excellent for practicing **multiple builders over an aggregate with lifecycle constraints**.

## What this kata teaches

- **Test Data Builders** — `ReportBuilder` and `ExpenseItemBuilder` let tests compose expense reports in one fluent line each.
- **Domain Value Types** — `Money` and `Category` are types, not `decimal` and `string`.
- **State Machine Testing** — Draft → Pending → Approved/Rejected lifecycle with explicit rejection of illegal transitions.
- **Domain-Specific Exceptions** — `EmptyReportException`, `ReportExceedsMaximumException`, `InvalidStatusTransitionException`, `ExpenseNotFoundException`, `InvalidAmountException`, `FinalizedReportException` name the rejection, not the mechanism.
- **Approval Policy as a Query** — `RequiresApproval` is a pure check over expenses and totals, not a side effect.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
