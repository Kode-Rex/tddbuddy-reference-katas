namespace Password;

// Identical byte-for-byte across C#, TypeScript, and Python.
// The failure message strings are the spec (see ../SCENARIOS.md).
public static class RuleNames
{
    public const string MinimumLength = "minimum length";
    public const string RequiresDigit = "requires digit";
    public const string RequiresSymbol = "requires symbol";
    public const string RequiresUppercase = "requires uppercase";
    public const string RequiresLowercase = "requires lowercase";
}

public static class PolicyDefaults
{
    public const int DefaultMinLength = 8;
}
