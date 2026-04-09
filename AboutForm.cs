using System.Reflection;

namespace TimerResolutionApp
{
    public partial class AboutForm : Form
    {
        private bool _dwmChrome;

        public AboutForm(bool dark)
        {
            InitializeComponent();
            lblVersion.Text = $"Versión {GetVersion()}";
            Load += AboutForm_Load;
        }

        private void AboutForm_Load(object? sender, EventArgs e)
        {
            _dwmChrome = DwmBackdrop.TryEnable(this);
            ApplyDarkStudioStyle();
        }

        private static string GetVersion()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
            }
            catch { return "1.0.0"; }
        }

        private void ApplyDarkStudioStyle()
        {
            BackColor = _dwmChrome ? Color.Black : Color.FromArgb(10, 10, 12);
            ForeColor = Color.FromArgb(244, 244, 245);
            lblTitle.ForeColor = Color.FromArgb(250, 250, 250);
            lblVersion.ForeColor = Color.FromArgb(161, 161, 170);
            lnkUrl.LinkColor = Color.FromArgb(96, 165, 250);
        }
    }
}
