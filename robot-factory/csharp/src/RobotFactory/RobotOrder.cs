namespace RobotFactory;

public class RobotOrder
{
    private static readonly PartType[] AllPartTypes =
        Enum.GetValues<PartType>();

    private readonly Dictionary<PartType, PartOption> _parts = new();

    public IReadOnlyDictionary<PartType, PartOption> Parts => _parts;

    public void Configure(PartType type, PartOption option)
    {
        _parts[type] = option;
    }

    public void Validate()
    {
        var missing = AllPartTypes.Where(t => !_parts.ContainsKey(t)).ToList();
        if (missing.Count > 0)
        {
            throw new OrderIncompleteException(
                $"Order is missing part types: {string.Join(", ", missing)}");
        }
    }
}
