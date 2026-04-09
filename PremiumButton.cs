using System.Drawing.Drawing2D;

namespace TimerResolutionApp
{
    public class PremiumButton : Button
    {
        private bool _hovered;
        private bool _pressed;

        /// <summary>Fase global 0–1 (sinusoide) para bordes vivos al pasar el ratón.</summary>
        public static float AmbientPulse { get; set; } = 1f;

        public bool IsPrimary { get; set; }

        public PremiumButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(250, 250, 250);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hovered = false;
            _pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _pressed = true;
                Invalidate();
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            var clip = ClientRectangle;
            g.SetClip(clip);

            // Borde interior: evita pintar fuera del control (artefactos junto a otros botones).
            var rect = new Rectangle(clip.X, clip.Y, clip.Width - 1, clip.Height - 1);

            Color border = IsPrimary
                ? Color.FromArgb(210, 186, 200, 255)
                : Color.FromArgb(63, 63, 70);

            if (IsPrimary)
            {
                Color cLeft = _pressed
                    ? Color.FromArgb(55, 79, 140, 255)
                    : _hovered
                        ? Color.FromArgb(255, 96, 165, 250)
                        : Color.FromArgb(255, 59, 130, 246);
                Color cRight = _pressed
                    ? Color.FromArgb(55, 160, 0, 110)
                    : _hovered
                        ? Color.FromArgb(255, 244, 80, 170)
                        : Color.FromArgb(255, 214, 0, 120);
                using (var brush = new LinearGradientBrush(
                           new Rectangle(rect.X, rect.Y, Math.Max(1, rect.Width), rect.Height),
                           cLeft,
                           cRight,
                           LinearGradientMode.Horizontal))
                    g.FillRectangle(brush, rect);

                if (!_pressed)
                {
                    int hiA = (int)(38 + 28 * AmbientPulse);
                    using var hi = new Pen(Color.FromArgb(hiA, 255, 255, 255), 1f);
                    int y = rect.Top + 1;
                    g.DrawLine(hi, rect.Left + 2, y, rect.Right - 2, y);
                }
            }
            else
            {
                Color top = _pressed ? Color.FromArgb(28, 28, 34) : _hovered ? Color.FromArgb(42, 42, 50) : Color.FromArgb(34, 34, 40);
                Color bottom = _pressed ? Color.FromArgb(22, 22, 28) : _hovered ? Color.FromArgb(36, 36, 44) : Color.FromArgb(28, 28, 34);
                using (var brush = new LinearGradientBrush(
                           new Rectangle(rect.X, rect.Y, rect.Width, Math.Max(1, rect.Height)),
                           _pressed ? bottom : top,
                           bottom,
                           LinearGradientMode.Vertical))
                    g.FillRectangle(brush, rect);

                if (!_pressed)
                {
                    int hiA = (int)(32 + 24 * AmbientPulse);
                    using var hi = new Pen(Color.FromArgb(hiA, 255, 255, 255), 1f);
                    int y = rect.Top + 1;
                    g.DrawLine(hi, rect.Left + 2, y, rect.Right - 2, y);
                }
            }

            float pulse = AmbientPulse;
            // Halo solo hacia dentro (no Inflate positivo hacia fuera).
            if (_hovered && !IsPrimary && rect.Width > 6 && rect.Height > 6)
            {
                int glowA = (int)(35 + 50 * pulse);
                using var glow = new Pen(Color.FromArgb(glowA, 56, 189, 248), 1f);
                var inner = Rectangle.Inflate(rect, -2, -2);
                g.DrawRectangle(glow, inner);
            }

            if (_hovered && IsPrimary && rect.Width > 6 && rect.Height > 6)
            {
                int edgeA = (int)(100 + 80 * pulse);
                using var edgeGlow = new Pen(Color.FromArgb(edgeA, 255, 180, 230), 1f);
                var inner = Rectangle.Inflate(rect, -2, -2);
                g.DrawRectangle(edgeGlow, inner);
            }

            using var borderPen = new Pen(border, 1f);
            g.DrawRectangle(borderPen, rect);

            // Padding según ancho/alto: símbolos (↻, ?) no se recortan; textos largos mantienen margen.
            bool shortGlyph = Text.Length <= 2;
            int padX = shortGlyph ? Math.Max(2, rect.Width / 10) : Math.Min(10, Math.Max(6, rect.Width / 12));
            int padY = shortGlyph ? Math.Max(1, rect.Height / 12) : Math.Min(8, Math.Max(3, rect.Height / 10));
            var textRect = new Rectangle(
                rect.Left + padX,
                rect.Top + padY,
                Math.Max(1, rect.Width - padX * 2),
                Math.Max(1, rect.Height - padY * 2));

            var tf = TextFormatFlags.HorizontalCenter
                     | TextFormatFlags.VerticalCenter
                     | TextFormatFlags.NoPrefix
                     | TextFormatFlags.SingleLine
                     | TextFormatFlags.NoPadding;
            if (!shortGlyph && rect.Width < 92)
                tf |= TextFormatFlags.EndEllipsis;

            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, tf);
            g.ResetClip();
        }
    }
}
