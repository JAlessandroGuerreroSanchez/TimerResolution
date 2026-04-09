namespace TimerResolutionApp;

/// <summary>Conversión entre unidades de 100 ns de ntdll y milisegundos.</summary>
public static class TimerMath
{
    public static double UnitsToMilliseconds(uint units) => units / 10_000.0;

    /// <summary>Convierte ms a unidades de ntdll; rechaza NaN, infinito y valores fuera del rango seguro para la UI.</summary>
    public static bool TryMillisecondsToUnits(double ms, out uint units)
    {
        units = 0;
        if (double.IsNaN(ms) || double.IsInfinity(ms))
            return false;
        if (ms < AppSettings.MinCustomResolutionMs || ms > AppSettings.MaxCustomResolutionMs)
            return false;

        var scaled = Math.Round(ms * 10_000.0);
        if (scaled < 1 || scaled > uint.MaxValue)
            return false;

        units = (uint)scaled;
        return true;
    }

    public static uint MillisecondsToUnits(double ms)
    {
        if (!TryMillisecondsToUnits(ms, out uint u))
            throw new ArgumentOutOfRangeException(nameof(ms));
        return u;
    }
}
