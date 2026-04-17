namespace JellyVsTower;

public class Jelly
{
    public string Id { get; }
    public ColorType Color { get; }
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;

    public Jelly(string id, ColorType color, int health)
    {
        if (health <= 0) throw new InvalidHealthException(health);
        Id = id;
        Color = color;
        Health = health;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
    }
}
