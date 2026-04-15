namespace ShoppingCart;

public class LineItemNotFoundException : Exception
{
    public LineItemNotFoundException(string message) : base(message) { }
}
