namespace CalcRefactor.Tests;

// Test-folder F2 builder. Each test opens with one readable line that names
// the key sequence it cares about — the refactor from four explicit Press()
// calls per scenario to `aCalculator().PressKeys("1+2=")` is the F2 payoff.
public class CalculatorBuilder
{
    private string _keys = string.Empty;

    public static CalculatorBuilder ACalculator() => new();

    public CalculatorBuilder PressKeys(string keys) { _keys += keys; return this; }

    public Calculator Build()
    {
        var calculator = new Calculator();
        foreach (var key in _keys) calculator.Press(key);
        return calculator;
    }
}
