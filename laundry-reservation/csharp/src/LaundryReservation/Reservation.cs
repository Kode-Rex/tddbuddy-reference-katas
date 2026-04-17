namespace LaundryReservation;

public class Reservation
{
    public Guid Id { get; }
    public DateTime Slot { get; }
    public int MachineNumber { get; }
    public Customer Customer { get; }
    public int Pin { get; private set; }
    public bool IsUsed { get; private set; }

    public Reservation(Guid id, DateTime slot, int machineNumber, Customer customer, int pin)
    {
        Id = id;
        Slot = slot;
        MachineNumber = machineNumber;
        Customer = customer;
        Pin = pin;
    }

    public void MarkUsed() => IsUsed = true;

    public void UpdatePin(int newPin) => Pin = newPin;
}
