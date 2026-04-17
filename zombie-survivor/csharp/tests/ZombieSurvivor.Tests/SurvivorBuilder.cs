namespace ZombieSurvivor.Tests;

public class SurvivorBuilder
{
    private string _name = "Bob";
    private int _wounds;
    private int _zombieKills;
    private readonly List<string> _equipment = new();

    public SurvivorBuilder Named(string name) { _name = name; return this; }
    public SurvivorBuilder WithWounds(int wounds) { _wounds = wounds; return this; }
    public SurvivorBuilder WithZombieKills(int kills) { _zombieKills = kills; return this; }
    public SurvivorBuilder WithEquipment(params string[] items)
    {
        _equipment.AddRange(items);
        return this;
    }

    public Survivor Build()
    {
        var survivor = new Survivor(_name);
        for (var i = 0; i < _zombieKills; i++)
            survivor.KillZombie();
        foreach (var item in _equipment)
            survivor.Equip(item);
        for (var i = 0; i < _wounds; i++)
            survivor.ReceiveWound();
        return survivor;
    }
}
