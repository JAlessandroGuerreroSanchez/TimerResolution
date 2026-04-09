using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace TimerResolutionApp
{
    /// <summary>Genera variantes del icono de bandeja (p. ej. punto de estado en alta precisión).</summary>
    internal static class TrayIconHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        public static Icon CreateTrayIcon(Icon? source, bool highResBadge)
        {
            if (source == null)
                return SystemIcons.Application;

            try
            {
                int size = 32;
                using var bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.Clear(Color.Transparent);
                    using var src = source.ToBitmap();
                    g.DrawImage(src, new Rectangle(0, 0, size, size));

                    if (highResBadge)
                    {
                        using var glow = new SolidBrush(Color.FromArgb(255, 56, 189, 248));
                        g.FillEllipse(glow, 20, 3, 10, 10);
                        using var core = new SolidBrush(Color.FromArgb(255, 224, 242, 254));
                        g.FillEllipse(core, 22, 5, 6, 6);
                    }
                }

                IntPtr h = bmp.GetHicon();
                try
                {
                    using var tmp = Icon.FromHandle(h);
                    return (Icon)tmp.Clone();
                }
                finally
                {
                    DestroyIcon(h);
                }
            }
            catch
            {
                return source;
            }
        }
    }
}
