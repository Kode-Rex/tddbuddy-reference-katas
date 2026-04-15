namespace Password.Tests;

public class PolicyBuilder
{
    private int _minLength = PolicyDefaults.DefaultMinLength;
    private bool _requiresDigit;
    private bool _requiresSymbol;
    private bool _requiresUpper;
    private bool _requiresLower;

    public PolicyBuilder MinLength(int n) { _minLength = n; return this; }
    public PolicyBuilder RequiresDigit() { _requiresDigit = true; return this; }
    public PolicyBuilder RequiresSymbol() { _requiresSymbol = true; return this; }
    public PolicyBuilder RequiresUpper() { _requiresUpper = true; return this; }
    public PolicyBuilder RequiresLower() { _requiresLower = true; return this; }

    public Policy Build() =>
        new(_minLength, _requiresDigit, _requiresSymbol, _requiresUpper, _requiresLower);
}
