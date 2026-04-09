using System.Text.Json;

namespace TimerResolutionApp;

public class AppSettings
{
    public const double MinCustomResolutionMs = 0.5;
    public const double MaxCustomResolutionMs = 30.0;
    private const int MaxSettingsFileBytes = 64 * 1024;
    private const int MaxResolutionModeLength = 32;

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

    private static readonly JsonSerializerOptions JsonReadOptions = new()
    {
        ReadCommentHandling = JsonCommentHandling.Disallow,
        AllowTrailingCommas = false,
        MaxDepth = 32
    };

    private static readonly JsonSerializerOptions JsonWriteOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>Restringe valores cargados desde disco a rangos y literales permitidos.</summary>
    public void Normalize()
    {
        if (LastResolutionMode.Length > MaxResolutionModeLength)
            LastResolutionMode = LastResolutionMode[..MaxResolutionModeLength];
        if (LastResolutionMode is not ("Default" or "Maximum" or "Custom"))
            LastResolutionMode = "Default";

        if (double.IsNaN(LastCustomResolutionMs) || double.IsInfinity(LastCustomResolutionMs))
            LastCustomResolutionMs = MinCustomResolutionMs;
        else
            LastCustomResolutionMs = Math.Clamp(LastCustomResolutionMs, MinCustomResolutionMs, MaxCustomResolutionMs);
    }

    public static AppSettings Load()
    {
        try
        {
            var path = SettingsPath;
            if (!File.Exists(path))
                return new AppSettings();

            var fileInfo = new FileInfo(path);
            if (fileInfo.Length > MaxSettingsFileBytes)
                return new AppSettings();

            var json = File.ReadAllText(path);
            if (json.Length > MaxSettingsFileBytes)
                return new AppSettings();

            var s = JsonSerializer.Deserialize<AppSettings>(json, JsonReadOptions) ?? new AppSettings();
            s.Normalize();
            return s;
        }
        catch (JsonException)
        {
            return new AppSettings();
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
            Normalize();
            var dir = Path.GetDirectoryName(SettingsPath)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(this, JsonWriteOptions);
            var tempPath = SettingsPath + ".tmp";
            File.WriteAllText(tempPath, json);
            if (File.Exists(SettingsPath))
                File.Replace(tempPath, SettingsPath, destinationBackupFileName: null);
            else
                File.Move(tempPath, SettingsPath);
        }
        catch
        {
            try
            {
                var tempPath = SettingsPath + ".tmp";
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
            catch
            {
                /* ignorar limpieza */
            }
        }
    }
}
