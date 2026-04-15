# Library Management — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Library** | The aggregate root (`Library`) holding books and members |
| **Book** | A catalog entry (`Book`) with title, author, ISBN, and per-copy statuses |
| **Copy** | A single physical copy (`Copy`) of a book with its own status |
| **Member** | A patron (`Member`) who can borrow and reserve books |
| **Loan** | A checkout of a copy to a member (`Loan`) with a due date |
| **Reservation** | A member's request (`Reservation`) to borrow a book that's currently out |
| **Notifier** | Collaborator that emails reservation holders; mocked in tests |
| **Clock** | Collaborator that returns "today" — injected to keep tests deterministic |

## Domain Rules

- A new library starts with **no books** and **no members**
- A copy's status is one of **Available**, **CheckedOut**, or **Reserved**
- **Checking out** a copy sets its status to `CheckedOut`, creates a `Loan`, and sets the due date to `today + loanDays` (default 14 days)
- **Returning** a copy sets its status to `Available` and closes the loan
- **Late return**: returning after the due date incurs a **fine of £0.10 per day** overdue
- **Reserving** a book when no copies are available places the member in the reservation queue
- **Returning** a copy with a non-empty queue sets the copy to `Reserved`, pops the head of the queue, and **notifies** the reserver
- A reservation **expires** if the notified member doesn't check out within **3 days**; the next reserver is notified

## Test Scenarios

### Books and Copies

1. **New library has no books**
2. **Adding a book with one copy makes the book available**
3. **Adding another copy of an existing book increments the copy count**
4. **Removing a copy decrements the copy count**
5. **Removing the last copy removes the book from the catalog**

### Members

6. **New library has no members**
7. **Registering a member adds them to the library**

### Checkouts

8. **Checking out an available copy marks the copy as checked out**
9. **Checking out an available copy creates a loan with a due date fourteen days from today**
10. **Checking out when no copy is available is rejected**

### Returns

11. **Returning a checked-out copy marks the copy as available**
12. **Returning a copy closes the loan**
13. **Returning on time incurs no fine**
14. **Returning one day late incurs a ten pence fine**
15. **Returning ten days late incurs a one pound fine**

### Reservations

16. **Reserving an unavailable book adds the member to the queue**
17. **Returning a copy with a non-empty queue marks the copy as reserved**
18. **Returning a copy with a non-empty queue notifies the head of the queue**
19. **Reservations older than three days expire and the next reserver is notified**
20. **Checking out a reserved copy satisfies the reservation and clears it**
