# Library Management

A multi-entity domain: books, members, loans, reservations. Great for showing how builders compose across aggregates and how collaborations (due-date notifications, reservation queues) are specified through mocks.

## What this kata teaches

- **Test Data Builders** — `BookBuilder`, `MemberBuilder`, `LoanBuilder` each fluent and composable.
- **Object Mothers** — canonical fixtures like `BookMother.AvailableFiction`, `MemberMother.ActivePatron`.
- **State Machines** — a book's status (`Available` → `CheckedOut` → `Reserved`) encoded as a first-class domain type, not flag checks scattered across methods.
- **Mocks as Specifications** — reservation notifications are collaborator behavior, tested with mocks.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
