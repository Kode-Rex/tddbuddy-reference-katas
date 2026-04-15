namespace Greeting;

public static class Greeter
{
    public static string Greet(string? name)
    {
        return GreetMany(new[] { name ?? "my friend" });
    }

    public static string Greet(string?[] names)
    {
        var resolved = new string[names.Length];
        for (var i = 0; i < names.Length; i++)
        {
            resolved[i] = names[i] ?? "my friend";
        }
        return GreetMany(resolved);
    }

    private static string GreetMany(string[] names)
    {
        var normals = new List<string>();
        var shouts = new List<string>();
        foreach (var name in names)
        {
            if (IsShout(name)) shouts.Add(name);
            else normals.Add(name);
        }

        if (shouts.Count == 0) return NormalGreeting(normals);
        if (normals.Count == 0) return ShoutGreeting(shouts);
        return $"{NormalGreeting(normals)}. AND {ShoutGreeting(shouts)}";
    }

    private static string NormalGreeting(List<string> names)
    {
        if (names.Count == 1) return $"Hello, {names[0]}.";
        if (names.Count == 2) return $"Hello, {names[0]} and {names[1]}";
        var head = string.Join(", ", names.GetRange(0, names.Count - 1));
        return $"Hello, {head}, and {names[^1]}";
    }

    private static string ShoutGreeting(List<string> names)
    {
        return $"HELLO {string.Join(" ", names)}!";
    }

    private static bool IsShout(string name)
    {
        var hasLetter = false;
        foreach (var c in name)
        {
            if (char.IsLetter(c))
            {
                hasLetter = true;
                if (char.IsLower(c)) return false;
            }
        }
        return hasLetter;
    }
}
