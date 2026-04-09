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
}
