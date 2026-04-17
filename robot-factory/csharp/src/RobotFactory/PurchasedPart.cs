namespace RobotFactory;

public record PurchasedPart(PartType Type, PartOption Option, Money Price, string SupplierName);
