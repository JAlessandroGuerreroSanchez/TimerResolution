using System.Runtime.InteropServices;

namespace TimerResolutionApp;

/// <summary>P/Invoke para NtQueryTimerResolution / NtSetTimerResolution (ntdll).</summary>
public static class TimerResolutionNative
{
    public const int StatusSuccess = 0;

    [DllImport("ntdll.dll")]
    public static extern int NtQueryTimerResolution(
        out uint minimumResolution,
        out uint maximumResolution,
        out uint currentResolution);

    [DllImport("ntdll.dll")]
    public static extern int NtSetTimerResolution(
        uint desiredResolution,
        [MarshalAs(UnmanagedType.U1)] bool setResolution,
        out uint currentResolution);
}
