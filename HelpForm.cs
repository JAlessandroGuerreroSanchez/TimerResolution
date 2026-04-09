namespace TimerResolutionApp
{
    public partial class HelpForm : Form
    {
        private bool _dwmChrome;

        public HelpForm(bool dark)
        {
            InitializeComponent();
            Load += HelpForm_Load;
        }

        private void HelpForm_Load(object? sender, EventArgs e)
        {
            _dwmChrome = DwmBackdrop.TryEnable(this);
            ApplyDarkStudioStyle();
        }

        private void ApplyDarkStudioStyle()
        {
            BackColor = _dwmChrome ? Color.Black : Color.FromArgb(10, 10, 12);
            ForeColor = Color.FromArgb(244, 244, 245);
            txtHelp.BackColor = Color.FromArgb(20, 20, 24);
            txtHelp.ForeColor = Color.FromArgb(212, 212, 216);
        }
    }
}
