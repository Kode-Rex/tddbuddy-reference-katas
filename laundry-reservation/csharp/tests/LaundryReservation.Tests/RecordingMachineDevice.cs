namespace LaundryReservation.Tests;

public record LockCall(string ReservationId, DateTime ReservationDateTime, int Pin);

public class RecordingMachineDevice : IMachineDevice
{
    private readonly List<LockCall> _lockCalls = new();
    private readonly List<string> _unlockCalls = new();
    private readonly HashSet<string> _lockedReservations = new();
    public bool ShouldAcceptLock { get; set; } = true;

    public IReadOnlyList<LockCall> LockCalls => _lockCalls;
    public IReadOnlyList<string> UnlockCalls => _unlockCalls;

    public bool Lock(string reservationId, DateTime reservationDateTime, int pin)
    {
        _lockCalls.Add(new LockCall(reservationId, reservationDateTime, pin));
        if (ShouldAcceptLock)
        {
            _lockedReservations.Add(reservationId);
            return true;
        }
        return false;
    }

    public void Unlock(string reservationId)
    {
        _unlockCalls.Add(reservationId);
        _lockedReservations.Remove(reservationId);
    }
}
