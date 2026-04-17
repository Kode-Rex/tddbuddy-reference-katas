# Social Network

A multi-entity social networking domain: users, posts, timelines, following, and walls. Great for showing how builders compose across aggregates and how a `Clock` collaborator keeps timestamp-sensitive tests deterministic.

## What this kata teaches

- **Test Data Builders** — `UserBuilder`, `PostBuilder`, `NetworkBuilder` each fluent and composable.
- **Aggregate Root** — `Network` owns users and posts; all mutations go through it.
- **Timeline vs Wall** — timeline shows a single user's posts; wall shows followed users' posts too. Both reverse-chronological.
- **Clock as Collaborator** — injected so tests control time and assert ordering without sleeping.
- **Implicit Registration** — posting auto-registers a user; the domain handles "create on first use."

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
