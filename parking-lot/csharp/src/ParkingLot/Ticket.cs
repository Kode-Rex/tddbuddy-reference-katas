namespace ParkingLot;

public sealed record Ticket(Vehicle Vehicle, SpotType SpotType, DateTime EntryTime);
