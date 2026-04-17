namespace RobotFactory.Tests;

public class RobotOrderBuilder
{
    private PartOption _head = PartOption.StandardVision;
    private PartOption _body = PartOption.Square;
    private PartOption _arms = PartOption.Hands;
    private PartOption _movement = PartOption.Wheels;
    private PartOption _power = PartOption.Solar;
    private readonly HashSet<PartType> _excluded = new();

    public RobotOrderBuilder WithHead(PartOption option) { _head = option; return this; }
    public RobotOrderBuilder WithBody(PartOption option) { _body = option; return this; }
    public RobotOrderBuilder WithArms(PartOption option) { _arms = option; return this; }
    public RobotOrderBuilder WithMovement(PartOption option) { _movement = option; return this; }
    public RobotOrderBuilder WithPower(PartOption option) { _power = option; return this; }
    public RobotOrderBuilder Without(PartType type) { _excluded.Add(type); return this; }

    public RobotOrder Build()
    {
        var order = new RobotOrder();
        if (!_excluded.Contains(PartType.Head)) order.Configure(PartType.Head, _head);
        if (!_excluded.Contains(PartType.Body)) order.Configure(PartType.Body, _body);
        if (!_excluded.Contains(PartType.Arms)) order.Configure(PartType.Arms, _arms);
        if (!_excluded.Contains(PartType.Movement)) order.Configure(PartType.Movement, _movement);
        if (!_excluded.Contains(PartType.Power)) order.Configure(PartType.Power, _power);
        return order;
    }
}
