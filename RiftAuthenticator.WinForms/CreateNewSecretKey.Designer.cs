namespace RiftAuthenticator.WinForms
{
    partial class CreateNewSecretKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateNewSecretKey));
            this.label1 = new System.Windows.Forms.Label();
            this.DeviceId = new System.Windows.Forms.TextBox();
            this.RecreateDeviceId = new System.Windows.Forms.Button();
            this.CreateSecretKey = new System.Windows.Forms.Button();
            this.CancelDialog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // DeviceId
            // 
            resources.ApplyResources(this.DeviceId, "DeviceId");
            this.DeviceId.Name = "DeviceId";
            // 
            // RecreateDeviceId
            // 
            resources.ApplyResources(this.RecreateDeviceId, "RecreateDeviceId");
            this.RecreateDeviceId.Name = "RecreateDeviceId";
            this.RecreateDeviceId.UseVisualStyleBackColor = true;
            this.RecreateDeviceId.Click += new System.EventHandler(this.RecreateDeviceId_Click);
            // 
            // CreateSecretKey
            // 
            resources.ApplyResources(this.CreateSecretKey, "CreateSecretKey");
            this.CreateSecretKey.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CreateSecretKey.Name = "CreateSecretKey";
            this.CreateSecretKey.UseVisualStyleBackColor = true;
            this.CreateSecretKey.Click += new System.EventHandler(this.CreateSecretKey_Click);
            // 
            // CancelDialog
            // 
            resources.ApplyResources(this.CancelDialog, "CancelDialog");
            this.CancelDialog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialog.Name = "CancelDialog";
            this.CancelDialog.UseVisualStyleBackColor = true;
            // 
            // CreateNewSecretKey
            // 
            this.AcceptButton = this.CreateSecretKey;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialog;
            this.Controls.Add(this.CancelDialog);
            this.Controls.Add(this.CreateSecretKey);
            this.Controls.Add(this.RecreateDeviceId);
            this.Controls.Add(this.DeviceId);
            this.Controls.Add(this.label1);
            this.Name = "CreateNewSecretKey";
            this.Load += new System.EventHandler(this.CreateNewSecretKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RecreateDeviceId;
        private System.Windows.Forms.Button CreateSecretKey;
        private System.Windows.Forms.Button CancelDialog;
        public System.Windows.Forms.TextBox DeviceId;
    }
}