namespace MultiThreadedSanta;

/// <summary>
/// Manages a fixed-size pool of elf workers.
/// Each elf is a long-running task assigned to a pipeline stage.
/// </summary>
public sealed class ElfPool
{
    public int ElfCount { get; }

    public ElfPool(int elfCount)
    {
        if (elfCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(elfCount), "Elf pool must have at least one elf.");
        ElfCount = elfCount;
    }
}
