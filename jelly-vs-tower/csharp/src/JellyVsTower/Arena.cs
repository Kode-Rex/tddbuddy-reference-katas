namespace JellyVsTower;

public class Arena
{
    private readonly IRandomSource _random;
    private readonly List<Tower> _towers;
    private readonly List<Jelly> _jellies;

    public Arena(IReadOnlyList<Tower> towers, IReadOnlyList<Jelly> jellies, IRandomSource random)
    {
        _random = random;
        _towers = new List<Tower>(towers);
        _jellies = new List<Jelly>(jellies);
    }

    public IReadOnlyList<Jelly> AliveJellies => _jellies.Where(j => j.IsAlive).ToList();

    public IReadOnlyList<CombatLog> ExecuteRound()
    {
        var logs = new List<CombatLog>();

        foreach (var tower in _towers)
        {
            var target = _jellies.FirstOrDefault(j => j.IsAlive);
            if (target == null) break;

            var damage = DamageTable.CalculateDamage(tower, target, _random);
            target.TakeDamage(damage);
            logs.Add(new CombatLog(tower.Id, target.Id, damage));
        }

        return logs;
    }
}
