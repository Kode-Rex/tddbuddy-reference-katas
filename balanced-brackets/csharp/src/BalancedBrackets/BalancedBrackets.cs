namespace BalancedBrackets;

public static class BalancedBrackets
{
    public static bool IsBalanced(string input)
    {
        var depth = 0;
        foreach (var c in input)
        {
            if (c == '[')
            {
                depth++;
            }
            else if (c == ']')
            {
                depth--;
                if (depth < 0) return false;
            }
        }
        return depth == 0;
    }
}
