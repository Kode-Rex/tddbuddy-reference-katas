using System.Globalization;
using System.Text;

namespace ConwaysSequence;

public static class ConwaysSequence
{
    public static string NextTerm(string term)
    {
        var output = new StringBuilder();
        var i = 0;
        while (i < term.Length)
        {
            var digit = term[i];
            var runLength = 1;
            while (i + runLength < term.Length && term[i + runLength] == digit)
            {
                runLength++;
            }
            output.Append(runLength.ToString(CultureInfo.InvariantCulture));
            output.Append(digit);
            i += runLength;
        }
        return output.ToString();
    }

    public static string LookAndSay(string seed, int iterations)
    {
        if (iterations < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(iterations),
                "iterations must be non-negative");
        }
        var term = seed;
        for (var step = 0; step < iterations; step++)
        {
            term = NextTerm(term);
        }
        return term;
    }
}
