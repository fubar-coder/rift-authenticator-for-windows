namespace RiftAuthenticator.WinForms
{
    partial class SecurityQuestions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityQuestions));
            this.SecurityQuestion1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SecurityAnswer1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SecurityAnswer2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SecurityQuestion2 = new System.Windows.Forms.TextBox();
            this.CancelDialog = new System.Windows.Forms.Button();
            this.DoRecovery = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SecurityQuestion1
            // 
            resources.ApplyResources(this.SecurityQuestion1, "SecurityQuestion1");
            this.SecurityQuestion1.Name = "SecurityQuestion1";
            this.SecurityQuestion1.ReadOnly = true;
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
            // SecurityAnswer1
            // 
            resources.ApplyResources(this.SecurityAnswer1, "SecurityAnswer1");
            this.SecurityAnswer1.Name = "SecurityAnswer1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // SecurityAnswer2
            // 
            resources.ApplyResources(this.SecurityAnswer2, "SecurityAnswer2");
            this.SecurityAnswer2.Name = "SecurityAnswer2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // SecurityQuestion2
            // 
            resources.ApplyResources(this.SecurityQuestion2, "SecurityQuestion2");
            this.SecurityQuestion2.Name = "SecurityQuestion2";
            this.SecurityQuestion2.ReadOnly = true;
            // 
            // CancelDialog
            // 
            resources.ApplyResources(this.CancelDialog, "CancelDialog");
            this.CancelDialog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialog.Name = "CancelDialog";
            this.CancelDialog.UseVisualStyleBackColor = true;
            // 
            // DoRecovery
            // 
            resources.ApplyResources(this.DoRecovery, "DoRecovery");
            this.DoRecovery.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.DoRecovery.Name = "DoRecovery";
            this.DoRecovery.UseVisualStyleBackColor = true;
            this.DoRecovery.Click += new System.EventHandler(this.DoRecovery_Click);
            // 
            // SecurityQuestions
            // 
            this.AcceptButton = this.DoRecovery;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialog;
            this.Controls.Add(this.CancelDialog);
            this.Controls.Add(this.DoRecovery);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SecurityAnswer2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SecurityQuestion2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SecurityAnswer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SecurityQuestion1);
            this.Name = "SecurityQuestions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CancelDialog;
        private System.Windows.Forms.Button DoRecovery;
        public System.Windows.Forms.TextBox SecurityQuestion1;
        public System.Windows.Forms.TextBox SecurityAnswer1;
        public System.Windows.Forms.TextBox SecurityAnswer2;
        public System.Windows.Forms.TextBox SecurityQuestion2;
    }
}