namespace ParkingLot.Tests;

public class VehicleBuilder
{
    private VehicleType _type = VehicleType.Car;
    private string _plate = "CAR-001";

    public VehicleBuilder AsMotorcycle() { _type = VehicleType.Motorcycle; _plate = "MC-001"; return this; }

    public VehicleBuilder AsCar() { _type = VehicleType.Car; _plate = "CAR-001"; return this; }

    public VehicleBuilder AsBus() { _type = VehicleType.Bus; _plate = "BUS-001"; return this; }

    public VehicleBuilder WithPlate(string plate) { _plate = plate; return this; }

    public Vehicle Build() => new(_type, _plate);
}
