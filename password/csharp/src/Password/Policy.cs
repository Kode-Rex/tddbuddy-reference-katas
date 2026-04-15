namespace Password;

public sealed record Policy(
    int MinLength,
    bool RequiresDigit,
    bool RequiresSymbol,
    bool RequiresUpper,
    bool RequiresLower)
{
    public ValidationResult Validate(string password)
    {
        var failures = new List<string>();

        if (password.Length < MinLength) failures.Add(RuleNames.MinimumLength);
        if (RequiresDigit && !password.Any(IsDigit)) failures.Add(RuleNames.RequiresDigit);
        if (RequiresSymbol && !password.Any(IsSymbol)) failures.Add(RuleNames.RequiresSymbol);
        if (RequiresUpper && !password.Any(IsUpper)) failures.Add(RuleNames.RequiresUppercase);
        if (RequiresLower && !password.Any(IsLower)) failures.Add(RuleNames.RequiresLowercase);

        return new ValidationResult(failures.Count == 0, failures);
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';
    private static bool IsUpper(char c) => c is >= 'A' and <= 'Z';
    private static bool IsLower(char c) => c is >= 'a' and <= 'z';
    private static bool IsSymbol(char c) => !IsDigit(c) && !IsUpper(c) && !IsLower(c);
}
