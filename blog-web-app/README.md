# Blog Web App

A multi-entity blog domain: users, posts, comments, tags, and authorization rules. Great for showing how builders compose across aggregates, how domain exceptions name rejections, and how authorization checks keep ownership boundaries explicit.

## What this kata teaches

- **Test Data Builders** — `BlogBuilder`, `PostBuilder`, `UserBuilder`, `CommentBuilder` each fluent and composable.
- **Aggregate Root** — `Blog` owns users, posts, and comments; all mutations go through it.
- **Domain Exceptions** — `UnauthorizedOperationException` names the rejection; the message string is byte-identical across languages.
- **Authorization** — edit, delete, and tag operations check ownership; tests drive the boundary from both sides.
- **Clock as Collaborator** — injected so tests control time and assert ordering without sleeping.
- **Queries** — recent posts, posts by tag, all tags for user — pure reads over the post list.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
