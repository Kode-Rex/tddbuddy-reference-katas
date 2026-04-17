namespace LaundryReservation;

public class ReservationService
{
    private const int MaxFailedAttempts = 5;

    private readonly IReservationRepository _repository;
    private readonly IEmailNotifier _emailNotifier;
    private readonly ISmsNotifier _smsNotifier;
    private readonly MachineApi _machineApi;
    private readonly IPinGenerator _pinGenerator;
    private readonly IMachineSelector _machineSelector;
    private readonly Dictionary<int, int> _failureCounts = new();

    public ReservationService(
        IReservationRepository repository,
        IEmailNotifier emailNotifier,
        ISmsNotifier smsNotifier,
        MachineApi machineApi,
        IPinGenerator pinGenerator,
        IMachineSelector machineSelector)
    {
        _repository = repository;
        _emailNotifier = emailNotifier;
        _smsNotifier = smsNotifier;
        _machineApi = machineApi;
        _pinGenerator = pinGenerator;
        _machineSelector = machineSelector;
    }

    public Reservation CreateReservation(DateTime slot, Customer customer)
    {
        var existing = _repository.FindActiveByCustomerEmail(customer.Email);
        if (existing != null)
        {
            throw new DuplicateReservationException(
                $"Customer '{customer.Email}' already has an active reservation.");
        }

        var id = Guid.NewGuid();
        var machineNumber = _machineSelector.SelectAvailable();
        var pin = _pinGenerator.Generate();
        var reservation = new Reservation(id, slot, machineNumber, customer, pin);

        _repository.Save(reservation);

        _emailNotifier.Send(
            customer.Email,
            "Wunda Wash Reservation Confirmation",
            $"Reservation {id}: Machine {machineNumber}, PIN {pin:D5}");

        _machineApi.Lock(id.ToString(), machineNumber, slot, pin);

        return reservation;
    }

    public bool ClaimReservation(int machineNumber, int pin)
    {
        var reservation = _repository.FindActiveByMachineNumber(machineNumber);
        if (reservation == null) return false;

        if (reservation.Pin == pin)
        {
            reservation.MarkUsed();
            _repository.Save(reservation);
            _machineApi.Unlock(machineNumber, reservation.Id.ToString());
            _failureCounts.Remove(machineNumber);
            return true;
        }

        _failureCounts.TryGetValue(machineNumber, out var failures);
        failures++;
        _failureCounts[machineNumber] = failures;

        if (failures >= MaxFailedAttempts)
        {
            var newPin = _pinGenerator.Generate();
            reservation.UpdatePin(newPin);
            _repository.Save(reservation);
            _smsNotifier.Send(
                reservation.Customer.CellPhone,
                $"Your new Wunda Wash PIN is {newPin:D5}.");
            _machineApi.Lock(
                reservation.Id.ToString(),
                machineNumber,
                reservation.Slot,
                newPin);
            _failureCounts[machineNumber] = 0;
        }

        return false;
    }
}
