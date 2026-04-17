namespace ParkingLot;

public class Lot
{
    public const decimal DefaultMotorcycleRate = 1m;
    public const decimal DefaultCarRate = 3m;
    public const decimal DefaultBusRate = 5m;

    private static readonly SpotType[] MotorcyclePreference = { SpotType.Motorcycle, SpotType.Compact, SpotType.Large };
    private static readonly SpotType[] CarPreference = { SpotType.Compact, SpotType.Large };
    private static readonly SpotType[] BusPreference = { SpotType.Large };

    private readonly IClock _clock;
    private readonly Dictionary<SpotType, int> _availableSpots;
    private readonly Dictionary<string, Ticket> _activeTickets = new();
    private readonly Dictionary<VehicleType, decimal> _rates;

    public Lot(
        int motorcycleSpots,
        int compactSpots,
        int largeSpots,
        IClock clock,
        decimal motorcycleRate = DefaultMotorcycleRate,
        decimal carRate = DefaultCarRate,
        decimal busRate = DefaultBusRate)
    {
        if (motorcycleSpots + compactSpots + largeSpots <= 0)
            throw new InvalidLotConfigurationException("Lot must have at least one spot");

        _clock = clock;
        _availableSpots = new Dictionary<SpotType, int>
        {
            { SpotType.Motorcycle, motorcycleSpots },
            { SpotType.Compact, compactSpots },
            { SpotType.Large, largeSpots }
        };
        _rates = new Dictionary<VehicleType, decimal>
        {
            { VehicleType.Motorcycle, motorcycleRate },
            { VehicleType.Car, carRate },
            { VehicleType.Bus, busRate }
        };
    }

    public Ticket ParkEntry(Vehicle vehicle)
    {
        if (_activeTickets.ContainsKey(vehicle.LicensePlate))
            throw new VehicleAlreadyParkedException($"Vehicle {vehicle.LicensePlate} is already parked");

        var spotType = AllocateSpot(vehicle);
        var now = _clock.Now();
        var ticket = new Ticket(vehicle, spotType, now);
        _activeTickets[vehicle.LicensePlate] = ticket;
        return ticket;
    }

    public Fee ProcessExit(Ticket ticket)
    {
        if (!_activeTickets.TryGetValue(ticket.Vehicle.LicensePlate, out var stored) || stored != ticket)
            throw new InvalidTicketException("Ticket is not valid");

        _activeTickets.Remove(ticket.Vehicle.LicensePlate);
        _availableSpots[ticket.SpotType] += 1;

        var now = _clock.Now();
        var elapsed = now - ticket.EntryTime;
        var hours = (int)Math.Ceiling(elapsed.TotalHours);
        if (hours < 1) hours = 1;

        var rate = _rates[ticket.Vehicle.Type];
        return new Fee(hours * rate);
    }

    private SpotType AllocateSpot(Vehicle vehicle)
    {
        var preference = vehicle.Type switch
        {
            VehicleType.Motorcycle => MotorcyclePreference,
            VehicleType.Car => CarPreference,
            VehicleType.Bus => BusPreference,
            _ => throw new ArgumentOutOfRangeException()
        };

        foreach (var spotType in preference)
        {
            if (_availableSpots[spotType] > 0)
            {
                _availableSpots[spotType] -= 1;
                return spotType;
            }
        }

        throw new NoAvailableSpotException($"No available spot for vehicle {vehicle.LicensePlate}");
    }
}
