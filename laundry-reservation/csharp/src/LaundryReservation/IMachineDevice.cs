namespace LaundryReservation;

public interface IMachineDevice
{
    bool Lock(string reservationId, DateTime reservationDateTime, int pin);
    void Unlock(string reservationId);
}
