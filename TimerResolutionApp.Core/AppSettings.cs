using System.Text.Json;

namespace TimerResolutionApp;

public class AppSettings
{
    public bool StartWithWindows { get; set; }
    public bool ApplyMaximumAtStartup { get; set; }
    public bool DarkTheme { get; set; }
    public string LastResolutionMode { get; set; } = "Default";

    /// <summary>Última resolución elegida en el menú personalizado (ms), p. ej. 0,5 o 1.</summary>
    public double LastCustomResolutionMs { get; set; } = 0.5;

    /// <summary>Si es true, al cerrar la ventana con alta precisión activa se pide confirmación.</summary>
    public bool WarnOnExitIfHighRes { get; set; } = true;

    private static string SettingsPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TimerResolutionApp",
            "settings.json");

    public static AppSettings Load()
    {
        try
        {
            var path = SettingsPath;
            if (!File.Exists(path)) return new AppSettings();
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsPath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
        catch { }
    }
}
