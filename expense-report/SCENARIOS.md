# Expense Report — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **ExpenseReport** | The aggregate root — owns a list of expense items, a status, and an employee name |
| **ExpenseItem** | A categorized line item with a description, amount, and category |
| **Category** | One of: Meals, Travel, Accommodation, Equipment, Other |
| **Money** | A positive monetary amount; the domain never treats money as a bare number |
| **Status** | Draft, Pending, Approved, Rejected — the report's lifecycle state |
| **PerItemLimit** | The maximum allowed amount for a single expense in a given category |
| **OverLimit** | An expense item whose amount exceeds its category's per-item limit |
| **ReportLimit** | The maximum total amount a report may contain ($5,000) |
| **ApprovalThreshold** | The total amount ($2,500) above which a report requires manager approval regardless of individual items |

## Domain Rules

- A new report starts in **Draft** status
- Each expense item has a **description**, **amount** (strictly positive), and **category**
- Amount must be strictly positive; zero and negative amounts are rejected
- Each category has a **per-item limit**: Meals $50, Travel $500, Accommodation $200, Equipment $1,000, Other $100
- An item exceeding its category limit is flagged as **over-limit**
- A report's total must not exceed **$5,000** — submitting a report over this limit is rejected with `"Report total exceeds the $5,000 maximum"`
- Submitting an **empty** report (no expenses) is rejected with `"Cannot submit an empty report"`
- Submitting moves a Draft report to **Pending**
- A report with **any** over-limit items requires approval — summary says `"Yes (over-limit items)"`
- A report over **$2,500** total requires approval regardless — summary says `"Yes (total exceeds $2,500)"`
- A report under $2,500 with no over-limit items does not require approval — summary says `"No"`
- Only **Pending** reports can be approved or rejected
- Approving a non-pending report is rejected with `"Only pending reports can be approved"`
- Rejecting requires a **reason**; rejecting a non-pending report is rejected with `"Only pending reports can be rejected"`
- **Approved** and **Rejected** reports cannot have expenses added — rejected with `"Cannot modify a finalized report"`
- Draft reports can have expenses **added** or **removed**
- Removing an expense that does not exist is rejected with `"Expense not found"`
- A rejected report can be **reopened**, moving it back to Draft — only rejected reports can be reopened, otherwise rejected with `"Only rejected reports can be reopened"`

## Test Scenarios

### Expense Items

1. **An expense item within its category limit is not flagged as over-limit**
2. **An expense item exceeding its category limit is flagged as over-limit**
3. **Each category has the correct per-item limit** (Meals $50, Travel $500, Accommodation $200, Equipment $1,000, Other $100)

### Creating and Adding Expenses

4. **A new report starts in Draft status**
5. **Adding an expense to a draft report increases the total**
6. **Adding an expense with zero amount is rejected**
7. **Adding an expense with negative amount is rejected**

### Submitting

8. **Submitting a draft report moves it to Pending**
9. **Submitting an empty report is rejected**
10. **Submitting a report over $5,000 total is rejected**

### Approval Rules

11. **A report under $2,500 with no over-limit items does not require approval**
12. **A report with an over-limit item requires approval**
13. **A report over $2,500 total requires approval regardless of individual items**

### Approve and Reject

14. **Approving a pending report moves it to Approved**
15. **Approving a non-pending report is rejected**
16. **Rejecting a pending report with a reason moves it to Rejected**
17. **Rejecting a non-pending report is rejected**

### Finalized Report Constraints

18. **Adding an expense to an approved report is rejected**
19. **Adding an expense to a rejected report is rejected**

### Reopen

20. **Reopening a rejected report moves it back to Draft**
21. **Reopening a non-rejected report is rejected**

### Summary Output

22. **Summary lists each expense with category, description, amount, and over-limit flag**
