using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace TimerResolutionApp
{
    /// <summary>
    /// Windows 11: Mica / sistema de fondo y modo oscuro en el marco (DWM).
    /// Windows 10: solo modo oscuro del marco cuando el sistema lo permite.
    /// </summary>
    internal static class DwmBackdrop
    {
        private const int DwmwaUseImmersiveDarkMode = 20;
        private const int DwmwaSystemBackdropType = 38;
        private const int DwmwaMicaEffect = 1029;

        private const int DwmsbtMainwindow = 2; // Mica

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);

        /// <summary>
        /// Devuelve el build de Windows (p. ej. 22631) leyendo el registro; más fiable que Environment solo.
        /// </summary>
        internal static int GetWindowsBuild()
        {
            try
            {
                using var k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                if (k == null) return 0;
                var n = k.GetValue("CurrentBuildNumber") as string;
                if (int.TryParse(n, out var b)) return b;
                n = k.GetValue("CurrentBuild")?.ToString();
                return int.TryParse(n, out b) ? b : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Activa caption oscuro y, en Win11 22H2+, Mica en el área cliente del formulario.
        /// </summary>
        internal static bool TryEnable(Form form)
        {
            if (!form.IsHandleCreated)
                return false;

            int dark = 1;
            DwmSetWindowAttribute(form.Handle, DwmwaUseImmersiveDarkMode, ref dark, sizeof(int));

            int build = GetWindowsBuild();
            if (build >= 22621)
            {
                int backdrop = DwmsbtMainwindow;
                if (DwmSetWindowAttribute(form.Handle, DwmwaSystemBackdropType, ref backdrop, sizeof(int)) == 0)
                    return true;
            }

            if (build >= 22000)
            {
                int on = 1;
                if (DwmSetWindowAttribute(form.Handle, DwmwaMicaEffect, ref on, sizeof(int)) == 0)
                    return true;
            }

            return false;
        }
    }
}
