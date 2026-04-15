namespace AnagramDetector;

public static class AnagramDetector
{
    public static bool AreAnagrams(string a, string b)
    {
        if (a == b) return false;
        var keyA = Key(a);
        var keyB = Key(b);
        if (keyA.Length == 0 || keyB.Length == 0) return false;
        return keyA == keyB;
    }

    public static IReadOnlyList<string> FindAnagrams(string subject, IEnumerable<string> candidates)
    {
        var result = new List<string>();
        foreach (var candidate in candidates)
        {
            if (AreAnagrams(subject, candidate)) result.Add(candidate);
        }
        return result;
    }

    public static IReadOnlyList<IReadOnlyList<string>> GroupAnagrams(IEnumerable<string> words)
    {
        var keyToGroup = new Dictionary<string, List<string>>();
        var order = new List<string>();
        foreach (var word in words)
        {
            var key = Key(word);
            if (!keyToGroup.TryGetValue(key, out var group))
            {
                group = new List<string>();
                keyToGroup[key] = group;
                order.Add(key);
            }
            group.Add(word);
        }
        return order.Select(k => (IReadOnlyList<string>)keyToGroup[k]).ToList();
    }

    private static string Key(string s)
    {
        var letters = s.Where(char.IsLetter).Select(char.ToLowerInvariant).ToArray();
        Array.Sort(letters);
        return new string(letters);
    }
}
