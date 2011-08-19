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
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device ID:";
            // 
            // DeviceId
            // 
            this.DeviceId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceId.Location = new System.Drawing.Point(76, 12);
            this.DeviceId.Name = "DeviceId";
            this.DeviceId.Size = new System.Drawing.Size(314, 20);
            this.DeviceId.TabIndex = 1;
            // 
            // RecreateDeviceId
            // 
            this.RecreateDeviceId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RecreateDeviceId.Location = new System.Drawing.Point(396, 10);
            this.RecreateDeviceId.Name = "RecreateDeviceId";
            this.RecreateDeviceId.Size = new System.Drawing.Size(75, 23);
            this.RecreateDeviceId.TabIndex = 2;
            this.RecreateDeviceId.Text = "Recreate";
            this.RecreateDeviceId.UseVisualStyleBackColor = true;
            this.RecreateDeviceId.Click += new System.EventHandler(this.RecreateDeviceId_Click);
            // 
            // CreateSecretKey
            // 
            this.CreateSecretKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CreateSecretKey.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CreateSecretKey.Location = new System.Drawing.Point(15, 53);
            this.CreateSecretKey.Name = "CreateSecretKey";
            this.CreateSecretKey.Size = new System.Drawing.Size(75, 23);
            this.CreateSecretKey.TabIndex = 3;
            this.CreateSecretKey.Text = "OK";
            this.CreateSecretKey.UseVisualStyleBackColor = true;
            this.CreateSecretKey.Click += new System.EventHandler(this.CreateSecretKey_Click);
            // 
            // CancelDialog
            // 
            this.CancelDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelDialog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialog.Location = new System.Drawing.Point(395, 53);
            this.CancelDialog.Name = "CancelDialog";
            this.CancelDialog.Size = new System.Drawing.Size(75, 23);
            this.CancelDialog.TabIndex = 4;
            this.CancelDialog.Text = "Cancel";
            this.CancelDialog.UseVisualStyleBackColor = true;
            // 
            // CreateNewSecretKey
            // 
            this.AcceptButton = this.CreateSecretKey;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialog;
            this.ClientSize = new System.Drawing.Size(483, 88);
            this.Controls.Add(this.CancelDialog);
            this.Controls.Add(this.CreateSecretKey);
            this.Controls.Add(this.RecreateDeviceId);
            this.Controls.Add(this.DeviceId);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreateNewSecretKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a new secret key";
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