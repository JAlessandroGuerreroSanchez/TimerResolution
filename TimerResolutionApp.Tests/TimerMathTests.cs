using Xunit;

namespace TimerResolutionApp.Tests;

public class TimerMathTests
{
    [Theory]
    [InlineData(5000u, 0.5)]
    [InlineData(10_000u, 1)]
    [InlineData(156_250u, 15.625)]
    public void UnitsToMilliseconds(uint units, double expectedMs)
    {
        Assert.Equal(expectedMs, TimerMath.UnitsToMilliseconds(units), 6);
    }

    [Theory]
    [InlineData(0.5, 5000u)]
    [InlineData(1, 10_000u)]
    [InlineData(15.625, 156_250u)]
    public void MillisecondsToUnits(double ms, uint expectedUnits)
    {
        Assert.Equal(expectedUnits, TimerMath.MillisecondsToUnits(ms));
    }

    [Fact]
    public void RoundTrip_nearHalfMillisecond()
    {
        uint u = TimerMath.MillisecondsToUnits(0.5);
        Assert.Equal(0.5, TimerMath.UnitsToMilliseconds(u), 4);
    }

    [Fact]
    public void MillisecondsToUnits_rejects_out_of_range()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TimerMath.MillisecondsToUnits(0.1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TimerMath.MillisecondsToUnits(100));
        Assert.Throws<ArgumentOutOfRangeException>(() => TimerMath.MillisecondsToUnits(double.NaN));
    }

    [Fact]
    public void TryMillisecondsToUnits_matches_valid_menu_values()
    {
        foreach (var ms in new[] { 0.5, 1.0, 2.0, 5.0, 15.625 })
        {
            Assert.True(TimerMath.TryMillisecondsToUnits(ms, out uint u));
            Assert.True(u > 0);
        }
    }
}
