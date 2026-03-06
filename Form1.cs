using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TimerResolutionApp
{
    public partial class Form1 : Form
    {
        #region P/Invoke - ntdll.dll

        [DllImport("ntdll.dll")]
        private static extern int NtQueryTimerResolution(
            out uint MinimumResolution,
            out uint MaximumResolution,
            out uint CurrentResolution);

        [DllImport("ntdll.dll")]
        internal static extern int NtSetTimerResolution(
            uint DesiredResolution,
            [MarshalAs(UnmanagedType.U1)] bool SetResolution,
            out uint CurrentResolution);

        private const int STATUS_SUCCESS = 0;

        #endregion

        #region Conversión de unidades

        private static double UnitsToMilliseconds(uint units) => units / 10_000.0;
        private static uint MillisecondsToUnits(double ms) => (uint)Math.Round(ms * 10_000.0);

        #endregion

        private static readonly double[] CustomResolutionMs = { 0.5, 1, 2, 5, 15.625 };
        private AppSettings _settings = null!;
        private double _lastKnownCurrentMs = 15.625;

        public Form1()
        {
            InitializeComponent();
            LoadAppIcon();
            _settings = AppSettings.Load();
            ApplySettingsToUI();
            StartupHelper.SetStartWithWindows(_settings.StartWithWindows);
            if (_settings.ApplyMaximumAtStartup)
                ApplyMaximumQuiet();
            SetupToolTips();
            RefreshResolutionLabels();
            UpdateModeIndicator();
            refreshTimer.Start();
            try { notifyIcon.Icon = Icon; } catch { }
        }

        private void LoadAppIcon()
        {
            try
            {
                using var stream = typeof(Form1).Assembly.GetManifestResourceStream("TimerResolutionApp.app.ico");
                if (stream != null)
                    Icon = new Icon(stream);
            }
            catch { }
        }

        private void ApplySettingsToUI()
        {
            chkStartWithWindows.Checked = _settings.StartWithWindows;
            chkApplyMaxAtStartup.Checked = _settings.ApplyMaximumAtStartup;
            chkDarkTheme.Checked = _settings.DarkTheme;
            if (_settings.DarkTheme)
                ApplyDarkTheme(true);
        }

        private void SetupToolTips()
        {
            toolTip.SetToolTip(btnMaximum, "Establece la resolución al mínimo (0,5 ms). Ctrl+M");
            toolTip.SetToolTip(btnDefault, "Restaura el valor por defecto de Windows. Ctrl+D");
            toolTip.SetToolTip(btnClose, "Restaura y cierra. Escape");
            toolTip.SetToolTip(btnApplyCustom, "Aplica la resolución seleccionada en la lista.");
            toolTip.SetToolTip(btnRefresh, "Actualizar valores ahora.");
            toolTip.SetToolTip(btnMeasureSleep, "Mide la duración real de Sleep(1) para ver el efecto.");
            toolTip.SetToolTip(chkApplyMaxAtStartup, "Al abrir la app se aplica Maximum automáticamente.");
        }

        private void SetStatus(string message, bool isError = false)
        {
            lblStatus.ForeColor = isError ? Color.DarkRed : SystemColors.GrayText;
            lblStatus.Text = message;
        }

        private void RefreshResolutionLabels()
        {
            int status = NtQueryTimerResolution(
                out uint minUnits,
                out uint maxUnits,
                out uint currentUnits);

            if (status != STATUS_SUCCESS)
            {
                lblMin.Text = "N/A";
                lblMax.Text = "N/A";
                lblCurrent.Text = "Error al consultar";
                return;
            }

            double minMs = UnitsToMilliseconds(minUnits);
            double maxMs = UnitsToMilliseconds(maxUnits);
            double curMs = UnitsToMilliseconds(currentUnits);
            _lastKnownCurrentMs = curMs;

            lblMin.Text = $"{minMs:F3} ms ({minMs * 1000:F0} µs)";
            lblMax.Text = $"{maxMs:F3} ms ({maxMs * 1000:F0} µs)";
            lblCurrent.Text = $"{curMs:F3} ms ({(curMs * 1000):F0} µs)";
        }

        private void UpdateModeIndicator()
        {
            bool isLow = _lastKnownCurrentMs < 2.0;
            lblMode.Text = isLow ? " Modo: Máximo (alta resolución) " : " Modo: Por defecto ";
            lblMode.BackColor = isLow ? Color.FromArgb(200, 255, 200) : Color.FromArgb(240, 240, 240);
            if (_settings.DarkTheme)
            {
                lblMode.BackColor = isLow ? Color.FromArgb(0, 80, 0) : Color.FromArgb(60, 60, 60);
                lblMode.ForeColor = Color.White;
            }
            else
                lblMode.ForeColor = SystemColors.ControlText;
        }

        private void refreshTimer_Tick(object? sender, EventArgs e)
        {
            RefreshResolutionLabels();
            UpdateModeIndicator();
        }

        private void ApplyMaximumQuiet()
        {
            NtSetTimerResolution(5000, true, out _);
            RefreshResolutionLabels();
            UpdateModeIndicator();
        }

        private void btnMaximum_Click(object sender, EventArgs e)
        {
            if (IsOnBattery())
            {
                var r = MessageBox.Show(
                    "Estás usando batería. La alta resolución puede aumentar el consumo. ¿Continuar?",
                    "Aviso",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (r != DialogResult.Yes) return;
            }

            int status = NtSetTimerResolution(5000, true, out uint actualUnits);
            if (status != STATUS_SUCCESS)
            {
                MessageBox.Show($"No se pudo establecer la resolución mínima. Código: 0x{status:X8}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SetStatus(" Error al establecer resolución máxima. ", true);
                return;
            }
            _settings.LastResolutionMode = "Maximum";
            _settings.Save();
            RefreshResolutionLabels();
            UpdateModeIndicator();
            SetStatus($" Resolución establecida a {UnitsToMilliseconds(actualUnits):F3} ms (mínimo). ");
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            int status = NtSetTimerResolution(0, false, out _);
            if (status != STATUS_SUCCESS)
            {
                MessageBox.Show($"No se pudo restaurar. Código: 0x{status:X8}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SetStatus(" Error al restaurar. ", true);
                return;
            }
            _settings.LastResolutionMode = "Default";
            _settings.Save();
            RefreshResolutionLabels();
            UpdateModeIndicator();
            if (_lastKnownCurrentMs < 2.0)
                SetStatus(" Otro proceso mantiene alta resolución; no cambiará hasta que lo cierres. ", true);
            else
                SetStatus(" Resolución restaurada al valor por defecto. ");
        }

        private void btnApplyCustom_Click(object sender, EventArgs e)
        {
            int idx = cboCustom.SelectedIndex;
            if (idx < 0 || idx >= CustomResolutionMs.Length) return;
            double ms = CustomResolutionMs[idx];
            uint units = MillisecondsToUnits(ms);
            int status = NtSetTimerResolution(units, true, out uint actualUnits);
            if (status != STATUS_SUCCESS)
            {
                SetStatus($" No se pudo aplicar {ms} ms. ", true);
                return;
            }
            RefreshResolutionLabels();
            UpdateModeIndicator();
            SetStatus($" Resolución aplicada: {UnitsToMilliseconds(actualUnits):F3} ms. ");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshResolutionLabels();
            UpdateModeIndicator();
            SetStatus(" Valores actualizados. ");
        }

        private void btnMeasureSleep_Click(object sender, EventArgs e)
        {
            const int n = 200;
            var times = new double[n];
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < n; i++)
            {
                sw.Restart();
                Thread.Sleep(1);
                sw.Stop();
                times[i] = sw.Elapsed.TotalMilliseconds;
            }
            double min = times.Min(), max = times.Max(), avg = times.Average();
            MessageBox.Show(
                $"Sleep(1) medido {n} veces:\n\nMín: {min:F2} ms\nMedia: {avg:F2} ms\nMáx: {max:F2} ms\n\nResolución actual: ~{_lastKnownCurrentMs:F2} ms.",
                "Medir Sleep(1)",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            using var f = new HelpForm(_settings.DarkTheme);
            f.ShowDialog(this);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using var f = new AboutForm(_settings.DarkTheme);
            f.ShowDialog(this);
        }

        private void chkStartWithWindows_CheckedChanged(object? sender, EventArgs e)
        {
            _settings.StartWithWindows = chkStartWithWindows.Checked;
            _settings.Save();
            StartupHelper.SetStartWithWindows(_settings.StartWithWindows);
        }

        private void chkApplyMaxAtStartup_CheckedChanged(object? sender, EventArgs e)
        {
            _settings.ApplyMaximumAtStartup = chkApplyMaxAtStartup.Checked;
            _settings.Save();
        }

        private void chkDarkTheme_CheckedChanged(object? sender, EventArgs e)
        {
            _settings.DarkTheme = chkDarkTheme.Checked;
            _settings.Save();
            ApplyDarkTheme(_settings.DarkTheme);
        }

        private void ApplyDarkTheme(bool dark)
        {
            if (dark)
            {
                BackColor = Color.FromArgb(45, 45, 48);
                ForeColor = Color.White;
                grpResolution.BackColor = Color.FromArgb(45, 45, 48);
                grpResolution.ForeColor = Color.White;
                lblMode.BackColor = Color.FromArgb(60, 60, 60);
                lblMode.ForeColor = Color.White;
            }
            else
            {
                BackColor = SystemColors.Control;
                ForeColor = SystemColors.ControlText;
                grpResolution.BackColor = SystemColors.Control;
                grpResolution.ForeColor = SystemColors.ControlText;
                UpdateModeIndicator();
            }
        }

        private static bool IsOnBattery()
        {
            try
            {
                return SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline;
            }
            catch { return false; }
        }

        private void Form1_Resize(object? sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            notifyIcon.Visible = false;
            refreshTimer.Stop();
            NtSetTimerResolution(0, false, out _);
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.M) { btnMaximum.PerformClick(); e.Handled = true; }
            else if (e.Control && e.KeyCode == Keys.D) { btnDefault.PerformClick(); e.Handled = true; }
            else if (e.KeyCode == Keys.Escape) { btnClose.PerformClick(); e.Handled = true; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            refreshTimer.Stop();
            NtSetTimerResolution(0, false, out _);
            Application.Exit();
        }
    }
}
