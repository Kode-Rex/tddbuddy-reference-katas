using System.Text;

namespace MarsRover.Tests;

public class CommandBuilder
{
    private readonly StringBuilder _buffer = new();

    public CommandBuilder Forward()  { _buffer.Append('F'); return this; }
    public CommandBuilder Backward() { _buffer.Append('B'); return this; }
    public CommandBuilder Left()     { _buffer.Append('L'); return this; }
    public CommandBuilder Right()    { _buffer.Append('R'); return this; }

    public string Build() => _buffer.ToString();
}
