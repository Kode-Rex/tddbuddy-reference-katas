namespace LaundryReservation;

public class DuplicateReservationException : Exception
{
    public DuplicateReservationException(string message) : base(message) { }
}
