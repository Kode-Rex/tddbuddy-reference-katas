using System.Runtime.CompilerServices;
using static ClamCard.Fares;

[assembly: InternalsVisibleTo("ClamCard.Tests")]

namespace ClamCard;

public sealed class Card
{
    private readonly IReadOnlyDictionary<string, Zone> _stations;
    private readonly List<Ride> _rides = new();
    private decimal _chargedZoneAToday;
    private decimal _chargedZoneBToday;

    internal Card(IReadOnlyDictionary<string, Zone> stations)
    {
        _stations = stations;
    }

    public IReadOnlyList<Ride> Rides() => _rides;

    public decimal TotalCharged() => _chargedZoneAToday + _chargedZoneBToday;

    public JourneyStart TravelFrom(string station)
    {
        EnsureKnown(station);
        return new JourneyStart(this, station);
    }

    private Ride CompleteJourney(string from, string to)
    {
        EnsureKnown(to);
        var fromZone = _stations[from];
        var toZone = _stations[to];
        var journeyZone = (fromZone == Zone.B || toZone == Zone.B) ? Zone.B : Zone.A;

        var fare = ChargeForZone(journeyZone);
        var ride = new Ride(from, to, journeyZone, fare);
        _rides.Add(ride);
        return ride;
    }

    private decimal ChargeForZone(Zone zone)
    {
        var (singleFare, cap, charged) = zone == Zone.A
            ? (ZoneASingleFare, ZoneADailyCap, _chargedZoneAToday)
            : (ZoneBSingleFare, ZoneBDailyCap, _chargedZoneBToday);

        var remaining = cap - charged;
        var fare = Math.Max(0m, Math.Min(singleFare, remaining));

        if (zone == Zone.A) _chargedZoneAToday += fare;
        else _chargedZoneBToday += fare;

        return fare;
    }

    private void EnsureKnown(string station)
    {
        if (!_stations.ContainsKey(station))
            throw new UnknownStationException();
    }

    public sealed class JourneyStart
    {
        private readonly Card _card;
        private readonly string _from;

        internal JourneyStart(Card card, string from)
        {
            _card = card;
            _from = from;
        }

        public Ride To(string station) => _card.CompleteJourney(_from, station);
    }
}
