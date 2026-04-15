namespace ClamCard;

public class UnknownStationException : ArgumentException
{
    public UnknownStationException() : base(CardMessages.UnknownStation) { }
}
