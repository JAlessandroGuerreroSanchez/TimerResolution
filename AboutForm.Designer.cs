namespace TimerResolutionApp
{
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblVersion;
        private LinkLabel lnkUrl;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblVersion = new Label();
            lnkUrl = new LinkLabel();
            SuspendLayout();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Variable Text Semibold", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(208, 25);
            lblTitle.Text = "Timer Resolution";
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI Variable Text", 9.5F, FontStyle.Regular);
            lblVersion.Location = new Point(21, 54);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(60, 15);
            lblVersion.Text = "Versión 1.0.0";
            lnkUrl.AutoSize = true;
            lnkUrl.Font = new Font("Segoe UI Variable Text", 9.5F, FontStyle.Regular);
            lnkUrl.Location = new Point(21, 85);
            lnkUrl.Name = "lnkUrl";
            lnkUrl.Size = new Size(165, 15);
            lnkUrl.TabStop = true;
            lnkUrl.Text = "Created by KALUR STUDIO";
            lnkUrl.LinkClicked += (s, ev) =>
            {
                if (s is LinkLabel lb) lb.LinkVisited = true;
                BrowserLaunch.OpenIfTrustedHttpOrHttps(AppConstants.SupportWebsiteUrl);
            };
            ClientSize = new Size(360, 132);
            Controls.Add(lblTitle);
            Controls.Add(lblVersion);
            Controls.Add(lnkUrl);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Acerca de";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
