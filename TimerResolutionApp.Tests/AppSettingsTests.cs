using System.Text.Json;
using Xunit;

namespace TimerResolutionApp.Tests;

public class AppSettingsTests
{
    [Fact]
    public void Serialize_deserialize_preserves_known_fields()
    {
        var s = new AppSettings
        {
            StartWithWindows = true,
            LastCustomResolutionMs = 2,
            WarnOnExitIfHighRes = false,
            LastResolutionMode = "Custom"
        };
        var json = JsonSerializer.Serialize(s);
        var back = JsonSerializer.Deserialize<AppSettings>(json);
        Assert.NotNull(back);
        Assert.True(back.StartWithWindows);
        Assert.Equal(2, back.LastCustomResolutionMs);
        Assert.False(back.WarnOnExitIfHighRes);
        Assert.Equal("Custom", back.LastResolutionMode);
    }

    [Fact]
    public void Default_warn_on_exit_is_true()
    {
        var s = new AppSettings();
        Assert.True(s.WarnOnExitIfHighRes);
    }
}
