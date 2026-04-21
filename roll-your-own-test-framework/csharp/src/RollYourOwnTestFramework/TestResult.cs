namespace RollYourOwnTestFramework;

public record TestResult(string Name, TestStatus Status, string? Message = null);
