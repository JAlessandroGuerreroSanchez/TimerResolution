using Microsoft.Win32;

namespace TimerResolutionApp
{
    public static class StartupHelper
    {
        private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "TimerResolutionApp";

        public static bool GetStartWithWindows()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RunKey, false);
                return key?.GetValue(AppName) != null;
            }
            catch { return false; }
        }

        public static void SetStartWithWindows(bool enable)
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RunKey, true);
                if (key == null) return;
                if (enable)
                    key.SetValue(AppName, Environment.ProcessPath ?? Application.ExecutablePath);
                else
                    key.DeleteValue(AppName, false);
            }
            catch { }
        }
    }
}
