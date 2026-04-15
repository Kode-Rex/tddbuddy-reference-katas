using System.Runtime.CompilerServices;
using static KataPotter.PricingRules;

[assembly: InternalsVisibleTo("KataPotter.Tests")]

namespace KataPotter;

public sealed class Basket
{
    // Counts indexed by book id 1..MaxBookId (index 0 unused).
    private readonly int[] _counts;

    internal Basket(int[] counts)
    {
        _counts = counts;
    }

    public decimal Price()
    {
        var sets = GroupIntoSets(_counts);
        AdjustFivePlusThreeIntoTwoFours(sets);
        var total = 0m;
        for (var k = 1; k <= MaxBookId; k++)
            total += sets[k] * PriceOfSet(k);
        return total;
    }

    // Greedy pass: repeatedly pull the largest possible set of distinct
    // titles out of the remaining counts. Produces a `sets` histogram
    // where `sets[k]` is the number of k-sized sets.
    private static int[] GroupIntoSets(int[] counts)
    {
        var remaining = (int[])counts.Clone();
        var sets = new int[MaxBookId + 1];
        while (true)
        {
            var distinct = 0;
            for (var id = MinBookId; id <= MaxBookId; id++)
                if (remaining[id] > 0) distinct++;
            if (distinct == 0) break;
            sets[distinct]++;
            for (var id = MinBookId; id <= MaxBookId; id++)
                if (remaining[id] > 0) remaining[id]--;
        }
        return sets;
    }

    // Adjustment pass: a 5-set plus a 3-set always costs more than two 4-sets
    // (51.60 vs 51.20 per pairing). Swap as many (5,3) pairs as possible for
    // (4,4) pairs. This is the only local swap that improves on greedy for
    // the 5-book / standard discount table.
    private static void AdjustFivePlusThreeIntoTwoFours(int[] sets)
    {
        var swaps = Math.Min(sets[5], sets[3]);
        sets[5] -= swaps;
        sets[3] -= swaps;
        sets[4] += 2 * swaps;
    }
}
