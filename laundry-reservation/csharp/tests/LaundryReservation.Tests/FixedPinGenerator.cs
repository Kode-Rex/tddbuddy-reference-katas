namespace LaundryReservation.Tests;

public class FixedPinGenerator : IPinGenerator
{
    private readonly Queue<int> _pins;

    public FixedPinGenerator(params int[] pins)
    {
        _pins = new Queue<int>(pins);
    }

    public int Generate() => _pins.Dequeue();
}
