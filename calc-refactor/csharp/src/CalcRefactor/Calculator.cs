namespace CalcRefactor;

// Pure domain object characterizing the legacy WPF calculator's behavior.
// The display string is what the original TextBox would show; the key set
// is the legacy button set. See ../../SCENARIOS.md for the contract.
public sealed class Calculator
{
    private string _display = DisplayStrings.Zero;
    private int _left;
    private char? _pendingOperator;
    private int _rememberedRight;
    private bool _hasRememberedRight;
    private bool _enteringOperand;
    private bool _justEvaluated;
    private bool _inError;

    public string Display => _display;

    public void Press(char key)
    {
        if (_inError)
        {
            if (key == Keys.Clear) Reset();
            return;
        }

        if (IsDigit(key)) { PressDigit(key); return; }
        if (IsOperator(key)) { PressOperator(key); return; }
        if (key == Keys.Equals) { PressEquals(); return; }
        if (key == Keys.Clear) { Reset(); return; }

        throw new ArgumentException(ErrorMessages.UnknownKey(key), nameof(key));
    }

    private void PressDigit(char key)
    {
        if (!_enteringOperand || _justEvaluated)
        {
            if (_justEvaluated)
            {
                // A digit after `=` starts a fresh calculation — clear the
                // accumulator and the pending operator so the new operand
                // is not folded into the remembered state.
                _left = 0;
                _pendingOperator = null;
                _hasRememberedRight = false;
            }
            _display = key.ToString();
            _enteringOperand = true;
            _justEvaluated = false;
            return;
        }

        _display = _display == DisplayStrings.Zero ? key.ToString() : _display + key;
    }

    private void PressOperator(char key)
    {
        if (_enteringOperand || _justEvaluated)
        {
            if (_pendingOperator is not null && _enteringOperand)
            {
                _left = Apply(_left, _pendingOperator.Value, int.Parse(_display));
                _display = _left.ToString();
                if (_inError) return;
            }
            else
            {
                _left = int.Parse(_display);
            }
            _pendingOperator = key;
            _enteringOperand = false;
            _justEvaluated = false;
            _hasRememberedRight = false;
            return;
        }

        // Operator pressed with no fresh operand since the last operator — the
        // legacy `1++2` path. Characterized as an error, not silent arithmetic.
        EnterError();
    }

    private void PressEquals()
    {
        if (_pendingOperator is null) return;

        int right = _hasRememberedRight && !_enteringOperand
            ? _rememberedRight
            : int.Parse(_display);

        int result = Apply(_left, _pendingOperator.Value, right);
        if (_inError) return;

        _rememberedRight = right;
        _hasRememberedRight = true;
        _left = result;
        _display = result.ToString();
        _enteringOperand = false;
        _justEvaluated = true;
    }

    private int Apply(int left, char op, int right)
    {
        switch (op)
        {
            case '+': return left + right;
            case '-': return left - right;
            case '*': return left * right;
            case '/':
                if (right == 0) { EnterError(); return 0; }
                return left / right; // C# integer division truncates toward zero
            default: throw new InvalidOperationException($"unreachable operator: {op}");
        }
    }

    private void EnterError()
    {
        _inError = true;
        _display = DisplayStrings.Error;
    }

    private void Reset()
    {
        _display = DisplayStrings.Zero;
        _left = 0;
        _pendingOperator = null;
        _rememberedRight = 0;
        _hasRememberedRight = false;
        _enteringOperand = false;
        _justEvaluated = false;
        _inError = false;
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';
    private static bool IsOperator(char c) => c is '+' or '-' or '*' or '/';
}
