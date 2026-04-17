# Social Network — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Network** | The aggregate root (`Network`) holding users and posts |
| **User** | A registered person (`User`) identified by name |
| **Post** | A message (`Post`) with author, content, and timestamp |
| **Timeline** | A user's own posts in reverse chronological order |
| **Wall** | A user's own posts plus posts from users they follow, in reverse chronological order |
| **Follow** | A directional subscription: follower sees followee's posts on their wall |
| **Clock** | Collaborator that returns "now" — injected so tests control timestamps deterministically |

## Domain Rules

- A new network starts with **no users** and **no posts**
- **Posting** a message with a user name that does not yet exist **auto-registers** the user
- Each post records its **author**, **content**, and the **timestamp** from the clock
- **Timeline** returns only the user's own posts, **most recent first**
- **Following** is directional — Alice following Bob does not mean Bob follows Alice
- **Wall** returns the user's own posts **plus** all posts from users they follow, **most recent first**
- A user cannot follow themselves
- Following the same user twice has no effect (idempotent)

## Test Scenarios

### Registration

1. **New network has no users**
2. **Posting auto-registers a new user**
3. **Posting with an existing user does not duplicate registration**

### Posting

4. **A user can post a message**
5. **A post records the content and timestamp from the clock**
6. **A user can post multiple messages**

### Timeline

7. **Timeline of a user with no posts is empty**
8. **Timeline returns the user's own posts**
9. **Timeline returns posts in reverse chronological order**
10. **Timeline does not include posts from other users**

### Following

11. **A user can follow another user**
12. **Following is idempotent**
13. **A user cannot follow themselves**

### Wall

14. **Wall of a user with no posts and no follows is empty**
15. **Wall shows the user's own posts**
16. **Wall includes posts from followed users**
17. **Wall returns posts in reverse chronological order across all authors**
18. **Wall does not include posts from users not followed**
