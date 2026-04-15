namespace CharacterCopy.Tests;

public class StringSource : ISource
{
    private readonly string _payload;
    private int _index;
    public int ReadCount { get; private set; }

    public StringSource(string payload) => _payload = payload;

    public char ReadChar()
    {
        ReadCount++;
        if (_index >= _payload.Length) return '\n';
        return _payload[_index++];
    }
}
