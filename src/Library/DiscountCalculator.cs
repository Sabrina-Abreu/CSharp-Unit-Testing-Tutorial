using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Library;

/// <summary>
/// Internal class used to demonstrate testing internal methods via InternalsVisibleTo (example 16).
/// </summary>
internal class DiscountCalculator
{
    internal decimal CalculateDiscount(decimal price, int customerTier) => customerTier switch
    {
        1 => price * 0.05m,   // 5 % for Bronze
        2 => price * 0.10m,   // 10 % for Silver
        3 => price * 0.20m,   // 20 % for Gold
        _ => throw new ArgumentOutOfRangeException(nameof(customerTier), "Tier must be 1, 2, or 3.")
    };

    internal decimal ApplySeasonalBonus(decimal discount, bool isSeason) =>
        isSeason ? discount * 1.5m : discount;

    internal bool IsEligibleForFreeShipping(decimal orderTotal, int customerTier) =>
        customerTier >= 2 || orderTotal >= 100m;
}
