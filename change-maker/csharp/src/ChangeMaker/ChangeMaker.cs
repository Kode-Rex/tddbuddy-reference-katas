namespace ChangeMaker;

public static class ChangeMaker
{
    public static IReadOnlyList<int> MakeChange(int amount, IReadOnlyList<int> denominations)
    {
        var coins = new List<int>();
        var remaining = amount;
        foreach (var coin in denominations)
        {
            while (remaining >= coin)
            {
                coins.Add(coin);
                remaining -= coin;
            }
        }
        return coins;
    }
}
