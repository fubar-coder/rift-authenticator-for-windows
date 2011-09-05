namespace RiftAuthenticator.WinForms
{
    partial class Information
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Information));
            this.DeviceId = new System.Windows.Forms.TextBox();
            this.SerialKey = new System.Windows.Forms.TextBox();
            this.SecretKey = new System.Windows.Forms.TextBox();
            this.TimeOffset = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DeviceId
            // 
            resources.ApplyResources(this.DeviceId, "DeviceId");
            this.DeviceId.Name = "DeviceId";
            this.DeviceId.ReadOnly = true;
            // 
            // SerialKey
            // 
            resources.ApplyResources(this.SerialKey, "SerialKey");
            this.SerialKey.Name = "SerialKey";
            this.SerialKey.ReadOnly = true;
            // 
            // SecretKey
            // 
            resources.ApplyResources(this.SecretKey, "SecretKey");
            this.SecretKey.Name = "SecretKey";
            this.SecretKey.ReadOnly = true;
            // 
            // TimeOffset
            // 
            resources.ApplyResources(this.TimeOffset, "TimeOffset");
            this.TimeOffset.Name = "TimeOffset";
            this.TimeOffset.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CloseButton
            // 
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // Description
            // 
            resources.ApplyResources(this.Description, "Description");
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Information
            // 
            this.AcceptButton = this.CloseButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TimeOffset);
            this.Controls.Add(this.SecretKey);
            this.Controls.Add(this.SerialKey);
            this.Controls.Add(this.DeviceId);
            this.Name = "Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DeviceId;
        private System.Windows.Forms.TextBox SerialKey;
        private System.Windows.Forms.TextBox SecretKey;
        private System.Windows.Forms.TextBox TimeOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Description;
    }
}