namespace LaundryReservation.Tests;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly List<Reservation> _reservations = new();

    public IReadOnlyList<Reservation> All => _reservations;

    public void Save(Reservation reservation)
    {
        var existing = _reservations.FindIndex(r => r.Id == reservation.Id);
        if (existing >= 0)
            _reservations[existing] = reservation;
        else
            _reservations.Add(reservation);
    }

    public Reservation? FindActiveByCustomerEmail(string email) =>
        _reservations.FirstOrDefault(r => r.Customer.Email == email && !r.IsUsed);

    public Reservation? FindActiveByMachineNumber(int machineNumber) =>
        _reservations.FirstOrDefault(r => r.MachineNumber == machineNumber && !r.IsUsed);
}
