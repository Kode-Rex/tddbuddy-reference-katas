namespace ZombieSurvivor;

public class Game
{
    private readonly IClock _clock;
    private readonly List<Survivor> _survivors = new();
    private readonly List<HistoryEntry> _history = new();
    private Level _gameLevel = Level.Blue;
    private bool _ended;

    public Game(IClock clock)
    {
        _clock = clock;
        RecordEvent("Game started.");
    }

    public IReadOnlyList<Survivor> Survivors => _survivors;
    public IReadOnlyList<HistoryEntry> History => _history;
    public Level GameLevel => _gameLevel;
    public bool HasEnded => _ended;

    public void AddSurvivor(Survivor survivor)
    {
        if (_survivors.Any(s => s.Name == survivor.Name))
            throw new DuplicateSurvivorNameException(
                $"A survivor named '{survivor.Name}' already exists.");

        _survivors.Add(survivor);
        RecordEvent($"Survivor added: {survivor.Name}.");
    }

    public void WoundSurvivor(Survivor survivor)
    {
        var wasAlive = survivor.IsAlive;
        var changed = survivor.ReceiveWound();
        if (!changed) return;

        RecordEvent($"Wound received: {survivor.Name}.");

        if (wasAlive && !survivor.IsAlive)
        {
            RecordEvent($"Survivor died: {survivor.Name}.");
            CheckGameEnd();
        }
    }

    public void EquipSurvivor(Survivor survivor, string itemName)
    {
        survivor.Equip(itemName);
        RecordEvent($"Equipment acquired: {survivor.Name} picked up {itemName}.");
    }

    public void KillZombie(Survivor survivor)
    {
        var previousXp = survivor.Experience;
        survivor.KillZombie();
        var levelUp = survivor.CheckLevelUp(previousXp);
        if (levelUp is not null)
        {
            RecordEvent($"Level up: {survivor.Name} reached {levelUp}.");
            if (levelUp == Level.Yellow)
            {
                survivor.UnlockSkill(Skill.PlusOneAction);
            }
            UpdateGameLevel();
        }
    }

    private void UpdateGameLevel()
    {
        var highestLevel = _survivors
            .Where(s => s.IsAlive)
            .Select(s => s.Level)
            .DefaultIfEmpty(Level.Blue)
            .Max();

        if (highestLevel != _gameLevel)
        {
            _gameLevel = highestLevel;
            RecordEvent($"Game level changed to {_gameLevel}.");
        }
    }

    private void CheckGameEnd()
    {
        if (_survivors.Count > 0 && _survivors.All(s => !s.IsAlive))
        {
            _ended = true;
            RecordEvent("Game ended.");
        }
    }

    private void RecordEvent(string description)
    {
        _history.Add(new HistoryEntry(_clock.Now(), description));
    }
}
