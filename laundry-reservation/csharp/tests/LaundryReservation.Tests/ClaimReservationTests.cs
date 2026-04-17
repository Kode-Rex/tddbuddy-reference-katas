using FluentAssertions;
using Xunit;

namespace LaundryReservation.Tests;

public class ClaimReservationTests
{
    private static readonly DateTime Slot = new(2026, 4, 15, 10, 0, 0);
    private static readonly Customer Customer = new("alice@example.com", "+27821234567");

    [Fact]
    public void Claiming_with_the_correct_PIN_marks_the_reservation_as_used()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        service.ClaimReservation(7, 12345);

        builder.Repository.All[0].IsUsed.Should().BeTrue();
    }

    [Fact]
    public void Claiming_with_the_correct_PIN_unlocks_the_machine()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        service.ClaimReservation(7, 12345);

        builder.Device.UnlockCalls.Should().ContainSingle();
    }

    [Fact]
    public void Claiming_with_an_incorrect_PIN_does_not_unlock_the_machine()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        service.ClaimReservation(7, 99999);

        builder.Device.UnlockCalls.Should().BeEmpty();
    }

    [Fact]
    public void Claiming_with_an_incorrect_PIN_does_not_mark_the_reservation_as_used()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        service.ClaimReservation(7, 99999);

        builder.Repository.All[0].IsUsed.Should().BeFalse();
    }

    [Fact]
    public void Five_consecutive_incorrect_PINs_sends_an_SMS_with_a_new_PIN()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345, 67890);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        for (var i = 0; i < 5; i++) service.ClaimReservation(7, 99999);

        builder.SmsNotifier.Sent.Should().ContainSingle();
        var sms = builder.SmsNotifier.Sent[0];
        sms.To.Should().Be("+27821234567");
        sms.Message.Should().Be("Your new Wunda Wash PIN is 67890.");
    }

    [Fact]
    public void Five_consecutive_incorrect_PINs_updates_the_reservation_with_the_new_PIN()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345, 67890);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        for (var i = 0; i < 5; i++) service.ClaimReservation(7, 99999);

        builder.Repository.All[0].Pin.Should().Be(67890);
    }

    [Fact]
    public void Five_consecutive_incorrect_PINs_re_locks_the_machine_with_the_new_PIN()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345, 67890);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        for (var i = 0; i < 5; i++) service.ClaimReservation(7, 99999);

        // First lock call is from CreateReservation, second is from the re-lock after 5 failures
        builder.Device.LockCalls.Should().HaveCount(2);
        var reLock = builder.Device.LockCalls[1];
        reLock.Pin.Should().Be(67890);
    }

    [Fact]
    public void A_successful_claim_resets_the_failure_counter()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        for (var i = 0; i < 4; i++) service.ClaimReservation(7, 99999);
        service.ClaimReservation(7, 12345);

        // After success, no SMS should have been sent
        builder.SmsNotifier.Sent.Should().BeEmpty();
    }

    [Fact]
    public void The_failure_counter_resets_after_a_new_PIN_is_issued_allowing_five_more_attempts()
    {
        var builder = new ReservationServiceBuilder().WithPins(12345, 67890, 11111);
        var (service, _) = builder.Build();
        service.CreateReservation(Slot, Customer);

        // First round: 5 bad PINs triggers new PIN (67890)
        for (var i = 0; i < 5; i++) service.ClaimReservation(7, 99999);
        builder.SmsNotifier.Sent.Should().HaveCount(1);

        // Second round: 4 bad PINs should not trigger another SMS
        for (var i = 0; i < 4; i++) service.ClaimReservation(7, 99999);
        builder.SmsNotifier.Sent.Should().HaveCount(1);

        // Fifth bad PIN of second round triggers second SMS with PIN 11111
        service.ClaimReservation(7, 99999);
        builder.SmsNotifier.Sent.Should().HaveCount(2);
    }
}
