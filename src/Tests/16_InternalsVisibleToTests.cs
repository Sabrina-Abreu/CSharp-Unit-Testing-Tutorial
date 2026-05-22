using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 16 — Testing Internal Methods via InternalsVisibleTo
///
/// Sometimes logic is intentionally internal (not public API) but still
/// needs test coverage. The solution:
///   1. Add [assembly: InternalsVisibleTo("Tests")] to the Library project.
///   2. The test project then has access to all internal members.
///
/// This avoids making implementation details public just to test them.
/// </summary>
public class InternalsVisibleToTests
{
    // DiscountCalculator is internal — accessible only because of
    // [assembly: InternalsVisibleTo("Tests")] in Library/DiscountCalculator.cs
    private readonly DiscountCalculator _calc = new();

    [Theory]
    [InlineData(1, 100.0, 5.0)]    // Bronze → 5 %
    [InlineData(2, 100.0, 10.0)]   // Silver → 10 %
    [InlineData(3, 100.0, 20.0)]   // Gold   → 20 %
    public void CalculateDiscount_KnownTiers_ReturnsCorrectAmount(
        int tier, double price, double expectedDiscount)
    {
        decimal discount = _calc.CalculateDiscount((decimal)price, tier);
        Assert.Equal((decimal)expectedDiscount, discount);
    }

    [Fact]
    public void CalculateDiscount_InvalidTier_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _calc.CalculateDiscount(100m, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => _calc.CalculateDiscount(100m, 4));
    }

    [Fact]
    public void ApplySeasonalBonus_DuringSeason_IncreasesDiscountByHalf()
    {
        decimal baseDiscount  = 20m;
        decimal withBonus     = _calc.ApplySeasonalBonus(baseDiscount, isSeason: true);
        decimal withoutBonus  = _calc.ApplySeasonalBonus(baseDiscount, isSeason: false);

        Assert.Equal(30m, withBonus);    // 20 × 1.5
        Assert.Equal(20m, withoutBonus); // unchanged
    }

    [Theory]
    [InlineData(1, 99.0,  false)]   // Bronze, order < 100 → no free shipping
    [InlineData(2, 10.0,  true)]    // Silver             → always free shipping
    [InlineData(1, 100.0, true)]    // Bronze, order >= 100 → free shipping
    [InlineData(3, 5.0,   true)]    // Gold               → always free shipping
    public void IsEligibleForFreeShipping_VariousCases(
        int tier, double total, bool expected)
    {
        bool result = _calc.IsEligibleForFreeShipping((decimal)total, tier);
        Assert.Equal(expected, result);
    }
}
