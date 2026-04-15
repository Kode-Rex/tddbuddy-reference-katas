namespace CalcRefactor;

// The legacy button set as single-character keys. Identical across the three
// language ports — the keys are the spec.
public static class Keys
{
    public const char Equals = '=';
    public const char Clear = 'C';
}

public static class DisplayStrings
{
    public const string Zero = "0";
    public const string Error = "Error";
}

public static class ErrorMessages
{
    // Byte-identical across C#, TypeScript, and Python.
    public static string UnknownKey(char key) => $"unknown key: {key}";
}
