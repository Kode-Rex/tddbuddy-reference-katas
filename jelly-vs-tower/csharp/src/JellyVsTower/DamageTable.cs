namespace JellyVsTower;

public static class DamageTable
{
    private static readonly Dictionary<(ColorType Tower, int Level, ColorType Jelly), DamageRange> Table = new()
    {
        // Blue Tower
        { (ColorType.Blue, 1, ColorType.Blue), new DamageRange(2, 5) },
        { (ColorType.Blue, 2, ColorType.Blue), new DamageRange(5, 9) },
        { (ColorType.Blue, 3, ColorType.Blue), new DamageRange(9, 12) },
        { (ColorType.Blue, 4, ColorType.Blue), new DamageRange(12, 15) },
        { (ColorType.Blue, 1, ColorType.Red), new DamageRange(0, 0) },
        { (ColorType.Blue, 2, ColorType.Red), new DamageRange(1, 1) },
        { (ColorType.Blue, 3, ColorType.Red), new DamageRange(2, 2) },
        { (ColorType.Blue, 4, ColorType.Red), new DamageRange(3, 3) },

        // Red Tower
        { (ColorType.Red, 1, ColorType.Blue), new DamageRange(0, 0) },
        { (ColorType.Red, 2, ColorType.Blue), new DamageRange(1, 1) },
        { (ColorType.Red, 3, ColorType.Blue), new DamageRange(2, 2) },
        { (ColorType.Red, 4, ColorType.Blue), new DamageRange(3, 3) },
        { (ColorType.Red, 1, ColorType.Red), new DamageRange(2, 5) },
        { (ColorType.Red, 2, ColorType.Red), new DamageRange(5, 9) },
        { (ColorType.Red, 3, ColorType.Red), new DamageRange(9, 12) },
        { (ColorType.Red, 4, ColorType.Red), new DamageRange(12, 15) },

        // BlueRed Tower
        { (ColorType.BlueRed, 1, ColorType.Blue), new DamageRange(2, 2) },
        { (ColorType.BlueRed, 2, ColorType.Blue), new DamageRange(2, 4) },
        { (ColorType.BlueRed, 3, ColorType.Blue), new DamageRange(4, 6) },
        { (ColorType.BlueRed, 4, ColorType.Blue), new DamageRange(6, 8) },
        { (ColorType.BlueRed, 1, ColorType.Red), new DamageRange(2, 2) },
        { (ColorType.BlueRed, 2, ColorType.Red), new DamageRange(2, 4) },
        { (ColorType.BlueRed, 3, ColorType.Red), new DamageRange(4, 6) },
        { (ColorType.BlueRed, 4, ColorType.Red), new DamageRange(6, 8) },
    };

    public static int CalculateDamage(Tower tower, Jelly jelly, IRandomSource random)
    {
        if (jelly.Color == ColorType.BlueRed)
        {
            var blueDamage = ResolveDamage(tower, ColorType.Blue, random);
            var redDamage = ResolveDamage(tower, ColorType.Red, random);
            return Math.Max(blueDamage, redDamage);
        }

        return ResolveDamage(tower, jelly.Color, random);
    }

    private static int ResolveDamage(Tower tower, ColorType jellyColor, IRandomSource random)
    {
        var range = Table[(tower.Color, tower.Level, jellyColor)];
        return range.Min == range.Max ? range.Min : random.Next(range.Min, range.Max);
    }
}
