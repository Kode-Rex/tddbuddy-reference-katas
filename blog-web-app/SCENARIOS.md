# Blog Web App — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Blog** | The aggregate root (`Blog`) holding users, posts, and comments |
| **User** | A registered person (`User`) identified by name |
| **Post** | A blog entry (`Post`) with id, title, body, author, tags, and timestamp |
| **Comment** | A remark (`Comment`) on a post, with id, author, body, and timestamp |
| **Tag** | A string label attached to a post by its author |
| **Clock** | Collaborator that returns "now" — injected so tests control timestamps deterministically |
| **Unauthorized** | An operation rejected because the acting user is not the owner of the target resource |

## Domain Rules

- A new blog starts with **no users**, **no posts**, and **no comments**
- **Creating a post** with a user name that does not yet exist **auto-registers** the user
- Each post has a unique **id** assigned by the blog, a **title**, a **body**, an **author**, a **timestamp** from the clock, an initially empty **tag list**, and an initially empty **comment list**
- A user can **edit** (update title and body of) their own post
- A user **cannot edit** another user's post — the blog rejects the operation with `"User '<actor>' is not the author of post '<postId>'"`
- A user can **delete** their own post; deleting removes the post and its comments
- A user **cannot delete** another user's post — same rejection message as edit
- Any user can **comment** on any post; commenting auto-registers the commenter
- Each comment records its **author**, **body**, **timestamp** from the clock, and a unique **id**
- A user can **delete their own comment** from a post
- A user **cannot delete** another user's comment — the blog rejects with `"User '<actor>' is not the author of comment '<commentId>'"`
- A post's author can **add tags** to their own post
- A user **cannot add tags** to another user's post — same rejection message as edit
- **Recent posts** returns the N most recent posts across all users, **most recent first**
- **Posts by tag** returns all posts with a given tag, **most recent first**
- **All tags for user** returns the distinct set of tags across all of a user's posts

## Test Scenarios

### Post Creation

1. **New blog has no posts**
2. **Creating a post auto-registers the user**
3. **A post records title, body, author, and timestamp**
4. **A user can create multiple posts**
5. **Each post receives a unique id**

### Editing Posts

6. **A user can edit their own post**
7. **Editing updates both title and body**
8. **Editing another user's post throws unauthorized**

### Deleting Posts

9. **A user can delete their own post**
10. **Deleting another user's post throws unauthorized**
11. **Deleting a post removes its comments**

### Comments

12. **Any user can comment on any post**
13. **A comment records author, body, and timestamp**
14. **Commenting auto-registers the user**
15. **A user can delete their own comment**
16. **Deleting another user's comment throws unauthorized**

### Tags

17. **A post author can add a tag to their own post**
18. **Adding a tag to another user's post throws unauthorized**
19. **Adding the same tag twice is idempotent**

### Queries

20. **Recent posts returns the N most recent posts**
21. **Recent posts returns fewer than N when not enough posts exist**
22. **Posts by tag returns matching posts most recent first**
23. **Posts by tag returns empty when no posts match**
24. **All tags for user returns distinct tags across their posts**
25. **All tags for user returns empty when user has no tags**
