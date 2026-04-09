using Xunit;

namespace TimerResolutionApp.Tests;

public class BrowserLaunchTests
{
    [Theory]
    [InlineData("https://kalur.me/")]
    [InlineData("http://example.com/path")]
    public void TryGetLaunchableUrl_accepts_http_https(string url)
    {
        Assert.True(BrowserLaunch.TryGetLaunchableUrl(url, out string? safe));
        Assert.NotNull(safe);
        Assert.StartsWith("http", safe, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("file:///C:/Windows/notepad.exe")]
    [InlineData("javascript:alert(1)")]
    [InlineData("cmd.exe")]
    [InlineData("https://user:pass@example.com/")]
    public void TryGetLaunchableUrl_rejects_unsafe(string url)
    {
        Assert.False(BrowserLaunch.TryGetLaunchableUrl(url, out _));
    }
}
