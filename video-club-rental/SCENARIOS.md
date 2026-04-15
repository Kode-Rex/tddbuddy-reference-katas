# Video Club Rental — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations. Test names in every language map 1:1 to the scenario titles below.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **VideoClub** | The aggregate root — the club itself (`VideoClub`) |
| **User** | A member of the club (`User`) with an age, name, email, priority points, and loyalty points |
| **Title** | A video in the catalog (`Title`) with a name, total copies, and available copies |
| **Rental** | A user's rental of a title (`Rental`) with a rented-on date and a due-on date |
| **PriorityPoints** | Points users earn by returning on time; 5+ grants priority access to new releases |
| **LoyaltyPoints** | Points users earn by donating titles (10 per donation) |
| **Wishlist** | A per-user list of titles the user wants to be notified about when available |
| **Notifier** | Collaborator that sends emails/alerts; mocked in tests because *sending* is the behavior |

## Domain Rules

### Registration
- Users must be **18 or older** to register
- A **welcome email** is dispatched on successful registration
- Only **admin** users may create or delete other users

### Rental Pricing
Simultaneous rentals are priced in a descending tier:

| Nth simultaneous rental | Price |
|-------------------------|-------|
| 1st | £2.50 |
| 2nd | £2.25 |
| 3rd | £1.75 |

A rental period is **15 days**.

### Returns
- Returned **on time** (≤15 days): user gains **+2 priority points**
- Returned **late** (>15 days): user loses **2 priority points** (floored at 0), a **late alert** is dispatched, and the user is **blocked from renting** until the overdue title is returned

### Priority Access
- Users with **≥5 priority points** may reserve new releases ahead of others
- Priority points never go below 0

### Donations
- Donating a **new title** creates a library entry with one copy
- Donating an **existing title** increments its total and available copies
- The donor gains **+10 loyalty points** per donation
- Users whose **wishlist** contains the donated title are notified

### Wishlist
- Title matching is **case-insensitive**
- Notifications fire only when a wished title becomes available (initial donation or later return)

## Test Scenarios

### Registration

1. **User aged eighteen registers successfully**
2. **User aged seventeen is rejected as too young**
3. **Registration dispatches a welcome email**
4. **Admin creates another user successfully**
5. **Non-admin attempting to create a user is rejected**

### Rental Pricing

6. **First simultaneous rental costs two pounds fifty**
7. **Second simultaneous rental costs two pounds twenty-five**
8. **Third simultaneous rental costs one pound seventy-five**
9. **Renting two titles charges four pounds seventy-five total**
10. **Renting three titles charges six pounds fifty total**

### Returns

11. **On-time return awards two priority points**
12. **Late return deducts two priority points**
13. **Late return dispatches a late alert**
14. **User with an overdue rental cannot rent another title**
15. **Returning the overdue title unblocks renting**

### Priority Access

16. **User with five priority points has priority access to new releases**
17. **User with four priority points does not have priority access**
18. **Priority points cannot go below zero**

### Donations

19. **Donating a new title creates a library entry with one copy**
20. **Donating an existing title increments its copy count**
21. **Donating awards ten loyalty points to the donor**

### Wishlist

22. **User can add a title to their wishlist**
23. **Wishlist matching is case-insensitive**
24. **Donating a wishlisted title notifies the wishlisting user**
