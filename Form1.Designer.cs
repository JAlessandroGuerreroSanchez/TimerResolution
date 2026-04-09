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
            panelTop = new Panel();
            lblAppTitle = new Label();
            lblAppSubtitle = new Label();
            lnkBrandTop = new LinkLabel();
            panelBody = new Panel();
            panelFooter = new Panel();
            lblFooterOpts = new Label();
            pnlModeStatus = new Panel();
            lblActions = new Label();
            grpResolution = new Panel();
            lblResolutionTitle = new Label();
            lblCurrent = new Label();
            lblCurrentLabel = new Label();
            lblMax = new Label();
            lblMaxLabel = new Label();
            lblMin = new Label();
            lblMinLabel = new Label();
            btnMaximum = new PremiumButton();
            btnDefault = new PremiumButton();
            btnResolution = new PremiumButton();
            btnApplyCustom = new PremiumButton();
            btnRefresh = new PremiumButton();
            btnClose = new PremiumButton();
            btnMeasureSleep = new PremiumButton();
            btnHelp = new PremiumButton();
            btnAbout = new PremiumButton();
            chkStartWithWindows = new CheckBox();
            chkApplyMaxAtStartup = new CheckBox();
            chkWarnOnExitIfHighRes = new CheckBox();
            lnkStudioFooter = new LinkLabel();
            lblStatus = new Label();
            refreshTimer = new System.Windows.Forms.Timer(components);
            toolTip = new ToolTip(components);
            notifyIcon = new NotifyIcon(components);
            trayMenu = new ContextMenuStrip(components);
            pnlResolutionMenu = new Panel();
            btnRes05 = new Button();
            btnRes1 = new Button();
            btnRes2 = new Button();
            btnRes5 = new Button();
            btnRes15625 = new Button();
            panelTop.SuspendLayout();
            panelBody.SuspendLayout();
            panelFooter.SuspendLayout();
            grpResolution.SuspendLayout();
            trayMenu.SuspendLayout();
            pnlResolutionMenu.SuspendLayout();
            SuspendLayout();
            //
            // panelTop
            //
            panelTop.Controls.Add(lblAppSubtitle);
            panelTop.Controls.Add(lblAppTitle);
            panelTop.Controls.Add(lnkBrandTop);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(800, 80);
            panelTop.TabIndex = 0;
            panelTop.Paint += panelTop_Paint;
            //
            // lblAppTitle
            //
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold);
            lblAppTitle.Location = new Point(22, 12);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(182, 31);
            lblAppTitle.TabIndex = 0;
            lblAppTitle.Text = "Timer Resolution";
            //
            // lblAppSubtitle
            //
            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblAppSubtitle.Location = new Point(24, 46);
            lblAppSubtitle.Name = "lblAppSubtitle";
            lblAppSubtitle.Size = new Size(304, 17);
            lblAppSubtitle.TabIndex = 1;
            lblAppSubtitle.Text = "Precisión del temporizador de Windows, simple y potente";
            //
            // lnkBrandTop
            //
            lnkBrandTop.ActiveLinkColor = Color.White;
            lnkBrandTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkBrandTop.AutoSize = true;
            lnkBrandTop.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lnkBrandTop.LinkBehavior = LinkBehavior.NeverUnderline;
            lnkBrandTop.LinkColor = Color.FromArgb(161, 161, 170);
            lnkBrandTop.Location = new Point(668, 18);
            lnkBrandTop.Name = "lnkBrandTop";
            lnkBrandTop.Size = new Size(100, 15);
            lnkBrandTop.TabIndex = 2;
            lnkBrandTop.TabStop = true;
            lnkBrandTop.Text = "KALUR STUDIO";
            lnkBrandTop.VisitedLinkColor = Color.FromArgb(161, 161, 170);
            lnkBrandTop.LinkClicked += lnkBrandTop_LinkClicked;
            //
            // lblShortcutHints
            //
            lblShortcutHints = new Label();
            lblShortcutHints.Dock = DockStyle.Bottom;
            lblShortcutHints.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblShortcutHints.ForeColor = Color.FromArgb(130, 140, 160);
            lblShortcutHints.Name = "lblShortcutHints";
            lblShortcutHints.Padding = new Padding(0, 2, 0, 6);
            lblShortcutHints.Size = new Size(800, 28);
            lblShortcutHints.TabIndex = 21;
            lblShortcutHints.Text = "Atajos: Ctrl+M máximo · Ctrl+D por defecto · Escape salir";
            lblShortcutHints.TextAlign = ContentAlignment.MiddleLeft;
            //
            // panelBody
            //
            panelBody.Controls.Add(grpResolution);
            panelBody.Controls.Add(pnlModeStatus);
            panelBody.Controls.Add(lblActions);
            panelBody.Controls.Add(btnMaximum);
            panelBody.Controls.Add(btnDefault);
            panelBody.Controls.Add(btnResolution);
            panelBody.Controls.Add(pnlResolutionMenu);
            panelBody.Controls.Add(btnApplyCustom);
            panelBody.Controls.Add(btnRefresh);
            panelBody.Controls.Add(btnClose);
            panelBody.Controls.Add(btnMeasureSleep);
            panelBody.Controls.Add(btnHelp);
            panelBody.Controls.Add(btnAbout);
            panelBody.Controls.Add(lblShortcutHints);
            panelBody.Controls.Add(panelFooter);
            panelBody.Dock = DockStyle.Fill;
            panelBody.Location = new Point(0, 80);
            panelBody.Name = "panelBody";
            panelBody.Padding = new Padding(24, 20, 24, 0);
            panelBody.Size = new Size(800, 398);
            panelBody.TabIndex = 1;
            //
            // pnlModeStatus
            //
            pnlModeStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlModeStatus.Location = new Point(0, 150);
            pnlModeStatus.Name = "pnlModeStatus";
            pnlModeStatus.Size = new Size(752, 48);
            pnlModeStatus.TabIndex = 1;
            //
            // lblActions
            //
            lblActions.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lblActions.AutoSize = true;
            lblActions.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblActions.ForeColor = Color.FromArgb(148, 163, 184);
            lblActions.Location = new Point(0, 208);
            lblActions.Name = "lblActions";
            lblActions.Size = new Size(60, 17);
            lblActions.TabIndex = 2;
            lblActions.Text = "Acciones";
            //
            // panelFooter
            //
            panelFooter.Controls.Add(lblFooterOpts);
            panelFooter.Controls.Add(chkStartWithWindows);
            panelFooter.Controls.Add(chkApplyMaxAtStartup);
            panelFooter.Controls.Add(chkWarnOnExitIfHighRes);
            panelFooter.Controls.Add(lblStatus);
            panelFooter.Controls.Add(lnkStudioFooter);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(800, 122);
            panelFooter.TabIndex = 20;
            //
            // lblFooterOpts
            //
            lblFooterOpts.AutoSize = true;
            lblFooterOpts.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblFooterOpts.ForeColor = Color.FromArgb(148, 163, 184);
            lblFooterOpts.Location = new Point(0, 2);
            lblFooterOpts.Name = "lblFooterOpts";
            lblFooterOpts.Size = new Size(60, 17);
            lblFooterOpts.TabIndex = 4;
            lblFooterOpts.Text = "Opciones";
            //
            // grpResolution
            //
            grpResolution.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpResolution.BorderStyle = BorderStyle.None;
            grpResolution.Controls.Add(lblResolutionTitle);
            grpResolution.Controls.Add(lblCurrent);
            grpResolution.Controls.Add(lblCurrentLabel);
            grpResolution.Controls.Add(lblMax);
            grpResolution.Controls.Add(lblMaxLabel);
            grpResolution.Controls.Add(lblMin);
            grpResolution.Controls.Add(lblMinLabel);
            grpResolution.Location = new Point(0, 0);
            grpResolution.Name = "grpResolution";
            grpResolution.Padding = new Padding(0);
            grpResolution.Size = new Size(752, 142);
            grpResolution.TabIndex = 0;
            //
            // lblResolutionTitle
            //
            lblResolutionTitle.AutoSize = true;
            lblResolutionTitle.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            lblResolutionTitle.Location = new Point(20, 14);
            lblResolutionTitle.Name = "lblResolutionTitle";
            lblResolutionTitle.Size = new Size(145, 19);
            lblResolutionTitle.TabIndex = 0;
            lblResolutionTitle.Text = "Valores del temporizador";
            //
            // lblMinLabel, lblMin, lblMaxLabel, lblMax, lblCurrentLabel, lblCurrent
            //
            lblMinLabel.AutoSize = true;
            lblMinLabel.Location = new Point(20, 48);
            lblMinLabel.Size = new Size(55, 15);
            lblMinLabel.Text = "Mínima:";
            lblMin.AutoSize = true;
            lblMin.Location = new Point(20, 70);
            lblMin.Text = "—";
            lblMaxLabel.AutoSize = true;
            lblMaxLabel.Location = new Point(260, 48);
            lblMaxLabel.Size = new Size(57, 15);
            lblMaxLabel.Text = "Máxima:";
            lblMax.AutoSize = true;
            lblMax.Location = new Point(260, 70);
            lblMax.Text = "—";
            lblCurrentLabel.AutoSize = true;
            lblCurrentLabel.Location = new Point(500, 48);
            lblCurrentLabel.Size = new Size(46, 15);
            lblCurrentLabel.Text = "Actual:";
            lblCurrent.AutoSize = true;
            lblCurrent.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblCurrent.Location = new Point(500, 66);
            lblCurrent.Text = "—";
            //
            // btnMaximum, btnDefault, btnResolution, btnApplyCustom, btnRefresh
            //
            btnMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnMaximum.Location = new Point(0, 232);
            btnMaximum.Size = new Size(132, 40);
            btnMaximum.TabIndex = 2;
            btnMaximum.Text = "Máximo";
            btnMaximum.Click += btnMaximum_Click;
            btnDefault.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnDefault.Location = new Point(140, 232);
            btnDefault.Size = new Size(154, 40);
            btnDefault.TabIndex = 3;
            btnDefault.Text = "Por defecto";
            btnDefault.Click += btnDefault_Click;
            btnResolution.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnResolution.Location = new Point(456, 232);
            btnResolution.Size = new Size(120, 40);
            btnResolution.TabIndex = 4;
            btnResolution.Text = "0,5 ms";
            btnResolution.Click += btnResolution_Click;
            btnApplyCustom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnApplyCustom.Location = new Point(584, 232);
            btnApplyCustom.Size = new Size(104, 40);
            btnApplyCustom.TabIndex = 5;
            btnApplyCustom.Text = "Aplicar";
            btnApplyCustom.Click += btnApplyCustom_Click;
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Location = new Point(696, 232);
            btnRefresh.Size = new Size(56, 40);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "↻";
            btnRefresh.Click += btnRefresh_Click;
            //
            // btnClose, btnMeasureSleep, btnHelp, btnAbout
            //
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnClose.Location = new Point(0, 284);
            btnClose.Size = new Size(132, 40);
            btnClose.TabIndex = 7;
            btnClose.Text = "Salir";
            btnClose.Click += btnClose_Click;
            btnMeasureSleep.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnMeasureSleep.Location = new Point(140, 284);
            btnMeasureSleep.Size = new Size(188, 40);
            btnMeasureSleep.TabIndex = 8;
            btnMeasureSleep.Text = "Medir Sleep(1)";
            btnMeasureSleep.Click += btnMeasureSleep_Click;
            btnHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHelp.Location = new Point(578, 284);
            btnHelp.Size = new Size(48, 40);
            btnHelp.TabIndex = 9;
            btnHelp.Text = "?";
            btnHelp.Click += btnHelp_Click;
            btnAbout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAbout.Location = new Point(634, 284);
            btnAbout.Size = new Size(118, 40);
            btnAbout.TabIndex = 10;
            btnAbout.Text = "Acerca de";
            btnAbout.Click += btnAbout_Click;
            //
            // chkStartWithWindows, chkApplyMaxAtStartup
            //
            chkStartWithWindows.AutoSize = true;
            chkStartWithWindows.Location = new Point(0, 24);
            chkStartWithWindows.Size = new Size(170, 19);
            chkStartWithWindows.TabIndex = 0;
            chkStartWithWindows.Text = "Inicio con Windows";
            chkStartWithWindows.CheckedChanged += chkStartWithWindows_CheckedChanged;
            chkApplyMaxAtStartup.AutoSize = true;
            chkApplyMaxAtStartup.Location = new Point(208, 24);
            chkApplyMaxAtStartup.Size = new Size(190, 19);
            chkApplyMaxAtStartup.TabIndex = 1;
            chkApplyMaxAtStartup.Text = "Máximo al iniciar";
            chkApplyMaxAtStartup.CheckedChanged += chkApplyMaxAtStartup_CheckedChanged;
            //
            // chkWarnOnExitIfHighRes
            //
            chkWarnOnExitIfHighRes.AutoSize = true;
            chkWarnOnExitIfHighRes.Location = new Point(0, 48);
            chkWarnOnExitIfHighRes.Name = "chkWarnOnExitIfHighRes";
            chkWarnOnExitIfHighRes.Size = new Size(420, 19);
            chkWarnOnExitIfHighRes.TabIndex = 5;
            chkWarnOnExitIfHighRes.Text = "Confirmar al cerrar si hay alta precisión";
            chkWarnOnExitIfHighRes.CheckedChanged += chkWarnOnExitIfHighRes_CheckedChanged;
            //
            // lblStatus
            //
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(0, 74);
            lblStatus.MaximumSize = new Size(480, 0);
            lblStatus.Size = new Size(480, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = " La resolución se actualiza automáticamente. ";
            //
            // lnkStudioFooter
            //
            lnkStudioFooter.ActiveLinkColor = Color.White;
            lnkStudioFooter.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lnkStudioFooter.AutoSize = true;
            lnkStudioFooter.Font = new Font("Segoe UI", 8.5F);
            lnkStudioFooter.LinkBehavior = LinkBehavior.NeverUnderline;
            lnkStudioFooter.LinkColor = Color.FromArgb(113, 113, 122);
            lnkStudioFooter.Location = new Point(520, 72);
            lnkStudioFooter.Name = "lnkStudioFooter";
            lnkStudioFooter.Size = new Size(183, 15);
            lnkStudioFooter.TabIndex = 3;
            lnkStudioFooter.TabStop = true;
            lnkStudioFooter.Text = "Created by KALUR STUDIO · kalur.me";
            lnkStudioFooter.VisitedLinkColor = Color.FromArgb(113, 113, 122);
            lnkStudioFooter.LinkClicked += lnkStudioFooter_LinkClicked;
            //
            // refreshTimer
            //
            refreshTimer.Interval = 2000;
            refreshTimer.Tick += refreshTimer_Tick;
            //
            // pnlResolutionMenu
            //
            pnlResolutionMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlResolutionMenu.BorderStyle = BorderStyle.None;
            pnlResolutionMenu.Controls.Add(btnRes15625);
            pnlResolutionMenu.Controls.Add(btnRes5);
            pnlResolutionMenu.Controls.Add(btnRes2);
            pnlResolutionMenu.Controls.Add(btnRes1);
            pnlResolutionMenu.Controls.Add(btnRes05);
            pnlResolutionMenu.Location = new Point(456, 274);
            pnlResolutionMenu.Name = "pnlResolutionMenu";
            pnlResolutionMenu.Size = new Size(116, 170);
            pnlResolutionMenu.TabIndex = 18;
            pnlResolutionMenu.Visible = false;
            pnlResolutionMenu.Paint += pnlResolutionMenu_Paint;
            //
            // btnRes05
            //
            btnRes05.Dock = DockStyle.Top;
            btnRes05.FlatStyle = FlatStyle.Flat;
            btnRes05.FlatAppearance.BorderSize = 0;
            btnRes05.Height = 34;
            btnRes05.Text = "0,5 ms";
            btnRes05.Tag = 0.5d;
            btnRes05.Click += customResolutionItem_Click;
            //
            // btnRes1
            //
            btnRes1.Dock = DockStyle.Top;
            btnRes1.FlatStyle = FlatStyle.Flat;
            btnRes1.FlatAppearance.BorderSize = 0;
            btnRes1.Height = 34;
            btnRes1.Text = "1 ms";
            btnRes1.Tag = 1d;
            btnRes1.Click += customResolutionItem_Click;
            //
            // btnRes2
            //
            btnRes2.Dock = DockStyle.Top;
            btnRes2.FlatStyle = FlatStyle.Flat;
            btnRes2.FlatAppearance.BorderSize = 0;
            btnRes2.Height = 34;
            btnRes2.Text = "2 ms";
            btnRes2.Tag = 2d;
            btnRes2.Click += customResolutionItem_Click;
            //
            // btnRes5
            //
            btnRes5.Dock = DockStyle.Top;
            btnRes5.FlatStyle = FlatStyle.Flat;
            btnRes5.FlatAppearance.BorderSize = 0;
            btnRes5.Height = 34;
            btnRes5.Text = "5 ms";
            btnRes5.Tag = 5d;
            btnRes5.Click += customResolutionItem_Click;
            //
            // btnRes15625
            //
            btnRes15625.Dock = DockStyle.Top;
            btnRes15625.FlatStyle = FlatStyle.Flat;
            btnRes15625.FlatAppearance.BorderSize = 0;
            btnRes15625.Height = 34;
            btnRes15625.Text = "15,625 ms";
            btnRes15625.Tag = 15.625d;
            btnRes15625.Click += customResolutionItem_Click;
            //
            // trayMenu
            //
            var mRestore = new ToolStripMenuItem("Restaurar ventana");
            mRestore.Click += (s, e) => { Show(); WindowState = FormWindowState.Normal; BringToFront(); };
            var mMax = new ToolStripMenuItem("Máximo (0,5 ms)");
            mMax.Click += (s, e) => btnMaximum.PerformClick();
            var mDefault = new ToolStripMenuItem("Por defecto");
            mDefault.Click += (s, e) => btnDefault.PerformClick();
            var mDefaultClose = new ToolStripMenuItem("Por defecto y cerrar");
            mDefaultClose.Click += (s, e) => btnClose.PerformClick();
            var mCopyDiag = new ToolStripMenuItem("Copiar información de diagnóstico");
            mCopyDiag.Click += (s, e) => CopyDiagnosticsToClipboard();
            var mSupport = new ToolStripMenuItem("Web y soporte (kalur.me)");
            mSupport.Click += (s, e) => OpenStudioUrl();
            var mExit = new ToolStripMenuItem("Salir");
            mExit.Click += (s, e) =>
            {
                refreshTimer.Stop();
                TimerResolutionNative.NtSetTimerResolution(0, false, out _);
                Application.Exit();
            };
            trayMenu.Items.Add(mRestore);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add(mMax);
            trayMenu.Items.Add(mDefault);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add(mDefaultClose);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add(mCopyDiag);
            trayMenu.Items.Add(mSupport);
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
            BackColor = Color.FromArgb(10, 10, 12);
            ClientSize = new Size(800, 520);
            Controls.Add(panelBody);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimumSize = new Size(760, 500);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Timer Resolution";
            FormClosing += Form1_FormClosing;
            Resize += Form1_Resize;
            KeyDown += Form1_KeyDown;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelBody.ResumeLayout(false);
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            grpResolution.ResumeLayout(false);
            grpResolution.PerformLayout();
            trayMenu.ResumeLayout(false);
            pnlResolutionMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelTop;
        private Panel panelBody;
        private Panel panelFooter;
        private Label lblFooterOpts;
        private Panel pnlModeStatus;
        private Label lblActions;
        private Label lblAppTitle;
        private Label lblAppSubtitle;
        private LinkLabel lnkBrandTop;
        private Panel grpResolution;
        private Label lblResolutionTitle;
        private Label lblMinLabel;
        private Label lblMin;
        private Label lblMaxLabel;
        private Label lblMax;
        private Label lblCurrentLabel;
        private Label lblCurrent;
        private PremiumButton btnMaximum;
        private PremiumButton btnDefault;
        private PremiumButton btnResolution;
        private PremiumButton btnApplyCustom;
        private PremiumButton btnRefresh;
        private PremiumButton btnClose;
        private PremiumButton btnMeasureSleep;
        private PremiumButton btnHelp;
        private PremiumButton btnAbout;
        private CheckBox chkStartWithWindows;
        private CheckBox chkApplyMaxAtStartup;
        private CheckBox chkWarnOnExitIfHighRes;
        private Label lblShortcutHints;
        private LinkLabel lnkStudioFooter;
        private Label lblStatus;
        private System.Windows.Forms.Timer refreshTimer;
        private ToolTip toolTip;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip trayMenu;
        private Panel pnlResolutionMenu;
        private Button btnRes05;
        private Button btnRes1;
        private Button btnRes2;
        private Button btnRes5;
        private Button btnRes15625;
    }
}
