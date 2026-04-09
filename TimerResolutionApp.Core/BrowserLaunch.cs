using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TimerResolutionApp;

/// <summary>Abre el navegador solo para URLs http(s) absolutas sin credenciales incrustadas.</summary>
public static class BrowserLaunch
{
    public static void OpenIfTrustedHttpOrHttps(string? url)
    {
        if (!TryGetLaunchableUrl(url, out string? safe))
            return;
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = safe,
                UseShellExecute = true
            });
        }
        catch
        {
            /* ignorar: sin permisos o sin navegador predeterminado */
        }
    }

    public static bool TryGetLaunchableUrl(string? url, [NotNullWhen(true)] out string? safe)
    {
        safe = null;
        if (string.IsNullOrWhiteSpace(url))
            return false;
        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out Uri? uri))
            return false;
        if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            return false;
        if (!string.IsNullOrEmpty(uri.UserInfo))
            return false;
        safe = uri.AbsoluteUri;
        return true;
    }
}
