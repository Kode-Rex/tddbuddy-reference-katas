# Bank Account — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Account** | The aggregate root (`Account`), owns a balance and a transaction log |
| **Transaction** | A dated credit or debit (`Transaction`) recorded against an account |
| **Money** | A positive monetary amount; the domain never treats money as a bare number |
| **Statement** | A formatted projection of the transaction log in chronological order with running balance |
| **Clock** | Collaborator that returns "today" — injected so tests control time without sleeping |

## Domain Rules

- A new account opens with a balance of **0**
- **Deposit** requires a **strictly positive** amount; zero and negative are rejected
- **Withdraw** requires a **strictly positive** amount; zero and negative are rejected
- **Withdraw** cannot overdraw the account; requests exceeding the balance are rejected
- Every accepted deposit or withdrawal is **recorded** with the date from the clock
- Rejected operations **leave the account unchanged** — no balance change, no log entry
- The **statement** prints one line per transaction, chronological, with running balance:
  ```
  Date       | Amount  | Balance
  2026-01-15 |  500.00 |  500.00
  2026-01-20 | -100.00 |  400.00
  ```

## Test Scenarios

### Opening

1. **New account opens with a zero balance**
2. **New account has no transactions**

### Deposits

3. **Depositing a positive amount increases the balance**
4. **Depositing records a transaction with the clock date**
5. **Depositing zero is rejected**
6. **Depositing a negative amount is rejected**
7. **Rejected deposit leaves the balance unchanged**
8. **Rejected deposit leaves no transaction on the log**

### Withdrawals

9. **Withdrawing a positive amount decreases the balance**
10. **Withdrawing records a transaction with the clock date**
11. **Withdrawing zero is rejected**
12. **Withdrawing a negative amount is rejected**
13. **Withdrawing more than the balance is rejected as insufficient funds**
14. **Rejected withdrawal leaves the balance unchanged**
15. **Rejected withdrawal leaves no transaction on the log**

### Statements

16. **Statement of a new account prints only the header**
17. **Statement lists transactions in chronological order**
18. **Statement shows running balance after each transaction**
19. **Statement formats amounts with two decimal places**
20. **Withdrawals appear as negative amounts in the statement**
