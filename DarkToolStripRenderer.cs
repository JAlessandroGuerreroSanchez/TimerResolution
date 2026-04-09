namespace TimerResolutionApp
{
    /// <summary>Menús contextuales (bandeja) coherentes con el tema oscuro.</summary>
    internal sealed class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkToolStripRenderer() : base(new DarkMenuColors()) { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using var p = new Pen(Color.FromArgb(55, 55, 62), 1f);
            var r = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            e.Graphics.DrawRectangle(p, r);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Enabled || e.Item.IsOnDropDown == false)
            {
                base.OnRenderMenuItemBackground(e);
                return;
            }

            var rc = new Rectangle(Point.Empty, e.Item.Size);
            if (e.Item.Selected)
            {
                using var b = new SolidBrush(Color.FromArgb(48, 48, 56));
                e.Graphics.FillRectangle(b, rc);
            }
        }

        private sealed class DarkMenuColors : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => Color.FromArgb(28, 28, 32);
            public override Color ImageMarginGradientBegin => Color.FromArgb(28, 28, 32);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(28, 28, 32);
            public override Color ImageMarginGradientEnd => Color.FromArgb(28, 28, 32);
            public override Color MenuBorder => Color.FromArgb(55, 55, 62);
            public override Color MenuItemBorder => Color.FromArgb(55, 55, 62);
            public override Color MenuItemSelected => Color.FromArgb(48, 48, 56);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(48, 48, 56);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(48, 48, 56);
            public override Color SeparatorDark => Color.FromArgb(55, 55, 62);
            public override Color SeparatorLight => Color.FromArgb(55, 55, 62);
        }
    }
}
