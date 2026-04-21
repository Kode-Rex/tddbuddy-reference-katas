namespace MultiThreadedSanta;

public class Present
{
    public int Id { get; }
    public PresentState State { get; private set; } = PresentState.Unmade;

    public Present(int id)
    {
        Id = id;
    }

    public void Make()
    {
        if (State != PresentState.Unmade)
            throw new InvalidOperationException($"Cannot make a present in state {State}.");
        State = PresentState.Made;
    }

    public void Wrap()
    {
        if (State != PresentState.Made)
            throw new InvalidOperationException($"Cannot wrap a present in state {State}.");
        State = PresentState.Wrapped;
    }

    public void Load()
    {
        if (State != PresentState.Wrapped)
            throw new InvalidOperationException($"Cannot load a present in state {State}.");
        State = PresentState.Loaded;
    }

    public void Deliver()
    {
        if (State != PresentState.Loaded)
            throw new InvalidOperationException($"Cannot deliver a present in state {State}.");
        State = PresentState.Delivered;
    }
}
