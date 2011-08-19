namespace RiftAuthenticator.WinForms
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LoginToken = new System.Windows.Forms.TextBox();
            this.RemainingValidTime = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SerialKey = new System.Windows.Forms.TextBox();
            this.Initialize = new System.Windows.Forms.Button();
            this.TimeSync = new System.Windows.Forms.Button();
            this.Information = new System.Windows.Forms.Button();
            this.Recover = new System.Windows.Forms.Button();
            this.ShowLicense = new System.Windows.Forms.Button();
            this.TokenUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // LoginToken
            // 
            this.LoginToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginToken.Location = new System.Drawing.Point(12, 12);
            this.LoginToken.Name = "LoginToken";
            this.LoginToken.ReadOnly = true;
            this.LoginToken.Size = new System.Drawing.Size(440, 20);
            this.LoginToken.TabIndex = 0;
            // 
            // RemainingValidTime
            // 
            this.RemainingValidTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemainingValidTime.Location = new System.Drawing.Point(12, 38);
            this.RemainingValidTime.Maximum = 30000;
            this.RemainingValidTime.Name = "RemainingValidTime";
            this.RemainingValidTime.Size = new System.Drawing.Size(440, 23);
            this.RemainingValidTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial Key:";
            // 
            // SerialKey
            // 
            this.SerialKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SerialKey.Location = new System.Drawing.Point(75, 67);
            this.SerialKey.Name = "SerialKey";
            this.SerialKey.ReadOnly = true;
            this.SerialKey.Size = new System.Drawing.Size(377, 20);
            this.SerialKey.TabIndex = 3;
            // 
            // Initialize
            // 
            this.Initialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Initialize.Location = new System.Drawing.Point(15, 101);
            this.Initialize.Name = "Initialize";
            this.Initialize.Size = new System.Drawing.Size(75, 23);
            this.Initialize.TabIndex = 4;
            this.Initialize.Text = "Initialize";
            this.Initialize.UseVisualStyleBackColor = true;
            this.Initialize.Click += new System.EventHandler(this.Initialize_Click);
            // 
            // TimeSync
            // 
            this.TimeSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TimeSync.Location = new System.Drawing.Point(96, 101);
            this.TimeSync.Name = "TimeSync";
            this.TimeSync.Size = new System.Drawing.Size(75, 23);
            this.TimeSync.TabIndex = 5;
            this.TimeSync.Text = "Time Sync";
            this.TimeSync.UseVisualStyleBackColor = true;
            this.TimeSync.Click += new System.EventHandler(this.TimeSync_Click);
            // 
            // Information
            // 
            this.Information.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Information.Location = new System.Drawing.Point(177, 101);
            this.Information.Name = "Information";
            this.Information.Size = new System.Drawing.Size(75, 23);
            this.Information.TabIndex = 6;
            this.Information.Text = "Info";
            this.Information.UseVisualStyleBackColor = true;
            this.Information.Click += new System.EventHandler(this.Information_Click);
            // 
            // Recover
            // 
            this.Recover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Recover.Location = new System.Drawing.Point(258, 100);
            this.Recover.Name = "Recover";
            this.Recover.Size = new System.Drawing.Size(75, 23);
            this.Recover.TabIndex = 7;
            this.Recover.Text = "Recover";
            this.Recover.UseVisualStyleBackColor = true;
            this.Recover.Click += new System.EventHandler(this.Recover_Click);
            // 
            // ShowLicense
            // 
            this.ShowLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowLicense.Location = new System.Drawing.Point(376, 100);
            this.ShowLicense.Name = "ShowLicense";
            this.ShowLicense.Size = new System.Drawing.Size(75, 23);
            this.ShowLicense.TabIndex = 8;
            this.ShowLicense.Text = "License";
            this.ShowLicense.UseVisualStyleBackColor = true;
            this.ShowLicense.Click += new System.EventHandler(this.ShowLicense_Click);
            // 
            // TokenUpdateTimer
            // 
            this.TokenUpdateTimer.Enabled = true;
            this.TokenUpdateTimer.Interval = 200;
            this.TokenUpdateTimer.Tick += new System.EventHandler(this.TokenUpdateTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 136);
            this.Controls.Add(this.ShowLicense);
            this.Controls.Add(this.Recover);
            this.Controls.Add(this.Information);
            this.Controls.Add(this.TimeSync);
            this.Controls.Add(this.Initialize);
            this.Controls.Add(this.SerialKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RemainingValidTime);
            this.Controls.Add(this.LoginToken);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RIFT™ Authenticator";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LoginToken;
        private System.Windows.Forms.ProgressBar RemainingValidTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SerialKey;
        private System.Windows.Forms.Button Initialize;
        private System.Windows.Forms.Button TimeSync;
        private System.Windows.Forms.Button Information;
        private System.Windows.Forms.Button Recover;
        private System.Windows.Forms.Button ShowLicense;
        private System.Windows.Forms.Timer TokenUpdateTimer;
    }
}

