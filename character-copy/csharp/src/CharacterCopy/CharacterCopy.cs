namespace CharacterCopy;

public static class CharacterCopy
{
    public static void Copy(ISource source, IDestination destination)
    {
        while (true)
        {
            char c = source.ReadChar();
            if (c == '\n') return;
            destination.WriteChar(c);
        }
    }
}
