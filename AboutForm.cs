using System.Reflection;

namespace TimerResolutionApp
{
    public partial class AboutForm : Form
    {
        public AboutForm(bool dark)
        {
            InitializeComponent();
            lblVersion.Text = $"Versión {GetVersion()}";
            if (dark) ApplyDarkTheme();
        }

        private static string GetVersion()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
            }
            catch { return "1.0.0"; }
        }

        private void ApplyDarkTheme()
        {
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            lblTitle.ForeColor = Color.White;
            lblVersion.ForeColor = Color.LightGray;
            lnkUrl.LinkColor = Color.LightBlue;
        }
    }
}
