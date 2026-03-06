namespace TimerResolutionApp
{
    public partial class HelpForm : Form
    {
        public HelpForm(bool dark)
        {
            InitializeComponent();
            if (dark) ApplyDarkTheme();
        }

        private void ApplyDarkTheme()
        {
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            txtHelp.BackColor = Color.FromArgb(30, 30, 30);
            txtHelp.ForeColor = Color.White;
        }
    }
}
