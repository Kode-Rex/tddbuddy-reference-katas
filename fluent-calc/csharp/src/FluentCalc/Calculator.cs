namespace FluentCalc;

public class Calculator
{
    private int _value;
    private bool _seeded;
    private readonly Stack<Operation> _undo = new();
    private readonly Stack<Operation> _redo = new();

    public Calculator Seed(int n)
    {
        if (_seeded) return this;
        _value = n;
        _seeded = true;
        return this;
    }

    public Calculator Plus(int n) => Apply(new Operation(Op.Plus, n));

    public Calculator Minus(int n) => Apply(new Operation(Op.Minus, n));

    public Calculator Undo()
    {
        if (_undo.Count == 0) return this;
        var operation = _undo.Pop();
        _value = Reverse(_value, operation);
        _redo.Push(operation);
        return this;
    }

    public Calculator Redo()
    {
        if (_redo.Count == 0) return this;
        var operation = _redo.Pop();
        _value = Forward(_value, operation);
        _undo.Push(operation);
        return this;
    }

    public Calculator Save()
    {
        _undo.Clear();
        _redo.Clear();
        return this;
    }

    public int Result() => _value;

    private Calculator Apply(Operation operation)
    {
        if (!_seeded) return this;
        _value = Forward(_value, operation);
        _undo.Push(operation);
        _redo.Clear();
        return this;
    }

    private static int Forward(int value, Operation operation) => operation.Kind switch
    {
        Op.Plus => value + operation.Operand,
        Op.Minus => value - operation.Operand,
        _ => value,
    };

    private static int Reverse(int value, Operation operation) => operation.Kind switch
    {
        Op.Plus => value - operation.Operand,
        Op.Minus => value + operation.Operand,
        _ => value,
    };

    private enum Op { Plus, Minus }

    private readonly record struct Operation(Op Kind, int Operand);
}
