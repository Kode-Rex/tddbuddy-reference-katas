namespace ZombieSurvivor;

public class Survivor
{
    private const int BaseActions = 3;
    private const int MaxWounds = 2;
    private const int BaseCapacity = 5;
    private const int MaxInHand = 2;

    private readonly List<Equipment> _equipment = new();
    private readonly List<Skill> _skills = new();

    public Survivor(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public int Wounds { get; private set; }
    public int Experience { get; private set; }
    public bool IsAlive => Wounds < MaxWounds;

    public Level Level => Experience switch
    {
        >= 43 => Level.Red,
        >= 19 => Level.Orange,
        >= 7 => Level.Yellow,
        _ => Level.Blue
    };

    public int ActionsPerTurn
    {
        get
        {
            var bonus = _skills.Count(s => s == Skill.PlusOneAction);
            return BaseActions + bonus;
        }
    }

    public int EquipmentCapacity
    {
        get
        {
            var hoardBonus = _skills.Count(s => s == Skill.Hoard);
            return BaseCapacity - Wounds + hoardBonus;
        }
    }

    public IReadOnlyList<Equipment> Equipment => _equipment;
    public IReadOnlyList<Skill> Skills => _skills;

    public int InHandCount => _equipment.Count(e => e.Slot == EquipmentSlot.InHand);
    public int InReserveCount => _equipment.Count(e => e.Slot == EquipmentSlot.InReserve);

    public bool ReceiveWound()
    {
        if (!IsAlive) return false;
        Wounds++;
        return true;
    }

    public void Equip(string itemName)
    {
        if (_equipment.Count >= EquipmentCapacity)
            throw new EquipmentCapacityExceededException(
                $"Cannot carry more than {EquipmentCapacity} pieces of equipment.");

        var slot = InHandCount < MaxInHand ? EquipmentSlot.InHand : EquipmentSlot.InReserve;
        _equipment.Add(new Equipment(itemName, slot));
    }

    public void Discard(string itemName)
    {
        var item = _equipment.FirstOrDefault(e => e.Name == itemName);
        if (item is not null)
            _equipment.Remove(item);
    }

    public bool NeedsToDiscard => _equipment.Count > EquipmentCapacity;

    public void KillZombie()
    {
        Experience++;
    }

    public Level? CheckLevelUp(int previousExperience)
    {
        var previousLevel = LevelFor(previousExperience);
        if (Level != previousLevel) return Level;
        return null;
    }

    public void UnlockSkill(Skill skill)
    {
        _skills.Add(skill);
    }

    private static Level LevelFor(int xp) => xp switch
    {
        >= 43 => Level.Red,
        >= 19 => Level.Orange,
        >= 7 => Level.Yellow,
        _ => Level.Blue
    };
}
