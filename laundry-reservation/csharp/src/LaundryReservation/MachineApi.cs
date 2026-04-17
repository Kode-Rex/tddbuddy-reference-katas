namespace LaundryReservation;

public class MachineApi
{
    private readonly Dictionary<int, IMachineDevice> _devices = new();
    private readonly Dictionary<string, int> _reservationToMachine = new();

    public void RegisterDevice(int machineNumber, IMachineDevice device)
    {
        _devices[machineNumber] = device;
    }

    public bool Lock(string reservationId, int machineNumber, DateTime reservationDateTime, int pin)
    {
        if (!_devices.TryGetValue(machineNumber, out var device))
            return false;

        if (_reservationToMachine.ContainsKey(reservationId))
        {
            device.Lock(reservationId, reservationDateTime, pin);
            return true;
        }

        var locked = device.Lock(reservationId, reservationDateTime, pin);
        if (locked)
        {
            _reservationToMachine[reservationId] = machineNumber;
        }
        return locked;
    }

    public void Unlock(int machineNumber, string reservationId)
    {
        if (_devices.TryGetValue(machineNumber, out var device))
        {
            device.Unlock(reservationId);
            _reservationToMachine.Remove(reservationId);
        }
    }
}
