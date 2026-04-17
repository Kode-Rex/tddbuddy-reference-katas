namespace LaundryReservation;

public interface IReservationRepository
{
    void Save(Reservation reservation);
    Reservation? FindActiveByCustomerEmail(string email);
    Reservation? FindActiveByMachineNumber(int machineNumber);
}
