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
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(180, 21);
            lblTitle.Text = "Timer Resolution";
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(20, 50);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(60, 15);
            lblVersion.Text = "Versión 1.0.0";
            lnkUrl.AutoSize = true;
            lnkUrl.Location = new Point(20, 80);
            lnkUrl.Name = "lnkUrl";
            lnkUrl.Size = new Size(200, 15);
            lnkUrl.TabStop = true;
            lnkUrl.Text = "Clon de la utilidad Timer Resolution";
            lnkUrl.LinkClicked += (s, ev) =>
            {
                if (s is LinkLabel lb) lb.LinkVisited = true;
                try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = "https://github.com", UseShellExecute = true }); } catch { }
            };
            ClientSize = new Size(320, 120);
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
