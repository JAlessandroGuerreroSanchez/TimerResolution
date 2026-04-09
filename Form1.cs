using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace TimerResolutionApp
{
    public partial class Form1 : Form
    {
        private static readonly Font UiFont = CreateUiFont(10f, FontStyle.Regular);
        private static readonly Font UiSemiboldFont = CreateUiFont(10f, FontStyle.Bold);
        private static readonly Font UiTitleFont = CreateUiFont(22f, FontStyle.Bold);

        private AppSettings _settings = null!;
        private Icon? _trayIconNormal;
        private Icon? _trayIconHigh;
        private double _lastKnownCurrentMs = 15.625;
        private double _selectedCustomMs = 0.5;
        private bool _highResActive;
        private string _modeStatusText = "Intervalo habitual (~15,6 ms)";
        private Color _modeStatusBack = Color.FromArgb(34, 34, 40);
        private Color _modeStatusFore = Color.FromArgb(212, 212, 220);
        private float _animPhase;
        private double _fadeAlpha;
        private int _animFrame;
        private bool _compositionChrome;
        private bool _panelBodyAmbientHooked;

        /// <summary>Consulta UI y animación solo cuando la ventana está visible y no minimizada.</summary>
        private bool IsWindowInteractive =>
            Visible && WindowState != FormWindowState.Minimized;

        private const int RefreshIntervalForegroundMs = 2000;
        private const int RefreshIntervalBackgroundMs = 8000;
        private const int AnimIntervalMs = 100;

        private readonly System.Windows.Forms.Timer _animTimer = new() { Interval = AnimIntervalMs };
        private readonly System.Windows.Forms.Timer _fadeTimer = new() { Interval = 16 };

        public Form1()
        {
            InitializeComponent();
            BuildMetricsLayout();
            grpResolution.Paint += grpResolution_Paint;
            pnlModeStatus.Paint += pnlModeStatus_Paint;
            LoadAppIcon();
            _settings = AppSettings.Load();
            ApplyPersistedCustomResolution();
            ApplySettingsToUI();
            StartupHelper.SetStartWithWindows(_settings.StartWithWindows);
            if (_settings.ApplyMaximumAtStartup)
                ApplyMaximumQuiet();
            SetupToolTips();
            ApplyModernStyle();
            ApplyAccessibility();
            ApplyDarkStudioStyle();
            ApplyTypography();
            lblActions.Font = CreateUiFont(9.5f, FontStyle.Bold);
            lblFooterOpts.Font = CreateUiFont(9.5f, FontStyle.Bold);
            ApplySquareUi();
            RefreshResolutionLabels();
            UpdateModeIndicator();
            ApplyWindowTitleVersion();
            refreshTimer.Start();
            try
            {
                notifyIcon.Icon = _trayIconNormal ?? Icon;
            }
            catch { }
            Deactivate += (_, _) => HideCustomResolutionMenu();

            _animTimer.Tick += AnimTimer_Tick;
            _fadeTimer.Tick += FadeTimer_Tick;
            VisibleChanged += (_, _) => SyncUiResourceTimers();
            Opacity = 0;
            Load += Form1_Load;
            Shown += Form1_Shown;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            _compositionChrome = DwmBackdrop.TryEnable(this);
            if (_compositionChrome && !_panelBodyAmbientHooked)
            {
                ConfigureOpaquePaintedPanel(panelBody);
                panelBody.Paint += PanelBody_AmbientPaint;
                _panelBodyAmbientHooked = true;
            }

            ApplyDarkStudioStyle();
            UpdateModeIndicator();
            if (_compositionChrome)
            {
                ConfigureCheckBox(chkStartWithWindows);
                ConfigureCheckBox(chkApplyMaxAtStartup);
                panelTop.Invalidate(false);
                panelBody.Invalidate(false);
                grpResolution.Invalidate(false);
            }
        }

        /// <summary>
        /// Panel opaco con pintura propia y doble búfer. Evita el parpadeo típico de SupportsTransparentBackColor + DWM.
        /// </summary>
        private static void ConfigureOpaquePaintedPanel(Panel panel)
        {
            const ControlStyles styles = ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw;
            typeof(Control).InvokeMember(
                "SetStyle",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                null,
                panel,
                new object[] { styles, true });
            typeof(Control).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                panel,
                new object[] { true });
        }

        private void PanelBody_AmbientPaint(object? sender, PaintEventArgs e)
        {
            if (!_compositionChrome || sender is not Panel panel)
                return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = panel.ClientRectangle;

            using (var baseTint = new SolidBrush(Color.FromArgb(255, 10, 11, 20)))
                g.FillRectangle(baseTint, r);

            // Halos fijos (sin animación): no hace falta Invalidate periódico del cuerpo.
            DrawAmbientBlob(g, r.Width - 40f, -20f, 220f, 200f, Color.FromArgb(51, 230, 0, 126));
            DrawAmbientBlob(g, 80f, r.Height - 80f, 200f, 170f, Color.FromArgb(45, 40, 80, 240));
        }

        private static void DrawAmbientBlob(Graphics g, float cx, float cy, float rx, float ry, Color core)
        {
            for (int i = 6; i >= 1; i--)
            {
                float t = i / 6f;
                int a = (int)(core.A * t * t);
                if (a < 1) continue;
                using var b = new SolidBrush(Color.FromArgb(a, core.R, core.G, core.B));
                float w = rx * t * 2f;
                float h = ry * t * 2f;
                g.FillEllipse(b, cx - w / 2f, cy - h / 2f, w, h);
            }
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            _fadeAlpha = 0;
            _fadeTimer.Start();
            SyncUiResourceTimers();
        }

        /// <summary>Ajusta intervalos de temporizadores y animación según ventana visible o solo bandeja.</summary>
        private void SyncUiResourceTimers()
        {
            refreshTimer.Interval = IsWindowInteractive
                ? RefreshIntervalForegroundMs
                : RefreshIntervalBackgroundMs;
            ApplyAnimTimerState();
        }

        private bool ShouldRunAnimationTimer() =>
            IsWindowInteractive && (_highResActive || pnlResolutionMenu.Visible);

        private void ApplyAnimTimerState()
        {
            if (ShouldRunAnimationTimer())
            {
                if (!_animTimer.Enabled)
                    _animTimer.Start();
            }
            else
            {
                if (_animTimer.Enabled)
                    _animTimer.Stop();
                PremiumButton.AmbientPulse = 1f;
            }
        }

        private void FadeTimer_Tick(object? sender, EventArgs e)
        {
            _fadeAlpha += 0.11;
            if (_fadeAlpha >= 1.0)
            {
                Opacity = 1;
                _fadeTimer.Stop();
            }
            else
                Opacity = _fadeAlpha;
        }

        private void AnimTimer_Tick(object? sender, EventArgs e)
        {
            if (!IsWindowInteractive)
                return;

            _animPhase += 0.16f;
            if (_animPhase > MathF.PI * 2f)
                _animPhase -= MathF.PI * 2f;

            float s = MathF.Sin(_animPhase);
            PremiumButton.AmbientPulse = 0.76f + 0.24f * s;

            // Cabecera y cuerpo: pintura estática; no invalidar aquí (evita parpadeo masivo).

            if (_highResActive)
            {
                pnlModeStatus.Invalidate(false);
                if ((_animFrame % 2) == 0)
                    grpResolution.Invalidate(false);
            }

            if (pnlResolutionMenu.Visible)
                pnlResolutionMenu.Invalidate(false);

            _animFrame++;
            if ((_animFrame & 3) == 0)
                InvalidatePremiumButtons();
        }

        private void InvalidatePremiumButtons()
        {
            btnMaximum.Invalidate(false);
            btnDefault.Invalidate(false);
            btnResolution.Invalidate(false);
            btnApplyCustom.Invalidate(false);
            btnRefresh.Invalidate(false);
            btnClose.Invalidate(false);
            btnMeasureSleep.Invalidate(false);
            btnHelp.Invalidate(false);
            btnAbout.Invalidate(false);
        }

        private void LoadAppIcon()
        {
            try
            {
                using var stream = typeof(Form1).Assembly.GetManifestResourceStream("TimerResolutionApp.app.ico");
                if (stream == null) return;
                Icon = new Icon(stream);
                _trayIconNormal?.Dispose();
                _trayIconHigh?.Dispose();
                _trayIconNormal = TrayIconHelper.CreateTrayIcon(Icon, false);
                _trayIconHigh = TrayIconHelper.CreateTrayIcon(Icon, true);
            }
            catch { }
        }

        private void ApplySettingsToUI()
        {
            chkStartWithWindows.Checked = _settings.StartWithWindows;
            chkApplyMaxAtStartup.Checked = _settings.ApplyMaximumAtStartup;
            chkWarnOnExitIfHighRes.Checked = _settings.WarnOnExitIfHighRes;
            ApplyDarkStudioStyle();
        }

        private void SetupToolTips()
        {
            toolTip.SetToolTip(btnMaximum, "Establece la resolución al mínimo (0,5 ms). Ctrl+M");
            toolTip.SetToolTip(btnDefault, "Restaura el intervalo habitual del temporizador de Windows (~15,6 ms). Ctrl+D");
            toolTip.SetToolTip(btnClose, "Restaura la resolución por defecto y cierra la app. Escape");
            toolTip.SetToolTip(btnResolution, "Selecciona resolución personalizada");
            toolTip.SetToolTip(btnApplyCustom, "Aplica la resolución personalizada seleccionada.");
            toolTip.SetToolTip(btnRefresh, "Actualizar valores ahora.");
            toolTip.SetToolTip(btnMeasureSleep, "Mide la duración real de Sleep(1) para ver el efecto.");
            toolTip.SetToolTip(chkApplyMaxAtStartup, "Al abrir la app se aplica Maximum automáticamente.");
            toolTip.SetToolTip(chkWarnOnExitIfHighRes,
                "Si está activa la alta precisión, pregunta antes de cerrar la ventana (la X). Al salir se restaura el temporizador.");
            toolTip.SetToolTip(lblShortcutHints, "Atajos de teclado principales.");
            toolTip.SetToolTip(lnkBrandTop, "Abrir sitio web de KALUR STUDIO");
            toolTip.SetToolTip(lnkStudioFooter, "Abrir https://kalur.me/");
            toolTip.SetToolTip(pnlModeStatus,
                "Estado del temporizador: azul = alta precisión (~0,5 ms). Gris = intervalo habitual de Windows (~15,6 ms).");
        }

        private void SetStatus(string message, bool isError = false)
        {
            lblStatus.ForeColor = isError ? Color.FromArgb(248, 113, 113) : Color.FromArgb(161, 161, 170);
            lblStatus.Text = message;
        }

        private void RefreshResolutionLabels()
        {
            int status = TimerResolutionNative.NtQueryTimerResolution(
                out uint minUnits,
                out uint maxUnits,
                out uint currentUnits);

            if (status != TimerResolutionNative.StatusSuccess)
            {
                lblMin.Text = "N/A";
                lblMax.Text = "N/A";
                lblCurrent.Text = "Error al consultar";
                return;
            }

            double minMs = TimerMath.UnitsToMilliseconds(minUnits);
            double maxMs = TimerMath.UnitsToMilliseconds(maxUnits);
            double curMs = TimerMath.UnitsToMilliseconds(currentUnits);
            _lastKnownCurrentMs = curMs;

            lblMin.Text = $"{minMs:F3} ms ({minMs * 1000:F0} µs)";
            lblMax.Text = $"{maxMs:F3} ms ({maxMs * 1000:F0} µs)";
            lblCurrent.Text = $"{curMs:F3} ms ({(curMs * 1000):F0} µs)";
        }

        private void UpdateModeIndicator()
        {
            bool isLow = _lastKnownCurrentMs < 2.0;
            string newText = isLow
                ? "Alta precisión (~0,5 ms)"
                : "Intervalo habitual (~15,6 ms)";
            Color newBack = isLow
                ? (_compositionChrome ? Color.FromArgb(236, 14, 26, 48) : Color.FromArgb(24, 38, 54))
                : (_compositionChrome ? Color.FromArgb(230, 26, 28, 40) : Color.FromArgb(34, 34, 40));
            Color newFore = isLow ? Color.FromArgb(232, 236, 254, 255) : Color.FromArgb(212, 212, 220);

            bool wasLow = _highResActive;
            bool changed = wasLow != isLow
                || newText != _modeStatusText
                || newBack != _modeStatusBack
                || newFore != _modeStatusFore;

            _highResActive = isLow;
            _modeStatusText = newText;
            _modeStatusBack = newBack;
            _modeStatusFore = newFore;

            if (changed && IsWindowInteractive)
                pnlModeStatus.Invalidate();

            UpdateTrayTooltip();
            ApplyAnimTimerState();
        }

        private void UpdateTrayTooltip()
        {
            try
            {
                string state = _highResActive ? "Alta precisión" : "Habitual";
                string tip = $"Timer Resolution — {state} · actual ~{_lastKnownCurrentMs:F2} ms";
                const int maxLen = 127;
                if (tip.Length > maxLen)
                    tip = tip[..(maxLen - 1)] + "…";
                notifyIcon.Text = tip;
                if (_trayIconNormal != null && _trayIconHigh != null)
                    notifyIcon.Icon = _highResActive ? _trayIconHigh : _trayIconNormal;
                else if (Icon != null)
                    notifyIcon.Icon = Icon;
            }
            catch { /* NotifyIcon no inicializado */ }
        }

        private static string FormatMsForButton(double ms)
        {
            if (Math.Abs(ms - 0.5) < 0.02) return "0,5 ms";
            if (Math.Abs(ms - 1) < 0.02) return "1 ms";
            if (Math.Abs(ms - 2) < 0.02) return "2 ms";
            if (Math.Abs(ms - 5) < 0.02) return "5 ms";
            if (Math.Abs(ms - 15.625) < 0.02) return "15,625 ms";
            string s = ms.ToString("F3", CultureInfo.InvariantCulture);
            return s.Replace('.', ',') + " ms";
        }

        private void ApplyPersistedCustomResolution()
        {
            double ms = _settings.LastCustomResolutionMs;
            bool invalid = ms < AppSettings.MinCustomResolutionMs || ms > AppSettings.MaxCustomResolutionMs
                || double.IsNaN(ms) || double.IsInfinity(ms);
            if (invalid)
                ms = AppSettings.MinCustomResolutionMs;
            _selectedCustomMs = ms;
            _settings.LastCustomResolutionMs = ms;
            btnResolution.Text = FormatMsForButton(ms);
            if (invalid)
                _settings.Save();
        }

        private void ApplyAccessibility()
        {
            grpResolution.AccessibleName = "Valores del temporizador";
            grpResolution.AccessibleDescription =
                "Resolución mínima, máxima y actual del temporizador del sistema en milisegundos.";
            pnlModeStatus.AccessibleName = "Estado de precisión";
            pnlModeStatus.AccessibleDescription =
                "Indica si el temporizador está en alta precisión o en el intervalo habitual de Windows.";
            btnMaximum.AccessibleName = "Máximo";
            btnMaximum.AccessibleDescription =
                "Establece la resolución al mínimo del sistema, unos 0,5 milisegundos. Atajo: Control+M.";
            btnDefault.AccessibleName = "Por defecto";
            btnDefault.AccessibleDescription =
                "Restaura el intervalo habitual del temporizador, unos 15,6 milisegundos. Atajo: Control+D.";
            btnClose.AccessibleName = "Salir";
            btnClose.AccessibleDescription =
                "Restaura la resolución por defecto y cierra la aplicación. Atajo: Escape.";
            btnResolution.AccessibleName = "Resolución personalizada";
            btnResolution.AccessibleDescription = "Abre el menú para elegir otra resolución en milisegundos.";
            btnApplyCustom.AccessibleName = "Aplicar";
            btnApplyCustom.AccessibleDescription = "Aplica la resolución personalizada seleccionada en el menú.";
            btnRefresh.AccessibleName = "Actualizar valores";
            btnRefresh.AccessibleDescription = "Vuelve a leer del sistema los valores mostrados.";
            btnMeasureSleep.AccessibleName = "Medir Sleep de un milisegundo";
            btnMeasureSleep.AccessibleDescription =
                "Ejecuta varias veces la espera de un milisegundo y muestra mínimo, media y máximo.";
            btnHelp.AccessibleName = "Ayuda";
            btnHelp.AccessibleDescription = "Abre la ventana de ayuda.";
            btnAbout.AccessibleName = "Acerca de";
            btnAbout.AccessibleDescription = "Muestra la versión y datos del autor.";
            lblActions.AccessibleName = "Acciones";
            lblFooterOpts.AccessibleName = "Opciones";
            chkStartWithWindows.AccessibleName = "Inicio con Windows";
            chkApplyMaxAtStartup.AccessibleName = "Máximo al iniciar";
            chkWarnOnExitIfHighRes.AccessibleName = "Confirmar al cerrar si hay alta precisión";
            chkWarnOnExitIfHighRes.AccessibleDescription =
                "Muestra un aviso al cerrar la ventana mientras el temporizador sigue en alta precisión.";
            lblShortcutHints.AccessibleName = "Atajos de teclado";
            lblShortcutHints.AccessibleDescription =
                "Control M para máximo, Control D para por defecto, Escape para salir.";
        }

        private void CopyDiagnosticsToClipboard()
        {
            try
            {
                var sb = new StringBuilder();
                var v = Assembly.GetExecutingAssembly().GetName().Version;
                sb.AppendLine("Timer Resolution — diagnóstico");
                sb.AppendLine($"Versión: {v}");
                sb.AppendLine($"Windows build: {DwmBackdrop.GetWindowsBuild()}");
                sb.AppendLine($"OS (reportado): {Environment.OSVersion.VersionString}");
                sb.AppendLine($"Sistema 64 bits: {Environment.Is64BitOperatingSystem}");
                sb.AppendLine($"Proceso 64 bits: {Environment.Is64BitProcess}");
                sb.AppendLine();
                int st = TimerResolutionNative.NtQueryTimerResolution(out uint minU, out uint maxU, out uint curU);
                if (st == TimerResolutionNative.StatusSuccess)
                {
                    sb.AppendLine("TimerResolutionNative.NtQueryTimerResolution: OK");
                    sb.AppendLine($"  Mínima: {TimerMath.UnitsToMilliseconds(minU):F3} ms");
                    sb.AppendLine($"  Máxima: {TimerMath.UnitsToMilliseconds(maxU):F3} ms");
                    sb.AppendLine($"  Actual: {TimerMath.UnitsToMilliseconds(curU):F3} ms");
                }
                else
                    sb.AppendLine($"TimerResolutionNative.NtQueryTimerResolution: 0x{st:X8}");

                sb.AppendLine($"Alta precisión (UI): {_highResActive}");
                sb.AppendLine($"Personalizada elegida: {_selectedCustomMs:F3} ms");
                sb.AppendLine($"Último modo guardado: {_settings.LastResolutionMode}");
                Clipboard.SetText(sb.ToString(), TextDataFormat.UnicodeText);
                SetStatus(" Información de diagnóstico copiada al portapapeles. ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo copiar al portapapeles: {ex.Message}",
                    "Timer Resolution",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void refreshTimer_Tick(object? sender, EventArgs e)
        {
            if (IsWindowInteractive && grpResolution.IsHandleCreated)
            {
                grpResolution.SuspendLayout();
                try
                {
                    RefreshResolutionLabels();
                    UpdateModeIndicator();
                }
                finally
                {
                    grpResolution.ResumeLayout(performLayout: false);
                }
            }
            else
            {
                RefreshResolutionLabels();
                UpdateModeIndicator();
            }
        }

        private void ApplyMaximumQuiet()
        {
            TimerResolutionNative.NtSetTimerResolution(5000, true, out _);
            RefreshResolutionLabels();
            UpdateModeIndicator();
        }

        private void btnMaximum_Click(object sender, EventArgs e)
        {
            HideCustomResolutionMenu();
            if (IsOnBattery())
            {
                var r = MessageBox.Show(
                    "Estás usando batería. La alta resolución puede aumentar el consumo. ¿Continuar?",
                    "Aviso",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (r != DialogResult.Yes) return;
            }

            int status = TimerResolutionNative.NtSetTimerResolution(5000, true, out uint actualUnits);
            if (status != TimerResolutionNative.StatusSuccess)
            {
                MessageBox.Show($"No se pudo establecer la resolución mínima. Código: 0x{status:X8}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SetStatus(" Error al establecer resolución máxima. ", true);
                return;
            }
            _settings.LastResolutionMode = "Maximum";
            _settings.Save();
            RefreshResolutionLabels();
            UpdateModeIndicator();
            SetStatus($" Resolución establecida a {TimerMath.UnitsToMilliseconds(actualUnits):F3} ms (mínimo). ");
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            HideCustomResolutionMenu();
            int status = TimerResolutionNative.NtSetTimerResolution(0, false, out _);
            if (status != TimerResolutionNative.StatusSuccess)
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
            HideCustomResolutionMenu();
            double ms = _selectedCustomMs;
            if (!TimerMath.TryMillisecondsToUnits(ms, out uint units))
            {
                MessageBox.Show(
                    $"El valor de resolución no es válido ({ms} ms). Elige otra opción del menú.",
                    "Timer Resolution",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int status = TimerResolutionNative.NtSetTimerResolution(units, true, out uint actualUnits);
            if (status != TimerResolutionNative.StatusSuccess)
            {
                SetStatus($" No se pudo aplicar {ms} ms. ", true);
                return;
            }
            _settings.LastCustomResolutionMs = ms;
            _settings.LastResolutionMode = "Custom";
            _settings.Save();
            RefreshResolutionLabels();
            UpdateModeIndicator();
            SetStatus($" Resolución aplicada: {TimerMath.UnitsToMilliseconds(actualUnits):F3} ms. ");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            HideCustomResolutionMenu();
            RefreshResolutionLabels();
            UpdateModeIndicator();
            SetStatus(" Valores actualizados. ");
        }

        private void btnMeasureSleep_Click(object sender, EventArgs e)
        {
            HideCustomResolutionMenu();
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
            HideCustomResolutionMenu();
            using var f = new HelpForm(_settings.DarkTheme);
            f.ShowDialog(this);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            HideCustomResolutionMenu();
            using var f = new AboutForm(_settings.DarkTheme);
            f.ShowDialog(this);
        }

        private void chkStartWithWindows_CheckedChanged(object? sender, EventArgs e)
        {
            HideCustomResolutionMenu();
            _settings.StartWithWindows = chkStartWithWindows.Checked;
            _settings.Save();
            StartupHelper.SetStartWithWindows(_settings.StartWithWindows);
        }

        private void chkApplyMaxAtStartup_CheckedChanged(object? sender, EventArgs e)
        {
            HideCustomResolutionMenu();
            _settings.ApplyMaximumAtStartup = chkApplyMaxAtStartup.Checked;
            _settings.Save();
        }

        private void chkWarnOnExitIfHighRes_CheckedChanged(object? sender, EventArgs e)
        {
            HideCustomResolutionMenu();
            _settings.WarnOnExitIfHighRes = chkWarnOnExitIfHighRes.Checked;
            _settings.Save();
        }

        private void ApplyDarkStudioStyle()
        {
            var bg = Color.FromArgb(11, 12, 18);
            var surface = Color.FromArgb(22, 24, 32);
            var text = Color.FromArgb(244, 244, 245);
            var muted = Color.FromArgb(161, 161, 170);

            ForeColor = text;

            if (_compositionChrome)
            {
                BackColor = Color.Black;
                panelTop.BackColor = Color.FromArgb(255, 14, 16, 24);
                panelBody.BackColor = Color.FromArgb(255, 10, 11, 20);
                panelFooter.BackColor = Color.FromArgb(255, 8, 9, 16);
                pnlModeStatus.BackColor = Color.FromArgb(255, 18, 20, 28);
                grpResolution.BackColor = Color.FromArgb(40, 24, 26, 42);
            }
            else
            {
                BackColor = bg;
                panelBody.BackColor = bg;
                panelFooter.BackColor = bg;
                pnlModeStatus.BackColor = bg;
                grpResolution.BackColor = surface;
                panelTop.BackColor = Color.FromArgb(14, 14, 18);
            }

            grpResolution.ForeColor = text;
            lblAppTitle.ForeColor = text;
            lblAppSubtitle.ForeColor = muted;
            lnkBrandTop.LinkColor = Color.FromArgb(186, 200, 255);
            lnkBrandTop.VisitedLinkColor = muted;
            lnkStudioFooter.LinkColor = Color.FromArgb(113, 113, 122);
            lnkStudioFooter.VisitedLinkColor = Color.FromArgb(113, 113, 122);
            btnResolution.BackColor = Color.FromArgb(28, 28, 34);
            btnResolution.ForeColor = text;
            chkStartWithWindows.ForeColor = muted;
            chkApplyMaxAtStartup.ForeColor = muted;
            chkWarnOnExitIfHighRes.ForeColor = muted;
            lblShortcutHints.ForeColor = Color.FromArgb(130, 140, 160);
            lblMin.ForeColor = text;
            lblMax.ForeColor = text;
            lblCurrent.ForeColor = Color.FromArgb(125, 211, 252);
            lblResolutionTitle.ForeColor = text;
            lblResolutionTitle.BackColor = Color.FromArgb(30, 34, 50);
            lblMinLabel.ForeColor = muted;
            lblMaxLabel.ForeColor = muted;
            lblCurrentLabel.ForeColor = muted;
            var sectionMuted = Color.FromArgb(148, 163, 184);
            lblActions.ForeColor = sectionMuted;
            lblFooterOpts.ForeColor = sectionMuted;

            trayMenu.BackColor = Color.FromArgb(26, 26, 32);
            trayMenu.ForeColor = text;
            trayMenu.RenderMode = ToolStripRenderMode.Professional;
            trayMenu.Renderer = new DarkToolStripRenderer();
            foreach (ToolStripItem item in trayMenu.Items)
            {
                if (item is not ToolStripSeparator)
                    item.ForeColor = text;
            }
        }

        private void ApplyModernStyle()
        {
            KeyPreview = true;
            DoubleBuffered = true;
            Font = UiFont;
            grpResolution.Font = UiSemiboldFont;
            lblAppTitle.Font = CreateUiFont(16f, FontStyle.Bold);
            lblAppSubtitle.Font = CreateUiFont(10.5f, FontStyle.Regular);
            lblCurrent.Font = CreateUiFont(12f, FontStyle.Bold);

            StyleButton(btnMaximum, true);
            StyleButton(btnDefault, false);
            StyleButton(btnResolution, false);
            StyleButton(btnApplyCustom, false);
            StyleButton(btnRefresh, false);
            StyleButton(btnClose, false);
            StyleButton(btnMeasureSleep, false);
            StyleButton(btnHelp, false);
            StyleButton(btnAbout, false);
            btnMaximum.Text = "Máximo";
            btnDefault.Text = "Por defecto";
            btnClose.Text = "Salir";
            btnRefresh.Font = CreateUiFont(13f, FontStyle.Regular);
            btnHelp.Font = CreateUiFont(12f, FontStyle.Bold);
            StyleMenuButton(btnRes05);
            StyleMenuButton(btnRes1);
            StyleMenuButton(btnRes2);
            StyleMenuButton(btnRes5);
            StyleMenuButton(btnRes15625);
            ConfigureCheckBox(chkStartWithWindows);
            ConfigureCheckBox(chkApplyMaxAtStartup);
            ConfigureCheckBox(chkWarnOnExitIfHighRes);

            lblActions.UseMnemonic = false;
            lblActions.UseCompatibleTextRendering = false;
            lblFooterOpts.UseMnemonic = false;
            lblFooterOpts.UseCompatibleTextRendering = false;
            EnableDoubleBuffer(grpResolution);
            EnableDoubleBuffer(pnlModeStatus);
            ConfigureOpaquePaintedPanel(panelTop);
            EnableDoubleBuffer(panelFooter);
            EnableDoubleBuffer(pnlResolutionMenu);

            btnMaximum.Cursor = Cursors.Hand;
            btnDefault.Cursor = Cursors.Hand;
            btnResolution.Cursor = Cursors.Hand;
            btnApplyCustom.Cursor = Cursors.Hand;
            btnRefresh.Cursor = Cursors.Hand;
            btnClose.Cursor = Cursors.Hand;
            btnMeasureSleep.Cursor = Cursors.Hand;
            btnHelp.Cursor = Cursors.Hand;
            btnAbout.Cursor = Cursors.Hand;
            lnkBrandTop.Cursor = Cursors.Hand;
            lnkStudioFooter.Cursor = Cursors.Hand;
            PositionResolutionMenu();
        }

        private void PositionResolutionMenu()
        {
            pnlResolutionMenu.Location = new Point(btnResolution.Left, btnResolution.Bottom + 2);
        }

        private static void StyleButton(Button button, bool primary)
        {
            button.ForeColor = Color.FromArgb(243, 246, 255);
            button.Font = UiSemiboldFont;
            button.Padding = new Padding(0, 0, 0, 1);
            if (button is PremiumButton premium)
                premium.IsPrimary = primary;
        }

        private void ConfigureCheckBox(CheckBox checkBox)
        {
            checkBox.FlatStyle = FlatStyle.Flat;
            checkBox.Font = UiFont;
            checkBox.BackColor = panelFooter.BackColor;
            checkBox.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            checkBox.FlatAppearance.CheckedBackColor = Color.FromArgb(37, 99, 235);
        }

        private static void StyleMenuButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 48);
            button.BackColor = Color.FromArgb(28, 28, 34);
            button.ForeColor = Color.FromArgb(244, 244, 245);
            button.Font = UiFont;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(14, 0, 0, 0);
            button.Cursor = Cursors.Hand;
        }

        private void ApplySquareUi()
        {
            foreach (var c in new Control[]
                     {
                         btnMaximum, btnDefault, btnResolution, btnApplyCustom, btnRefresh, btnClose,
                         btnMeasureSleep, btnHelp, btnAbout, grpResolution, pnlModeStatus
                     })
            {
                ClearRoundedRegion(c);
            }
        }

        private static void ClearRoundedRegion(Control? c)
        {
            if (c == null) return;
            var old = c.Region;
            c.Region = null;
            old?.Dispose();
        }

        private void ApplyTypography()
        {
            foreach (Control control in Controls)
            {
                ApplyTypographyRecursive(control);
            }
        }

        private static void ApplyTypographyRecursive(Control control)
        {
            if (control.Name is "lblActions" or "lblFooterOpts" or "lblShortcutHints")
            {
                foreach (Control child in control.Controls)
                    ApplyTypographyRecursive(child);
                return;
            }

            if (control is Button or CheckBox or LinkLabel or GroupBox or Label or ComboBox)
            {
                if (control.Font.Size < 9.5f) control.Font = UiFont;
            }

            foreach (Control child in control.Controls)
                ApplyTypographyRecursive(child);
        }

        private static Font CreateUiFont(float size, FontStyle style)
        {
            try
            {
                return new Font("Segoe UI Variable Text", size, style, GraphicsUnit.Point);
            }
            catch
            {
                return new Font("Segoe UI", size, style, GraphicsUnit.Point);
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
            // Evita layouts recortados por tamaños extremos o DPI altos.
            if (Width < MinimumSize.Width) Width = MinimumSize.Width;
            if (Height < MinimumSize.Height) Height = MinimumSize.Height;

            if (WindowState != FormWindowState.Minimized)
            {
                ApplySquareUi();
                PositionResolutionMenu();
            }

            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }

            SyncUiResourceTimers();
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing
                && _settings.WarnOnExitIfHighRes
                && _lastKnownCurrentMs < 2.0)
            {
                var r = MessageBox.Show(
                    "El temporizador sigue en alta precisión (~0,5 ms). ¿Cerrar la aplicación?\n\n"
                    + "Al salir se restaurará el intervalo habitual de Windows.",
                    "Timer Resolution",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (r != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            notifyIcon.Visible = false;
            refreshTimer.Stop();
            _animTimer.Stop();
            _fadeTimer.Stop();
            _animTimer.Dispose();
            _fadeTimer.Dispose();
            _trayIconNormal?.Dispose();
            _trayIconHigh?.Dispose();
            _trayIconNormal = null;
            _trayIconHigh = null;
            TimerResolutionNative.NtSetTimerResolution(0, false, out _);
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.M) { btnMaximum.PerformClick(); e.Handled = true; }
            else if (e.Control && e.KeyCode == Keys.D) { btnDefault.PerformClick(); e.Handled = true; }
            else if (e.KeyCode == Keys.Escape) { btnClose.PerformClick(); e.Handled = true; }
        }

        private void panelTop_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var r = panelTop.ClientRectangle;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // Valor fijo: la cabecera no se invalida en bucle (evita parpadeo).
            const float w = 0.52f;

            if (_compositionChrome)
            {
                using (var topGradient = new LinearGradientBrush(
                           r,
                           Color.FromArgb(210, 22, 24, 40),
                           Color.FromArgb(120, 6, 8, 18),
                           LinearGradientMode.Vertical))
                    g.FillRectangle(topGradient, r);

                using (var magSheen = new LinearGradientBrush(
                           new Rectangle(r.Width - 200, 0, 200, r.Height),
                           Color.FromArgb(55, 200, 0, 120),
                           Color.FromArgb(0, 200, 0, 120),
                           LinearGradientMode.Horizontal))
                    g.FillRectangle(magSheen, r.Width - 200, 0, 200, r.Height);
            }
            else
            {
                using (var topGradient = new LinearGradientBrush(
                           r,
                           Color.FromArgb(26, 28, 38),
                           Color.FromArgb(14, 15, 22),
                           LinearGradientMode.Vertical))
                    g.FillRectangle(topGradient, r);
            }

            int sheenA = (int)(26 + 22 * w);
            using var sheen = new SolidBrush(Color.FromArgb(sheenA, 56, 139, 253));
            g.FillRectangle(sheen, 0, 0, r.Width, 11);

            const int ledH = 3;
            int ledY = r.Height - ledH - 1;
            int barCore = (int)(175 + 65 * w);
            using var ledCore = new SolidBrush(Color.FromArgb(barCore, 96, 165, 250));
            g.FillRectangle(ledCore, 0, ledY, r.Width, ledH);
            using var ledSoft = new SolidBrush(Color.FromArgb((int)(40 + 30 * w), 59, 130, 246));
            g.FillRectangle(ledSoft, 0, ledY - 1, r.Width, 1);

            using var line = new Pen(Color.FromArgb(52, 72, 88, 110), 1f);
            g.DrawLine(line, 0, r.Height - 1, r.Width, r.Height - 1);
        }

        private void grpResolution_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            var rect = new Rectangle(0, 0, grpResolution.Width - 1, grpResolution.Height - 1);
            g.SetClip(rect);
            Color gTop = _compositionChrome
                ? Color.FromArgb(210, 36, 40, 62)
                : Color.FromArgb(34, 38, 52);
            Color gBot = _compositionChrome
                ? Color.FromArgb(200, 16, 18, 34)
                : Color.FromArgb(18, 20, 30);
            using (var bg = new LinearGradientBrush(rect, gTop, gBot, LinearGradientMode.Vertical))
                g.FillRectangle(bg, rect);

            int stripeA = _highResActive
                ? (int)(195 + 60 * (0.55f + 0.45f * MathF.Sin(_animPhase * 1.4f)))
                : 238;
            using (var stripe = new SolidBrush(Color.FromArgb(stripeA, 56, 139, 255)))
                g.FillRectangle(stripe, 0, 0, 4, grpResolution.Height);
            g.ResetClip();

            int edgeA = _highResActive ? (int)(90 + 70 * MathF.Sin(_animPhase * 1.1f)) : (_compositionChrome ? 88 : 95);
            using var edge = new Pen(Color.FromArgb(edgeA, 56, 189, 248), 1f);
            g.DrawRectangle(edge, rect);

            if (_compositionChrome && rect.Width > 6 && rect.Height > 6)
            {
                var inner = Rectangle.Inflate(rect, -1, -1);
                using var glass = new Pen(Color.FromArgb(38, 255, 255, 255), 1f);
                g.DrawRectangle(glass, inner);
            }
        }

        private void pnlModeStatus_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            int w = pnlModeStatus.Width;
            int h = pnlModeStatus.Height;
            if (w <= 1 || h <= 1) return;

            const int ledSlotLeft = 12;
            const int ledSlotW = 42;
            const int textPadRight = 16;
            var bounds = new Rectangle(0, 0, w - 1, h - 1);

            using (var bg = new SolidBrush(_modeStatusBack))
                g.FillRectangle(bg, 0, 0, w, h);

            g.SmoothingMode = SmoothingMode.None;
            if (_highResActive)
            {
                int pulse = (int)(55 + 45 * MathF.Sin(_animPhase * 1.25f));
                using var inner = new Pen(Color.FromArgb(pulse, 56, 189, 248), 1.5f);
                g.DrawRectangle(inner, bounds);
            }
            else
            {
                using var pen = new Pen(Color.FromArgb(58, 58, 68), 1f);
                g.DrawRectangle(pen, bounds);
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawModeStatusLed(g, new Rectangle(ledSlotLeft, 0, ledSlotW, h));

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            float textX = ledSlotLeft + ledSlotW + 14f;
            var textRect = new RectangleF(textX, 1f, w - textX - textPadRight, h - 2f);
            using var tb = new SolidBrush(_modeStatusFore);
            using var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.LineLimit
            };
            g.DrawString(_modeStatusText, UiSemiboldFont, tb, textRect, sf);
        }

        private void DrawModeStatusLed(Graphics g, Rectangle ledClip)
        {
            g.SetClip(ledClip);
            float cx = ledClip.Left + ledClip.Width / 2f;
            float cy = ledClip.Top + ledClip.Height / 2f;
            float maxR = Math.Min(
                Math.Min(cx - ledClip.Left - 2f, ledClip.Right - cx - 2f),
                Math.Min(cy - ledClip.Top - 2f, ledClip.Bottom - cy - 2f));

            if (_highResActive)
            {
                float breathe = 0.88f + 0.12f * MathF.Sin(_animPhase * 2.1f);
                for (int i = 4; i >= 1; i--)
                {
                    float r = (6f + i * 2.5f) * breathe;
                    if (r > maxR)
                        r = maxR;
                    int a = (int)((12 + i * 16) * (0.85f + 0.15f * breathe));
                    using var b = new SolidBrush(Color.FromArgb(a, 56, 139, 255));
                    g.FillEllipse(b, cx - r, cy - r, r * 2f, r * 2f);
                }

                float coreR = Math.Min(6.5f, maxR - 1f);
                if (coreR > 2f)
                {
                    using (var core = new SolidBrush(Color.FromArgb(255, 186, 230, 253)))
                        g.FillEllipse(core, cx - coreR, cy - coreR, coreR * 2f, coreR * 2f);
                    float hotR = Math.Min(2.6f, coreR * 0.4f);
                    using (var hot = new SolidBrush(Color.FromArgb(255, 255, 255)))
                        g.FillEllipse(hot, cx - hotR, cy - hotR - 0.5f, hotR * 2f, hotR * 2f);
                }
            }
            else
            {
                float rOff = Math.Min(5f, maxR - 1f);
                if (rOff > 2f)
                {
                    using var dim = new SolidBrush(Color.FromArgb(100, 55, 60, 70));
                    g.FillEllipse(dim, cx - rOff, cy - rOff, rOff * 2f, rOff * 2f);
                    using var ring = new Pen(Color.FromArgb(80, 70, 75, 85), 1f);
                    g.DrawEllipse(ring, cx - rOff, cy - rOff, rOff * 2f, rOff * 2f);
                }
            }

            g.ResetClip();
        }

        private void ApplyWindowTitleVersion()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            Text = v != null
                ? $"Timer Resolution  ·  v{v.Major}.{v.Minor}.{v.Build}"
                : "Timer Resolution";
        }

        private static void EnableDoubleBuffer(Control c)
        {
            typeof(Control).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                c,
                new object[] { true });
        }

        private void pnlResolutionMenu_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            var rect = new Rectangle(0, 0, pnlResolutionMenu.Width - 1, pnlResolutionMenu.Height - 1);
            using var fill = new SolidBrush(Color.FromArgb(236, 22, 22, 32));
            g.FillRectangle(fill, rect);
            int edgeA = (int)(100 + 60 * MathF.Sin(_animPhase * 1.6f));
            using var edge = new Pen(Color.FromArgb(edgeA, 56, 189, 248), 1f);
            g.DrawRectangle(edge, rect);
            int magEdge = (int)(55 + 35 * MathF.Sin(_animPhase * 1.9f));
            using var mag = new Pen(Color.FromArgb(magEdge, 230, 80, 160), 1f);
            g.DrawRectangle(mag, Rectangle.Inflate(rect, -1, -1));
        }

        private void BuildMetricsLayout()
        {
            var lblBg = Color.FromArgb(26, 28, 42);
            var tbl = new TableLayoutPanel
            {
                Name = "tblMetrics",
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                BackColor = lblBg,
                Padding = new Padding(20, 0, 20, 8)
            };
            EnableDoubleBuffer(tbl);
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            void Place(Control c, int col, int row)
            {
                tbl.Controls.Add(c, col, row);
                c.Dock = DockStyle.Fill;
            }

            Place(lblMinLabel, 0, 0);
            Place(lblMaxLabel, 1, 0);
            Place(lblCurrentLabel, 2, 0);
            Place(lblMin, 0, 1);
            Place(lblMax, 1, 1);
            Place(lblCurrent, 2, 1);

            foreach (var lab in new[] { lblMinLabel, lblMaxLabel, lblCurrentLabel })
            {
                lab.TextAlign = ContentAlignment.BottomLeft;
                lab.AutoSize = false;
                lab.Margin = new Padding(0, 4, 12, 4);
            }

            lblMin.TextAlign = ContentAlignment.TopLeft;
            lblMax.TextAlign = ContentAlignment.TopLeft;
            lblCurrent.TextAlign = ContentAlignment.TopLeft;
            lblMin.AutoSize = false;
            lblMax.AutoSize = false;
            lblCurrent.AutoSize = false;
            lblMin.Margin = new Padding(0, 0, 12, 0);
            lblMax.Margin = new Padding(0, 0, 12, 0);
            lblCurrent.Margin = new Padding(0, 0, 12, 0);

            foreach (var lab in new[] { lblMinLabel, lblMaxLabel, lblCurrentLabel, lblMin, lblMax, lblCurrent })
            {
                lab.BackColor = lblBg;
                lab.UseCompatibleTextRendering = false;
            }

            grpResolution.SuspendLayout();
            grpResolution.Controls.Remove(lblResolutionTitle);
            grpResolution.Controls.Remove(lblMinLabel);
            grpResolution.Controls.Remove(lblMaxLabel);
            grpResolution.Controls.Remove(lblCurrentLabel);
            grpResolution.Controls.Remove(lblMin);
            grpResolution.Controls.Remove(lblMax);
            grpResolution.Controls.Remove(lblCurrent);

            grpResolution.Controls.Add(tbl);
            grpResolution.Controls.Add(lblResolutionTitle);
            lblResolutionTitle.Dock = DockStyle.Top;
            lblResolutionTitle.AutoSize = false;
            lblResolutionTitle.Height = 36;
            lblResolutionTitle.Padding = new Padding(20, 10, 0, 6);
            lblResolutionTitle.TextAlign = ContentAlignment.BottomLeft;
            grpResolution.ResumeLayout(true);
        }

        private static void OpenStudioUrl() =>
            BrowserLaunch.OpenIfTrustedHttpOrHttps(AppConstants.SupportWebsiteUrl);

        private void lnkBrandTop_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e) => OpenStudioUrl();

        private void lnkStudioFooter_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e) => OpenStudioUrl();

        private void btnResolution_Click(object? sender, EventArgs e)
        {
            pnlResolutionMenu.Visible = !pnlResolutionMenu.Visible;
            pnlResolutionMenu.BringToFront();
            ApplyAnimTimerState();
        }

        private void customResolutionItem_Click(object? sender, EventArgs e)
        {
            if (sender is not Button item || item.Tag is not double value) return;
            _selectedCustomMs = value;
            btnResolution.Text = item.Text;
            _settings.LastCustomResolutionMs = value;
            _settings.Save();
            pnlResolutionMenu.Visible = false;
            ApplyAnimTimerState();
        }

        private void HideCustomResolutionMenu()
        {
            if (pnlResolutionMenu.Visible)
            {
                pnlResolutionMenu.Visible = false;
                ApplyAnimTimerState();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            refreshTimer.Stop();
            TimerResolutionNative.NtSetTimerResolution(0, false, out _);
            Application.Exit();
        }
    }
}
