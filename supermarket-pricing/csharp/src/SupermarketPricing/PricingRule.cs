namespace SupermarketPricing;

/// <summary>
/// Strategy for computing the charge for a product given its scanned quantity
/// and (optionally) weight. Each rule variant implements this interface.
/// </summary>
public interface IPricingRule
{
    Money Calculate(int quantity, Weight weight);
}
