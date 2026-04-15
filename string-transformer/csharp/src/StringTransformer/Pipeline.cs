namespace StringTransformer;

public sealed class Pipeline
{
    private readonly IReadOnlyList<ITransformation> _steps;

    public Pipeline(IReadOnlyList<ITransformation> steps)
    {
        _steps = steps;
    }

    public string Run(string input)
    {
        var current = input;
        foreach (var step in _steps)
        {
            current = step.Apply(current);
        }
        return current;
    }
}
