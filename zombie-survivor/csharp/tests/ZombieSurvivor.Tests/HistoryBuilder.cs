namespace ZombieSurvivor.Tests;

public class HistoryBuilder
{
    private readonly DateTime _startTime = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private readonly List<Action<Game>> _actions = new();

    public HistoryBuilder WithSurvivor(string name)
    {
        _actions.Add(game => game.AddSurvivor(new Survivor(name)));
        return this;
    }

    public HistoryBuilder WithWound(string survivorName)
    {
        _actions.Add(game =>
        {
            var survivor = game.Survivors.First(s => s.Name == survivorName);
            game.WoundSurvivor(survivor);
        });
        return this;
    }

    public HistoryBuilder WithZombieKill(string survivorName, int count = 1)
    {
        _actions.Add(game =>
        {
            var survivor = game.Survivors.First(s => s.Name == survivorName);
            for (var i = 0; i < count; i++)
                game.KillZombie(survivor);
        });
        return this;
    }

    public (Game Game, FixedClock Clock) Build()
    {
        var clock = new FixedClock(_startTime);
        var game = new Game(clock);
        foreach (var action in _actions)
            action(game);
        return (game, clock);
    }
}
