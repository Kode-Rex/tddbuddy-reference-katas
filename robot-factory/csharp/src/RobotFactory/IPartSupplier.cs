namespace RobotFactory;

public interface IPartSupplier
{
    string Name { get; }
    PartQuote? GetQuote(PartType type, PartOption option);
    PurchasedPart Purchase(PartType type, PartOption option);
}
