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
            this.TimeSync = new System.Windows.Forms.Button();
            this.TokenUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileQuitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.accountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountCreateMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountRecoverMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountManageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.AccountInfoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.AccountExportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountImportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpLicenseMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.HelpAboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportAccountDialog = new System.Windows.Forms.OpenFileDialog();
            this.ExportAccountDialog = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.Accounts = new System.Windows.Forms.ComboBox();
            this.MainMenu.SuspendLayout();
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
            // TimeSync
            // 
            resources.ApplyResources(this.TimeSync, "TimeSync");
            this.TimeSync.Name = "TimeSync";
            this.TimeSync.UseVisualStyleBackColor = true;
            this.TimeSync.Click += new System.EventHandler(this.TimeSync_Click);
            // 
            // TokenUpdateTimer
            // 
            this.TokenUpdateTimer.Enabled = true;
            this.TokenUpdateTimer.Interval = 200;
            this.TokenUpdateTimer.Tick += new System.EventHandler(this.TokenUpdateTimer_Tick);
            // 
            // MainMenu
            // 
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationToolStripMenuItem,
            this.accountToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MainMenu.Name = "MainMenu";
            // 
            // applicationToolStripMenuItem
            // 
            resources.ApplyResources(this.applicationToolStripMenuItem, "applicationToolStripMenuItem");
            this.applicationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileQuitMenu});
            this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
            // 
            // FileQuitMenu
            // 
            resources.ApplyResources(this.FileQuitMenu, "FileQuitMenu");
            this.FileQuitMenu.Name = "FileQuitMenu";
            this.FileQuitMenu.Click += new System.EventHandler(this.FileQuitMenu_Click);
            // 
            // accountToolStripMenuItem
            // 
            resources.ApplyResources(this.accountToolStripMenuItem, "accountToolStripMenuItem");
            this.accountToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AccountCreateMenu,
            this.AccountRecoverMenu,
            this.AccountManageMenu,
            this.toolStripMenuItem1,
            this.AccountInfoMenu,
            this.toolStripMenuItem3,
            this.AccountExportMenu,
            this.AccountImportMenu});
            this.accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            // 
            // AccountCreateMenu
            // 
            resources.ApplyResources(this.AccountCreateMenu, "AccountCreateMenu");
            this.AccountCreateMenu.Name = "AccountCreateMenu";
            this.AccountCreateMenu.Click += new System.EventHandler(this.AccountCreateMenu_Click);
            // 
            // AccountRecoverMenu
            // 
            resources.ApplyResources(this.AccountRecoverMenu, "AccountRecoverMenu");
            this.AccountRecoverMenu.Name = "AccountRecoverMenu";
            this.AccountRecoverMenu.Click += new System.EventHandler(this.AccountRecoverMenu_Click);
            // 
            // AccountManageMenu
            // 
            resources.ApplyResources(this.AccountManageMenu, "AccountManageMenu");
            this.AccountManageMenu.Name = "AccountManageMenu";
            this.AccountManageMenu.Click += new System.EventHandler(this.AccountManageMenu_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // AccountInfoMenu
            // 
            resources.ApplyResources(this.AccountInfoMenu, "AccountInfoMenu");
            this.AccountInfoMenu.Name = "AccountInfoMenu";
            this.AccountInfoMenu.Click += new System.EventHandler(this.AccountInfoMenu_Click);
            // 
            // toolStripMenuItem3
            // 
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            // 
            // AccountExportMenu
            // 
            resources.ApplyResources(this.AccountExportMenu, "AccountExportMenu");
            this.AccountExportMenu.Name = "AccountExportMenu";
            this.AccountExportMenu.Click += new System.EventHandler(this.AccountExportMenu_Click);
            // 
            // AccountImportMenu
            // 
            resources.ApplyResources(this.AccountImportMenu, "AccountImportMenu");
            this.AccountImportMenu.Name = "AccountImportMenu";
            this.AccountImportMenu.Click += new System.EventHandler(this.AccountImportMenu_Click);
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpLicenseMenu,
            this.toolStripMenuItem2,
            this.HelpAboutMenu});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // HelpLicenseMenu
            // 
            resources.ApplyResources(this.HelpLicenseMenu, "HelpLicenseMenu");
            this.HelpLicenseMenu.Name = "HelpLicenseMenu";
            this.HelpLicenseMenu.Click += new System.EventHandler(this.HelpLicenseMenu_Click);
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // HelpAboutMenu
            // 
            resources.ApplyResources(this.HelpAboutMenu, "HelpAboutMenu");
            this.HelpAboutMenu.Name = "HelpAboutMenu";
            this.HelpAboutMenu.Click += new System.EventHandler(this.HelpAboutMenu_Click);
            // 
            // ImportAccountDialog
            // 
            this.ImportAccountDialog.DefaultExt = "xml";
            resources.ApplyResources(this.ImportAccountDialog, "ImportAccountDialog");
            // 
            // ExportAccountDialog
            // 
            this.ExportAccountDialog.DefaultExt = "xml";
            resources.ApplyResources(this.ExportAccountDialog, "ExportAccountDialog");
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Accounts
            // 
            resources.ApplyResources(this.Accounts, "Accounts");
            this.Accounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Accounts.FormattingEnabled = true;
            this.Accounts.Name = "Accounts";
            this.Accounts.SelectedIndexChanged += new System.EventHandler(this.Accounts_SelectedIndexChanged);
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Accounts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TimeSync);
            this.Controls.Add(this.SerialKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RemainingValidTime);
            this.Controls.Add(this.LoginToken);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LoginToken;
        private System.Windows.Forms.ProgressBar RemainingValidTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SerialKey;
        private System.Windows.Forms.Button TimeSync;
        private System.Windows.Forms.Timer TokenUpdateTimer;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileQuitMenu;
        private System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AccountCreateMenu;
        private System.Windows.Forms.ToolStripMenuItem AccountManageMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem AccountExportMenu;
        private System.Windows.Forms.ToolStripMenuItem AccountImportMenu;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpAboutMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpLicenseMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem AccountRecoverMenu;
        private System.Windows.Forms.ToolStripMenuItem AccountInfoMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.OpenFileDialog ImportAccountDialog;
        private System.Windows.Forms.SaveFileDialog ExportAccountDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Accounts;
    }
}

