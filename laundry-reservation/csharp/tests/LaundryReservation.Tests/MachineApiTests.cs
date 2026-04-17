using FluentAssertions;
using Xunit;

namespace LaundryReservation.Tests;

public class MachineApiTests
{
    private static readonly DateTime Slot = new(2026, 4, 15, 10, 0, 0);

    [Fact]
    public void Locking_a_machine_delegates_to_the_device_with_reservation_ID_date_time_and_PIN()
    {
        var device = new RecordingMachineDevice();
        var api = new MachineApi();
        api.RegisterDevice(7, device);

        api.Lock("res-1", 7, Slot, 12345);

        device.LockCalls.Should().ContainSingle();
        var call = device.LockCalls[0];
        call.ReservationId.Should().Be("res-1");
        call.ReservationDateTime.Should().Be(Slot);
        call.Pin.Should().Be(12345);
    }

    [Fact]
    public void Locking_a_machine_returns_true_when_the_device_accepts_the_lock()
    {
        var device = new RecordingMachineDevice { ShouldAcceptLock = true };
        var api = new MachineApi();
        api.RegisterDevice(7, device);

        var result = api.Lock("res-1", 7, Slot, 12345);

        result.Should().BeTrue();
    }

    [Fact]
    public void Locking_a_machine_returns_false_when_the_device_rejects_the_lock()
    {
        var device = new RecordingMachineDevice { ShouldAcceptLock = false };
        var api = new MachineApi();
        api.RegisterDevice(7, device);

        var result = api.Lock("res-1", 7, Slot, 12345);

        result.Should().BeFalse();
    }

    [Fact]
    public void Locking_a_machine_with_an_existing_reservation_ID_updates_the_PIN_and_date_time()
    {
        var device = new RecordingMachineDevice();
        var api = new MachineApi();
        api.RegisterDevice(7, device);
        api.Lock("res-1", 7, Slot, 12345);

        var newSlot = Slot.AddHours(1);
        var result = api.Lock("res-1", 7, newSlot, 67890);

        result.Should().BeTrue();
        device.LockCalls.Should().HaveCount(2);
        var updateCall = device.LockCalls[1];
        updateCall.ReservationId.Should().Be("res-1");
        updateCall.ReservationDateTime.Should().Be(newSlot);
        updateCall.Pin.Should().Be(67890);
    }

    [Fact]
    public void Unlocking_a_machine_delegates_to_the_device_with_the_reservation_ID()
    {
        var device = new RecordingMachineDevice();
        var api = new MachineApi();
        api.RegisterDevice(7, device);
        api.Lock("res-1", 7, Slot, 12345);

        api.Unlock(7, "res-1");

        device.UnlockCalls.Should().ContainSingle()
            .Which.Should().Be("res-1");
    }
}
