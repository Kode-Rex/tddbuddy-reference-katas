namespace RobotFactory;

public record CostBreakdown(IReadOnlyList<PartQuote> Parts, Money Total);
