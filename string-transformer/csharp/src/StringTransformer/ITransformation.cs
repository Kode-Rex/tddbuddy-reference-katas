namespace StringTransformer;

// Strategy. Every transformation is a self-contained rule — one input,
// one output, no state. The Pipeline composes them; it does not know
// which rule any given strategy implements.
public interface ITransformation
{
    string Apply(string input);
}
