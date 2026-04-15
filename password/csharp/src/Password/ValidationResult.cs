namespace Password;

public sealed record ValidationResult(bool Ok, IReadOnlyList<string> Failures);
