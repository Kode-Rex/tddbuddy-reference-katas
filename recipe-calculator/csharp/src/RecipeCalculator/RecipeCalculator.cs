namespace RecipeCalculator;

public static class RecipeCalculator
{
    public static IReadOnlyDictionary<string, double> Scale(
        IReadOnlyDictionary<string, double> recipe,
        double factor)
    {
        if (factor < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(factor),
                "Scale factor must be non-negative.");
        }

        var scaled = new Dictionary<string, double>(recipe.Count);
        foreach (var (ingredient, quantity) in recipe)
        {
            scaled[ingredient] = quantity * factor;
        }
        return scaled;
    }
}
