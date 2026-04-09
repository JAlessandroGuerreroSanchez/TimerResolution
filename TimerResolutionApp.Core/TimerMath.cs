namespace TimerResolutionApp;

/// <summary>Conversión entre unidades de 100 ns de ntdll y milisegundos.</summary>
public static class TimerMath
{
    public static double UnitsToMilliseconds(uint units) => units / 10_000.0;

    public static uint MillisecondsToUnits(double ms) => (uint)Math.Round(ms * 10_000.0);
}
