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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
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
            resources.ApplyResources(this.LoginToken, "LoginToken");
            this.LoginToken.Name = "LoginToken";
            this.LoginToken.ReadOnly = true;
            // 
            // RemainingValidTime
            // 
            resources.ApplyResources(this.RemainingValidTime, "RemainingValidTime");
            this.RemainingValidTime.Maximum = 30000;
            this.RemainingValidTime.Name = "RemainingValidTime";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // SerialKey
            // 
            resources.ApplyResources(this.SerialKey, "SerialKey");
            this.SerialKey.Name = "SerialKey";
            this.SerialKey.ReadOnly = true;
            // 
            // Initialize
            // 
            resources.ApplyResources(this.Initialize, "Initialize");
            this.Initialize.Name = "Initialize";
            this.Initialize.UseVisualStyleBackColor = true;
            this.Initialize.Click += new System.EventHandler(this.Initialize_Click);
            // 
            // TimeSync
            // 
            resources.ApplyResources(this.TimeSync, "TimeSync");
            this.TimeSync.Name = "TimeSync";
            this.TimeSync.UseVisualStyleBackColor = true;
            this.TimeSync.Click += new System.EventHandler(this.TimeSync_Click);
            // 
            // Information
            // 
            resources.ApplyResources(this.Information, "Information");
            this.Information.Name = "Information";
            this.Information.UseVisualStyleBackColor = true;
            this.Information.Click += new System.EventHandler(this.Information_Click);
            // 
            // Recover
            // 
            resources.ApplyResources(this.Recover, "Recover");
            this.Recover.Name = "Recover";
            this.Recover.UseVisualStyleBackColor = true;
            this.Recover.Click += new System.EventHandler(this.Recover_Click);
            // 
            // ShowLicense
            // 
            resources.ApplyResources(this.ShowLicense, "ShowLicense");
            this.ShowLicense.Name = "ShowLicense";
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
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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

