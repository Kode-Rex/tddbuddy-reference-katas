using System.Text;

namespace CharacterCopy.Tests;

public class RecordingDestination : IDestination
{
    private readonly StringBuilder _buffer = new();

    public void WriteChar(char c) => _buffer.Append(c);

    public string Written => _buffer.ToString();
}
