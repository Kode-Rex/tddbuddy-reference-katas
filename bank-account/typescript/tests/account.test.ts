import { describe, expect, it } from 'vitest';
import { Money } from '../src/Money.js';
import { Transaction } from '../src/Transaction.js';
import { AccountBuilder } from './AccountBuilder.js';

const Jan15 = new Date(Date.UTC(2026, 0, 15));
const Jan20 = new Date(Date.UTC(2026, 0, 20));
const Jan25 = new Date(Date.UTC(2026, 0, 25));

describe('Account', () => {
  it('new account opens with a zero balance', () => {
    const { account } = new AccountBuilder().build();
    expect(account.balance.equals(Money.zero)).toBe(true);
  });

  it('new account has no transactions', () => {
    const { account } = new AccountBuilder().build();
    expect(account.transactions).toEqual([]);
  });

  it('depositing a positive amount increases the balance', () => {
    const { account } = new AccountBuilder().build();
    account.deposit(new Money(500));
    expect(account.balance.equals(new Money(500))).toBe(true);
  });

  it('depositing records a transaction with the clock date', () => {
    const { account } = new AccountBuilder().openedOn(Jan15).build();
    account.deposit(new Money(500));
    expect(account.transactions).toHaveLength(1);
    const t = account.transactions[0] as Transaction;
    expect(t.date).toEqual(Jan15);
    expect(t.amount.amount).toBe(500);
    expect(t.balanceAfter.amount).toBe(500);
  });

  it('depositing zero is rejected', () => {
    const { account } = new AccountBuilder().build();
    expect(account.deposit(Money.zero)).toBe(false);
  });

  it('depositing a negative amount is rejected', () => {
    const { account } = new AccountBuilder().build();
    expect(account.deposit(new Money(-50))).toBe(false);
  });

  it('rejected deposit leaves the balance unchanged', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 600).build();
    account.deposit(new Money(-50));
    expect(account.balance.amount).toBe(600);
  });

  it('rejected deposit leaves no transaction on the log', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 600).build();
    const before = account.transactions.length;
    account.deposit(Money.zero);
    expect(account.transactions.length).toBe(before);
  });

  it('withdrawing a positive amount decreases the balance', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 500).build();
    account.withdraw(new Money(100));
    expect(account.balance.amount).toBe(400);
  });

  it('withdrawing records a transaction with the clock date', () => {
    const { account, clock } = new AccountBuilder().withDepositOn(Jan15, 500).build();
    clock.advanceTo(Jan20);
    account.withdraw(new Money(100));
    const last = account.transactions[account.transactions.length - 1] as Transaction;
    expect(last.date).toEqual(Jan20);
    expect(last.amount.amount).toBe(-100);
    expect(last.balanceAfter.amount).toBe(400);
  });

  it('withdrawing zero is rejected', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 500).build();
    expect(account.withdraw(Money.zero)).toBe(false);
  });

  it('withdrawing a negative amount is rejected', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 500).build();
    expect(account.withdraw(new Money(-10))).toBe(false);
  });

  it('withdrawing more than the balance is rejected as insufficient funds', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 600).build();
    expect(account.withdraw(new Money(700))).toBe(false);
  });

  it('rejected withdrawal leaves the balance unchanged', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 600).build();
    account.withdraw(new Money(700));
    expect(account.balance.amount).toBe(600);
  });

  it('rejected withdrawal leaves no transaction on the log', () => {
    const { account } = new AccountBuilder().withDepositOn(Jan15, 600).build();
    const before = account.transactions.length;
    account.withdraw(new Money(700));
    expect(account.transactions.length).toBe(before);
  });

  it('statement of a new account prints only the header', () => {
    const { account } = new AccountBuilder().build();
    expect(account.printStatement()).toBe('Date       | Amount  | Balance');
  });

  it('statement lists transactions in chronological order', () => {
    const { account, clock } = new AccountBuilder()
      .withDepositOn(Jan15, 500)
      .withDepositOn(Jan25, 200)
      .build();
    clock.advanceTo(Jan20);
    account.withdraw(new Money(100));
    const lines = account.printStatement().split('\n');
    expect(lines[1]).toContain('2026-01-15');
    expect(lines[2]).toContain('2026-01-25');
    expect(lines[3]).toContain('2026-01-20');
  });

  it('statement shows running balance after each transaction', () => {
    const { account, clock } = new AccountBuilder().openedOn(Jan15).build();
    account.deposit(new Money(500));
    clock.advanceTo(Jan20);
    account.withdraw(new Money(100));
    clock.advanceTo(Jan25);
    account.deposit(new Money(200));
    const statement = account.printStatement();
    expect(statement).toContain('500.00 |  500.00');
    expect(statement).toContain('-100.00 |  400.00');
    expect(statement).toContain('200.00 |  600.00');
  });

  it('statement formats amounts with two decimal places', () => {
    const { account } = new AccountBuilder().openedOn(Jan15).build();
    account.deposit(new Money(42.5));
    expect(account.printStatement()).toContain('42.50');
  });

  it('withdrawals appear as negative amounts in the statement', () => {
    const { account, clock } = new AccountBuilder().withDepositOn(Jan15, 500).build();
    clock.advanceTo(Jan20);
    account.withdraw(new Money(100));
    expect(account.printStatement()).toContain('-100.00');
  });
});
