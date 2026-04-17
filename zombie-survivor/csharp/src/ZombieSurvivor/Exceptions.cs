namespace ZombieSurvivor;

public class EquipmentCapacityExceededException : Exception
{
    public EquipmentCapacityExceededException(string message) : base(message) { }
}

public class DuplicateSurvivorNameException : Exception
{
    public DuplicateSurvivorNameException(string message) : base(message) { }
}
