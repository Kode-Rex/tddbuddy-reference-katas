namespace LaundryReservation.Tests;

public class FixedMachineSelector : IMachineSelector
{
    private readonly int _machineNumber;

    public FixedMachineSelector(int machineNumber)
    {
        _machineNumber = machineNumber;
    }

    public int SelectAvailable() => _machineNumber;
}
