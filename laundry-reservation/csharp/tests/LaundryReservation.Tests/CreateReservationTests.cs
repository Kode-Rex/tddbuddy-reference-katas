using FluentAssertions;
using Xunit;

namespace LaundryReservation.Tests;

public class CreateReservationTests
{
    private static readonly DateTime Slot = new(2026, 4, 15, 10, 0, 0);
    private static readonly Customer Customer = new("alice@example.com", "+27821234567");

    [Fact]
    public void Creating_a_reservation_assigns_a_unique_reservation_ID()
    {
        var builder = new ReservationServiceBuilder();
        var (service, _) = builder.Build();

        var reservation = service.CreateReservation(Slot, Customer);

        reservation.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Creating_a_reservation_assigns_a_machine_number_from_the_selector()
    {
        var builder = new ReservationServiceBuilder().WithMachineNumber(14);
        var (service, _) = builder.Build();

        var reservation = service.CreateReservation(Slot, Customer);

        reservation.MachineNumber.Should().Be(14);
    }

    [Fact]
    public void Creating_a_reservation_assigns_a_five_digit_PIN_from_the_generator()
    {
        var builder = new ReservationServiceBuilder().WithPins(98765);
        var (service, _) = builder.Build();

        var reservation = service.CreateReservation(Slot, Customer);

        reservation.Pin.Should().Be(98765);
    }

    [Fact]
    public void Creating_a_reservation_sends_a_confirmation_email_with_machine_number_reservation_ID_and_PIN()
    {
        var builder = new ReservationServiceBuilder()
            .WithMachineNumber(7)
            .WithPins(12345);
        var (service, _) = builder.Build();

        var reservation = service.CreateReservation(Slot, Customer);

        builder.EmailNotifier.Sent.Should().ContainSingle();
        var email = builder.EmailNotifier.Sent[0];
        email.To.Should().Be("alice@example.com");
        email.Body.Should().Contain("Machine 7");
        email.Body.Should().Contain(reservation.Id.ToString());
        email.Body.Should().Contain("12345");
    }

    [Fact]
    public void Creating_a_reservation_saves_the_reservation_to_the_repository()
    {
        var builder = new ReservationServiceBuilder();
        var (service, _) = builder.Build();

        var reservation = service.CreateReservation(Slot, Customer);

        builder.Repository.All.Should().ContainSingle()
            .Which.Id.Should().Be(reservation.Id);
    }

    [Fact]
    public void Creating_a_reservation_locks_the_machine_via_the_Machine_API()
    {
        var builder = new ReservationServiceBuilder()
            .WithMachineNumber(7)
            .WithPins(12345);
        var (service, _) = builder.Build();

        var reservation = service.CreateReservation(Slot, Customer);

        builder.Device.LockCalls.Should().ContainSingle();
        var lockCall = builder.Device.LockCalls[0];
        lockCall.ReservationId.Should().Be(reservation.Id.ToString());
        lockCall.ReservationDateTime.Should().Be(Slot);
        lockCall.Pin.Should().Be(12345);
    }

    [Fact]
    public void Creating_a_second_reservation_for_the_same_customer_is_rejected()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345, 67890);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        var act = () => service.CreateReservation(Slot.AddHours(2), Customer);

        act.Should().Throw<DuplicateReservationException>()
            .WithMessage("Customer 'alice@example.com' already has an active reservation.");
    }
}
