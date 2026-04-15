using System.Globalization;
using System.Text;

namespace BankAccount;

public class Account
{
    private readonly IClock _clock;
    private readonly List<Transaction> _transactions = new();

    public Account(IClock clock)
    {
        _clock = clock;
    }

    public Money Balance { get; private set; } = Money.Zero;

    public IReadOnlyList<Transaction> Transactions => _transactions;

    public bool Deposit(Money amount)
    {
        if (!amount.IsPositive) return false;
        Balance += amount;
        _transactions.Add(new Transaction(_clock.Today(), amount, Balance));
        return true;
    }

    public bool Withdraw(Money amount)
    {
        if (!amount.IsPositive) return false;
        if (amount > Balance) return false;
        Balance -= amount;
        _transactions.Add(new Transaction(_clock.Today(), new Money(-amount.Amount), Balance));
        return true;
    }

    public string PrintStatement()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date       | Amount  | Balance");
        foreach (var t in _transactions)
        {
            var date = t.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var amount = t.Amount.Amount.ToString("F2", CultureInfo.InvariantCulture).PadLeft(7);
            var balance = t.BalanceAfter.Amount.ToString("F2", CultureInfo.InvariantCulture).PadLeft(7);
            sb.AppendLine($"{date} | {amount} | {balance}");
        }
        return sb.ToString().TrimEnd('\r', '\n');
    }
}
