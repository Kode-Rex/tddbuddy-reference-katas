import { describe, expect, it } from 'vitest';
import { Account } from '../src/Account.js';
import { Money } from '../src/Money.js';
import { AccountNotOpenException, AccountClosedException, InsufficientFundsException, InvalidAmountException, NonZeroBalanceException } from '../src/Exceptions.js';
import { AccountBuilder } from './AccountBuilder.js';
import { aMoneyDeposited } from './EventBuilder.js';

const T1 = new Date(Date.UTC(2026, 0, 15, 10, 0, 0));
const T2 = new Date(Date.UTC(2026, 0, 16, 10, 0, 0));
const T3 = new Date(Date.UTC(2026, 0, 17, 10, 0, 0));
const T4 = new Date(Date.UTC(2026, 0, 18, 10, 0, 0));

describe('Account', () => {
  // --- Replay — Balance ---

  it('replaying an opened account has a zero balance', () => {
    const account = new AccountBuilder().build();
    expect(account.balance.equals(Money.zero)).toBe(true);
  });

  it('replaying a single deposit yields that amount as the balance', () => {
    const account = new AccountBuilder().withDeposit(100).build();
    expect(account.balance.amount).toBe(100);
  });

  it('replaying multiple deposits yields their sum', () => {
    const account = new AccountBuilder().withDeposit(100).withDeposit(50).build();
    expect(account.balance.amount).toBe(150);
  });

  it('replaying deposits and withdrawals yields the net balance', () => {
    const account = new AccountBuilder().withDeposit(100).withWithdrawal(30).build();
    expect(account.balance.amount).toBe(70);
  });

  it('withdrawing the full balance yields zero', () => {
    const account = new AccountBuilder().withDeposit(100).withWithdrawal(100).build();
    expect(account.balance.equals(Money.zero)).toBe(true);
  });

  // --- Command Validation — Deposits ---

  it('depositing a positive amount appends a MoneyDeposited event', () => {
    const account = new AccountBuilder().build();
    const result = account.deposit(new Money(200), T1);
    expect(result.type).toBe('MoneyDeposited');
    expect(account.balance.amount).toBe(200);
  });

  it('depositing zero is rejected', () => {
    const account = new AccountBuilder().build();
    expect(() => account.deposit(Money.zero, T1)).toThrow(InvalidAmountException);
    expect(() => account.deposit(Money.zero, T1)).toThrow('Deposit amount must be positive.');
  });

  it('depositing a negative amount is rejected', () => {
    const account = new AccountBuilder().build();
    expect(() => account.deposit(new Money(-50), T1)).toThrow(InvalidAmountException);
    expect(() => account.deposit(new Money(-50), T1)).toThrow('Deposit amount must be positive.');
  });

  it('depositing into a closed account is rejected', () => {
    const account = new AccountBuilder().closed().build();
    expect(() => account.deposit(new Money(100), T1)).toThrow(AccountClosedException);
    expect(() => account.deposit(new Money(100), T1)).toThrow('Account is closed.');
  });

  // --- Command Validation — Withdrawals ---

  it('withdrawing a positive amount appends a MoneyWithdrawn event', () => {
    const account = new AccountBuilder().withDeposit(500).build();
    const result = account.withdraw(new Money(100), T1);
    expect(result.type).toBe('MoneyWithdrawn');
    expect(account.balance.amount).toBe(400);
  });

  it('withdrawing zero is rejected', () => {
    const account = new AccountBuilder().withDeposit(500).build();
    expect(() => account.withdraw(Money.zero, T1)).toThrow(InvalidAmountException);
    expect(() => account.withdraw(Money.zero, T1)).toThrow('Withdrawal amount must be positive.');
  });

  it('withdrawing a negative amount is rejected', () => {
    const account = new AccountBuilder().withDeposit(500).build();
    expect(() => account.withdraw(new Money(-10), T1)).toThrow(InvalidAmountException);
    expect(() => account.withdraw(new Money(-10), T1)).toThrow('Withdrawal amount must be positive.');
  });

  it('withdrawing more than the balance is rejected as insufficient funds', () => {
    const account = new AccountBuilder().withDeposit(100).build();
    expect(() => account.withdraw(new Money(150), T1)).toThrow(InsufficientFundsException);
    expect(() => account.withdraw(new Money(150), T1)).toThrow('Withdrawal amount exceeds current balance.');
  });

  it('withdrawing from a closed account is rejected', () => {
    const account = new AccountBuilder().closed().build();
    expect(() => account.withdraw(new Money(50), T1)).toThrow(AccountClosedException);
    expect(() => account.withdraw(new Money(50), T1)).toThrow('Account is closed.');
  });

  // --- Command Validation — Lifecycle ---

  it('operating on an account that was never opened is rejected', () => {
    expect(() => Account.rebuild([aMoneyDeposited(100)])).toThrow(AccountNotOpenException);
    expect(() => Account.rebuild([aMoneyDeposited(100)])).toThrow('Account was never opened.');
  });

  it('closing an account with a zero balance appends an AccountClosed event', () => {
    const account = new AccountBuilder().build();
    const result = account.close(T1);
    expect(result.type).toBe('AccountClosed');
    expect(account.status).toBe('Closed');
  });

  it('closing an account with a non-zero balance is rejected', () => {
    const account = new AccountBuilder().withDeposit(100).build();
    expect(() => account.close(T1)).toThrow(NonZeroBalanceException);
    expect(() => account.close(T1)).toThrow('Cannot close account with non-zero balance.');
  });

  // --- Projection — Transaction History ---

  it('transaction history of a new account is empty', () => {
    const account = new AccountBuilder().build();
    expect(account.transactionHistory()).toEqual([]);
  });

  it('transaction history lists deposits and withdrawals with running balances', () => {
    const account = new AccountBuilder()
      .withDeposit(100, T1)
      .withDeposit(50, T2)
      .withWithdrawal(30, T3)
      .build();

    const history = account.transactionHistory();
    expect(history).toHaveLength(3);
    expect(history[0]!.amount.amount).toBe(100);
    expect(history[0]!.balanceAfter.amount).toBe(100);
    expect(history[1]!.amount.amount).toBe(50);
    expect(history[1]!.balanceAfter.amount).toBe(150);
    expect(history[2]!.amount.amount).toBe(-30);
    expect(history[2]!.balanceAfter.amount).toBe(120);
  });

  it('withdrawals appear as negative amounts in the transaction history', () => {
    const account = new AccountBuilder()
      .withDeposit(200, T1)
      .withWithdrawal(75, T2)
      .build();

    const history = account.transactionHistory();
    expect(history[1]!.amount.amount).toBe(-75);
  });

  // --- Projection — Account Summary ---

  it('account summary shows owner name, balance, transaction count, and open status', () => {
    const account = new AccountBuilder()
      .withOwnerName('Alice')
      .withDeposit(100)
      .withDeposit(50)
      .build();

    const summary = account.summary();
    expect(summary.ownerName).toBe('Alice');
    expect(summary.balance.amount).toBe(150);
    expect(summary.transactionCount).toBe(2);
    expect(summary.status).toBe('Open');
  });

  it('account summary reflects closed status after closing', () => {
    const account = new AccountBuilder()
      .withOwnerName('Bob')
      .closed()
      .build();

    const summary = account.summary();
    expect(summary.status).toBe('Closed');
  });

  // --- Temporal Queries ---

  it('balance at a point in time replays only events up to that timestamp', () => {
    const account = new AccountBuilder()
      .withDeposit(100, T1)
      .withDeposit(50, T2)
      .withDeposit(25, T3)
      .build();

    expect(account.balanceAt(T2).amount).toBe(150);
  });

  it('transactions in a date range returns only matching entries', () => {
    const account = new AccountBuilder()
      .withDeposit(100, T1)
      .withDeposit(50, T2)
      .withWithdrawal(30, T3)
      .withDeposit(25, T4)
      .build();

    const result = account.transactionsInRange(T2, T3);
    expect(result).toHaveLength(2);
    expect(result[0]!.amount.amount).toBe(50);
    expect(result[1]!.amount.amount).toBe(-30);
  });
});
