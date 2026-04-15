namespace StringTransformer.Tests;

public class PipelineBuilder
{
    private readonly List<ITransformation> _steps = new();

    public PipelineBuilder Capitalise() { _steps.Add(new Capitalise()); return this; }
    public PipelineBuilder Reverse() { _steps.Add(new Reverse()); return this; }
    public PipelineBuilder RemoveWhitespace() { _steps.Add(new RemoveWhitespace()); return this; }
    public PipelineBuilder SnakeCase() { _steps.Add(new SnakeCase()); return this; }
    public PipelineBuilder CamelCase() { _steps.Add(new CamelCase()); return this; }
    public PipelineBuilder Truncate(int n) { _steps.Add(new Truncate(n)); return this; }
    public PipelineBuilder Repeat(int n) { _steps.Add(new Repeat(n)); return this; }
    public PipelineBuilder Replace(string target, string replacement)
    {
        _steps.Add(new Replace(target, replacement));
        return this;
    }

    public Pipeline Build() => new(_steps);
}
