# Video Club Rental

A rich domain kata for practicing TDD with multiple interacting business rules: users, rentals, tiered pricing, priority points, wishlists, and donations. Based on Jason Gorman's classic workshop exercise.

## What this kata teaches

- **Test Data Builders** — `UserBuilder`, `TitleBuilder`, `RentalBuilder` make every test a one-liner that reads like a sentence.
- **Object Mothers** — canonical instances (`UserMother.AdultMember`, `TitleMother.NewRelease`) for tests that don't care about incidentals.
- **Mocks as Behavioral Specifications** — email and notification dispatch are collaborations; they're tested with mocks because sending is the behavior.
- **Ubiquitous Language** — `Title`, `Rental`, `PriorityPoints`, `Wishlist` flow from the brief into test names and domain types.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification satisfied by all three language implementations.
