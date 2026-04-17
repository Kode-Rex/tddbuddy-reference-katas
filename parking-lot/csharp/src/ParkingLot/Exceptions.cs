namespace ParkingLot;

public class VehicleAlreadyParkedException : Exception
{
    public VehicleAlreadyParkedException(string message) : base(message) { }
}

public class NoAvailableSpotException : Exception
{
    public NoAvailableSpotException(string message) : base(message) { }
}

public class InvalidTicketException : Exception
{
    public InvalidTicketException(string message) : base(message) { }
}

public class InvalidLotConfigurationException : Exception
{
    public InvalidLotConfigurationException(string message) : base(message) { }
}
