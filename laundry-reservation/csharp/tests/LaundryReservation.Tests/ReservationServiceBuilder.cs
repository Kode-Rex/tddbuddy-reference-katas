namespace LaundryReservation.Tests;

public class ReservationServiceBuilder
{
    private int _machineNumber = 7;
    private int[] _pins = { 12345 };
    private readonly RecordingMachineDevice _device = new();

    public RecordingEmailNotifier EmailNotifier { get; } = new();
    public RecordingSmsNotifier SmsNotifier { get; } = new();
    public InMemoryReservationRepository Repository { get; } = new();
    public RecordingMachineDevice Device => _device;

    public ReservationServiceBuilder WithMachineNumber(int machineNumber)
    {
        _machineNumber = machineNumber;
        return this;
    }

    public ReservationServiceBuilder WithPins(params int[] pins)
    {
        _pins = pins;
        return this;
    }

    public ReservationServiceBuilder WithDeviceRejectingLock()
    {
        _device.ShouldAcceptLock = false;
        return this;
    }

    public (ReservationService Service, MachineApi MachineApi) Build()
    {
        var machineApi = new MachineApi();
        machineApi.RegisterDevice(_machineNumber, _device);

        var service = new ReservationService(
            Repository,
            EmailNotifier,
            SmsNotifier,
            machineApi,
            new FixedPinGenerator(_pins),
            new FixedMachineSelector(_machineNumber));

        return (service, machineApi);
    }
}
