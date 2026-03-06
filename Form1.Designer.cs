namespace TimerResolutionApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            grpResolution = new GroupBox();
            lblCurrent = new Label();
            lblCurrentLabel = new Label();
            lblMax = new Label();
            lblMaxLabel = new Label();
            lblMin = new Label();
            lblMinLabel = new Label();
            lblMode = new Label();
            btnMaximum = new Button();
            btnDefault = new Button();
            cboCustom = new ComboBox();
            btnApplyCustom = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            btnMeasureSleep = new Button();
            btnHelp = new Button();
            btnAbout = new Button();
            chkStartWithWindows = new CheckBox();
            chkApplyMaxAtStartup = new CheckBox();
            chkDarkTheme = new CheckBox();
            lblStatus = new Label();
            refreshTimer = new System.Windows.Forms.Timer(components);
            toolTip = new ToolTip(components);
            notifyIcon = new NotifyIcon(components);
            trayMenu = new ContextMenuStrip(components);
            grpResolution.SuspendLayout();
            trayMenu.SuspendLayout();
            SuspendLayout();
            //
            // grpResolution
            //
            grpResolution.Controls.Add(lblCurrent);
            grpResolution.Controls.Add(lblCurrentLabel);
            grpResolution.Controls.Add(lblMax);
            grpResolution.Controls.Add(lblMaxLabel);
            grpResolution.Controls.Add(lblMin);
            grpResolution.Controls.Add(lblMinLabel);
            grpResolution.Location = new Point(16, 16);
            grpResolution.Name = "grpResolution";
            grpResolution.Padding = new Padding(10, 8, 10, 10);
            grpResolution.Size = new Size(400, 100);
            grpResolution.TabIndex = 0;
            grpResolution.TabStop = false;
            grpResolution.Text = " Resolución del sistema (ms / µs) ";
            //
            // lblMinLabel, lblMin, lblMaxLabel, lblMax, lblCurrentLabel, lblCurrent
            //
            lblMinLabel.Location = new Point(18, 18);
            lblMinLabel.Size = new Size(55, 15);
            lblMinLabel.Text = "Mínima:";
            lblMinLabel.AutoSize = true;
            lblMin.Location = new Point(140, 18);
            lblMin.AutoSize = true;
            lblMin.Text = "—";
            lblMaxLabel.Location = new Point(18, 38);
            lblMaxLabel.Size = new Size(57, 15);
            lblMaxLabel.Text = "Máxima:";
            lblMaxLabel.AutoSize = true;
            lblMax.Location = new Point(140, 38);
            lblMax.AutoSize = true;
            lblMax.Text = "—";
            lblCurrentLabel.Location = new Point(18, 58);
            lblCurrentLabel.Size = new Size(46, 15);
            lblCurrentLabel.Text = "Actual:";
            lblCurrentLabel.AutoSize = true;
            lblCurrent.Location = new Point(140, 58);
            lblCurrent.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblCurrent.AutoSize = true;
            lblCurrent.Text = "—";
            //
            // lblMode
            //
            lblMode.BackColor = Color.FromArgb(240, 240, 240);
            lblMode.BorderStyle = BorderStyle.FixedSingle;
            lblMode.Location = new Point(16, 124);
            lblMode.Size = new Size(400, 24);
            lblMode.Text = " Modo: Por defecto ";
            lblMode.TextAlign = ContentAlignment.MiddleLeft;
            //
            // btnMaximum, btnDefault, cboCustom, btnApplyCustom, btnRefresh, btnClose
            //
            btnMaximum.Location = new Point(16, 156);
            btnMaximum.Size = new Size(88, 28);
            btnMaximum.Text = "Maximum";
            btnMaximum.Click += btnMaximum_Click;
            btnDefault.Location = new Point(110, 156);
            btnDefault.Size = new Size(88, 28);
            btnDefault.Text = "Default";
            btnDefault.Click += btnDefault_Click;
            cboCustom.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCustom.Location = new Point(204, 158);
            cboCustom.Size = new Size(100, 23);
            cboCustom.Items.AddRange(new object[] { "0,5 ms", "1 ms", "2 ms", "5 ms", "15,625 ms" });
            cboCustom.SelectedIndex = 0;
            btnApplyCustom.Location = new Point(308, 156);
            btnApplyCustom.Size = new Size(52, 28);
            btnApplyCustom.Text = "Aplicar";
            btnApplyCustom.Click += btnApplyCustom_Click;
            btnRefresh.Location = new Point(366, 156);
            btnRefresh.Size = new Size(50, 28);
            btnRefresh.Text = "↻";
            btnRefresh.Click += btnRefresh_Click;
            btnClose.Location = new Point(16, 192);
            btnClose.Size = new Size(88, 28);
            btnClose.Text = "Close";
            btnClose.Click += btnClose_Click;
            btnMeasureSleep.Location = new Point(110, 192);
            btnMeasureSleep.Size = new Size(120, 28);
            btnMeasureSleep.Text = "Medir Sleep(1)";
            btnMeasureSleep.Click += btnMeasureSleep_Click;
            btnHelp.Location = new Point(236, 192);
            btnHelp.Size = new Size(28, 28);
            btnHelp.Text = "?";
            btnHelp.Click += btnHelp_Click;
            btnAbout.Location = new Point(270, 192);
            btnAbout.Size = new Size(90, 28);
            btnAbout.Text = "Acerca de";
            btnAbout.Click += btnAbout_Click;
            //
            // chkStartWithWindows, chkApplyMaxAtStartup, chkDarkTheme
            //
            chkStartWithWindows.Location = new Point(16, 228);
            chkStartWithWindows.Size = new Size(180, 20);
            chkStartWithWindows.Text = "Inicio con Windows";
            chkStartWithWindows.CheckedChanged += chkStartWithWindows_CheckedChanged;
            chkApplyMaxAtStartup.Location = new Point(200, 228);
            chkApplyMaxAtStartup.Size = new Size(160, 20);
            chkApplyMaxAtStartup.Text = "Máximo al iniciar";
            chkApplyMaxAtStartup.CheckedChanged += chkApplyMaxAtStartup_CheckedChanged;
            chkDarkTheme.Location = new Point(366, 228);
            chkDarkTheme.Size = new Size(52, 20);
            chkDarkTheme.Text = "Oscuro";
            chkDarkTheme.CheckedChanged += chkDarkTheme_CheckedChanged;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = SystemColors.GrayText;
            lblStatus.Location = new Point(16, 254);
            lblStatus.MaximumSize = new Size(400, 0);
            lblStatus.Size = new Size(400, 15);
            lblStatus.Text = " La resolución se actualiza automáticamente. ";
            //
            // refreshTimer
            //
            refreshTimer.Interval = 1500;
            refreshTimer.Tick += refreshTimer_Tick;
            //
            // trayMenu
            //
            var mRestore = new ToolStripMenuItem("Restaurar ventana");
            mRestore.Click += (s, e) => { Show(); WindowState = FormWindowState.Normal; BringToFront(); };
            var mMax = new ToolStripMenuItem("Maximum (0,5 ms)");
            mMax.Click += (s, e) => btnMaximum.PerformClick();
            var mDefault = new ToolStripMenuItem("Default");
            mDefault.Click += (s, e) => btnDefault.PerformClick();
            var mDefaultClose = new ToolStripMenuItem("Default y cerrar");
            mDefaultClose.Click += (s, e) => btnClose.PerformClick();
            var mExit = new ToolStripMenuItem("Salir");
            mExit.Click += (s, e) => { refreshTimer.Stop(); NtSetTimerResolution(0, false, out _); Application.Exit(); };
            trayMenu.Items.Add(mRestore);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add(mMax);
            trayMenu.Items.Add(mDefault);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add(mDefaultClose);
            trayMenu.Items.Add(mExit);
            //
            // notifyIcon
            //
            notifyIcon.Text = "Timer Resolution";
            notifyIcon.DoubleClick += (s, e) => { Show(); WindowState = FormWindowState.Normal; BringToFront(); };
            notifyIcon.ContextMenuStrip = trayMenu;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(424, 278);
            Controls.Add(grpResolution);
            Controls.Add(lblMode);
            Controls.Add(btnMaximum);
            Controls.Add(btnDefault);
            Controls.Add(cboCustom);
            Controls.Add(btnApplyCustom);
            Controls.Add(btnRefresh);
            Controls.Add(btnClose);
            Controls.Add(btnMeasureSleep);
            Controls.Add(btnHelp);
            Controls.Add(btnAbout);
            Controls.Add(chkStartWithWindows);
            Controls.Add(chkApplyMaxAtStartup);
            Controls.Add(chkDarkTheme);
            Controls.Add(lblStatus);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new Size(440, 316);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Timer Resolution";
            FormClosing += Form1_FormClosing;
            Resize += Form1_Resize;
            KeyDown += Form1_KeyDown;
            grpResolution.ResumeLayout(false);
            grpResolution.PerformLayout();
            trayMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpResolution;
        private Label lblMinLabel;
        private Label lblMin;
        private Label lblMaxLabel;
        private Label lblMax;
        private Label lblCurrentLabel;
        private Label lblCurrent;
        private Label lblMode;
        private Button btnMaximum;
        private Button btnDefault;
        private ComboBox cboCustom;
        private Button btnApplyCustom;
        private Button btnRefresh;
        private Button btnClose;
        private Button btnMeasureSleep;
        private Button btnHelp;
        private Button btnAbout;
        private CheckBox chkStartWithWindows;
        private CheckBox chkApplyMaxAtStartup;
        private CheckBox chkDarkTheme;
        private Label lblStatus;
        private System.Windows.Forms.Timer refreshTimer;
        private ToolTip toolTip;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip trayMenu;
    }
}
