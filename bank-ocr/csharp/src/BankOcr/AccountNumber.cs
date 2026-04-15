using System.Text;

namespace BankOcr;

public class AccountNumber
{
    public IReadOnlyList<Digit> Digits { get; }

    public AccountNumber(IEnumerable<Digit> digits)
    {
        var list = digits.ToList();
        if (list.Count != OcrDimensions.AccountLength)
            throw new ArgumentException(
                $"Account number must have exactly {OcrDimensions.AccountLength} digits.",
                nameof(digits));
        Digits = list;
    }

    public bool IsLegible => Digits.All(d => d.IsKnown);

    public bool IsChecksumValid
    {
        get
        {
            if (!IsLegible) return false;
            var sum = 0;
            for (var i = 0; i < OcrDimensions.AccountLength; i++)
            {
                var position = OcrDimensions.AccountLength - i; // d1 -> 9, d9 -> 1
                sum += Digits[i].Value!.Value * position;
            }
            return sum % 11 == 0;
        }
    }

    public string Number
    {
        get
        {
            var sb = new StringBuilder(OcrDimensions.AccountLength);
            foreach (var d in Digits) sb.Append(d.ToString());
            return sb.ToString();
        }
    }

    public string Status
    {
        get
        {
            if (!IsLegible) return $"{Number} ILL";
            if (!IsChecksumValid) return $"{Number} ERR";
            return Number;
        }
    }
}
