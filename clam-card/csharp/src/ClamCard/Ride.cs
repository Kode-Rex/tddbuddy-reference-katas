namespace ClamCard;

// A completed journey. Carries the endpoints, the zone the fare was
// charged at, and the fare actually charged (may be less than the
// single-fare rate if a daily cap was reached).
public readonly record struct Ride(string From, string To, Zone Zone, decimal Fare);
